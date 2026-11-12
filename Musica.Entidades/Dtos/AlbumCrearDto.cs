using System.ComponentModel.DataAnnotations;

namespace Musica.Entidades.Dtos;

public class AlbumCrearDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El artista es obligatorio.")]
    [StringLength(100)]
    public string Artista { get; set; } = string.Empty;

    [Required(ErrorMessage = "El género es obligatorio.")]
    [StringLength(50)]
    public string Genero { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100.")]
    public int AnioLanzamiento { get; set; }
}