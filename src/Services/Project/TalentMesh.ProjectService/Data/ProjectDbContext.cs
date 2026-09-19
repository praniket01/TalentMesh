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

        public DbSet<ProjectSkillRequirement> projectSkillRequirements => Set<ProjectSkillRequirement>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectSkillRequirement>()
                .HasOne(x => x.Project)
                .WithMany(x => x.SkillRequirements)
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<Project>()
                .Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Project>()
          .Property(x => x.ClientName)
          .HasMaxLength(200);

            modelBuilder.Entity<ProjectSkillRequirement>()
                .Property(x => x.SkillName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<ProjectSkillRequirement>()
                .Property(x => x.RequiredLevel)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
