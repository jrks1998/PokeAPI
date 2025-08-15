using System.Text.Json.Serialization;

namespace DTOs;

public record PokemonList
{
     [property: JsonPropertyName("results")]
     public List<DadosPokemon> Pokemons { get; init; }
}