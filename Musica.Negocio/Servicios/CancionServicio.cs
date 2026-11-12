using AutoMapper;
using Musica.Datos.Repositorios;
using Musica.Entidades;
using Musica.Entidades.Dtos;
using Musica.Negocio.Excepciones;

namespace Musica.Negocio.Servicios;

public class CancionServicio : ICancionServicio
{
    private readonly ICancionRepositorio _repo;
    private readonly IAlbumRepositorio _albumRepo;
    private readonly IMapper _mapper;

    public CancionServicio(ICancionRepositorio repo, IAlbumRepositorio albumRepo, IMapper mapper)
    {
        _repo = repo;
        _albumRepo = albumRepo;
        _mapper = mapper;
    }

    public async Task<List<CancionDto>> ListarAsync(int? albumId, string? titulo, int? duracionMax)
    {
        var canciones = await _repo.ListarAsync(albumId, titulo, duracionMax);
        return _mapper.Map<List<CancionDto>>(canciones);
    }

    // Devuelve null si el álbum no existe (ruta anidada)
    public async Task<List<CancionDto>?> ListarPorAlbumAsync(int albumId)
    {
        if (!await _albumRepo.ExisteAsync(albumId)) return null;

        var canciones = await _repo.ListarPorAlbumAsync(albumId);
        return _mapper.Map<List<CancionDto>>(canciones);
    }

    public async Task<CancionDto?> ObtenerPorIdAsync(int id)
    {
        var cancion = await _repo.ObtenerPorIdAsync(id);
        return cancion == null ? null : _mapper.Map<CancionDto>(cancion);
    }

    public async Task<CancionDto> CrearAsync(CancionCrearDto dto)
    {
        if (!await _albumRepo.ExisteAsync(dto.AlbumId))
            throw new DatoInvalidoException(nameof(dto.AlbumId), "El álbum indicado no existe.");

        // Regla 2: no repetir número de pista dentro del álbum
        if (await _repo.ExistePistaAsync(dto.AlbumId, dto.NumeroPista))
            throw new ReglaNegocioException("Ya existe una canción con ese número de pista en el álbum.");

        var cancion = _mapper.Map<Cancion>(dto);
        await _repo.CrearAsync(cancion);

        var creada = await _repo.ObtenerPorIdAsync(cancion.Id);
        return _mapper.Map<CancionDto>(creada!);
    }

    public async Task<bool> ActualizarAsync(int id, CancionActualizarDto dto)
    {
        var cancion = await _repo.ObtenerParaEditarAsync(id);
        if (cancion == null) return false;

        if (!await _albumRepo.ExisteAsync(dto.AlbumId))
            throw new DatoInvalidoException(nameof(dto.AlbumId), "El álbum indicado no existe.");

        if (await _repo.ExistePistaAsync(dto.AlbumId, dto.NumeroPista, id))
            throw new ReglaNegocioException("Ya existe otra canción con ese número de pista en el álbum.");

        _mapper.Map(dto, cancion);
        await _repo.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var cancion = await _repo.ObtenerParaEditarAsync(id);
        if (cancion == null) return false;

        await _repo.EliminarAsync(cancion);
        return true;
    }
}