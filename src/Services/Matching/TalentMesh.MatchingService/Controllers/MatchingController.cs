using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentMesh.MatchingService.Clients;
using TalentMesh.MatchingService.Servicies;
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
        private readonly Servicies.MatchingService _matchingService;

        public MatchingController(
        ProjectServiceClient projectClient,
        EmployeeServiceClient employeeClient , MatchingService.Servicies.MatchingService matchingService)
        {
            _projectClient = projectClient;
            _employeeClient = employeeClient;
            _matchingService = matchingService;
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

        [HttpGet("projects/{projectId:guid}")]
        public async Task<IActionResult> TestCommunication(
        Guid projectId)
        {
            var token = GetToken();

            if (token == null) return Unauthorized();

            try
            {
                var matches = await _matchingService.FindMatchesAsync(projectId, token);
                return Ok(matches);
            }
            catch
            {
                return NotFound("Project not found.");  
            }
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
