using AutoMapper;
using Musica.Datos.Repositorios;
using Musica.Entidades;
using Musica.Entidades.Dtos;
using Musica.Negocio.Excepciones;

namespace Musica.Negocio.Servicios;

public class AlbumServicio : IAlbumServicio
{
    private readonly IAlbumRepositorio _repo;
    private readonly IMapper _mapper;

    public AlbumServicio(IAlbumRepositorio repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<AlbumDto>> ListarAsync(string? artista, string? genero, int? anio, string? q)
    {
        var albumes = await _repo.ListarAsync(artista, genero, anio, q);
        return _mapper.Map<List<AlbumDto>>(albumes);
    }

    public async Task<AlbumDto?> ObtenerPorIdAsync(int id)
    {
        var album = await _repo.ObtenerPorIdAsync(id);
        return album == null ? null : _mapper.Map<AlbumDto>(album);
    }

    public async Task<AlbumDto> CrearAsync(AlbumCrearDto dto)
    {
        if (await _repo.ExisteDuplicadoAsync(dto.Titulo, dto.Artista))
            throw new ReglaNegocioException("Ya existe un álbum con ese título para este artista.");

        var album = _mapper.Map<Album>(dto);
        await _repo.CrearAsync(album);
        return _mapper.Map<AlbumDto>(album);
    }

    public async Task<bool> ActualizarAsync(int id, AlbumActualizarDto dto)
    {
        var album = await _repo.ObtenerParaEditarAsync(id);
        if (album == null) return false;

        if (await _repo.ExisteDuplicadoAsync(dto.Titulo, dto.Artista, id))
            throw new ReglaNegocioException("Ya existe otro álbum con ese título para este artista.");

        _mapper.Map(dto, album);
        await _repo.GuardarCambiosAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var album = await _repo.ObtenerParaEditarAsync(id);
        if (album == null) return false;

        if (await _repo.TieneCancionesAsync(id))
            throw new ReglaNegocioException("No se puede eliminar un álbum que tiene canciones.");

        await _repo.EliminarAsync(album);
        return true;
    }
}