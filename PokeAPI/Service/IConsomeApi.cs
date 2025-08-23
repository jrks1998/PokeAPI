namespace Service;

public interface IConsomeApi
{
    Task<string> ObterDadosPokemon(string url);
}