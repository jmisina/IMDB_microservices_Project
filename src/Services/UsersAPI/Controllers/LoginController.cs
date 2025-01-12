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
        private readonly TokenProvider _tokenProvider;

        public LoginController(DataContext context, TokenProvider tokenProvider)
        {
            _context = context;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        public async Task<string> Login(UserLoginRequest request)
        {
            UserAuthorisation? user = await _context.UserAuthorisations.SingleOrDefaultAsync(u => u.Email == request.Email);
            

            if (user == null)
            {
                throw new Exception("User not found in database!");
            }

            var passwordHasher = new PasswordHasher();

            bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!verified)
            {
                throw new Exception("Incorrect password");
            }

            User? userIdentity = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.UserId);
            string token = _tokenProvider.Create(userIdentity);


            return token;


        }
    }
}
