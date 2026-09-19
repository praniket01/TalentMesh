using TalentMesh.ProjectService.DTOs;

namespace TalentMesh.ProjectService.Services
{
    public interface IProjectServices
    {
        Task<List<ProjectDto>> GetAllAsync();

        Task<ProjectDto?> GetByIdAsync(Guid id);

        Task<ProjectDto> CreateAsync(
            CreateProjectRequestDto request,
            Guid userId);
    }
}
