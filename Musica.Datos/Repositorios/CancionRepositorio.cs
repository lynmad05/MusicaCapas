using Microsoft.EntityFrameworkCore;
using Musica.Entidades;

namespace Musica.Datos.Repositorios;

public class CancionRepositorio : ICancionRepositorio
{
    private readonly MusicaContext _context;

    public CancionRepositorio(MusicaContext context) => _context = context;

    public async Task<List<Cancion>> ListarAsync(int? albumId, string? titulo, int? duracionMax)
    {
        IQueryable<Cancion> query = _context.Cancions
            .AsNoTracking()
            .Include(c => c.Album);

        if (albumId.HasValue)
            query = query.Where(c => c.AlbumId == albumId.Value);
        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(c => c.Titulo.Contains(titulo));
        if (duracionMax.HasValue)
            query = query.Where(c => c.DuracionSegundos <= duracionMax.Value);

        return await query.OrderBy(c => c.AlbumId).ThenBy(c => c.NumeroPista).ToListAsync();
    }

    public async Task<List<Cancion>> ListarPorAlbumAsync(int albumId)
        => await _context.Cancions
            .AsNoTracking()
            .Include(c => c.Album)
            .Where(c => c.AlbumId == albumId)
            .OrderBy(c => c.NumeroPista)
            .ToListAsync();

    public async Task<Cancion?> ObtenerPorIdAsync(int id)
        => await _context.Cancions
            .AsNoTracking()
            .Include(c => c.Album)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Cancion?> ObtenerParaEditarAsync(int id)
        => await _context.Cancions.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> ExistePistaAsync(int albumId, int numeroPista, int? excluirId = null)
        => await _context.Cancions.AsNoTracking().AnyAsync(c =>
            c.AlbumId == albumId && c.NumeroPista == numeroPista && (excluirId == null || c.Id != excluirId));

    public async Task CrearAsync(Cancion cancion)
    {
        _context.Cancions.Add(cancion);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Cancion cancion)
    {
        _context.Cancions.Remove(cancion);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync() => await _context.SaveChangesAsync();
}