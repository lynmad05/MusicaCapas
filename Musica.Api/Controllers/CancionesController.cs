using Microsoft.AspNetCore.Mvc;
using Musica.Entidades.Dtos;
using Musica.Negocio.Excepciones;
using Musica.Negocio.Servicios;

namespace Musica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CancionesController : ControllerBase
{
    private readonly ICancionServicio _servicio;

    public CancionesController(ICancionServicio servicio) => _servicio = servicio;

    // GET api/canciones?albumId=&titulo=&duracionMax=
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CancionDto>>> GetCanciones(
        [FromQuery] int? albumId,
        [FromQuery] string? titulo,
        [FromQuery] int? duracionMax)
    {
        return Ok(await _servicio.ListarAsync(albumId, titulo, duracionMax));
    }

    // GET api/canciones/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CancionDto>> GetCancion(int id)
    {
        var cancion = await _servicio.ObtenerPorIdAsync(id);
        if (cancion == null) return NotFound();
        return Ok(cancion);
    }

    // POST api/canciones
    [HttpPost]
    public async Task<ActionResult<CancionDto>> PostCancion(CancionCrearDto dto)
    {
        try
        {
            var creada = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(GetCancion), new { id = creada.Id }, creada);
        }
        catch (DatoInvalidoException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return ValidationProblem(ModelState);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/canciones/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCancion(int id, CancionActualizarDto dto)
    {
        try
        {
            var actualizada = await _servicio.ActualizarAsync(id, dto);
            return actualizada ? NoContent() : NotFound();
        }
        catch (DatoInvalidoException ex)
        {
            ModelState.AddModelError(ex.Campo, ex.Message);
            return ValidationProblem(ModelState);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE api/canciones/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCancion(int id)
    {
        var eliminada = await _servicio.EliminarAsync(id);
        return eliminada ? NoContent() : NotFound();
    }
}