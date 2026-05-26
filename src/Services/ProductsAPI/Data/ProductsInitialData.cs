using Marten.Schema;
using ProductsAPI.Models;
using Marten;

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
                },
                // New 40 construction products
                new Product { Id = Guid.NewGuid(), Name = "Młot udarowy DeWalt", Description = "Młot udarowy SDS-Max 1050W", Stock = 15, Price = 1250.00M, Category = new List<string> { "Narzędzia", "Elektronarzędzia", "Młoty" } },
                new Product { Id = Guid.NewGuid(), Name = "Szlifierka kątowa Makita", Description = "Szlifierka kątowa 125mm 840W", Stock = 40, Price = 280.00M, Category = new List<string> { "Narzędzia", "Elektronarzędzia", "Szlifierki" } },
                new Product { Id = Guid.NewGuid(), Name = "Wkrętarka akumulatorowa Milwaukee", Description = "Wkrętarka M18 FUEL 135Nm", Stock = 25, Price = 890.00M, Category = new List<string> { "Narzędzia", "Elektronarzędzia", "Wkrętarki" } },
                new Product { Id = Guid.NewGuid(), Name = "Poziomica aluminiowa 120cm", Description = "Poziomica profesjonalna PRO 3 libelle", Stock = 50, Price = 145.00M, Category = new List<string> { "Narzędzia", "Pomiarowe" } },
                new Product { Id = Guid.NewGuid(), Name = "Dalmierz laserowy Bosch", Description = "Dalmierz laserowy GLM 50-27 C do 50m", Stock = 20, Price = 550.00M, Category = new List<string> { "Narzędzia", "Pomiarowe" } },
                new Product { Id = Guid.NewGuid(), Name = "Betoniarka wolnospadowa 160L", Description = "Betoniarka elektryczna 230V 800W", Stock = 5, Price = 1800.00M, Category = new List<string> { "Maszyny", "Budowlane" } },
                new Product { Id = Guid.NewGuid(), Name = "Agregat prądotwórczy Honda", Description = "Agregat jednofazowy 3.0kW", Stock = 8, Price = 3200.00M, Category = new List<string> { "Maszyny", "Zasilanie" } },
                new Product { Id = Guid.NewGuid(), Name = "Płyta gipsowo-kartonowa", Description = "Płyta G-K zwykła 1200x2600x12.5mm", Stock = 200, Price = 45.00M, Category = new List<string> { "Materiały", "Płyty" } },
                new Product { Id = Guid.NewGuid(), Name = "Profile CD60 3m", Description = "Profil sufitowy do płyt G-K", Stock = 300, Price = 22.00M, Category = new List<string> { "Materiały", "Profile" } },
                new Product { Id = Guid.NewGuid(), Name = "Wełna mineralna ISOVER 15cm", Description = "Wełna szklana do ocieplenia poddaszy", Stock = 80, Price = 185.00M, Category = new List<string> { "Materiały", "Izolacja" } },
                new Product { Id = Guid.NewGuid(), Name = "Styropian fasadowy EPS 70 10cm", Description = "Styropian grafitowy do elewacji", Stock = 100, Price = 210.00M, Category = new List<string> { "Materiały", "Izolacja" } },
                new Product { Id = Guid.NewGuid(), Name = "Pianka montażowa niskoprężna", Description = "Pianka poliuretanowa pistoletowa 750ml", Stock = 150, Price = 32.00M, Category = new List<string> { "Chemia", "Pianki" } },
                new Product { Id = Guid.NewGuid(), Name = "Silikon sanitarny biały", Description = "Silikon odporny na pleśń 300ml", Stock = 120, Price = 24.00M, Category = new List<string> { "Chemia", "Uszczelniacze" } },
                new Product { Id = Guid.NewGuid(), Name = "Klej do płytek C2TE", Description = "Klej elastyczny do gresu 25kg", Stock = 90, Price = 55.00M, Category = new List<string> { "Chemia", "Kleje" } },
                new Product { Id = Guid.NewGuid(), Name = "Zaprawa tynkarska maszynowa", Description = "Tynk gipsowy lekki 30kg", Stock = 110, Price = 38.00M, Category = new List<string> { "Chemia", "Zaprawy" } },
                new Product { Id = Guid.NewGuid(), Name = "Fuga epoksydowa 2kg", Description = "Fuga dwuskładnikowa do łazienek", Stock = 60, Price = 115.00M, Category = new List<string> { "Chemia", "Fugi" } },
                new Product { Id = Guid.NewGuid(), Name = "Grunt głęboko penetrujący 5L", Description = "Grunt pod tynki i farby", Stock = 70, Price = 42.00M, Category = new List<string> { "Chemia", "Grunty" } },
                new Product { Id = Guid.NewGuid(), Name = "Pręt żebrowany FI 12 6m", Description = "Stal zbrojeniowa do fundamentów", Stock = 500, Price = 28.00M, Category = new List<string> { "Materiały", "Stal" } },
                new Product { Id = Guid.NewGuid(), Name = "Siatka zbrojeniowa 2x3m", Description = "Siatka do wylewek oczko 10x10cm", Stock = 140, Price = 65.00M, Category = new List<string> { "Materiały", "Stal" } },
                new Product { Id = Guid.NewGuid(), Name = "Rura kanalizacyjna PCV 110 2m", Description = "Rura do kanalizacji zewnętrznej", Stock = 100, Price = 48.00M, Category = new List<string> { "Instalacje", "Kanalizacja" } },
                new Product { Id = Guid.NewGuid(), Name = "Kolano PCV 110/90 stopni", Description = "Złączka do rur kanalizacyjnych", Stock = 200, Price = 12.00M, Category = new List<string> { "Instalacje", "Kanalizacja" } },
                new Product { Id = Guid.NewGuid(), Name = "Kabel YDYp 3x2.5 100m", Description = "Przewód podtynkowy do gniazdek", Stock = 30, Price = 450.00M, Category = new List<string> { "Instalacje", "Elektryka" } },
                new Product { Id = Guid.NewGuid(), Name = "Rozdzielnica modułowa 2x12", Description = "Skrzynka bezpiecznikowa natynkowa", Stock = 15, Price = 140.00M, Category = new List<string> { "Instalacje", "Elektryka" } },
                new Product { Id = Guid.NewGuid(), Name = "Puszka podtynkowa FI 60", Description = "Puszka do osprzętu elektrycznego", Stock = 1000, Price = 1.20M, Category = new List<string> { "Instalacje", "Elektryka" } },
                new Product { Id = Guid.NewGuid(), Name = "Młotek ślusarski 500g", Description = "Młotek z rękojeścią z włókna szklanego", Stock = 60, Price = 35.00M, Category = new List<string> { "Narzędzia", "Ręczne" } },
                new Product { Id = Guid.NewGuid(), Name = "Klucz nastawny 250mm", Description = "Klucz typu szwed o szerokim rozstawie", Stock = 45, Price = 68.00M, Category = new List<string> { "Narzędzia", "Ręczne" } },
                new Product { Id = Guid.NewGuid(), Name = "Zestaw wkrętaków 6 szt.", Description = "Wkrętaki izolowane do 1000V", Stock = 55, Price = 85.00M, Category = new List<string> { "Narzędzia", "Ręczne" } },
                new Product { Id = Guid.NewGuid(), Name = "Kielnia murarska trójkątna", Description = "Kielnia ze stali nierdzewnej 180mm", Stock = 70, Price = 42.00M, Category = new List<string> { "Narzędzia", "Murarskie" } },
                new Product { Id = Guid.NewGuid(), Name = "Paca zębata 10mm", Description = "Paca do nakładania kleju", Stock = 80, Price = 28.00M, Category = new List<string> { "Narzędzia", "Murarskie" } },
                new Product { Id = Guid.NewGuid(), Name = "Wiadro budowlane 12L", Description = "Wiadro z elastycznego tworzywa", Stock = 300, Price = 8.50M, Category = new List<string> { "Narzędzia", "Akcesoria" } },
                new Product { Id = Guid.NewGuid(), Name = "Kastel budowlany 90L", Description = "Pojemnik prostokątny do zapraw", Stock = 40, Price = 55.00M, Category = new List<string> { "Narzędzia", "Akcesoria" } },
                new Product { Id = Guid.NewGuid(), Name = "Rękawice robocze powlekane", Description = "Rękawice ochronne nitrylowe", Stock = 500, Price = 4.50M, Category = new List<string> { "BHP", "Ochrona rąk" } },
                new Product { Id = Guid.NewGuid(), Name = "Kask ochronny żółty", Description = "Hełm budowlany z regulacją", Stock = 100, Price = 25.00M, Category = new List<string> { "BHP", "Ochrona głowy" } },
                new Product { Id = Guid.NewGuid(), Name = "Okulary ochronne przeciwodpryskowe", Description = "Okulary z poliwęglanu", Stock = 150, Price = 18.00M, Category = new List<string> { "BHP", "Ochrona oczu" } },
                new Product { Id = Guid.NewGuid(), Name = "Maska przeciwpyłowa FFP2", Description = "Półmaska filtrująca 10 szt.", Stock = 200, Price = 45.00M, Category = new List<string> { "BHP", "Ochrona dróg oddechowych" } },
                new Product { Id = Guid.NewGuid(), Name = "Drabina aluminiowa 3x7", Description = "Drabina wielofunkcyjna wolnostojąca", Stock = 12, Price = 480.00M, Category = new List<string> { "Sprzęt", "Wysokościowe" } },
                new Product { Id = Guid.NewGuid(), Name = "Taczka budowlana 85L", Description = "Taczka tłoczona, koło pneumatyczne", Stock = 20, Price = 260.00M, Category = new List<string> { "Sprzęt", "Transport" } },
                new Product { Id = Guid.NewGuid(), Name = "Folia fundamentowa kubełkowa", Description = "Folia HDPE 0.5x20m", Stock = 60, Price = 110.00M, Category = new List<string> { "Materiały", "Hydroizolacja" } },
                new Product { Id = Guid.NewGuid(), Name = "Papa termozgrzewalna 5.2mm", Description = "Papa podkładowa na włókninie poliestrowej", Stock = 50, Price = 240.00M, Category = new List<string> { "Materiały", "Dachowe" } },
                new Product { Id = Guid.NewGuid(), Name = "Wkręty do drewna 4x50 500szt.", Description = "Wkręty fosfatowane do płyt G-K", Stock = 100, Price = 35.00M, Category = new List<string> { "Materiały", "Zamocowania" } },
                new Product { Id = Guid.NewGuid(), Name = "Kołki rozporowe 8x40 100szt.", Description = "Kołki uniwersalne z wkrętem", Stock = 150, Price = 22.00M, Category = new List<string> { "Materiały", "Zamocowania" } }
        };
    }
}
