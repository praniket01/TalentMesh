using TalentMesh.IdentityService.DTOs;

namespace TalentMesh.IdentityService.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}