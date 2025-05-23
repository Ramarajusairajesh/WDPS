using Microsoft.EntityFrameworkCore;
using WDPS.Core.Models;

namespace WDPS.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SystemMetrics> SystemMetrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SystemMetrics>()
                .Property(s => s.Timestamp)
                .IsRequired();

            modelBuilder.Entity<SystemMetrics>()
                .Property(s => s.CpuUsage)
                .IsRequired();

            modelBuilder.Entity<SystemMetrics>()
                .Property(s => s.MemoryUsage)
                .IsRequired();
        }
    }
} 