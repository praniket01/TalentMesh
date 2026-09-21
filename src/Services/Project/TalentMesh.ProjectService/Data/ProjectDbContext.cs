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

        public DbSet<ProjectskillRequirements> projectskillRequirementss => Set<ProjectskillRequirements>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectskillRequirements>()
                .HasOne(x => x.Project)
                .WithMany(x => x.skillRequirementss)
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
