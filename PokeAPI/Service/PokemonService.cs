using System.Text.Json;
using Models;
using DTOs;
using Repository;

namespace Service;

public class PokemonService
{
    private IConsomeApi _consomeApi;
    private readonly ICorPokemonRepository _corRepository;
    private readonly IPokemonRepository _pokemonRepository;

    public PokemonService(IConsomeApi consomeApi, ICorPokemonRepository corRepository, IPokemonRepository pokemonRepository)
    {
        _consomeApi = consomeApi;
        _corRepository = corRepository;
        _pokemonRepository = pokemonRepository;
    }

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

    public async Task<List<Pokemon>> listaPokemon()
    {
        int i = 1;
        List<Pokemon> listaDadosCadastroPokemon = new List<Pokemon>();
        var nomePokemons = await obterNomesPokemons();
        foreach (var nomePokemon in nomePokemons)
        {
            string nome = nomePokemon.Nome;
            string corPokemon = await obterCorPokemon(nome);
            CorPokemon cor = new CorPokemon(corPokemon);
            Pokemon pokemon = new Pokemon(nome, cor);
            listaDadosCadastroPokemon.Add(pokemon);
            Console.WriteLine($"{i} - {pokemon.Nome}, {pokemon.Cor.Cor}");
            i += 1;
        }
        Console.WriteLine("finalizado");

        return listaDadosCadastroPokemon;
    }
    public string stringAgrupadoPorCor(Dictionary<string, List<string>> dados)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string jsonAgrupadoPorCorString = JsonSerializer.Serialize(dados, options);

        return jsonAgrupadoPorCorString;
    }

    public Dictionary<string, List<string>> corPokemonAgruparPorCor(List<CorPokemonDTO> corPokemonDTOLista)
    {
        Dictionary<string, List<string>> agrupado = corPokemonDTOLista
            .GroupBy(cp => cp.cor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.SelectMany(cp => cp.pokemons).ToList()
            );

        return agrupado;
    }

    public Dictionary<string, List<string>> pokemonAgruparPorCor(List<Pokemon> pokemonLista)
    {
        Dictionary<string, List<string>> agrupado = pokemonLista
            .GroupBy(p => p.Cor.Cor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(cp => cp.Nome).ToList()
            );

        return agrupado;
    }

    private CorPokemon verificarPelaCorCorPokemonSalva(string cor)
    {
        return _corRepository.ObterCorPeloNome(cor);
    }

    private Pokemon verificarPeloNomePokemonSalvo(string nome)
    {
        return _pokemonRepository.ObterPokemonPeloNome(nome);
    }

    public List<Pokemon> CadastrarPokemons(Dictionary<string, List<string>> dadosPokemons)
    {
        List<Pokemon> pokemonsCadastrados = new List<Pokemon>();

        foreach (var dadosPokemon in dadosPokemons)
        {
            string cor = dadosPokemon.Key;
            List<string> nomes = dadosPokemon.Value;
            CorPokemon corPokemon = verificarPelaCorCorPokemonSalva(cor);
            if (corPokemon == null)
            {
                corPokemon = new CorPokemon(cor);
                _corRepository.SalvarCor(corPokemon);
            }
            foreach (string nome in nomes)
            {
                Pokemon pokemon = verificarPeloNomePokemonSalvo(nome);
                if (pokemon == null)
                {
                    pokemon = new Pokemon(nome, corPokemon);
                    _pokemonRepository.SalvarPokemon(pokemon);
                }
                pokemonsCadastrados.Add(pokemon);
            }
        }

        return pokemonsCadastrados;
    }
}