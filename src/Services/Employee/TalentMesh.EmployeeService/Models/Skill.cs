namespace TalentMesh.EmployeeService.Models;

public class Skill
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public ICollection<EmployeeSkill> EmployeeSkills { get; set; }
        = new List<EmployeeSkill>();
}