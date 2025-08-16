using System.Text.Json;
using Models;
using DTOs;
using Repository;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Service;

public class PokemonService
{
    private IConsomeApi _consomeApi;
    private readonly ICorPokemonRepository _corRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly AppDbContext _dbContext;

    public PokemonService(IConsomeApi consomeApi, ICorPokemonRepository corRepository, IPokemonRepository pokemonRepository, AppDbContext dbContext)
    {
        _consomeApi = consomeApi;
        _corRepository = corRepository;
        _pokemonRepository = pokemonRepository;
        _dbContext = dbContext;
    }

    public async Task<List<DadosPokemon>> ObterNomesPokemons()
    {
        var json = await _consomeApi.ObterDadosPokemon();
        if (json != null)
        {
            PokemonList listaPokemons = JsonSerializer.Deserialize<PokemonList>(json);
            return listaPokemons.Pokemons;
        }

        return null;
    }

    public async Task<string> ObterCorPokemon(string nome)
    {
        var json = await _consomeApi.ObterDadosEspecies(nome);
        if (json != null)
        {
            DadosCorPokemon corPokemon = JsonSerializer.Deserialize<PokemonSpecie>(json).Color;
            var cor = corPokemon.Cor;
            return cor;
        }

        return null;
    }

    public async Task<List<Pokemon>> ListaPokemon()
    {
        int i = 1;
        List<Pokemon> listaDadosCadastroPokemon = new List<Pokemon>();
        var nomePokemons = await ObterNomesPokemons();
        foreach (var nomePokemon in nomePokemons)
        {
            string nome = nomePokemon.Nome;
            string corPokemon = await ObterCorPokemon(nome);
            CorPokemon cor = new CorPokemon(corPokemon);
            Pokemon pokemon = new Pokemon(nome, cor);
            listaDadosCadastroPokemon.Add(pokemon);
            Console.WriteLine($"{i} - {pokemon.Nome}, {pokemon.Cor.Cor}");
            i += 1;
        }
        Console.WriteLine("finalizado");

        return listaDadosCadastroPokemon;
    }
    public string StringAgrupadoPorCor(Dictionary<string, List<string>> dados)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string jsonAgrupadoPorCorString = JsonSerializer.Serialize(dados, options);

        return jsonAgrupadoPorCorString;
    }

    public Dictionary<string, List<string>> CorPokemonAgruparPorCor(List<CorPokemonDTO> corPokemonDTOLista)
    {
        Dictionary<string, List<string>> agrupado = corPokemonDTOLista
            .GroupBy(cp => cp.cor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.SelectMany(cp => cp.pokemons).ToList()
            );

        return agrupado;
    }

    public Dictionary<string, List<string>> PokemonAgruparPorCor(List<Pokemon> pokemonLista)
    {
        Dictionary<string, List<string>> agrupado = pokemonLista
            .GroupBy(p => p.Cor.Cor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(cp => cp.Nome).ToList()
            );

        return agrupado;
    }

    private CorPokemon VerificarPelaCorCorPokemonSalva(string cor)
    {
        return _corRepository.ObterCorPeloNome(cor);
    }

    private Pokemon VerificarPeloNomePokemonSalvo(string nome)
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
            CorPokemon corPokemon = VerificarPelaCorCorPokemonSalva(cor);
            if (corPokemon == null)
            {
                corPokemon = new CorPokemon(cor);
                _corRepository.SalvarCor(corPokemon);
            }
            foreach (string nome in nomes)
            {
                Pokemon pokemon = VerificarPeloNomePokemonSalvo(nome);
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

    public List<CorPokemonDTO> ObterCoresComPokemons()
    {
        List<CorPokemonDTO> coresComPokemons = _dbContext.Cores
            .Include(c => c.Pokemons)
            .Select(c => new CorPokemonDTO
            {
                cor = c.Cor,
                pokemons = c.Pokemons.Select(p => p.Nome).ToList()
            }).ToList();

        return coresComPokemons;
    }
}