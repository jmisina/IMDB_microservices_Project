using Microsoft.AspNetCore.Mvc;
using UsersAPI.Data;
using UsersAPI.DTO;
using UsersAPI.Models;
using UsersAPI.Security;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth;


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

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                throw new Exception("This account uses external authentication. Please use Google Login.");
            }

            var passwordHasher = new PasswordHasher();

            bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!verified)
            {
                throw new Exception("Incorrect password");
            }

            User? userIdentity = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.UserId);
            string token = _tokenProvider.Create(userIdentity, user.Role);


            return token;
        }

        [HttpPost("google")]
        public async Task<string> GoogleLogin(GoogleLoginRequest request)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            }
            catch (InvalidJwtException)
            {
                throw new Exception("Invalid Google Token");
            }

            var userAuth = await _context.UserAuthorisations.SingleOrDefaultAsync(u => u.Email == payload.Email);

            if (userAuth == null)
            {
                // Create new user if they don't exist
                string username = payload.Email.Split('@')[0];
                
                // Ensure username is unique
                int counter = 1;
                string baseUsername = username;
                while (await _context.Users.AnyAsync(u => u.Username == username))
                {
                    username = $"{baseUsername}{counter++}";
                }

                // Call the existing stored procedure to create the user
                // Pass null for password as this is a Google login
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL createuser({username}, {null}, {payload.Email}, {payload.GivenName ?? ""}, {payload.FamilyName ?? ""})");
                
                // Re-fetch to get the newly created user's data
                userAuth = await _context.UserAuthorisations.SingleOrDefaultAsync(u => u.Email == payload.Email);
            }

            if (userAuth == null)
            {
                throw new Exception("Failed to create or find user.");
            }

            User? userIdentity = await _context.Users.SingleOrDefaultAsync(u => u.Id == userAuth.UserId);
            
            if (userIdentity == null)
            {
                throw new Exception("User identity not found.");
            }

            string token = _tokenProvider.Create(userIdentity, userAuth.Role);

            return token;
        }
    }
}
