using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("pokemon_colors")]
public class CorPokemon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("color_id")]
    public int Id { get; init; }
    [Column("color_name")]
    public string Cor { get; set; }
    public List<Pokemon> Pokemons { get; set; }

    CorPokemon() { }

    public CorPokemon(string cor)
    {
        Cor = cor;
    }
}