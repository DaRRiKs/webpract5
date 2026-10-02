using System.Net;
using System.Net.Http.Json;
using StudentClient.Models;

namespace StudentClient.Services;

public class StudentApiService : IStudentApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StudentApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        var client = _httpClientFactory.CreateClient("StudentApi");
        return await client.GetFromJsonAsync<List<Student>>("api/students") ?? new List<Student>();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");

        try
        {
            return await client.GetFromJsonAsync<Student>($"api/students/{id}");
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<bool> CreateAsync(Student student)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");
        var response = await client.PostAsJsonAsync("api/students", student);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");
        var response = await client.DeleteAsync($"api/students/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        var client = _httpClientFactory.CreateClient("StudentApi");
        var response = await client.PutAsJsonAsync($"api/students/{student.Id}", student);
        return response.IsSuccessStatusCode;
    }
}
