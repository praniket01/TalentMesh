namespace Talentmesh.AllocationService.DTOs
{
    public class AllocationDto
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid EmployeeId { get; set; }

        public int AllocationPercentage { get; set; }

        public string Status { get; set; } = string.Empty;

        public Guid RequestedBy { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTime RequestedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public string? RejectionReason { get; set; }
    }
}
