namespace TalentMesh.EmployeeService.Models;

public class Employee
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    public decimal ExperienceYears { get; set; }

    public string Location { get; set; } = string.Empty;

    public int AllocationPercentage { get; set; }

    public ICollection<EmployeeSkill> Skills { get; set; }
        = new List<EmployeeSkill>();
}