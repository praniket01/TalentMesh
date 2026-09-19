namespace TalentMesh.ProjectService.Models
{
    public class ProjectSkillRequirement
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; } = null!;

        public string SkillName { get; set; } = string.Empty;

        public string RequiredLevel { get; set; } = string.Empty;

        public int RequiredCount { get; set; }
    }
}
