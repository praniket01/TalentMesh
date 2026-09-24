using System.ComponentModel.DataAnnotations.Schema;
using TalentMesh.EmployeeService.Models;

namespace TalentMesh.EmployeeService.DTOs
{
    public class EmployeeSkillDto
    {
        public Guid Id { get; set; }
        public Skill Skill { get; set; } = null!;

        public string Level { get; set; } = string.Empty;

        public int Score { get; set; }

        public decimal YearsOfExperience { get; set; }
    }
}
