namespace Musica.Entidades;

public partial class Usuario
{
    public int Id { get; set; }
    public string Correo { get; set; } = null!;
    public string ClaveHash { get; set; } = null!;
    public string Rol { get; set; } = null!;
}