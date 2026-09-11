using TalentMesh.IdentityService.DTOs;

namespace TalentMesh.IdentityService.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}