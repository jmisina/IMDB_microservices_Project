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
    }
}
