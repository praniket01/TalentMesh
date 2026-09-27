using Microsoft.EntityFrameworkCore;
using Talentmesh.AllocationService.Models;

namespace Talentmesh.AllocationService.Data
{
    public class AllocationDbContext : DbContext
    {
        public AllocationDbContext(DbContextOptions<AllocationDbContext> options) :base(options)
        {
            
        }

        public DbSet<Allocation> Allocations => Set<Allocation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Allocation>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Status).HasMaxLength(50).IsRequired();

                entity.Property(x => x.AllocationPercentage).IsRequired();

                entity.Property(x => x.RequestedAt).IsRequired();

                entity.Property(x => x.RejectionReason).IsRequired();
            });
        }
    }
}
