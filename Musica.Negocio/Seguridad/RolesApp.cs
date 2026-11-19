namespace Musica.Negocio.Seguridad;

public static class RolesApp
{
    public const string Oyente = "Oyente";
    public const string Curador = "Curador";
    public const string GerenteCatalogo = "GerenteCatalogo";

   
    public const string CuradorOGerente = Curador + "," + GerenteCatalogo;
}