namespace TalentMesh.EmployeeService.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();
    }
}
