using Microsoft.EntityFrameworkCore;
using Musica.Entidades;

namespace Musica.Datos.Repositorios;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly MusicaContext _context;

    public UsuarioRepositorio(MusicaContext context) => _context = context;

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        => await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Correo == correo);

    public async Task AgregarAsync(Usuario usuario)
        => await _context.Usuarios.AddAsync(usuario);

    public async Task GuardarAsync()
        => await _context.SaveChangesAsync();
}