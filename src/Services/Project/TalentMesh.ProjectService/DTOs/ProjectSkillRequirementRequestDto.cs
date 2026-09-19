namespace TalentMesh.ProjectService.DTOs
{
    public class ProjectSkillRequirementRequestDto
    {
        public string SkillName { get; set; } = string.Empty;

        public string RequiredLevel { get; set; } = string.Empty;

        public int RequiredCount { get; set; }
    }
}
