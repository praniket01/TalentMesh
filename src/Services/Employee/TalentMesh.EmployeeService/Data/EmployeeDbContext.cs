using Microsoft.EntityFrameworkCore;
using TalentMesh.EmployeeService.Models;

namespace TalentMesh.EmployeeService.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Skill> Skills => Set<Skill>();

        public DbSet<EmployeeSkill> EmployeeSkills => Set<EmployeeSkill>();

        protected override void OnModelCreating(
        ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(x => x.Employee)
                .WithMany(x => x.Skills)
                .HasForeignKey(x => x.EmployeeId);

            modelBuilder.Entity<EmployeeSkill>()
                .HasOne(x => x.Skill)
                .WithMany(x => x.EmployeeSkills)
                .HasForeignKey(x => x.SkillId);

            modelBuilder.Entity<Skill>()
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
