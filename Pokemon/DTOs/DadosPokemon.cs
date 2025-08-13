using System.Text.Json.Serialization;

namespace DTOs;

public record DadosPokemon
{
    [property: JsonPropertyName("name")] public string Nome { get; set; }
}