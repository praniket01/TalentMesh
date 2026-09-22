namespace TalentMesh.MatchingService.Models;

public class EmployeeSkillDto
{
    public Guid Id { get; set; }

    public Guid SkillId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public string Level { get; set; } = string.Empty;

    public int Score { get; set; }

    public decimal YearsOfExperience { get; set; }
}