using Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using PokemonClass = Pokemon.Pokemon;
using Pokemon;

namespace Controller;

[ApiController]
[Route("pokemons")]
public class PokemonController : ControllerBase
{
    private readonly PokemonRepository _dbContext;

    public PokemonController(PokemonRepository dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("save")]
    public async Task<DadosPokemonsCadastrados> cadastrarPokemon([FromBody] Dictionary<string, List<string>> dadosPokemons)
    {
        List<PokemonClass> pokemonsCadastrados = new List<PokemonClass>();
        foreach (var dadosPokemonPorCor in dadosPokemons)
        {
            string nomeCor = dadosPokemonPorCor.Key;
            List<string> nomes = dadosPokemonPorCor.Value;
            CorPokemon cor = new CorPokemon(nomeCor);
            CorPokemon? verificaCor = await _dbContext.Cores
                .FirstOrDefaultAsync(c => c.Cor.ToUpper() == cor.Cor.ToUpper());
            Console.WriteLine(verificaCor);
            if (verificaCor == null)
            {
                Console.WriteLine("cadastrando nova cor " + cor.Cor);
                _dbContext.Cores.Add(cor);
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
        var agrupado = pokemonsCadastrados
            .GroupBy(p => p.CorPokemon.Cor)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(p => p.Nome).ToList()
            );

        return new DadosPokemonsCadastrados("Pokemons cadastrados com sucesso!", agrupado);
    }

    [HttpGet("group-by-color")]
    public async Task<string> agruparPorCor()
    {
        PokemonService service = new PokemonService();
        string json = await service.agruparPorCor();
        return json;
    }
}