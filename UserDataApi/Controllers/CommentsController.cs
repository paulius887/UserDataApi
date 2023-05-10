using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using UserDataApi.Data;
using UserDataApi.Models;

namespace UserDataApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase {
        private readonly UserContext _context;

        HttpClient client;

        public CommentsController(UserContext context) {
            _context = context;
            client = new HttpClient();
            client.BaseAddress = new Uri("http://host.docker.internal:5001");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments() {
            if (_context.Comments == null) {
                return NotFound();
            }
            return await _context.Comments.ToListAsync();
        }
        /*
        [HttpGet("/{bookid}")]
        public async Task<ActionResult<JObject>> GetBooksEntries(int bookid) {
            if (_context.Comments == null) {
                return NotFound();
            }
            HttpResponseMessage response = client.GetAsync("api/books/" + bookid).Result;
            if (response.IsSuccessStatusCode) {
                Book book = response.Content.ReadFromJsonAsync<Book>().Result;
                BookComments bookComments = new BookComments(book);
                bookComments.userComments = _context.Comments.Where(x => x.BookId == books[i].id).ToList();
                return Ok(bookComments);
            }
            else {
                return BadRequest();
            }
        }*/
    }
}