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
    
    public async Task<string> ObterDadosPokemon()
    {
        try
        {
            HttpResponseMessage resp = await _client.GetAsync(urlBase + "pokemon?limit=25");
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return json;
        }
        catch
        {
            return null;
        }
    }

    public async Task<string> ObterDadosEspecies(string nome)
    {
        try
        {
            HttpResponseMessage resp = await _client.GetAsync(urlBase + "pokemon-species/" + nome);
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

