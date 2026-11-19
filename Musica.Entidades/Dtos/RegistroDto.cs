using System.ComponentModel.DataAnnotations;

namespace Musica.Entidades.Dtos;

public class RegistroDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
    [StringLength(100)]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clave es obligatoria.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La clave debe tener al menos 8 caracteres.")]
    public string Clave { get; set; } = string.Empty;
}