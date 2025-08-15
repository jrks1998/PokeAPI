namespace DTOs;

public record CorPokemonDTO
{
    public string cor { get; init; }
    public List<string> pokemons { get; init; }
}