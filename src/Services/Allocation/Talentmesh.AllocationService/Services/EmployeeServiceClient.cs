using System.Net.Http.Headers;
using TalentMesh.AllocationService.DTOs;

namespace Talentmesh.AllocationService.Services
{
    public class EmployeeServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public EmployeeServiceClient( HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
        }
    
        public async Task<EmployeeDto?> GetEmployeeAsyn(Guid EmployeeId)
        {
            var authorizationHeader = _contextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault();

            if(!string.IsNullOrEmpty(authorizationHeader))
                _httpClient.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValue.Parse(
                    authorizationHeader);

            return await _httpClient.GetFromJsonAsync<EmployeeDto>(
                $"api/employee/{EmployeeId}"
                );
        }
    }
}
