namespace Musica.Entidades.Dtos;

public class CancionDto
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string AlbumTitulo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public int NumeroPista { get; set; }
    public int DuracionSegundos { get; set; }
    public int Reproducciones { get; set; }
}