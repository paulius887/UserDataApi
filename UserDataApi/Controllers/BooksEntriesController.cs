using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using UserDataApi.Data;
using UserDataApi.Models;

namespace UserDataApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BooksEntriesController : ControllerBase {
        private readonly UserContext _context;
        private HttpClient client;
        public BooksEntriesController(UserContext context) {
            _context = context;
            client = new HttpClient();
            client.BaseAddress = new Uri("http://host.docker.internal:5001");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: api/BooksEntries/
        [HttpGet("")]
        public async Task<ActionResult<JObject>> GetBooksEntries() {
            if (_context.Entries == null) {
                return NotFound();
            }
            HttpResponseMessage response = client.GetAsync("api/books/").Result;
            if (response.IsSuccessStatusCode) {
                List<Book> books = response.Content.ReadFromJsonAsync<List<Book>>().Result;
                List<BookEntries> booksEntries = new List<BookEntries>();
                for (int i = 0; i < books.Count; ++i) {
                    BookEntries bookEntries = new BookEntries(books[i]);
                    bookEntries.userEntries = _context.Entries.Where(x => x.BookId == books[i].id).ToList();
                    booksEntries.Add(bookEntries);
                }
                return Ok(booksEntries);
            }
            else {
                return BadRequest();
            }
        }

        // GET: api/BooksEntries/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> GetBookEntries(int id) {
            if (_context.Entries == null) {
                return NotFound();
            }
            HttpResponseMessage response = client.GetAsync("api/books/" + id).Result;
            if (response.IsSuccessStatusCode) {
                Book book = response.Content.ReadFromJsonAsync<Book>().Result;
                book.id = id;
                BookEntries bookEntries = new BookEntries(book);
                bookEntries.userEntries = _context.Entries.Where(x => x.BookId == id).ToList();
                return Ok(bookEntries);
            }
            else {
                return BadRequest();
            }
        }
    }
}
