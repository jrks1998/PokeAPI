using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PokemonClass = Pokemon.Pokemon;

namespace Pokemon;

[Table("pokemon_colors")]
public class CorPokemon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("color_id")]
    public int Id { get; init; }
    [Column("color_name")]
    public string Cor { get; set; }
    public List<PokemonClass> Pokemons { get; set; } = new List<PokemonClass>();

    CorPokemon() { }

    public CorPokemon(string cor)
    {
        Cor = cor;
    }
}