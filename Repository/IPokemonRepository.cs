using Models;

namespace Repository;

public interface IPokemonRepository
{
    Pokemon ObterPokemonPeloNome(string nome);
    void SalvarPokemon(Pokemon pokemon); 
}