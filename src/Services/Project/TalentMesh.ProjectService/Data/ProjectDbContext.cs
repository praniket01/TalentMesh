using Microsoft.EntityFrameworkCore;
using TalentMesh.ProjectService.Models;

namespace TalentMesh.ProjectService.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options):base(options)
        {
            
        }

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<ProjectskillRequirements> projectskillRequirements => Set<ProjectskillRequirements>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectskillRequirements>()
                .HasOne(x => x.Project)
                .WithMany(x => x.skillRequirements)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Project>()
                .Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Project>()
          .Property(x => x.ClientName)
          .HasMaxLength(200);

            modelBuilder.Entity<ProjectskillRequirements>()
                .Property(x => x.SkillName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<ProjectskillRequirements>()
                .Property(x => x.RequiredLevel)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
