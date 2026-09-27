using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using TalentMesh.EmployeeService.Data;
using TalentMesh.EmployeeService.DTOs;
using TalentMesh.EmployeeService.Models;
using TalentMesh.EmployeeService.Services;

namespace TalentMesh.EmployeeService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly EmployeeDbContext _context;

        public EmployeeController(  IEmployeeService employeeService,EmployeeDbContext context)
        {
            _employeeService = employeeService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employees = await _employeeService.GetAllAsync();

            return Ok(employees);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Skills)
                .ThenInclude(es => es.Skill)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            // map to DTO

            EmployeeDto employeeDto = new EmployeeDto
            {   
        Id = employee.Id,
        Name = employee.Name,
        Designation = employee.Designation,
        ExperienceYears = employee.ExperienceYears,
        Location = employee.Location,
        AllocationPercentage = employee.AllocationPercentage,

        Skills = employee.Skills
            .Select(es => new EmployeeSkillDto
            {
                Id = es.Id,

                Skill = new Skill
                {
                    Id = es.Skill.Id,
                    Name = es.Skill.Name,
                    Category = es.Skill.Category
                },

                Level = es.Level,
                Score = es.Score,
                YearsOfExperience = es.YearsOfExperience
                }).ToList()
            };

            return Ok(employeeDto);
        }

    }
}
