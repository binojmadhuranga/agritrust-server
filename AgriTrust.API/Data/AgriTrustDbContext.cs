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
        public DbSet<Certificate> Certificates => Set<Certificate>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CertificateRequest>()
                .Property(c => c.Status)
                .HasConversion<string>();
            
            // One-to-One Relationship: User (Farmer) <-> Certificate
            modelBuilder.Entity<User>()
                .HasOne(u => u.Certificate)
                .WithOne(c => c.Farmer)
                .HasForeignKey<Certificate>(c => c.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Farmer can have only one certificate
            modelBuilder.Entity<Certificate>()
                .HasIndex(c => c.FarmerId)
                .IsUnique();
        }
        
    }
}
