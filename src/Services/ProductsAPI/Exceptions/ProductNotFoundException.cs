namespace ProductsAPI.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base("Product not found in the database!")
        { 
        }
    }
}
