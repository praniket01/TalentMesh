using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentMesh.MatchingService.Clients;
using TalentMesh.ProjectService.Clients;

namespace TalentMesh.MatchingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatchingController : ControllerBase
    {
        private readonly ProjectServiceClient _projectClient;
        private readonly EmployeeServiceClient _employeeClient;

        public MatchingController(
        ProjectServiceClient projectClient,
        EmployeeServiceClient employeeClient)
        {
            _projectClient = projectClient;
            _employeeClient = employeeClient;
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                service = "TalentMesh Matching Service",
                status = "Healthy"
            });
        }

        [HttpGet("test/{projectId:guid}")]
        public async Task<IActionResult> TestCommunication(
        Guid projectId)
        {
            var token = GetToken();

            if (token == null)
                return Unauthorized();

            var project =
                await _projectClient.GetProjectAsync(
                    projectId,
                    token);

            if (project == null)
            {
                return NotFound(
                    "Project could not be retrieved.");
            }

            var employees =
                await _employeeClient.GetEmployeesAsync(
                    token);

            return Ok(new
            {
                project = project.Name,
                requirements = project.SkillRequirements,
                employeeCount = employees.Count,
                employees = employees.Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Designation,
                    x.ExperienceYears,
                    x.AllocationPercentage,
                    x.Skills
                })
            });
        }

        private string? GetToken()
        {
            var authorization =
                Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorization))
                return null;

            if (!authorization.StartsWith(
                    "Bearer ",
                    StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return authorization["Bearer ".Length..].Trim();
        }

    }
}
