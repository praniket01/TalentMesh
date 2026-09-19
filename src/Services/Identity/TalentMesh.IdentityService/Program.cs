using Microsoft.EntityFrameworkCore;
using TalentMesh.IdentityService.Data;
using TalentMesh.IdentityService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IAuthService,AuthService>();

builder.Services.AddCors(options => {
       options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:8000") 
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); 
    });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

    await context.Database.MigrateAsync();
    await DbSeeder.SeedAsync(context);
}


app.MapControllers();


app.Run();