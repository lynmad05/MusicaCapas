using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musica.Entidades.Dtos;
using Musica.Negocio.Excepciones;
using Musica.Negocio.Servicios;

namespace Musica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServicio _auth;

    public AuthController(IAuthServicio auth) => _auth = auth;

    // POST api/auth/registro
    [AllowAnonymous]
    [HttpPost("registro")]
    public async Task<ActionResult<PerfilDto>> Registro(RegistroDto dto)
    {
        try
        {
            var perfil = await _auth.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created, perfil);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // POST api/auth/login
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await _auth.LoginAsync(dto);
        if (token == null)
            return Unauthorized(new { mensaje = "Credenciales inválidas." });

        return Ok(token);
    }

    // GET api/auth/perfil  (datos tomados de los claims del token, no de la BD)
    [Authorize]
    [HttpGet("perfil")]
    public ActionResult<PerfilDto> Perfil()
    {
        var correo = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;
        var rol = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? string.Empty;

        return Ok(new PerfilDto { Correo = correo, Rol = rol });
    }
}