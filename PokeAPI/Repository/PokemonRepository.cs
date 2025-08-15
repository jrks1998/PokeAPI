using Data;
using Models;

namespace Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly AppDbContext _dbContext;

    public PokemonRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Pokemon ObterPokemonPeloNome(string nome)
    {
        return _dbContext.Pokemons
            .FirstOrDefault(p => p.Nome.ToUpper() == nome.ToUpper());
    }

    public void SalvarPokemon(Pokemon pokemon)
    {
        _dbContext.Pokemons.Add(pokemon);
        _dbContext.SaveChanges();
    }
}