using System.Net.Http.Headers;
using TalentMesh.ProjectService.DTOs;

namespace TalentMesh.ProjectService.Clients
{
    public class ProjectServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProjectServiceClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ProjectDto?> GetProjectAsync(Guid projectId, string token)
        {
            var baseUrl =
           _configuration["Services:ProjectService"];

            using var request = new HttpRequestMessage(
                HttpMethod.Get, $"{baseUrl}/api/project/{projectId}");
        
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token);

            var response =
            await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
           .ReadFromJsonAsync<ProjectDto>();
        }
    }
}
