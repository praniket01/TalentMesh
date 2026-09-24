namespace TalentMesh.MatchingService.Models;

public class EmployeeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;

    public decimal ExperienceYears { get; set; }

    public string Location { get; set; } = string.Empty;

    public int AllocationPercentage { get; set; }

    public List<EmployeeSkillDto> Skills { get; set; } = [];

}