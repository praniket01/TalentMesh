namespace TalentMesh.EmployeeService.Models;

public class EmployeeSkill
{
    public Guid Id { get; set; }

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public Guid SkillId { get; set; }

    public Skill Skill { get; set; } = null!;

    public string Level { get; set; } = string.Empty;

    public int Score { get; set; }

    public decimal YearsOfExperience { get; set; }
}