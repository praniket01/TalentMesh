namespace Talentmesh.AllocationService.DTOs
{
    public class CreateAllocationRequest
    {
        public Guid ProjectId { get; set; }

        public Guid EmployeeId { get; set; }

        public int AllocationPercentage { get; set; }   
    }
}
