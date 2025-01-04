using System.ComponentModel.DataAnnotations;
using System.Net;

namespace UsersAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public UserAuthorisation UserAuthorisation { get; set; }
        public UserProfile UserProfile { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
