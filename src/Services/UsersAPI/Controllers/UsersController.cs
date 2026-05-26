using Microsoft.AspNetCore.Mvc;
using UsersAPI.Data;
using UsersAPI.Models;
using UsersAPI.DTO;
using UsersAPI.Security;
using Microsoft.AspNetCore.Authorization;
using Mapster;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UsersAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: /users
        [HttpGet]
        public async Task<List<GetUserResult>> GetUsers()
        {
            var result = await (from u in _context.Users
                                join ua in _context.UserAuthorisations on u.Id equals ua.UserId
                                join up in _context.UserProfiles on u.Id equals up.UserId
                                select new GetUserResult
                                {
                                    Id = u.Id,
                                    Username = u.Username,
                                    FirstName = up.FirstName,
                                    LastName = up.LastName,
                                    Phone = up.Phone,
                                    Email = ua.Email,
                                    Role = ua.Role
                                })
                            .ToListAsync(); ;


            return result;
        }

        // GET: /users/5
        [HttpGet("{id}")]
        public async Task<GetUserResult> GetUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var userAuth = await _context.UserAuthorisations.FindAsync(id);
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (user == null | userAuth == null | userProfile == null)
            {
                throw new Exception("User not found!");
            }

            var response = new GetUserResult()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Phone = userProfile.Phone,
                Email = userAuth.Email,
                Role = userAuth.Role
            };


            return response;
        }

        // PUT: /users/5
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, ChangePasswordCommand command)
        {
            UserAuthorisation? userAuth = await _context.UserAuthorisations.SingleOrDefaultAsync(u => u.Email == command.Email);

            if (userAuth == null)
            {
                throw new Exception("User not found in database!");
            }

            var passwordHasher = new PasswordHasher();

            bool verified = passwordHasher.Verify(command.OldPassword, userAuth.PasswordHash);

            if (!verified)
            {
                throw new Exception("Incorrect password");
            }

            var currentDateTime = DateTime.UtcNow;
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {

                if (userAuth != null)
                {
                    userAuth.PasswordHash = passwordHasher.Hash(command.NewPassword);

                    user.UpdatedAt = currentDateTime;

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return NotFound(new { Message = "User not found." });
                }
            }
            else
            {
                return NotFound(new { Message = "User not found." });
            }
        }

        // POST: /users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateUserResult>> PostUser(CreateUserRequest userData)
        {
            var emailExists = await _context.UserAuthorisations.AnyAsync(ua => ua.Email == userData.Email);
            if (emailExists)
            {
                return BadRequest(new { Message = "Email address already in use." });
            }

            var usernameExists = await _context.Users.AnyAsync(u => u.Username == userData.Username);
            if (usernameExists)
            {
                return BadRequest(new { Message = "Username already in use." });
            }

            var passwordHasher = new PasswordHasher();
            var passwordHash = passwordHasher.Hash(userData.PasswordRaw);
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL createuser({userData.Username},{passwordHash},{userData.Email},{userData.FirstName},{userData.LastName})");
            await _context.SaveChangesAsync();

            return new CreateUserResult() { Result = "success" };
        }

        // DELETE: /users/5
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($"CALL deleteuser({id})");
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: /users/5/role
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}/role")]
        public async Task<IActionResult> PatchUserRole(int id, UpdateUserRoleCommand command)
        {
            var userAuth = await _context.UserAuthorisations.FirstOrDefaultAsync(ua => ua.UserId == id);

            if (userAuth == null)
            {
                return NotFound(new { Message = "User authorisation not found." });
            }

            userAuth.Role = command.Role.ToUpper(); // Ensure roles are stored in uppercase

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
