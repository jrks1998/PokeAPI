using System.Text.Json.Serialization;

namespace Pokemon;

public record DadosPokemon
{
    [property: JsonPropertyName("name")] public string Nome { get; set; }
}