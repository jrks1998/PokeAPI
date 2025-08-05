using System.Text.Json.Serialization;

namespace pokeAPI.Pokemons;

public record DadosPokemon
{
    [property: JsonPropertyName("name")] public string Nome { get; set; }
}