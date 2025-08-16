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
    public async Task<DadosRetornoPokemonsCadastrados> CadastrarPokemon([FromBody] Dictionary<string, List<string>> dadosPokemons)
    {
        List<Pokemon> pokemonsCadastrados = _pokemonService.CadastrarPokemons(dadosPokemons);
        Dictionary<string, List<string>> agrupado = _pokemonService.PokemonAgruparPorCor(pokemonsCadastrados);

        DadosRetornoPokemonsCadastrados retornoPokemonsCadastrados = new DadosRetornoPokemonsCadastrados("Pokemons cadastrados com sucesso!", agrupado);
        return retornoPokemonsCadastrados;
    }

    [HttpGet("group-by-color")]
    public async Task<string> AgruparPorCor()
    {
        List<Pokemon> pokemons = await _pokemonService.ListaPokemon();
        Dictionary<string, List<string>> dictionaryPokemon = _pokemonService.PokemonAgruparPorCor(pokemons);
        string jsonAgrupadoPorCor = _pokemonService.StringAgrupadoPorCor(dictionaryPokemon);
        return jsonAgrupadoPorCor;
    }

    [HttpGet("from-db")]
    public string ConsultarPokemonsSalvos()
    {
        List<CorPokemonDTO> coresComPokemons = _pokemonService.ObterCoresComPokemons();
        Dictionary<string, List<string>> dictionaryCorPokemon = _pokemonService.CorPokemonAgruparPorCor(coresComPokemons);

        string jsonAgrupadoPorCor = _pokemonService.StringAgrupadoPorCor(dictionaryCorPokemon);
        return jsonAgrupadoPorCor;
    }
}