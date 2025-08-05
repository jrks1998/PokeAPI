namespace pokeApi.Service;

public class ConsomeApi
{
    private string urlBase = "https://pokeapi.co/api/v2/";
    
    public async Task<string> obterDadosPokemon()
    {
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage resp = await client.GetAsync(urlBase + "pokemon?limit=100");
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return json;
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Erro ao obter dados: {e.Message}");
        }
    }

    public async Task<string> obterDadosEspecies(string nome)
    {
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage resp = await client.GetAsync(urlBase + "pokemon-species/" + nome);
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return json;
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Erro ao obter dados: {e.Message}");
        }
    }
}

