using Microsoft.EntityFrameworkCore;
using src.Services.Identity.TalentMesh.IdentityService.Models;

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