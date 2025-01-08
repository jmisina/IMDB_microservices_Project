using Microsoft.AspNetCore.Mvc;
using UsersAPI.Data;
using UsersAPI.DTO;
using UsersAPI.Models;
using UsersAPI.Security;
using Microsoft.EntityFrameworkCore;


namespace UsersAPI.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly DataContext _context;

        public LoginController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<UserAuthorisation> Login(UserLoginRequest request)
        {
            var passwordHasher = new PasswordHasher();
            UserAuthorisation? user = await _context.UserAuthorisations.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("User not found in database!");
            }

            bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!verified)
            {
                throw new Exception("Incorrect password");
            }

            return user;


        }
    }
}
