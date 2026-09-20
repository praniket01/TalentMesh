namespace TalentMesh.ProjectService.DTOs
{
    public class ProjectskillRequirementsRequestDto
    {
        public string SkillName { get; set; } = string.Empty;

        public string RequiredLevel { get; set; } = string.Empty;

        public int RequiredCount { get; set; }
    }
}
