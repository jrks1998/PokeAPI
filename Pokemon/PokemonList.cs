using System.Text.Json.Serialization;

namespace Pokemon;

public record PokemonList
{
     [property: JsonPropertyName("results")]
     public List<DadosPokemon> Pokemons { get; init; }
}