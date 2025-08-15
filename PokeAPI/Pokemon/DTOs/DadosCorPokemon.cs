using System.Text.Json.Serialization;

namespace DTOs;

public record DadosCorPokemon
{
    [property: JsonPropertyName("name")] public string Cor { get; init; }
}