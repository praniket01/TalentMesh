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

        public async Task<bool> UpdateAllocationAsync( Guid employeeId,int allocationPercentage)
        {
            var authorizationHeader =  _contextAccessor.HttpContext?.Request.Headers.Authorization
                    .FirstOrDefault();

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(
                        authorizationHeader);
            }

            var request = new
            {
                allocationPercentage
            };

            var response = await _httpClient.PutAsJsonAsync(
                $"api/employee/{employeeId}/allocation",
                request);

            return response.IsSuccessStatusCode;
        }
    }
}
