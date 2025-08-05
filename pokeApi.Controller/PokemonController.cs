using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pokeAPI.Dados;
using pokeAPI.Pokemons;

namespace pokeApi.Controller;

[ApiController]
[Route("pokemons")]
public class PokemonController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public PokemonController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<Pokemon>> cadastrarPokemon(DadosCadastroPokemon dados)
    {
        var pokemon = new Pokemon(dados);
        _dbContext.Pokemons.Add(pokemon);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(detalharPokemon), new { id = pokemon.Id }, pokemon);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pokemon>>> listarPokemons()
    {
        return await _dbContext.Pokemons.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pokemon>> detalharPokemon(int id)
    {
        var pokemon = await _dbContext.Pokemons.FindAsync(id);
        if (pokemon == null)
        {
            return NotFound();
        }

        return pokemon;
    }
}