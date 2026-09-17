using TalentMesh.EmployeeService.Models;

namespace TalentMesh.EmployeeService.Data
{
    public class DbSeeder
    {
        public static async Task SeedAsync(EmployeeDbContext context)
        {
            if (context.Employees.Any())
                return;

            var dotnet = new Skill
            {
                Id = Guid.NewGuid(),
                Name = ".NET",
                Category = "Backend"
            };

            var angular = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Angular",
                Category = "Frontend"
            };

            var react = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "React",
                Category = "Frontend"
            };

            var postgres = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "PostgreSQL",
                Category = "Database"
            };

            var azure = new Skill
            {
                Id = Guid.NewGuid(),
                Name = "Azure",
                Category = "Cloud"
            };


            context.Skills.AddRange(
                dotnet,
                angular,
                react,
                postgres,
                azure);

            var rahul = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Rahul Sharma",
                Email = "rahul.sharma@talentmesh.com",
                Department = "Engineering",
                Designation = "Senior .NET Developer",
                ExperienceYears = 5,
                Location = "Mumbai",
                AllocationPercentage = 40
            };

            var sneha = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Sneha Joshi",
                Email = "sneha.joshi@talentmesh.com",
                Department = "Engineering",
                Designation = "Angular Developer",
                ExperienceYears = 3,
                Location = "Pune",
                AllocationPercentage = 70
            };


            var amit = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Amit Verma",
                Email = "amit.verma@talentmesh.com",
                Department = "Engineering",
                Designation = "Full Stack Developer",
                ExperienceYears = 6,
                Location = "Bangalore",
                AllocationPercentage = 20
            };

            context.Employees.AddRange(
           rahul,
           sneha,
           amit);

            context.EmployeeSkills.AddRange(

            new EmployeeSkill
            {
                Id = Guid.NewGuid(),
                EmployeeId = rahul.Id,
                SkillId = dotnet.Id,
                Level = "Advanced",
                Score = 90,
                YearsOfExperience = 5
            },

            new EmployeeSkill
            {
                Id = Guid.NewGuid(),
                EmployeeId = rahul.Id,
                SkillId = azure.Id,
                Level = "Intermediate",
                Score = 70,
                YearsOfExperience = 2
            },

            new EmployeeSkill
            {
                Id = Guid.NewGuid(),
                EmployeeId = sneha.Id,
                SkillId = angular.Id,
                Level = "Advanced",
                Score = 88,
                YearsOfExperience = 3
            },

            new EmployeeSkill
            {
                Id = Guid.NewGuid(),
                EmployeeId = amit.Id,
                SkillId = dotnet.Id,
                Level = "Advanced",
                Score = 92,
                YearsOfExperience = 6
            },

            new EmployeeSkill
            {
                Id = Guid.NewGuid(),
                EmployeeId = amit.Id,
                SkillId = react.Id,
                Level = "Advanced",
                Score = 85,
                YearsOfExperience = 4
            },

            new EmployeeSkill
            {
                Id = Guid.NewGuid(),
                EmployeeId = amit.Id,
                SkillId = postgres.Id,
                Level = "Intermediate",
                Score = 75,
                YearsOfExperience = 3
            }
        );

            await context.SaveChangesAsync();
        }
    }
}
