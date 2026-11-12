using Musica.Entidades.Dtos;

namespace Musica.Negocio.Servicios;

public interface ICancionServicio
{
    Task<List<CancionDto>> ListarAsync(int? albumId, string? titulo, int? duracionMax);
    Task<List<CancionDto>?> ListarPorAlbumAsync(int albumId);
    Task<CancionDto?> ObtenerPorIdAsync(int id);
    Task<CancionDto> CrearAsync(CancionCrearDto dto);
    Task<bool> ActualizarAsync(int id, CancionActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}