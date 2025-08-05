using Microsoft.EntityFrameworkCore;
using pokeAPI.Pokemons;

namespace pokeAPI.Dados;

public class AppDbContext : DbContext
{
    public DbSet<Pokemons.Pokemon> Pokemons { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemons.Pokemon>(entity =>
        {
            entity.Property(p => p.Id).ValueGeneratedOnAdd();
            entity.Property(p => p.Nome).IsRequired().HasMaxLength(100);
        });
    }
}