namespace Musica.Entidades.Dtos;

public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracion { get; set; }
}