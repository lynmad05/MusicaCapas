using System.ComponentModel.DataAnnotations;

namespace Musica.Entidades.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clave es obligatoria.")]
    public string Clave { get; set; } = string.Empty;
}