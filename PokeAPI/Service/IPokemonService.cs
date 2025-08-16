using DTOs;
using Models;

namespace Service;

public interface IPokemonService
{
    Task<List<DadosPokemon>> obterNomesPokemons();
    Task<string> obterCorPokemon(string nome);
    Task<List<Pokemon>> listaPokemon();
    string stringAgrupadoPorCor(Dictionary<string, List<string>> dados);
    Dictionary<string, List<string>> corPokemonAgruparPorCor(List<CorPokemonDTO> corPokemonDTOLista);
    Dictionary<string, List<string>> pokemonAgruparPorCor(List<Pokemon> pokemonLista);
    CorPokemon verificarPelaCorCorPokemonSalva(string cor);
    Pokemon verificarPeloNomePokemonSalvo(string nome);
    List<Pokemon> CadastrarPokemons(Dictionary<string, List<string>> dadosPokemons);
    List<CorPokemonDTO> ObterCoresComPokemons();
}