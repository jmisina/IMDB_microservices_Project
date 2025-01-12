using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    [Table("Addresses")]
    public class Address
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [StringLength(20)]
        public string? AddressType { get; set; }

        [StringLength(255)]
        public string? AddressLine { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
