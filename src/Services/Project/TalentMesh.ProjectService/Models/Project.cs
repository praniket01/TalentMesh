namespace TalentMesh.ProjectService.Models
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ClientName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Planning";

        public Guid CreatedBy { get; set; }

        public ICollection<ProjectSkillRequirement> SkillRequirements { get; set; }
            = new List<ProjectSkillRequirement>();
    }
}
