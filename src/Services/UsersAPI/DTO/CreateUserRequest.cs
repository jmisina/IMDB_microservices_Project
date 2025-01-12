using System.ComponentModel.DataAnnotations;

namespace UsersAPI.DTO
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string PasswordRaw { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
