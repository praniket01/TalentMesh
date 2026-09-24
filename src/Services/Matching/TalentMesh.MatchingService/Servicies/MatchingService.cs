using TalentMesh.MatchingService.Clients;
using TalentMesh.MatchingService.Models;
using TalentMesh.ProjectService.Clients;

namespace TalentMesh.MatchingService.Servicies
{
    public class MatchingService
    {
        private readonly ProjectServiceClient _projectClient;
        private readonly EmployeeServiceClient _employeeClient;
        private readonly MatchingScoreCalculator _calculator;

        public MatchingService(ProjectServiceClient projectClient, EmployeeServiceClient employeeClient, MatchingScoreCalculator calculator)
        {
            _projectClient = projectClient;
            _employeeClient = employeeClient;
            _calculator = calculator;
        }


        public async Task<MatchingResultDto> FindMatchesAsync(Guid projectId, string token)
        {
            var project = await _projectClient.GetProjectAsync(projectId, token);

            if (project == null) throw new InvalidOperationException("Project not found.");

            var employees = await _employeeClient.GetEmployeesAsync(token);

            var matches = new List<CandidateMatchDto>();


            foreach (var employee in employees)
            {

                var availability = 100 - employee.AllocationPercentage;

                if (availability <= 0)
                    continue;

                var skillMatch =
                    _calculator.CalculateSkillMatch(
                        project,
                        employee);

                if (skillMatch <= 0)
                    continue;

                var availabilityScore =
                    _calculator.CalculateAvailabilityScore(
                        employee);

                var experienceScore =
                    _calculator.CalculateExperienceScore(
                        employee);

                var finalScore =
                    _calculator.CalculateFinalScore(
                        skillMatch,
                        availabilityScore,
                        experienceScore);

                matches.Add(new CandidateMatchDto
                {
                    EmployeeId = employee.Id,
                    Name = employee.Name,
                    Designation = employee.Designation,
                    Location = employee.Location,
                    ExperienceYears = employee.ExperienceYears,
                    AllocationPercentage =
                   employee.AllocationPercentage,

                    AvailabilityPercentage =
                   availabilityScore,

                    SkillMatch = skillMatch,

                    AvailabilityScore =
                   availabilityScore,

                    ExperienceScore =
                   experienceScore,

                    MatchScore = finalScore
                });

            }
            return new MatchingResultDto
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                candidates = matches
                .Select(x => new CandidateMatchDto
                {
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    Designation = x.Designation,
                    Location = x.Location,
                    ExperienceYears = x.ExperienceYears,
                    AllocationPercentage = x.AllocationPercentage,
                    AvailabilityPercentage = x.AvailabilityPercentage,
                    SkillMatch = x.SkillMatch,
                    AvailabilityScore = x.AvailabilityScore,
                    ExperienceScore = x.ExperienceScore,
                    MatchScore = x.MatchScore,
                }).ToList()
            };
        }
    }
}
