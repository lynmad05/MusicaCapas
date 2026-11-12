using Musica.Entidades;

namespace Musica.Datos.Repositorios;

public interface IAlbumRepositorio
{
    Task<List<Album>> ListarAsync(string? artista, string? genero, int? anio, string? q);
    Task<Album?> ObtenerPorIdAsync(int id);
    Task<Album?> ObtenerParaEditarAsync(int id);
    Task<bool> ExisteAsync(int id);
    Task<bool> ExisteDuplicadoAsync(string titulo, string artista, int? excluirId = null);
    Task<bool> TieneCancionesAsync(int id);
    Task CrearAsync(Album album);
    Task EliminarAsync(Album album);
    Task GuardarCambiosAsync();
}