using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UsersAPI.Models
{
    [Table("Addresses")]
    public class Address
    {
        [Key, ForeignKey("User")]
        [Column("UserId", Order = 1)]
        public int UserId { get; set; }

        [Column("AddressType", Order = 2)]
        [Required, StringLength(20)]
        public string AddressType { get; set; }

        [Required, StringLength(255)]
        [Column("AddressLine")]
        public string AddressLine { get; set; }

        [Required, StringLength(50)]
        [Column("City")]
        public string City { get; set; }

        [Required, StringLength(20)]
        [Column("PostalCode")]
        public string PostalCode { get; set; }

        [Required, StringLength(50)]
        [Column("Country")]
        public string Country { get; set; }

        // Navigation property
        public User User { get; set; }
    }
}
