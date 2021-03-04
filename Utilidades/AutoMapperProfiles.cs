using AutoMapper;
using NetTopologySuite.Geometries;
using PeliculasApi.Entidades;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.Foto, o => o.Ignore());
            CreateMap<CineCreacionDTO, Cine>().ForMember(x => x.Ubicacion, o => o.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));
            CreateMap<Cine, CineDTO>().ForMember(x => x.Latitud, dto => dto.MapFrom(campo => campo.Ubicacion.Y)).ForMember(x => x.Longitud, dto => dto.MapFrom(campo => campo.Ubicacion.X));
        }
    }
}
