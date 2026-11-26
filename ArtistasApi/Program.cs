using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
    throw new InvalidOperationException("Jwt:Key debe tener al menos 32 caracteres.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = "role",
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

var artistas = new List<Artista>
{
    new(1, "The Beatles", "Reino Unido", "Rock", 1960),
    new(2, "Michael Jackson", "Estados Unidos", "Pop", 1964),
    new(3, "Bad Bunny", "Puerto Rico", "Reggaetón", 2016),
    new(4, "Miles Davis", "Estados Unidos", "Jazz", 1945),
    new(5, "Nirvana", "Estados Unidos", "Grunge", 1987),
    new(6, "Los Mirlos", "Perú", "Cumbia amazónica", 1966)
};

var api = app.MapGroup("/api/artistas").RequireAuthorization();


api.MapGet("/", (string? pais, string? genero) =>
{
    var lista = artistas.AsEnumerable();

    if (!string.IsNullOrWhiteSpace(pais))
        lista = lista.Where(a => a.Pais.Contains(pais, StringComparison.OrdinalIgnoreCase));
    if (!string.IsNullOrWhiteSpace(genero))
        lista = lista.Where(a => a.GeneroPrincipal.Contains(genero, StringComparison.OrdinalIgnoreCase));

    return Results.Ok(lista.ToList());
});


api.MapGet("/{id:int}", (int id) =>
{
    var artista = artistas.FirstOrDefault(a => a.Id == id);
    return artista is null ? Results.NotFound() : Results.Ok(artista);
});

app.Run();

record Artista(int Id, string Nombre, string Pais, string GeneroPrincipal, int AnioInicio);