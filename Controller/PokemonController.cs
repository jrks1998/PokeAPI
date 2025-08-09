using Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using PokemonClass = Pokemon.Pokemon;
using Pokemon;
using System.Text.Json;
// using Validacoes;

namespace Controller;

[ApiController]
[Route("pokemons")]
public class PokemonController : ControllerBase
{
    private readonly PokemonRepository _dbContext;
    private readonly PokemonService _pokemonService;

    public PokemonController(PokemonRepository dbContext, PokemonService pokemonService)
    {
        _dbContext = dbContext;
        _pokemonService = pokemonService;
    }

    [HttpPost("save")]
    public async Task<DadosRetornoPokemonsCadastrados> cadastrarPokemon([FromBody] DadosCadastroPokemon dadosPokemons)
    {
        List<PokemonClass> pokemonsCadastrados = new List<PokemonClass>();
        foreach (var dadosPokemonPorCor in dadosPokemons)
        {
            string nomeCor = dadosPokemonPorCor.Key;
            List<string> nomes = dadosPokemonPorCor.Value;
            CorPokemon cor;
            CorPokemon? verificaCor = await _dbContext.Cores
                .FirstOrDefaultAsync(c => c.Cor.ToUpper() == nomeCor.ToUpper());
            Console.WriteLine(verificaCor);
            if (verificaCor == null)
            {
                cor = new CorPokemon(nomeCor); ;
                Console.WriteLine("cadastrando nova cor " + cor.Cor);
                _dbContext.Cores.Add(cor);
            }
            else
            {
                cor = verificaCor;
            }
            foreach (var nome in nomes)
            {
                PokemonClass pokemon = new PokemonClass(nome, cor);
                PokemonClass? verificaPokemon = await _dbContext.Pokemons
                    .FirstOrDefaultAsync(p => p.Nome.ToUpper() == nome.ToUpper());
                if (verificaPokemon == null)
                {
                    _dbContext.Pokemons.Add(pokemon);
                    pokemonsCadastrados.Add(pokemon);
                }
            }
        }
        await _dbContext.SaveChangesAsync();
        DadosCadastroPokemon agrupado = _pokemonService.agruparPorCorDictionary(pokemonsCadastrados);

        return new DadosRetornoPokemonsCadastrados("Pokemons cadastrados com sucesso!", agrupado);
    }

    [HttpGet("group-by-color")]
    public async Task<string> agruparPorCor()
    {
        string json = await _pokemonService.agruparPorCor();
        return json;
    }

    [HttpGet("from-db")]
    public async Task<string> consultarPokemonsSalvos()
    {
        List<PokemonClass> pokemonsSalvos = await _dbContext.Pokemons
            .ToListAsync();
        DadosCadastroPokemon dictionaryPokemonsCadastrados = _pokemonService.agruparPorCorDictionary(pokemonsSalvos);

        string jsonPokemonsCadastrados = _pokemonService.agruparPorCorString(dictionaryPokemonsCadastrados);

        return jsonPokemonsCadastrados;
    }
}