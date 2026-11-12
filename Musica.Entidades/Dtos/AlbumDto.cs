namespace Musica.Entidades.Dtos;

public class AlbumDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Artista { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    public int AnioLanzamiento { get; set; }
    public int TotalCanciones { get; set; }
}