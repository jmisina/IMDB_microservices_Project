namespace UsersAPI.DTO
{
    public class ModifyAddressCommand
    {
        public string? AddressType { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
