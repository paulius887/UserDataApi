using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, UserDto userDto) {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            String errorMessage;
            errorMessage = UsernameIsValid.IsValid(userDto.Username, id, _context);
            if (errorMessage == "") {
                errorMessage = EmailIsValid.IsValid(userDto.Email, id, _context);
            }
            if (errorMessage == "") {
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
                _context.Entry(user).State = EntityState.Modified;

                try {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!UserExists(id)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return user;
            }
            else {
                return BadRequest(errorMessage);
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(UserDto userDto) {
            var user = new User {
                Username = userDto.Username,
                Email = userDto.Email,
                RegisterDate = DateTime.Now
            };
            if (_context.Users == null) {
                return Problem("Entity set 'UserContext.Users'  is null.");
            }
            String errorMessage;
            errorMessage = UsernameIsValid.IsValid(userDto.Username, _context);
            if (errorMessage == "") {
                errorMessage = EmailIsValid.IsValid(userDto.Email, _context);
            }
            if (errorMessage == "") {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            else {
                return BadRequest(errorMessage);
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

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id) {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
