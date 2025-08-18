using DTOs;
using Models;

namespace Service;

public interface IPokemonService
{
    Task<List<DadosPokemon>> ObterNomesPokemons();
    Task<string> ObterCorPokemon(string nome);
    Task<List<Pokemon>> ListaPokemon();
    Dictionary<string, List<string>> CorPokemonAgruparPorCor(List<CorPokemonDTO> corPokemonDTOLista);
    Dictionary<string, List<string>> PokemonAgruparPorCor(List<Pokemon> pokemonLista);
    List<Pokemon> CadastrarPokemons(Dictionary<string, List<string>> dadosPokemons);
    List<CorPokemonDTO> ObterCoresComPokemons();
}