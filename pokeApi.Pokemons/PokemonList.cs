using System.Text.Json.Serialization;

namespace pokeAPI.Pokemons;

public record PokemonList
{
     [property: JsonPropertyName("results")]
     public List<DadosPokemon> Pokemons { get; init; }
}