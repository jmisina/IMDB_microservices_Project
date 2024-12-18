namespace ProductsAPI.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public int Stock { get; set; }
        public List<string> Category { get; set; } = new();
    }
}
