using System.Text.Json;
using PokemonClass = Pokemon.Pokemon;
using Pokemon;

namespace Service;

public class PokemonService
{
    private ConsomeApi _consomeApi = new ConsomeApi();
    private List<PokemonClass> listaDadosCadastroPokemon = new List<PokemonClass>();

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
        var nomePokemons = await obterNomesPokemons();
        foreach (var nomePokemon in nomePokemons)
        {
            string nome = nomePokemon.Nome;
            string corPokemon = await obterCorPokemon(nome);
            CorPokemon cor = new CorPokemon(corPokemon);
            PokemonClass pokemon = new PokemonClass(nome, cor);
            Console.WriteLine($"inserindo Pokemon: {pokemon.Nome} com cor: {pokemon.CorPokemon.Cor}");
            listaDadosCadastroPokemon.Add(pokemon);
        }

        var agrupado = listaDadosCadastroPokemon
            .GroupBy(p => p.CorPokemon.Cor)
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