using Microsoft.EntityFrameworkCore;
using TalentMesh.IdentityService.Models;

namespace TalentMesh.IdentityService.Data;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}