using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using UserDataApi.Data;
using UserDataApi.Models;
using UserDataApi.Validation;

namespace UserDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly UserContext _context;
        HttpClient client;

        public UsersController(UserContext context) {
            _context = context;
            client = new HttpClient();
            client.BaseAddress = new Uri("http://host.docker.internal:5001");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
            if (_context.Users == null) {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id) {
            if (_context.Users == null) {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null) {
                return NotFound();
            }
            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserDto userDto) {
            var user = new User {
                Username = userDto.Username,
                Email = userDto.Email,
                RegisterDate = DateTime.Now
            };
            if (_context.Users == null) {
                return Problem("Entity set 'UserContext.Users' is null.");
            }
            UserDataValidation(userDto.Username, userDto.Email);
            if (ModelState.IsValid) {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            else {
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, UserDto userDto) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            UserDataValidation(userDto.Username, userDto.Email);
            if (ModelState.IsValid) {
                user.Username = userDto.Username;
                user.Email = userDto.Email;
                if (userDto.DisplayName != null) {
                    if (userDto.DisplayName == "" || userDto.DisplayName == null) {
                        user.DisplayName = null;
                    }
                    else {
                        user.DisplayName = userDto.DisplayName;
                    }
                }
                await _context.SaveChangesAsync();
                return user;
            }
            else {
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
            if (_context.Users == null) {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            _context.Comments.RemoveRange(_context.Comments.Where(x => x.UserId == id));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Users/{id}/Comments
        [HttpGet("{id}/Comments")]
        public async Task<ActionResult<IEnumerable<CommentBook>>> GetUserComments(int id) {
            if (_context.Comments == null) {
                return NotFound();
            }
            HttpResponseMessage response = null;
            List<Comment> comments = await _context.Comments.Where(x => x.UserId == id).ToListAsync();
            try {
                response = client.GetAsync("api/books/").Result;
            }
            catch (Exception ex) {
                List<CommentBookNoBookInfo> CommentsBooksNoInfo = new List<CommentBookNoBookInfo>();
                for (int i = 0; i < comments.Count; ++i) {
                    CommentsBooksNoInfo.Add(new CommentBookNoBookInfo(comments[i]));
                }
                return Ok(CommentsBooksNoInfo);
            }
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelError("Book", "Book with specified BookId could not be found");
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
            List<Book> books = response.Content.ReadFromJsonAsync<List<Book>>().Result;
            List<CommentBook> CommentsBooks = new List<CommentBook>();
            for (int i = 0; i < comments.Count; ++i) {
                int bookId = comments[i].BookId;
                Boolean changed = false;
                for (int j = 0; j < books.Count; ++j) {
                    if (books[j].id == bookId) {
                        CommentsBooks.Add(new CommentBook(comments[i], books[j]));
                        changed = true;
                        break;
                    }
                }
                if (changed == false) {
                    ModelState.AddModelError("Comment " + i, "Book with specified BookId could not be found (id = " + bookId + ") ");
                }
            }
            if (ModelState.IsValid) {
                return Ok(CommentsBooks);
            }
            else {
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
        }

        // GET: api/Users/{id}/Comments/{id}
        [HttpGet("{id}/Comments/{commentid}")]
        public async Task<ActionResult<CommentBook>> GetUserComments(int id, int commentid) {
            if (_context.Comments == null) {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(commentid, id);
            if (comment == null) {
                return NotFound();
            }
            try {
                HttpResponseMessage response = client.GetAsync("api/books/" + comment.BookId).Result;
                if (!response.IsSuccessStatusCode) {
                    ModelState.AddModelError("Book", "Book with specified BookId could not be found");
                    return BadRequest(new ValidationProblemDetails(this.ModelState));
                }
                Book book = response.Content.ReadFromJsonAsync<Book>().Result;
                book.id = comment.BookId;
                return Ok(new CommentBook(comment, book));
            }
            catch (Exception ex) {
                return Ok(new CommentBookNoBookInfo(comment));
            }
        }

        // POST: api/Users/{id}/Comments
        [HttpPost("{id}/Comments")]
        public async Task<ActionResult<CommentBook>> PostComment(int id, CommentBookDto commentBookDto) {
            var json = JsonConvert.SerializeObject(commentBookDto.bookDto);
            HttpResponseMessage response = null;
            try {
                response = client.PostAsync("api/books/", new StringContent(json, Encoding.UTF8, "application/json")).Result;
            }
            catch {
                ModelState.AddModelError("BookService", "BookService is unavailable.");
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelError("Book", "Given book could not be added to the database");
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
            Book book = response.Content.ReadFromJsonAsync<Book>().Result;
            var newComment = new Comment {
                Id = _context.Comments.Where(x => x.UserId == id).Max(x => (int?)x.Id) + 1 ?? 1,
                UserId = id,
                BookId = book.id,
                CommentText = commentBookDto.commentDto.CommentText,
                LastEdited = DateTime.Now
            };
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return Ok(new CommentBook(newComment, book));
        }

        // PUT: api/Users/{id}/Comments/{commentid}
        [HttpPut("{id}/Comments/{commentid}")]
        public async Task<ActionResult<Comment>> PutComment(int id, int commentid, CommentDto commentDto) {
            if (await _context.Users.FindAsync(id) == null) {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(commentid, id);
            if (comment == null) {
                return NotFound();
            }
            comment.CommentText = commentDto.CommentText;
            comment.LastEdited = DateTime.Now;
            await _context.SaveChangesAsync();
            return comment;
        }

        // DELETE: api/Users/{id}/Comments/{commentid}
        [HttpDelete("{id}/Comments/{commentid}")]
        public async Task<IActionResult> DeleteComment(int id, int commentid) {
            if (await _context.Users.FindAsync(id) == null) {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(commentid, id);
            if (comment == null) {
                return NotFound();
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
		
		// DELETE: api/Users/{id}/Comments/
        [HttpDelete("{id}/Comments")]
        public async Task<IActionResult> DeleteAllComments(int id) {
            if (await _context.Users.FindAsync(id) == null) {
                return NotFound();
            }
            _context.Comments.RemoveRange(_context.Comments.Where(x => x.UserId == id));
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private void UserDataValidation(string username, string email) {
            String errorMessage = UsernameIsValid.IsValid(username, _context);
            if (errorMessage != "") {
                ModelState.AddModelError("Username", errorMessage);
            }
            errorMessage = EmailIsValid.IsValid(email, _context);
            if (errorMessage != "") {
                ModelState.AddModelError("Email", errorMessage);
            }
        }

        private bool UserExists(int id) {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
