using Microsoft.EntityFrameworkCore;
using AgriTrust.API.Models;

namespace AgriTrust.API.Data
{
    public class AgriTrustDbContext : DbContext
    {
        public AgriTrustDbContext(DbContextOptions<AgriTrustDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
    }
}
