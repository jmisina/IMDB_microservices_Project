using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    public class UserProfile
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }


        // Navigation property
        public User User { get; set; }
    }
}
