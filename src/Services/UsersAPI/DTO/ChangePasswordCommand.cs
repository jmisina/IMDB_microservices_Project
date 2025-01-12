using System.ComponentModel.DataAnnotations;

namespace UsersAPI.DTO
{
    public class ChangePasswordCommand
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}
