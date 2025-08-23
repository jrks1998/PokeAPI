using System.Text.Json;
using Models;
using DTOs;
using Repository;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Service;

public class PokemonService : IPokemonService
{
    private IConsomeApi _consomeApi;
    private readonly ICorPokemonRepository _corRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<PokemonService> _logger;

    public PokemonService(IConsomeApi consomeApi, ICorPokemonRepository corRepository, IPokemonRepository pokemonRepository, AppDbContext dbContext, ILogger<PokemonService> logger)
    {
        _consomeApi = consomeApi;
        _corRepository = corRepository;
        _pokemonRepository = pokemonRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<List<DadosPokemon>> ObterNomesPokemons()
    {
        var json = await _consomeApi.ObterDadosPokemon("pokemon?limit=100");
        if (json != null)
        {
            PokemonList listaPokemons = JsonSerializer.Deserialize<PokemonList>(json);
            return listaPokemons.Pokemons;
        }

        return null;
    }

    public async Task<string> ObterCorPokemon(string nome)
    {
        var json = await _consomeApi.ObterDadosPokemon("pokemon-species/" + nome);
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
        List<DadosPokemon> nomePokemons = await ObterNomesPokemons();
        if (nomePokemons == null || nomePokemons.Count == 0)
        {
            _logger.LogWarning("Nenhum Pokemon retornado da API");
            return null;
        }

        foreach (var nomePokemon in nomePokemons)
        {
            string nome = nomePokemon.Nome;
            string corPokemon = await ObterCorPokemon(nome);
            if (corPokemon == null)
            {
                _logger.LogWarning($"Cor do Pokemon {nome} não encontrada");
                continue;
            }
            CorPokemon cor = new CorPokemon(corPokemon);
            Pokemon pokemon = new Pokemon(nome, cor);
            listaDadosCadastroPokemon.Add(pokemon);
            _logger.LogInformation($"{i} - {pokemon.Nome}, {pokemon.Cor.Cor}");
            i += 1;
        }
        _logger.LogInformation("finalizado");

        return listaDadosCadastroPokemon;
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

    private CorPokemon CadastrarCorPokemon(string cor)
    {
        CorPokemon corPokemon = VerificarPelaCorCorPokemonSalva(cor);
        if (corPokemon == null)
        {
            corPokemon = new CorPokemon(cor);
            _corRepository.SalvarCor(corPokemon);
        }
        return corPokemon;
    }

    public List<Pokemon> CadastrarPokemons(Dictionary<string, List<string>> dadosPokemons)
    {
        List<Pokemon> pokemonsCadastrados = new List<Pokemon>();

        foreach (var dadosPokemon in dadosPokemons)
        {
            string cor = dadosPokemon.Key;
            List<string> nomes = dadosPokemon.Value;
            CorPokemon corPokemon = CadastrarCorPokemon(cor);
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