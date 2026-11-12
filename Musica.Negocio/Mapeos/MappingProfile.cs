using AutoMapper;
using Musica.Entidades;
using Musica.Entidades.Dtos;

namespace Musica.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Album, AlbumDto>()
            .ForMember(dest => dest.TotalCanciones, opt => opt.MapFrom(src => src.Cancions.Count));

        CreateMap<Cancion, CancionDto>()
            .ForMember(dest => dest.AlbumTitulo, opt => opt.MapFrom(src => src.Album.Titulo));

        CreateMap<AlbumCrearDto, Album>();
        CreateMap<AlbumActualizarDto, Album>();
        CreateMap<CancionCrearDto, Cancion>();
        CreateMap<CancionActualizarDto, Cancion>();
    }
}