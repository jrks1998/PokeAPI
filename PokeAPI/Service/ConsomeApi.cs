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
    
    public string ObterDadosPokemon()
    {
        try
        {
            HttpResponseMessage resp = _client.Get(urlBase + "pokemon?limit=10");
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsString();
            return json;
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Erro ao obter dados dos pokemons, " + e.Message);
        }
    }

    public string ObterDadosEspecies(string nome)
    {
        try
        {
            var resp = _client.Get(urlBase + "pokemon-species/" + nome);
            resp.EnsureSuccessStatusCode();
            string json = resp.Content.ReadAsString();
            return json;
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Erro ao obter dados das especies, " + e.Message);
        }
    }
}

