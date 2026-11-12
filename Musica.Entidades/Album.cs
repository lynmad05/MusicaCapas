namespace Musica.Entidades;

public partial class Album
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Artista { get; set; } = null!;
    public string Genero { get; set; } = null!;
    public int AnioLanzamiento { get; set; }
    public virtual ICollection<Cancion> Cancions { get; set; } = new List<Cancion>();
}