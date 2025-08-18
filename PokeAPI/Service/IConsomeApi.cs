namespace Service;

public interface IConsomeApi
{
    Task<string> ObterDadosPokemon();
    Task<string> ObterDadosEspecies(string nome);
}