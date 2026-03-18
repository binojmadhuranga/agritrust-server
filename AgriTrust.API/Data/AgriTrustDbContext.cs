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
        public DbSet<CertificateRequest> CertificateRequests { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CertificateRequest>()
                .Property(c => c.Status)
                .HasConversion<string>();
        }
        
    }
}
