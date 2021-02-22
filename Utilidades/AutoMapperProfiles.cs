using AutoMapper;
using PeliculasApi.Entidades;
using PeliculasAPI.DTOs;

namespace PeliculasAPI.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
        }
    }
}
