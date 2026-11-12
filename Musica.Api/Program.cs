using Microsoft.EntityFrameworkCore;
using Musica.Datos;
using Musica.Datos.Repositorios;
using Musica.Negocio.Mapeos;
using Musica.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DbContext
builder.Services.AddDbContext<MusicaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MusicaDB")));

// Repositorios
builder.Services.AddScoped<IAlbumRepositorio, AlbumRepositorio>();
builder.Services.AddScoped<ICancionRepositorio, CancionRepositorio>();

// Servicios
builder.Services.AddScoped<IAlbumServicio, AlbumServicio>();
builder.Services.AddScoped<ICancionServicio, CancionServicio>();

// AutoMapper (busca el MappingProfile en la capa de Negocio)
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Musica API en capas v1");
    options.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();