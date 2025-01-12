using System.ComponentModel.DataAnnotations;

namespace UsersAPI.DTO
{
    public class ModifyUserProfileCommand
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Phone { get; set; }

    }
}
