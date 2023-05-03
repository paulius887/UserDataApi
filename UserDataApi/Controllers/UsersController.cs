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
            _context.Entries.RemoveRange(_context.Entries.Where(x => x.UserId == id));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/Users/{id}/Entries
        [HttpGet("{id}/Entries")]
        public async Task<ActionResult<IEnumerable<EntryBook>>> GetUserEntries(int id) {
            if (_context.Entries == null) {
                return NotFound();
            }
            List<Entry> entries = await _context.Entries.Where(x => x.UserId == id).ToListAsync();
            HttpResponseMessage response = client.GetAsync("api/books/").Result;
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelError("Book", "Book with specified BookId could not be found");
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
            List<Book> books = response.Content.ReadFromJsonAsync<List<Book>>().Result;
            List<EntryBook> EntriesBooks = new List<EntryBook>();
            for (int i = 0; i < entries.Count; ++i) {
                int bookId = entries[i].BookId;
                Boolean changed = false;
                for (int j = 0; j < books.Count; ++j) {
                    if (books[i].id == bookId) {
                        EntriesBooks.Add(new EntryBook(entries[i], books[i]));
                        changed = true;
                        break;
                    }
                }
                if (changed == false) {
                    ModelState.AddModelError("Entry " + i, "Book with specified BookId could not be found");
                }
            }
            if (ModelState.IsValid) {
                return EntriesBooks;
            }
            else {
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
        }

        // GET: api/Users/{id}/Entries/{id}
        [HttpGet("{id}/Entries/{entryid}")]
        public async Task<ActionResult<EntryBook>> GetUserEntries(int id, int entryid) {
            if (_context.Entries == null) {
                return NotFound();
            }
            var entry = await _context.Entries.FindAsync(entryid, id);
            if (entry == null) {
                return NotFound();
            }
            try {
                HttpResponseMessage response = client.GetAsync("api/books/" + entry.BookId).Result;
                if (!response.IsSuccessStatusCode) {
                    ModelState.AddModelError("Book", "Book with specified BookId could not be found");
                    return BadRequest(new ValidationProblemDetails(this.ModelState));
                }
                Book book = response.Content.ReadFromJsonAsync<Book>().Result;
                book.id = entry.BookId;
                return Ok(new EntryBook(entry, book));
            }
            catch (Exception ex) {
                return Ok(new EntryBookNoBookInfo(entry));
            }
        }

        // POST: api/Users/{id}/Entries
        [HttpPost("{id}/Entries")]
        public async Task<ActionResult<EntryBook>> PostEntry(int id, EntryBookDto entryBookDto) {
            var json = JsonConvert.SerializeObject(entryBookDto.bookDto);
            HttpResponseMessage response = client.PostAsync("api/books/", new StringContent(json, Encoding.UTF8, "application/json")).Result;
            if (!response.IsSuccessStatusCode) {
                ModelState.AddModelError("Book", "Given book could not be added to the database");
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
            Book book = response.Content.ReadFromJsonAsync<Book>().Result;
            var newEntry = new Entry {
                Id = _context.Entries.Where(x => x.UserId == id).Max(x => (int?)x.Id) + 1 ?? 1,
                UserId = id,
                BookId = book.id,
                EntryText = entryBookDto.entryDto.EntryText,
                LastEdited = DateTime.Now
            };
            _context.Entries.Add(newEntry);
            await _context.SaveChangesAsync();
            return Ok(new EntryBook(newEntry, book));
        }

        // PUT: api/Users/{id}/Entries/{entryid}
        [HttpPut("{id}/Entries/{entryid}")]
        public async Task<ActionResult<Entry>> PutEntry(int id, int entryid, EntryDto entryDto) {
            if (await _context.Users.FindAsync(id) == null) {
                return NotFound();
            }
            var entry = await _context.Entries.FindAsync(entryid, id);
            if (entry == null) {
                return NotFound();
            }
            entry.EntryText = entryDto.EntryText;
            entry.LastEdited = DateTime.Now;
            await _context.SaveChangesAsync();
            return entry;
        }

        // DELETE: api/Users/{id}/Entries/{entryid}
        [HttpDelete("{id}/Entries/{entryid}")]
        public async Task<IActionResult> DeleteEntry(int id, int entryid) {
            if (await _context.Users.FindAsync(id) == null) {
                return NotFound();
            }
            var entry = await _context.Entries.FindAsync(entryid, id);
            if (entry == null) {
                return NotFound();
            }
            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }
		
		// DELETE: api/Users/{id}/Entries/
        [HttpDelete("{id}/Entries")]
        public async Task<IActionResult> DeleteAllEntries(int id) {
            if (await _context.Users.FindAsync(id) == null) {
                return NotFound();
            }
            _context.Entries.RemoveRange(_context.Entries.Where(x => x.UserId == id));
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
