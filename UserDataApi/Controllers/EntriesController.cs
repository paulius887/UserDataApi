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
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntries() {
            if (_context.Entries == null) {
                return NotFound();
            }
            return await _context.Entries.ToListAsync();
        }
        // GET: api/Entries/5/5
        [HttpGet("{userid}/{id}")]
        public async Task<ActionResult<Entry>> GetEntry(int userid, int id) {
            if (_context.Entries == null) {
                return NotFound();
            }
            var entry = await _context.Entries.FindAsync(id, userid);
            if (entry == null) {
                return NotFound();
            }
            return entry;
        }
        // POST: api/Entries
        [HttpPost]
        public async Task<ActionResult<Entry>> PostEntry(Entry entry) {
            if (_context.Entries == null) {
                return Problem("Entity set 'UserContext.Entries' is null.");
            }
            if (_context.Users == null) {
                return Problem("Entity set 'UserContext.Users' is null.");
            }
            var user = await _context.Users.FindAsync(entry.UserId);
            if (user == null) {
                ModelState.AddModelError("UserId", "User with given UserId doesn't exist");
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
            if (await _context.Entries.FindAsync(entry.Id, entry.UserId) != null) {
                ModelState.AddModelError("Id/UserId", "Entry with given Id and UserId already exists");
            }
            if (ModelState.IsValid) {
                _context.Entries.Add(entry);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(PostEntry), new { id = entry.Id, userId = entry.UserId }, entry);
            }
            else {
                return BadRequest(new ValidationProblemDetails(this.ModelState));
            }
        }
        // PUT: api/Entries/{userid}/{entryid}
        [HttpPut("{userid}/{entryid}")]
        public async Task<ActionResult<Entry>> PutEntry(int userid, int entryid, Entry entryDto) {
            if (await _context.Users.FindAsync(userid) == null) {
                return NotFound();
            }
            var entry = await _context.Entries.FindAsync(entryid, userid);
            if (entry == null) {
                return NotFound();
            }
            entry.EntryText = entryDto.EntryText;
            entry.LastEdited = DateTime.Now;
            await _context.SaveChangesAsync();
            return entry;
        }
    }
}