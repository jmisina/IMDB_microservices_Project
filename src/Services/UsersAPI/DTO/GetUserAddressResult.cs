using System.ComponentModel.DataAnnotations;

namespace UsersAPI.DTO
{
    public class GetUserAddressResult
    {
        public int UserId { get; set; }

        public string? AddressType { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
