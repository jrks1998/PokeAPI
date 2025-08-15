using Moq;
using Xunit;
using Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Controller;
using Service;
using Data;
using Repository;
using Microsoft.EntityFrameworkCore;

namespace pokeAPI.Tests;

public class UnitTest1
{
    [Fact]
    public async Task AgruparPorCor()
    {
        CorPokemon green = new CorPokemon("green");
        CorPokemon red = new CorPokemon("red");
        CorPokemon blue = new CorPokemon("blue");
        Pokemon bulbasaur = new Pokemon("bulbasaur", green);
        Pokemon charmander = new Pokemon("charmander", red);
        Pokemon squirtle = new Pokemon("squirtle", blue);
        List<Pokemon> fakeListaPokemon = new List<Pokemon>();
        fakeListaPokemon.Add(bulbasaur);
        fakeListaPokemon.Add(charmander);
        fakeListaPokemon.Add(squirtle);
        Dictionary<string, List<string>> fakeDictionary = new Dictionary<string, List<string>>
        {
            ["green"] = new List<string> { "bulbasaur" },
            ["red"] = new List<string> { "charmander" },
            ["blue"] = new List<string> { "squirtle" }
        };
        string fakeJson = $"{{\"green\":[\"Bulbasaur\"],\"red\":[\"Charmander\"]}}";

        Mock<IPokemonService> MockPokemonService = new Mock<IPokemonService>();
        Mock<ILogger<PokemonController>> MockLogger = new Mock<ILogger<PokemonController>>();

        MockPokemonService
            .Setup(ps => ps.listaPokemon())
            .ReturnsAsync(fakeListaPokemon);

        MockPokemonService
            .Setup(ps => ps.pokemonAgruparPorCor(fakeListaPokemon))
            .Returns(fakeDictionary);

        MockPokemonService
            .Setup(ps => ps.stringAgrupadoPorCor(fakeDictionary))
            .Returns(fakeJson);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var inMemoryDbContext = new AppDbContext(options);

        PokemonController controller = new PokemonController(inMemoryDbContext, MockPokemonService.Object, MockLogger.Object);

        var resultado = await controller.agruparPorCor();

        Assert.Equal(fakeJson, resultado);
    }
}