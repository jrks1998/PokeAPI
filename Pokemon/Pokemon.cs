using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pokemon;

[Table("pokemons")]
public class Pokemon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("pokemon_id")]
    public int Id { get; init; }
    [Column("name")]
    public string Nome { get; set; }
    [Column("color_id")]
    public int CorPokemonId { get; set; }
    public CorPokemon CorPokemon { get; set; }

    Pokemon() { }

    public Pokemon(string nome, CorPokemon cor)
    {
        Nome = nome;
        CorPokemonId = cor.Id;
        CorPokemon = cor;
    }
}