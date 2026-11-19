using Musica.Entidades.Dtos;

namespace Musica.Negocio.Servicios;

public interface IAuthServicio
{
    Task<PerfilDto> RegistrarAsync(RegistroDto dto);
    Task<TokenDto?> LoginAsync(LoginDto dto);
}