using Microsoft.AspNetCore.Mvc;
using Musica.Entidades.Dtos;
using Musica.Negocio.Excepciones;
using Musica.Negocio.Servicios;

using Microsoft.AspNetCore.Authorization;
using Musica.Negocio.Seguridad;

namespace Musica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CancionesController : ControllerBase
{
    private readonly ICancionServicio _servicio;

    public CancionesController(ICancionServicio servicio) => _servicio = servicio;

    [AllowAnonymous]
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

    [Authorize(Roles = RolesApp.CuradorOGerente)]
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

    [Authorize(Roles = RolesApp.CuradorOGerente)]
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

   [Authorize(Roles = RolesApp.GerenteCatalogo)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCancion(int id)
    {
        var eliminada = await _servicio.EliminarAsync(id);
        return eliminada ? NoContent() : NotFound();
    }
}