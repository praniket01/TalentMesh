using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TalentMesh.ProjectService.DTOs;
using TalentMesh.ProjectService.Services;
using TalentMesh.ProjectService.Services.Impl;

namespace TalentMesh.ProjectService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectService;

        public ProjectController(
             IProjectServices projectServices
            )
        {
         _projectService = projectServices;   
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects =
                await _projectService.GetAllAsync();

            return Ok(projects);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(
       CreateProjectRequestDto request)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var project =
                await _projectService.CreateAsync(
                    request,
                    userId);

            return CreatedAtAction(
                nameof(GetProject),
                new { id = project.Id },
                project);
        }
    }
}
