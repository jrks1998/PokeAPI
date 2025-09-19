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
using System.Threading.Tasks;

namespace pokeAPI.Tests;

public class UnitTest1
{
    [Fact]
    public async void AgruparPorCor()
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

        Mock<IPokemonService> MockPokemonService = new Mock<IPokemonService>();

        MockPokemonService
            .Setup(ps => ps.ListaPokemon())
            .ReturnsAsync(fakeListaPokemon);

        MockPokemonService
            .Setup(ps => ps.PokemonAgruparPorCor(fakeListaPokemon))
            .Returns(fakeDictionary);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        AppDbContext inMemoryDbContext = new AppDbContext(options);
        Mock<ILogger<PokemonController>> MockLogger = new Mock<ILogger<PokemonController>>();

        PokemonController controller = new PokemonController(inMemoryDbContext, MockPokemonService.Object, MockLogger.Object);

        var resultado = await controller.AgruparPorCor();
        var okResultado = Assert.IsType<OkObjectResult>(resultado.Result);
        var dadosRetorno = Assert.IsType<Dictionary<string, List<string>>>(okResultado.Value);

        Assert.NotNull(resultado);
        Assert.Equal(fakeDictionary, dadosRetorno);
    }

    [Fact]
    public async Task AgruparPorCor_NenhumPokemonEncontrado_RetornaNotFound()
    {
        Mock<IPokemonService> mockPokemonService = new Mock<IPokemonService>();
        mockPokemonService
            .Setup(ps => ps.ListaPokemon())
            .ReturnsAsync(new List<Pokemon>());

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        AppDbContext inMemoryDbContext = new AppDbContext(options);
        Mock<ILogger<PokemonController>> mockLogger = new Mock<ILogger<PokemonController>>();

        PokemonController controller = new PokemonController(inMemoryDbContext, mockPokemonService.Object, mockLogger.Object);

        var resultado = await controller.AgruparPorCor();
        var notFoundResultado = Assert.IsType<NotFoundObjectResult>(resultado.Result);
        var mensagemRetorno = Assert.IsType<string>(notFoundResultado.Value);

        Assert.Equal("Nenhum Pokemon encontrado", mensagemRetorno);
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
        Mock<IPokemonService> mockPokemonService = new Mock<IPokemonService>();

        mockPokemonService
            .Setup(ps => ps.ObterCoresComPokemons())
            .Returns(fakeListaDTO);
        mockPokemonService
            .Setup(ps => ps.CorPokemonAgruparPorCor(fakeListaDTO))
            .Returns(fakeDictionary);

        Mock<ILogger<PokemonController>> mockLogger = new Mock<ILogger<PokemonController>>();
        PokemonController controller = new PokemonController(inMemoryDbContext, mockPokemonService.Object, mockLogger.Object);

        var resultado = controller.ConsultarPokemonsSalvos();
        var okResultado = Assert.IsType<OkObjectResult>(resultado.Result);
        var dadosRetorno = Assert.IsType<Dictionary<string, List<string>>>(okResultado.Value);

        Assert.NotNull(resultado);
        Assert.Equal(fakeDictionary, dadosRetorno);
    }

    [Fact]
    public void ConsultarPokemonsSalvos_NenhumPokemonCadastrado_RetornaNotFound()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        AppDbContext inMemoryDbContext = new AppDbContext(options);

        Mock<IPokemonService> mockPokemonService = new Mock<IPokemonService>();
        mockPokemonService
            .Setup(ps => ps.ObterCoresComPokemons())
            .Returns(new List<CorPokemonDTO>());
        Mock<ILogger<PokemonController>> mockLogger = new Mock<ILogger<PokemonController>>();

        PokemonController controller = new PokemonController(inMemoryDbContext, mockPokemonService.Object, mockLogger.Object);

        var resultado = controller.ConsultarPokemonsSalvos();
        var okResultado = Assert.IsType<NotFoundObjectResult>(resultado.Result);
        var dadosRetorno = Assert.IsType<string>(okResultado.Value);

        Assert.Equal("Nenhum pokemon cadastrado no banco de dados", dadosRetorno);
    }

    [Fact]
    public async void CadastrarPokemon()
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

        Mock<IPokemonService> MockPokemonService = new Mock<IPokemonService>();

        MockPokemonService
            .Setup(ps => ps.ListaPokemon())
            .ReturnsAsync(fakeListaPokemon);

        MockPokemonService
            .Setup(ps => ps.PokemonAgruparPorCor(fakeListaPokemon))
            .Returns(fakeDictionary);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        AppDbContext inMemoryDbContext = new AppDbContext(options);
        Mock<ILogger<PokemonController>> MockLogger = new Mock<ILogger<PokemonController>>();

        PokemonController controller = new PokemonController(inMemoryDbContext, MockPokemonService.Object, MockLogger.Object);

        var resultadoAgruparPorCor = await controller.AgruparPorCor();
        var okResultadoAgruparPorCor = Assert.IsType<OkObjectResult>(resultadoAgruparPorCor.Result);
        var dadosRetornoAgruparPorCor = Assert.IsType<Dictionary<string, List<string>>>(okResultadoAgruparPorCor.Value);

        Assert.NotNull(resultadoAgruparPorCor);
        Assert.Equal(fakeDictionary, dadosRetornoAgruparPorCor);

        green.Pokemons.Add(bulbasaur);
        inMemoryDbContext.Cores.Add(green);
        inMemoryDbContext.Pokemons.Add(bulbasaur);

        red.Pokemons.Add(charmander);
        inMemoryDbContext.Cores.Add(red);
        inMemoryDbContext.Pokemons.Add(charmander);

        blue.Pokemons.Add(charmander);
        inMemoryDbContext.Cores.Add(blue);
        inMemoryDbContext.Pokemons.Add(squirtle);

        inMemoryDbContext.SaveChanges();

        Mock<IPokemonService> mockPokemonService = new Mock<IPokemonService>();
        mockPokemonService
            .Setup(ps => ps.PokemonAgruparPorCor(fakeListaPokemon))
            .Returns(fakeDictionary);
        mockPokemonService
            .Setup(ps => ps.CadastrarPokemons(fakeDictionary))
            .Returns(fakeListaPokemon);
        mockPokemonService
            .Setup(ps => ps.PokemonAgruparPorCor(fakeListaPokemon))
            .Returns(fakeDictionary);
        DadosRetornoPokemonsCadastrados fakeDadosRetorno = new DadosRetornoPokemonsCadastrados("Pokemons cadastrados com sucesso!", fakeDictionary);

        var resultadoCadastrarPokemon = controller.CadastrarPokemon();
        var actionResultCadastrarPokemon = Assert.IsType<ActionResult<DadosRetornoPokemonsCadastrados>>(resultadoCadastrarPokemon.Result);
        var okResultadoCadastrarPorCor = Assert.IsType<OkObjectResult>(actionResultCadastrarPokemon.Result);
        var dadosRetornoCadastrarPorCor = Assert.IsType<DadosRetornoPokemonsCadastrados>(okResultadoCadastrarPorCor.Value);

        Assert.NotNull(resultadoCadastrarPokemon);
        Assert.Equal(dadosRetornoAgruparPorCor, fakeDictionary);
    }
}