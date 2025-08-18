using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class AppDbContext : DbContext
{
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<CorPokemon> Cores { get; set;  }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CorPokemon>(entity =>
        {
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Cor).IsRequired().HasMaxLength(50);
            entity.HasMany(c => c.Pokemons)
                .WithOne(p => p.Cor)
                .HasForeignKey(p => p.CorPokemonId);
        });

         modelBuilder.Entity<Pokemon>(entity =>
        {
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(50);
            entity.Property(p => p.CorPokemonId).IsRequired();
            entity.HasOne(p => p.Cor)
                .WithMany(c => c.Pokemons)
                .HasForeignKey(p => p.CorPokemonId);
        });
    }
}