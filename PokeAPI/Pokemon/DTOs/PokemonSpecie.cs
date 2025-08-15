using System.Text.Json.Serialization;

namespace DTOs;

public record PokemonSpecie
{
    [property: JsonPropertyName("color")]
    public DadosCorPokemon Color { get; init; }
}