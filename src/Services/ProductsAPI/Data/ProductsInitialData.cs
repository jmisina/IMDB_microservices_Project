using Marten.Schema;

namespace ProductsAPI.ProductsInitialData
{
    public class ProductsInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
                return;

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
        {
            new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Name = "Cegła gliniana",
                    Description = "Cegła gliniana 250x120x65",
                    Stock = 10000,
                    Price = 15.00M,
                    Category = new List<string> { "Materiały", "Cegły" }
                },
                new Product()
                {
                    Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                    Name = "Pustak betonowy",
                    Description = "Pustak betonowy o wysokości 238 mm",
                    Stock = 150000,
                    Price = 35.00M,
                    Category = new List<string> { "Materiały", "Pustaki" }
                },
                new Product()
                {
                    Id = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
                    Name = "Pustak ceramiczny",
                    Description = "Pustak ceramiczny o wysokości 220 mm",
                    Stock = 125000,
                    Price = 30.00M,
                    Category = new List<string> { "Materiały", "Pustaki" }
                },
                new Product()
                {
                    Id = new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
                    Name = "Cement worek 20kg",
                    Description = "Cement budowalny 32,5 20kg ",
                    Stock = 100,
                    Price = 15.00M,
                    Category = new List<string> { "Materiały", "Cement" }
                },
                new Product()
                {
                    Id = new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
                    Name = "Cement worek 25kg",
                    Description = "Cement montażowy 25 kg",
                    Stock = 100,
                    Price = 65.00M,
                    Category = new List<string> { "Materiały", "Cement" }
                },
                new Product()
                {
                    Id = new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
                    Name = "Zestaw szpachli",
                    Description = "Zestaw szpachli nierdzewnych 3 szt.",
                    Stock = 100,
                    Price = 100.00M,
                    Category = new List<string> { "Narzędzia", "Szpachle", "Zestawy" }
                },
                new Product()
                {
                    Id = new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
                    Name = "Wiertarka udarowa Bosch ",
                    Description = "Wiertarka udarowa Bosch Professional GSB600 to narzędzie, które łączy w sobie wytrzymałość i wydajność. Jej silnik o mocy 600 W gwarantuje większą efektywność pracy. ",
                    Stock = 30,
                    Price = 300.00M,
                    Category = new List<string> { "Narzędzia", "Elektronarzędzia", "Wiertarki", "Wiertarki udarowe" }
                }
        };
    }
}
