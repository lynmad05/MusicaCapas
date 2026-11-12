using System.ComponentModel.DataAnnotations;

namespace Musica.Entidades.Dtos;

public class CancionCrearDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Indique un álbum válido.")]
    public int AlbumId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "El número de pista debe estar entre 1 y 100.")]
    public int NumeroPista { get; set; }

    [Range(1, 7200, ErrorMessage = "La duración debe estar entre 1 y 7200 segundos.")]
    public int DuracionSegundos { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Las reproducciones no pueden ser negativas.")]
    public int Reproducciones { get; set; }
}