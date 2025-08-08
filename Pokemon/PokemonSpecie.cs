using System.Text.Json.Serialization;

namespace Pokemon;

public record PokemonSpecie
{
    [property: JsonPropertyName("color")]
    public DadosCorPokemon Color { get; init; }
}