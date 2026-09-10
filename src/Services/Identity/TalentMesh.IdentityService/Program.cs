using Microsoft.EntityFrameworkCore;
using TalentMesh.IdentityService.Data;
using src.Services.Identity.TalentMesh.IdentityService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// builder.Services.AddScoped<IAuthService, AuthService>();

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();


app.Run();