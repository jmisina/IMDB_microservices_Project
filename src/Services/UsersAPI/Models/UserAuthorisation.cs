using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class UserAuthorisation
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, StringLength(255)]
        public string PasswordHash { get; set; }

        [Required, StringLength(255)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Role { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
