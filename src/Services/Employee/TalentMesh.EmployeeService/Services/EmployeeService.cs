using Microsoft.EntityFrameworkCore;
using TalentMesh.EmployeeService.Data;

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
            return _context.Employees.Include(x => x.Skills).ThenInclude(x => x.Skill).Select(
                x =>new EmployeeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Designation = x.Designation,
                    ExperienceYears = x.ExperienceYears,
                    Location = x.Location,
                    AllocationPercentage = x.AllocationPercentage,
                    Skills = x.Skills.Select(s => s.Skill.Name).ToList()
                })
                .ToListAsync();
        }
    }
}
