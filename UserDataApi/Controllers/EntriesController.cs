using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDataApi.Data;
using UserDataApi.Models;
using UserDataApi.Validation;

namespace UserDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase {
        private readonly UserContext _context;

        public EntriesController(UserContext context) {
            _context = context;
        }

        // GET: api/Entries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entry>>> GetUsers() {
            if (_context.Entries == null) {
                return NotFound();
            }
            return await _context.Entries.ToListAsync();
        }
	}
}