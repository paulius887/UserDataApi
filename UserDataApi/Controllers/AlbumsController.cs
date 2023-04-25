using Microsoft.AspNetCore.Mvc;
using UserDataApi.Data;
using UserDataApi.Models;

namespace UserDataApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase {
        private readonly UserContext _context;
        private HttpClient client;

        public AlbumsController(UserContext context) {
            _context = context;
            client = new HttpClient();
            client.BaseAddress = new Uri("http://host.docker.internal:5001");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: api/Albums
        [HttpGet]
        public async Task<ActionResult> GetAlbums() {
            HttpResponseMessage response = client.GetAsync("albums").Result;
            if (response.IsSuccessStatusCode) {
                return Ok(response.Content.ReadFromJsonAsync<List<Album>>().Result);
            }
            else {
                return BadRequest();
            }
        }
    }
}
