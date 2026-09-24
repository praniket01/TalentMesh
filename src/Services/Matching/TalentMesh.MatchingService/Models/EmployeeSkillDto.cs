namespace TalentMesh.MatchingService.Models;

public class EmployeeSkillDto
{
    public Guid Id { get; set; }

    public SkillDto Skill { get; set; } = null!;

    public string Level { get; set; } = string.Empty;

    public int Score { get; set; }

    public decimal YearsOfExperience { get; set; }
}