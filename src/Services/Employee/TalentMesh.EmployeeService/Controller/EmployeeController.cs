using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TalentMesh.EmployeeService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[]
            {
                new
                {
                      Id = 1,
                Name = "Rahul Sharma",
                Department = "Engineering",
                Designation = "Senior .NET Developer",
                Experience = 5,
                Availability = 60
                },
                new
                {
                     Id = 2,
                Name = "Sneha Joshi",
                Department = "Engineering",
                Designation = "Frontend Developer",
                Experience = 3,
                Availability = 80
                },
            });
        }
    }
}
