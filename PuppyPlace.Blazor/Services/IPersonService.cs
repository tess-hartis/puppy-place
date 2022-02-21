using System.Net.Http.Json;
using System.Text.Json;
using LanguageExt;
using PuppyPlace.Domain;

namespace PuppyPlace.Blazor.Services;

public interface IPersonService
{
    Task<List<Person>> GetPersons();
}

public class PersonService : IPersonService
{
    private readonly HttpClient _httpClient;
    // private readonly JsonSerializerOptions _options;

    public PersonService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // _options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
    }
    
    public async Task<List<Person>> GetPersons()
    {
        return await _httpClient.GetFromJsonAsync<List<Person>>("Person");
        // var content = await response.Content.ReadAsStringAsync();
        // // if (!response.IsSuccessStatusCode)
        // // {
        // //     throw new ApplicationException(content);
        // // }
        //
        // var persons = JsonSerializer.Deserialize<List<Person>>(content);
        // return persons;
    }
}