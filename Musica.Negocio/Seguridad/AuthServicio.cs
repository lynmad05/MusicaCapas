using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Musica.Datos.Repositorios;
using Musica.Entidades;
using Musica.Entidades.Dtos;
using Musica.Negocio.Excepciones;
using Musica.Negocio.Seguridad;

namespace Musica.Negocio.Servicios;

public class AuthServicio : IAuthServicio
{
    private readonly IUsuarioRepositorio _repo;
    private readonly IMapper _mapper;
    private readonly JwtOpciones _jwt;

    public AuthServicio(IUsuarioRepositorio repo, IMapper mapper, JwtOpciones jwt)
    {
        _repo = repo;
        _mapper = mapper;
        _jwt = jwt;
    }

    public async Task<PerfilDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        if (await _repo.ObtenerPorCorreoAsync(correo) != null)
            throw new ReglaNegocioException("Ya existe un usuario con ese correo.");

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave), // nunca se guarda la clave en texto
            Rol = RolesApp.Oyente                                  // el rol lo decide el servidor
        };

        await _repo.AgregarAsync(usuario);
        await _repo.GuardarAsync();

        return _mapper.Map<PerfilDto>(usuario);
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var usuario = await _repo.ObtenerPorCorreoAsync(correo);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Correo),
            new("role", usuario.Rol),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);
        var expira = DateTime.UtcNow.AddMinutes(_jwt.ExpiraMinutos);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expira,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiracion = expira
        };
    }
}