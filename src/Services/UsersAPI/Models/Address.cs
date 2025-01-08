using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    [Table("Addresses")]
    public class Address
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [Required, StringLength(20)]
        public string AddressType { get; set; }

        [Required, StringLength(255)]
        public string AddressLine { get; set; }

        [Required, StringLength(50)]
        public string City { get; set; }

        [Required, StringLength(20)]
        public string PostalCode { get; set; }

        [Required, StringLength(50)]
        public string Country { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
