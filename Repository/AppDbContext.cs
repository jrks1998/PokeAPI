using Microsoft.EntityFrameworkCore;
using Pokemon;
using PokemonClass = Pokemon.Pokemon;

namespace Repository;

public class PokemonRepository : DbContext
{
    public DbSet<PokemonClass> Pokemons { get; set; }
    public DbSet<CorPokemon> Cores { get; set;  }

    public PokemonRepository(DbContextOptions<PokemonRepository> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PokemonClass>(entity =>
        {
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            entity.Property(p => p.CorPokemonId).IsRequired();
        });

        modelBuilder.Entity<CorPokemon>(entity =>
        {
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Cor).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<CorPokemon>()
            .HasMany(c => c.Pokemons)
            .WithOne(p => p.CorPokemon)
            .HasForeignKey(p => p.CorPokemonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}