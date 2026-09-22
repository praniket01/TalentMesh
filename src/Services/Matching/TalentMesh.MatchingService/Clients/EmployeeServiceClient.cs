using System.Net.Http.Headers;
using System.Net.Http.Json;
using TalentMesh.MatchingService.Models;

namespace TalentMesh.MatchingService.Clients;

public class EmployeeServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public EmployeeServiceClient(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync(
        string token)
    {
        var baseUrl =
            _configuration["Services:EmployeeService"];

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{baseUrl}/api/employee");

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        return await response.Content
            .ReadFromJsonAsync<List<EmployeeDto>>()
            ?? [];
    }
}