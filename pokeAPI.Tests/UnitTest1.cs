using Moq;
using Xunit;
using Models;
using DTOs;
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

        string fakeJson = $"{{\"green\":[\"Bulbasaur\"],\"red\":[\"Charmander\"],\"blue\":[\"squirtle\"]}}";

        Mock<IPokemonService> MockPokemonService = new Mock<IPokemonService>();

        MockPokemonService
            .Setup(ps => ps.ListaPokemon())
            .ReturnsAsync(fakeListaPokemon);

        MockPokemonService
            .Setup(ps => ps.PokemonAgruparPorCor(fakeListaPokemon))
            .Returns(fakeDictionary);

        MockPokemonService
            .Setup(ps => ps.StringAgrupadoPorCor(fakeDictionary))
            .Returns(fakeJson);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        AppDbContext inMemoryDbContext = new AppDbContext(options);
        Mock<ILogger<PokemonController>> MockLogger = new Mock<ILogger<PokemonController>>();

        PokemonController controller = new PokemonController(inMemoryDbContext, MockPokemonService.Object, MockLogger.Object);

        string resultado = await controller.AgruparPorCor();

        Assert.NotNull(resultado);
        Assert.Equal(fakeJson, resultado);
    }

    [Fact]
    public void ConsultarPokemonsSalvos()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        AppDbContext inMemoryDbContext = new AppDbContext(options);

        CorPokemon corVerde = new CorPokemon("green");
        corVerde.Pokemons.Add(new Pokemon("Bulbasaur", corVerde));
        inMemoryDbContext.Pokemons.Add(new Pokemon("Bulbasaur", corVerde));
        inMemoryDbContext.Cores.Add(corVerde);

        CorPokemon corVermelho = new CorPokemon("red");
        corVermelho.Pokemons.Add(new Pokemon("Charmander", corVermelho));
        inMemoryDbContext.Pokemons.Add(new Pokemon("Charmander", corVermelho));
        inMemoryDbContext.Cores.Add(corVermelho);

        CorPokemon corAzul = new CorPokemon("green");
        corAzul.Pokemons.Add(new Pokemon("Squirtle", corAzul));
        inMemoryDbContext.Pokemons.Add(new Pokemon("Squirtle", corAzul));
        inMemoryDbContext.Cores.Add(corAzul);

        inMemoryDbContext.SaveChanges();
        List<string> listaNomesPokemonsVerdes = new List<string> { "Bulbasaur" };
        List<string> listaNomesPokemonsVermelhos = new List<string> { "Charmander" };
        List<string> listaNomesPokemonsAzuis = new List<string> { "Squirtle" };

        CorPokemonDTO corPokemonVerde = new CorPokemonDTO
        {
            cor = corVerde.Cor,
            pokemons = listaNomesPokemonsVerdes
        };
        CorPokemonDTO corPokemonVermelho = new CorPokemonDTO
        {
            cor = corVermelho.Cor,
            pokemons = listaNomesPokemonsVermelhos
        };
        CorPokemonDTO corPokemonAzul = new CorPokemonDTO
        {
            cor = corAzul.Cor,
            pokemons = listaNomesPokemonsAzuis
        };

        List<CorPokemonDTO> fakeListaDTO = new List<CorPokemonDTO>();
        fakeListaDTO.Add(corPokemonVerde);

        Dictionary<string, List<string>> fakeDictionary = new Dictionary<string, List<string>>
        {
            ["green"] = listaNomesPokemonsVerdes,
            ["red"] = listaNomesPokemonsVermelhos,
            ["blue"] = listaNomesPokemonsAzuis
        };
        string fakeJson = $"{{\"green\":[\"Bulbasaur\"],\"red\":[\"Charmander\"],\"blue\":[\"Squirtle\"]}}";
        Mock<IPokemonService> mockPokemonService = new Mock<IPokemonService>();

        mockPokemonService
            .Setup(ps => ps.ObterCoresComPokemons())
            .Returns(fakeListaDTO);
        mockPokemonService
            .Setup(ps => ps.CorPokemonAgruparPorCor(fakeListaDTO))
            .Returns(fakeDictionary);
        mockPokemonService
            .Setup(ps => ps.StringAgrupadoPorCor(fakeDictionary))
            .Returns(fakeJson);

        Mock<ILogger<PokemonController>> mockLogger = new Mock<ILogger<PokemonController>>();
        PokemonController controller = new PokemonController(inMemoryDbContext, mockPokemonService.Object, mockLogger.Object);
        string resultado = controller.ConsultarPokemonsSalvos();

        Assert.Equal(resultado, fakeJson);
    }

    [Fact]
    public async Task CadastrarPokemon()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        AppDbContext inMemoryDbContext = new AppDbContext(options);

        List<Pokemon> fakeListaPokemon = new List<Pokemon>();

        CorPokemon corVerde = new CorPokemon("green");
        Pokemon bulbasaur = new Pokemon("Bulbasaur", corVerde);
        fakeListaPokemon.Add(bulbasaur);
        corVerde.Pokemons.Add(bulbasaur);
        inMemoryDbContext.Cores.Add(corVerde);
        inMemoryDbContext.Pokemons.Add(bulbasaur);

        CorPokemon corVermelho = new CorPokemon("red");
        Pokemon charmander = new Pokemon("Charmander", corVermelho);
        fakeListaPokemon.Add(charmander);
        corVermelho.Pokemons.Add(charmander);
        inMemoryDbContext.Cores.Add(corVermelho);
        inMemoryDbContext.Pokemons.Add(charmander);

        inMemoryDbContext.SaveChanges();

        Dictionary<string, List<string>> fakeDictionary = new Dictionary<string, List<string>>
        {
            [corVerde.Cor] = new List<string> { bulbasaur.Nome },
            [corVermelho.Cor] = new List<string> { charmander.Nome }
        };
        
        Mock<IPokemonService> mockPokemonService = new Mock<IPokemonService>();
        Mock<ILogger<PokemonController>> mockLogger = new Mock<ILogger<PokemonController>>();
        PokemonController controller = new PokemonController(inMemoryDbContext, mockPokemonService.Object, mockLogger.Object);

        mockPokemonService
            .Setup(ps => ps.CadastrarPokemons(fakeDictionary))
            .Returns(fakeListaPokemon);
        mockPokemonService
            .Setup(ps => ps.PokemonAgruparPorCor(fakeListaPokemon))
            .Returns(fakeDictionary);
        DadosRetornoPokemonsCadastrados fakeDadosRetorno = new DadosRetornoPokemonsCadastrados("Pokemons cadastrados com sucesso!", fakeDictionary);

        DadosRetornoPokemonsCadastrados resultado = await controller.CadastrarPokemon(fakeDictionary);

        Assert.Equal(resultado, fakeDadosRetorno);
    }
}