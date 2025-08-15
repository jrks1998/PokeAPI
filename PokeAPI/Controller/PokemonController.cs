using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using DTOs;
using Models;
using Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Controller;

[ApiController]
[Route("pokemons")]
public class PokemonController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IPokemonService _pokemonService;
    private readonly ILogger<PokemonController> _logger;

    public PokemonController(AppDbContext dbContext, IPokemonService pokemonService, ILogger<PokemonController> logger)
    {
        _dbContext = dbContext;
        _pokemonService = pokemonService;
        _logger = logger;
    }

    [HttpPost("save")]
    public async Task<DadosRetornoPokemonsCadastrados> cadastrarPokemon([FromBody] Dictionary<string, List<string>> dadosPokemons)
    {
        List<Pokemon> pokemonsCadastrados = _pokemonService.CadastrarPokemons(dadosPokemons);
        Dictionary<string, List<string>> agrupado = _pokemonService.pokemonAgruparPorCor(pokemonsCadastrados);

        DadosRetornoPokemonsCadastrados retornoPokemonsCadastrados = new DadosRetornoPokemonsCadastrados("Pokemons cadastrados com sucesso!", agrupado);
        return retornoPokemonsCadastrados;
    }

    [HttpGet("group-by-color")]
    public async Task<string> agruparPorCor()
    {
        List<Pokemon> pokemons = await _pokemonService.listaPokemon();
        Dictionary<string, List<string>> dictionaryPokemon = _pokemonService.pokemonAgruparPorCor(pokemons);
        string jsonAgrupadoPorCor = _pokemonService.stringAgrupadoPorCor(dictionaryPokemon);
        return jsonAgrupadoPorCor;
    }

    [HttpGet("from-db")]
    public string consultarPokemonsSalvos()
    {
        List<CorPokemonDTO> coresComPokemons = _dbContext.Cores
            .Include(c => c.Pokemons)
            .Select(c => new CorPokemonDTO
            {
                cor = c.Cor,
                pokemons = c.Pokemons.Select(p => p.Nome).ToList()
            }).ToList();

        Dictionary<string, List<string>> dictionaryCorPokemon = _pokemonService.corPokemonAgruparPorCor(coresComPokemons);
        string jsonAgrupadoPorCor = _pokemonService.stringAgrupadoPorCor(dictionaryCorPokemon);
        return jsonAgrupadoPorCor;
    }
}