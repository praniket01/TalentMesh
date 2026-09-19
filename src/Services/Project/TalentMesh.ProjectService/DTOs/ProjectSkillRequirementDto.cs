namespace TalentMesh.ProjectService.DTOs
{
    public class ProjectSkillRequirementDto
    {
        public Guid Id { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string RequiredLevel { get; set; } = string.Empty;

        public int RequiredCount { get; set; }
    }
}
