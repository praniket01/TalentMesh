using TalentMesh.IdentityService.Models;

namespace TalentMesh.IdentityService.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        IdentityDbContext context)
    {
        if (context.Users.Any())
            return;

        var users = new List<User>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Email = "manager@talentmesh.com",
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = "ProjectManager"
            },

            new()
            {
                Id = Guid.NewGuid(),
                Email = "resource@talentmesh.com",
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = "ResourceManager"
            },

            new()
            {
                Id = Guid.NewGuid(),
                Email = "employee@talentmesh.com",
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = "Employee"
            },

            new()
            {
                Id = Guid.NewGuid(),
                Email = "admin@talentmesh.com",
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = "Admin"
            }
        };

        context.Users.AddRange(users);

        await context.SaveChangesAsync();
    }
}