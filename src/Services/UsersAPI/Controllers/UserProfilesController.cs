using Microsoft.AspNetCore.Mvc;
using UsersAPI.Data;
using UsersAPI.DTO;
using UsersAPI.Models;

namespace UsersAPI.Controllers
{
    [Route("profiles")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly DataContext _context;

        public UserProfilesController(DataContext context)
        {
            _context = context;
        }

        // GET: userprofiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        // GET: userprofiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return userProfile;
        }

        // PUT: userprofiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(int id, ModifyUserProfileCommand command)
        {
            var currentDateTime = DateTime.UtcNow;
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(a => a.UserId == id);

                if (userProfile != null)
                {
                    userProfile.FirstName = command.FirstName;
                    userProfile.LastName = command.LastName;
                    userProfile.Phone = command.Phone;
                    user.UpdatedAt = currentDateTime;

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return NotFound(new { Message = "Profile not found." });
                }
            }
            else
            {
                return NotFound(new { Message = "User not found." });
            }
        }
    }
}
