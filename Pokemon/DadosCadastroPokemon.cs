namespace Pokemon;

public class DadosCadastroPokemon : Dictionary<string, List<string>>
{
    public DadosCadastroPokemon() : base()
    { }

    public DadosCadastroPokemon(IDictionary<string, List<string>> dictionary) : base(dictionary)
    { }
}