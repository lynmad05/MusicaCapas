namespace Musica.Negocio.Excepciones;

public class DatoInvalidoException : Exception
{
    public string Campo { get; }

    public DatoInvalidoException(string campo, string mensaje) : base(mensaje)
    {
        Campo = campo;
    }
}