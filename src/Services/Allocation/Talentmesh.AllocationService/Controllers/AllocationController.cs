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
        [Authorize(Roles = "ProjectManager")]
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

        [HttpGet]
        [Authorize(Roles = "ResourceManager")]
        public async Task<IActionResult> GetAllAllocations([FromQuery] string? status= null){
            var query = _context.Allocations.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(status)){
                query = query.Where(a => a.Status == status);
            }

            var allocations = await query.OrderBy(a => a.RequestedAt).ToListAsync();

            return Ok(allocations);
        }

        [HttpPost("{id:guid}/approve")]
        [Authorize(Roles = "ResourceManager")]
        public async Task<IActionResult> ApproveAllocation(Guid id)
        {
            var allocation =
                await _context.Allocations
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (allocation == null)
            {
                return NotFound(new
                {
                    message = "Allocation request not found."
                });
            }

            if (allocation.Status != AllocationStatus.PendingApproval)
            {
                return BadRequest(new
                {
                    message =
                        $"Allocation cannot be approved because " +
                        $"its current status is {allocation.Status}."
                });
            }

            var employee =
                await _employeeServiceClient.GetEmployeeAsyn(allocation.EmployeeId);

            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee not found."
                });
            }

            var newAllocation =
                employee.AllocationPercentage +
                allocation.AllocationPercentage;

            if (newAllocation > 100)
            {
                return BadRequest(new
                {
                    message =
                        $"Cannot approve allocation. " +
                        $"Employee is currently allocated at " +
                        $"{employee.AllocationPercentage}%. " +
                        $"Requested additional allocation: " +
                        $"{allocation.AllocationPercentage}%."
                });
            }

            var updated =
                await _employeeServiceClient
                    .UpdateAllocationAsync(
                        allocation.EmployeeId,
                        newAllocation);

            if (!updated)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    new
                    {
                        message =
                            "Failed to update employee allocation."
                    });
            }

            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(
                    userIdClaim,
                    out var approvedBy))
            {
                return Unauthorized();
            }

            allocation.Status =
                AllocationStatus.Approved;

            allocation.ApprovedBy =
                approvedBy;

            allocation.ApprovedAt =
                DateTime.UtcNow;

            await _context.SaveChangesAsync();

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

        [HttpPost("{id:guid}/reject")]
        [Authorize(Roles = "ResourceManager")]
        public async Task<IActionResult> RejectAllocation(Guid id,RejectAllocationRequest request)
        {
            var allocation =
                await _context.Allocations
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (allocation == null)
            {
                return NotFound(new
                {
                    message = "Allocation request not found."
                });
            }

            if (allocation.Status != AllocationStatus.PendingApproval)
            {
                return BadRequest(new
                {
                    message =
                        $"Allocation cannot be rejected because " +
                        $"its current status is {allocation.Status}."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                return BadRequest(new
                {
                    message = "Rejection reason is required."
                });
            }

            var userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(
                    userIdClaim,
                    out var approvedBy))
            {
                return Unauthorized();
            }

            allocation.Status =
                AllocationStatus.Rejected;

            allocation.ApprovedBy =
                approvedBy;

            allocation.ApprovedAt =
                DateTime.UtcNow;

            allocation.RejectionReason =
                request.Reason.Trim();

            await _context.SaveChangesAsync();

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
