using Microsoft.EntityFrameworkCore;
using TalentMesh.ProjectService.Data;
using TalentMesh.ProjectService.DTOs;
using TalentMesh.ProjectService.Models;

namespace TalentMesh.ProjectService.Services.Impl
{
    public class ProjectServices : IProjectServices
    {
        private readonly ProjectDbContext _context;
        public ProjectServices(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<ProjectDto> CreateAsync(CreateProjectRequestDto request, Guid userId)
        {
            var project = new Project
            {

                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                ClientName = request.ClientName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = "Planning",
                CreatedBy = userId
            };

            foreach(var requirement in request.skillRequirementss)
            {
                project.skillRequirementss.Add(new ProjectskillRequirements
                {
                    Id = Guid.NewGuid(),
                    SkillName = requirement.SkillName,
                    RequiredLevel = requirement.RequiredLevel,
                    RequiredCount = requirement.RequiredCount
                });
            }

            _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return (await GetByIdAsync(project.Id));
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            return await _context.Projects
             .Include(x => x.skillRequirementss)
             .Select(x => new ProjectDto
             {
                 Id = x.Id,
                 Name = x.Name,
                 Description = x.Description,
                 ClientName = x.ClientName,
                 StartDate = x.StartDate,
                 EndDate = x.EndDate,
                 Status = x.Status,

                 skillRequirementss = x.skillRequirementss
                     .Select(skill => new ProjectskillRequirementsDto
                     {
                         Id = skill.Id,
                         SkillName = skill.SkillName,
                         RequiredLevel = skill.RequiredLevel,
                         RequiredCount = skill.RequiredCount
                     })
                     .ToList()
             })
             .ToListAsync();
        }

        public async Task<ProjectDto?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
            .Include(x => x.skillRequirementss)
            .Where(x => x.Id == id)
            .Select(x => new ProjectDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ClientName = x.ClientName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status,

                skillRequirementss = x.skillRequirementss
                    .Select(skill => new ProjectskillRequirementsDto
                    {
                        Id = skill.Id,
                        SkillName = skill.SkillName,
                        RequiredLevel = skill.RequiredLevel,
                        RequiredCount = skill.RequiredCount
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
        }
    }
}
