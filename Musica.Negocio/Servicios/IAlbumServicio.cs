using Musica.Entidades.Dtos;

namespace Musica.Negocio.Servicios;

public interface IAlbumServicio
{
    Task<List<AlbumDto>> ListarAsync(string? artista, string? genero, int? anio, string? q);
    Task<AlbumDto?> ObtenerPorIdAsync(int id);
    Task<AlbumDto> CrearAsync(AlbumCrearDto dto);
    Task<bool> ActualizarAsync(int id, AlbumActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}