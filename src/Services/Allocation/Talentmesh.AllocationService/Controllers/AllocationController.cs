using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talentmesh.AllocationService.Data;
using Talentmesh.AllocationService.DTOs;
using Talentmesh.AllocationService.Models;
using Talentmesh.AllocationService.Services;
using TalentMesh.AllocationService.DTOs;

namespace Talentmesh.AllocationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AllocationController : ControllerBase
    {

        private readonly AllocationDbContext _context;
        private readonly EmployeeServiceClient _employeeServiceClient;

        public AllocationController(AllocationDbContext context, EmployeeServiceClient employeeServiceClient)
        {
            _context = context;
            _employeeServiceClient = employeeServiceClient;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAllocation(CreateAllocationRequest request)
        {
            if (request.AllocationPercentage <= 0 ||
           request.AllocationPercentage > 100)
            {
                return BadRequest(new
                {
                    message =
                        "Allocation percentage must be between 1 and 100."
                });
            }

            var employee = await _employeeServiceClient.GetEmployeeAsyn(request.EmployeeId);

            if (employee == null) return NotFound(new { message = " Employee Not Found " });

            var newAllocation = employee.AllocationPercentage + request.AllocationPercentage;

            if (newAllocation > 100) return BadRequest(new { message = $"Allocation exceeds 100%. " + $"Current allocation: " + $"{employee.AllocationPercentage}%. " + $"Requested: " + $"{request.AllocationPercentage}%." });

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var requestedBy))
            {
                return Unauthorized();
            }

            var allocation = new Allocation
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                EmployeeId = request.EmployeeId,
                AllocationPercentage =
               request.AllocationPercentage,
                Status = AllocationStatus.PendingApproval,
                RequestedBy = requestedBy,
                RequestedAt = DateTime.UtcNow,
                RejectionReason = string.Empty
            };

            _context.Allocations.Add(allocation);

            await _context.SaveChangesAsync();

            var response = new AllocationDto
            {
                Id = allocation.Id,
                ProjectId = allocation.ProjectId,
                EmployeeId = allocation.EmployeeId,
                AllocationPercentage =
                    allocation.AllocationPercentage,
                Status = allocation.Status,
                RequestedBy = allocation.RequestedBy,
                RequestedAt = allocation.RequestedAt
            };

            return CreatedAtAction(
                nameof(GetAllocation),
                new { id = allocation.Id },
                response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAllocation(Guid id)
        {
            var allocation =
                await _context.Allocations.FirstOrDefaultAsync(x => x.Id == id);

            if (allocation == null)
            {
                return NotFound();
            }

            return Ok(new AllocationDto
            {
                Id = allocation.Id,
                ProjectId = allocation.ProjectId,
                EmployeeId = allocation.EmployeeId,
                AllocationPercentage =
                    allocation.AllocationPercentage,
                Status = allocation.Status,
                RequestedBy = allocation.RequestedBy,
                ApprovedBy = allocation.ApprovedBy,
                RequestedAt = allocation.RequestedAt,
                ApprovedAt = allocation.ApprovedAt,
                RejectionReason =
                    allocation.RejectionReason
            });
        }
    }
}
