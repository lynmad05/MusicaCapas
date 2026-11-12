using Musica.Entidades;

namespace Musica.Datos.Repositorios;

public interface ICancionRepositorio
{
    Task<List<Cancion>> ListarAsync(int? albumId, string? titulo, int? duracionMax);
    Task<List<Cancion>> ListarPorAlbumAsync(int albumId);
    Task<Cancion?> ObtenerPorIdAsync(int id);
    Task<Cancion?> ObtenerParaEditarAsync(int id);
    Task<bool> ExistePistaAsync(int albumId, int numeroPista, int? excluirId = null);
    Task CrearAsync(Cancion cancion);
    Task EliminarAsync(Cancion cancion);
    Task GuardarCambiosAsync();
}