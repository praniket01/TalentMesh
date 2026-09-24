using TalentMesh.MatchingService.Models;

namespace TalentMesh.MatchingService.Servicies
{
    public class MatchingScoreCalculator
    {
        private static readonly Dictionary<string, int> SkillLevels = new(StringComparer.OrdinalIgnoreCase)
      {
          ["Beginner"] = 1,
          ["Intermediate"] = 2,
          ["Advanced"] = 3,
          ["Expert"] = 4
      };

        private static decimal CalculateSkillScore(string requiredLevel, EmployeeSkillDto employeeSkill)
        {
            if(!SkillLevels.TryGetValue(requiredLevel, out var required))
            {
                return 0;
            }

            if (!SkillLevels.TryGetValue(employeeSkill.Level, out var employee))
            {
                return 0;
            }

            if (employee >= required)
            {
                return 100;
            }

            if (employee == required - 1)
            {
                return 70;
            }

            return 40;  
        }
    
        public decimal CalculateSkillMatch(ProjectDto project,EmployeeDto employee)
        {
            if (project.SkillRequirements.Count == 0)
                return 0;

            decimal totalScore = 0;

            foreach (var requirement in project.SkillRequirements) 
            {
                var employeeSkill = employee.Skills
                    .FirstOrDefault(x => string.Equals(x.Skill.Name, requirement.SkillName, StringComparison.OrdinalIgnoreCase));

                if (employeeSkill == null) continue;

                totalScore += CalculateSkillScore(requirement.RequiredLevel,employeeSkill);

            }
            return Math.Round(totalScore / project.SkillRequirements.Count,2);
        }

        public decimal CalculateAvailabilityScore(EmployeeDto employee)
        {
            return Math.Clamp(
                100 - employee.AllocationPercentage,
                0,
                100);
        }

        public decimal CalculateExperienceScore( EmployeeDto employee)
        {
            return Math.Min(employee.ExperienceYears * 10,100);
        }

        public decimal CalculateFinalScore(decimal skillMatch,decimal availabilityScore,decimal experienceScore)
        {
            return Math.Round( (skillMatch * 0.50m) + (availabilityScore * 0.30m) +  (experienceScore * 0.20m), 2);
        }
    
        
    }
}
