using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pokeAPI.Pokemons;

[Table("pokemons")]
public class Pokemon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; init; }
    [Column("nome")]
    public string Nome { get; set; }

    Pokemon() { }

    public Pokemon(DadosCadastroPokemon dados)
    {
        Nome = dados.Nome;
    }
}