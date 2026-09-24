namespace TalentMesh.MatchingService.Models
{
    public class CandidateMatchDto
    {
        public Guid EmployeeId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Designation { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public decimal ExperienceYears { get; set; }

        public int AllocationPercentage { get; set; }

        public decimal AvailabilityPercentage { get; set; }

        public decimal SkillMatch { get; set; }

        public decimal AvailabilityScore { get; set; }

        public decimal ExperienceScore { get; set; }

        public decimal MatchScore { get; set; }
    }
}
