using Microsoft.AspNetCore.Mvc;
using Musica.Entidades.Dtos;
using Musica.Negocio.Excepciones;
using Musica.Negocio.Servicios;

namespace Musica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumesController : ControllerBase
{
    private readonly IAlbumServicio _albumServicio;
    private readonly ICancionServicio _cancionServicio;

    public AlbumesController(IAlbumServicio albumServicio, ICancionServicio cancionServicio)
    {
        _albumServicio = albumServicio;
        _cancionServicio = cancionServicio;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbumes(
        [FromQuery] string? artista,
        [FromQuery] string? genero,
        [FromQuery] int? anio,
        [FromQuery] string? q)
    {
        return Ok(await _albumServicio.ListarAsync(artista, genero, anio, q));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlbumDto>> GetAlbum(int id)
    {
        var album = await _albumServicio.ObtenerPorIdAsync(id);
        if (album == null) return NotFound();
        return Ok(album);
    }

    [HttpGet("{id}/canciones")]
    public async Task<ActionResult<IEnumerable<CancionDto>>> GetCancionesDeAlbum(int id)
    {
        var canciones = await _cancionServicio.ListarPorAlbumAsync(id);
        if (canciones == null) return NotFound();
        return Ok(canciones);
    }
    [HttpPost]
    public async Task<ActionResult<AlbumDto>> PostAlbum(AlbumCrearDto dto)
    {
        try
        {
            var creado = await _albumServicio.CrearAsync(dto);
            return CreatedAtAction(nameof(GetAlbum), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAlbum(int id, AlbumActualizarDto dto)
    {
        try
        {
            var actualizado = await _albumServicio.ActualizarAsync(id, dto);
            return actualizado ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        try
        {
            var eliminado = await _albumServicio.EliminarAsync(id);
            return eliminado ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}