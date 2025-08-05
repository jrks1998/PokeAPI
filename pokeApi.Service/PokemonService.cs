using System.Text.Json;
using pokeAPI.Pokemon;
using pokeAPI.Pokemons;

namespace pokeApi.Service;

public class PokemonService
{
    private ConsomeApi _consomeApi = new ConsomeApi();
    private List<DadosCadastroPokemon> listaDadosCadastroPokemon = new List<DadosCadastroPokemon>();
    
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
        var pokemons = await obterNomesPokemons();
        foreach (var pokemon in pokemons)
        {
            string nome = pokemon.Nome;
            string cor = await obterCorPokemon(nome);
            DadosCadastroPokemon dados = new DadosCadastroPokemon(nome, cor);
            listaDadosCadastroPokemon.Add(dados);
        }

        var agrupado = listaDadosCadastroPokemon
            .GroupBy(p => p.Cor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(p => p.Nome).ToList()
            );
        return JsonSerializer.Serialize(agrupado, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}