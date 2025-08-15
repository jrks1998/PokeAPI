namespace Service;

public interface IConsomeApi
{
    Task<string> obterDadosPokemon();
    Task<string> obterDadosEspecies(string nome);
}