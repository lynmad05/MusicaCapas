namespace Musica.Entidades;

public partial class Cancion
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Titulo { get; set; } = null!;
    public int NumeroPista { get; set; }
    public int DuracionSegundos { get; set; }
    public int Reproducciones { get; set; }
    public virtual Album Album { get; set; } = null!;
}