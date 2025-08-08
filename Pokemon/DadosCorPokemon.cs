using System.Text.Json.Serialization;

namespace Pokemon;

public record DadosCorPokemon
{
    [property: JsonPropertyName("name")] public string Cor { get; init; }
}