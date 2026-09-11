using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TalentMesh.IdentityService.Data;
using TalentMesh.IdentityService.Data;

namespace TalentMesh.IdentityService.Services;


public class AuthService : IAuthService
{
    private readonly IdentityDbContext _context;
    private readonly IConfiguration _configuration;

        public AuthService(
        IdentityDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request){
        var user = await _context.Users.FirstOrDefault(x => x.Email == request.Email);

        if(user == null) return null;

        var passwordValid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if(!passwordValid) return null;

        var claims = new [] {
             new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.Role,
                user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);
        
        return new LoginResponse{
            Token = new JwtSecurityTokenHandler().WriteToken(token),

            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    } 
}