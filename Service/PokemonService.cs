using System.Text.Json;
using PokemonClass = Pokemon.Pokemon;
using Pokemon;

namespace Service;

public class PokemonService
{
    private ConsomeApi _consomeApi = new ConsomeApi();

    public async Task<List<DadosPokemon>> obterNomesPokemons()
    {
        var json = await _consomeApi.obterDadosPokemon();
        if (json != null)
        {
            PokemonList listaPokemons = JsonSerializer.Deserialize<PokemonList>(json);
            return listaPokemons.Pokemons;
        }

        return null;
    }

    public async Task<string> obterCorPokemon(string nome)
    {
        var json = await _consomeApi.obterDadosEspecies(nome);
        if (json != null)
        {
            DadosCorPokemon corPokemon = JsonSerializer.Deserialize<PokemonSpecie>(json).Color;
            var cor = corPokemon.Cor;
            return cor;
        }

        return null;
    }

    public async Task<string> agruparPorCor()
    {
        List<PokemonClass> listaDadosCadastroPokemon = new List<PokemonClass>();
        var nomePokemons = await obterNomesPokemons();
        foreach (var nomePokemon in nomePokemons)
        {
            string nome = nomePokemon.Nome;
            string corPokemon = await obterCorPokemon(nome);
            CorPokemon cor = new CorPokemon(corPokemon);
            PokemonClass pokemon = new PokemonClass(nome, cor);
            listaDadosCadastroPokemon.Add(pokemon);
        }

        Dictionary<string, List<string>> dictionaryAgrupadoPorCor = agruparPorCorDictionary(listaDadosCadastroPokemon);
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string jsonAgrupadoPorCor = JsonSerializer.Serialize(dictionaryAgrupadoPorCor, options);
        return jsonAgrupadoPorCor;
    }

    public DadosCadastroPokemon agruparPorCorDictionary(List<PokemonClass> listaPokemons)
    {
        Dictionary<string, List<string>> agrupadoPorCor = listaPokemons
            .GroupBy(lp => lp.Cor.Cor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(lp => lp.Nome).ToList()
            );

        return new DadosCadastroPokemon(agrupadoPorCor);
    }

    public string agruparPorCorString(DadosCadastroPokemon dados)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string jsonAgrupadoPorCorString = JsonSerializer.Serialize(dados, options);

        return jsonAgrupadoPorCorString;
    }
}