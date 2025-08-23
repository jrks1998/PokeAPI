namespace Service;

public class ConsomeApi : IConsomeApi
{
    private string urlBase = "https://pokeapi.co/api/v2/";
    private readonly HttpClient _client;

    public ConsomeApi(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.Add("Connection", "keep-alive");
    }
    
    public async Task<string> ObterDadosPokemon(string url)
    {
        try
        {
            HttpResponseMessage resp = await _client.GetAsync(urlBase + url);
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return json;
        }
        catch
        {
            return null;
        }
    }
}

