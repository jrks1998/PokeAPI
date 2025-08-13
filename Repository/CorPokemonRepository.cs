using Data;
using Models;

namespace Repository;

public class CorPokemonRepository : ICorPokemonRepository
{
    private readonly AppDbContext _dbContext;

    public CorPokemonRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public CorPokemon ObterCorPeloNome(string cor)
    {
        return _dbContext.Cores
            .FirstOrDefault(c => c.Cor.ToUpper() == cor.ToUpper());
    }

    public void SalvarCor(CorPokemon cor)
    {
        _dbContext.Cores.Add(cor);
        _dbContext.SaveChanges();
    }
}