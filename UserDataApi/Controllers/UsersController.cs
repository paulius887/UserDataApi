using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDataApi.Data;
using UserDataApi.Models;
using UserDataApi.Validation;

namespace UserDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly UserContext _context;

        public UsersController(UserContext context) {
            _context = context;
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
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, UserDto userDto) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            String errorMessage = UsernameIsValid.IsValid(userDto.Username, _context);
            if (errorMessage != "") {
                ModelState.AddModelError("Username", errorMessage);
            }
            errorMessage = EmailIsValid.IsValid(userDto.Email, _context);
            if (errorMessage != "") {
                ModelState.AddModelError("Email", errorMessage);
            }
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
            String errorMessage = UsernameIsValid.IsValid(userDto.Username, _context);
            if (errorMessage != "") {
                ModelState.AddModelError("Username", errorMessage);
            }
            errorMessage = EmailIsValid.IsValid(userDto.Email, _context);
            if (errorMessage != "") {
                ModelState.AddModelError("Email", errorMessage);
            }
            if (ModelState.IsValid) {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
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
        public async Task<ActionResult<IEnumerable<Entry>>> GetUserEntries(int id) {
            if (_context.Entries == null) {
                return NotFound();
            }
            return await _context.Entries.Where(x => x.UserId == id).ToListAsync();
        }

        // GET: api/Users/{id}/Entries/{id}
        [HttpGet("{id}/Entries/{entryid}")]
        public async Task<ActionResult<Entry>> GetUserEntries(int id, int entryid) {
            if (_context.Entries == null) {
                return NotFound();
            }
            var entry = await _context.Entries.FindAsync(entryid, id);
            if (entry == null) {
                return NotFound();
            }
            return entry;
        }

        // POST: api/Users/{id}/Entries
        [HttpPost("{id}/Entries")]
        public async Task<ActionResult<Entry>> PostEntry(int id, EntryDto entryDto) {
            var newEntry = new Entry {
                Id = _context.Entries.Where(x => x.UserId == id).Max(x => (int?)x.Id) + 1 ?? 1,
                UserId = id,
                EntryText = entryDto.EntryText,
                LastEdited = DateTime.Now
            };
            _context.Entries.Add(newEntry);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostEntry), new { id = newEntry.Id, userId = newEntry.UserId }, newEntry);
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

        private bool UserExists(int id) {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
