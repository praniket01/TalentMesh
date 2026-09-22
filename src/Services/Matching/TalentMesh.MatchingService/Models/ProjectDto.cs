namespace TalentMesh.MatchingService.Models;

public class ProjectDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ClientName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<ProjectSkillRequirementDto>
        SkillRequirements
    { get; set; } = [];
}