using Models;

namespace Repository;

public interface ICorPokemonRepository
{
    CorPokemon ObterCorPeloNome(string cor);
    void SalvarCor(CorPokemon cor);
}