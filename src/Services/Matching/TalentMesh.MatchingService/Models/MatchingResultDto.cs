namespace TalentMesh.MatchingService.Models
{
    public class MatchingResultDto
    {
        public Guid ProjectId {  get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public List<CandidateMatchDto> candidates { get; set; } = [];

    }
}
