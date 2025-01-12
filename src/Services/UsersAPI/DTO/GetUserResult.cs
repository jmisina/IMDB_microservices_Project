using System.ComponentModel.DataAnnotations;

namespace UsersAPI.DTO
{
    public class GetUserResult
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }

    }
}
