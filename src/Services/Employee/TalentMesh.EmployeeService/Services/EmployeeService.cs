using Microsoft.EntityFrameworkCore;
using TalentMesh.EmployeeService.Data;
using TalentMesh.EmployeeService.DTOs;

namespace TalentMesh.EmployeeService.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeDbContext _context;

        public EmployeeService(EmployeeDbContext context)
        {
            _context = context;
        }

        public Task<List<EmployeeDto>> GetAllAsync()
        {
            return _context.Employees.Select(
                x =>new EmployeeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Designation = x.Designation,
                    ExperienceYears = x.ExperienceYears,
                    Location = x.Location,
                    AllocationPercentage = x.AllocationPercentage,
                    Skills = x.Skills
                                .Select(es => new EmployeeSkillDto
                                {
                                    Id = es.SkillId,
                                   Skill = es.Skill,
                                    Level = es.Level,
                                    Score = es.Score,
                                    YearsOfExperience = es.YearsOfExperience,
                                })
                .ToList(),
                })
                .ToListAsync();
        }
    }
}
