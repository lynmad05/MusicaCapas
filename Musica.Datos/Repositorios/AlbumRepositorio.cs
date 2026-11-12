using Microsoft.EntityFrameworkCore;
using Musica.Entidades;

namespace Musica.Datos.Repositorios;

public class AlbumRepositorio : IAlbumRepositorio
{
    private readonly MusicaContext _context;

    public AlbumRepositorio(MusicaContext context) => _context = context;

    public async Task<List<Album>> ListarAsync(string? artista, string? genero, int? anio, string? q)
    {
        IQueryable<Album> query = _context.Albums
            .AsNoTracking()
            .Include(a => a.Cancions);

        if (!string.IsNullOrWhiteSpace(artista))
            query = query.Where(a => a.Artista.Contains(artista));
        if (!string.IsNullOrWhiteSpace(genero))
            query = query.Where(a => a.Genero == genero);
        if (anio.HasValue)
            query = query.Where(a => a.AnioLanzamiento == anio.Value);
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(a => a.Titulo.Contains(q));

        return await query.OrderBy(a => a.Artista).ThenBy(a => a.Titulo).ToListAsync();
    }

    public async Task<Album?> ObtenerPorIdAsync(int id)
        => await _context.Albums
            .AsNoTracking()
            .Include(a => a.Cancions)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Album?> ObtenerParaEditarAsync(int id)
        => await _context.Albums.FirstOrDefaultAsync(a => a.Id == id);

    public async Task<bool> ExisteAsync(int id)
        => await _context.Albums.AsNoTracking().AnyAsync(a => a.Id == id);

    public async Task<bool> ExisteDuplicadoAsync(string titulo, string artista, int? excluirId = null)
        => await _context.Albums.AsNoTracking().AnyAsync(a =>
            a.Titulo == titulo && a.Artista == artista && (excluirId == null || a.Id != excluirId));

    public async Task<bool> TieneCancionesAsync(int id)
        => await _context.Cancions.AsNoTracking().AnyAsync(c => c.AlbumId == id);

    public async Task CrearAsync(Album album)
    {
        _context.Albums.Add(album);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Album album)
    {
        _context.Albums.Remove(album);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync() => await _context.SaveChangesAsync();
}