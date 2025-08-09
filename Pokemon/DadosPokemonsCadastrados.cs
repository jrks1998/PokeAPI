using PokemonClass = Pokemon.Pokemon;

namespace Pokemon;

public record DadosPokemonsCadastrados
{
    public string Cor { get; init; }
    public string Nome { get; init; }

    public DadosPokemonsCadastrados(PokemonClass pokemon)
    {
        Cor = pokemon.Cor.Cor;
        Nome = pokemon.Nome;
    }
};