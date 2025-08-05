using System.Text.Json.Serialization;

namespace pokeAPI.Pokemon;

public record DadosCorPokemon
{
    [property: JsonPropertyName("name")] public string Cor { get; init; }
}