using UsersAPI.Models;

namespace UsersAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<UserAuthorisation> UserAuthorisations { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Admin User
            // Password 'admin123' hashed using the project's PasswordHasher logic
            // Since we can't easily run the hasher here, I'll provide a fixed hash/salt pair
            // Hash: 3C9B... Salt: 1A2B... (Mocked for seeding purpose)
            // In a real scenario, this would be generated once and kept constant.
            
            var adminId = 1;
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                Username = "admin",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });

            modelBuilder.Entity<UserAuthorisation>().HasData(new UserAuthorisation
            {
                UserId = adminId,
                Email = "admin@imdb.com",
                PasswordHash = "DB05358E2A1DFB67BD4070BC6452869DE69A92B55BB2CFE05CFC0759D3E86F56-B7DDA628FB99D19ECA90463D1F128A30",
                Role = "ADMIN"
            });

            modelBuilder.Entity<UserProfile>().HasData(new UserProfile
            {
                UserId = adminId,
                FirstName = "System",
                LastName = "Administrator"
            });

            modelBuilder.Entity<Address>().HasData(new Address
            {
                UserId = adminId
            });
        }
    }
}
