namespace TalentMesh.ProjectService.DTOs
{
    public class CreateProjectRequestDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ClientName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<ProjectskillRequirementsRequestDto> skillRequirementss { get; set; }
            = [];
    }
}
