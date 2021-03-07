using AutoMapper;
using NetTopologySuite.Geometries;
using PeliculasApi.Entidades;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System.Collections.Generic;

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
            CreateMap<PeliculaCreacionDTO, Pelicula>().ForMember(x => x.Poster, o => o.Ignore()).ForMember(x => x.PeliculasGeneros, o => o.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.PeliculasCines, o => o.MapFrom(MapearPeliculasCines)).ForMember(x => x.PeliculasActores, o => o.MapFrom(MapearPeliculasActores));
            CreateMap<Pelicula, PeliculaDTO>().ForMember(x => x.Generos, o => o.MapFrom(MapearPeliculaGeneros))
                .ForMember(x => x.Actores, o => o.MapFrom(MapearPeliculasActores)).ForMember(x => x.Cines, o => o.MapFrom(MapearPeliculasCines));
        }

        private List<PeliculaActorDTO> MapearPeliculasActores(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            List<PeliculaActorDTO> resultado;

            resultado = new List<PeliculaActorDTO>();

            if (pelicula.PeliculasActores != null)
            {
                foreach (PeliculasActores actores in pelicula.PeliculasActores)
                {
                    resultado.Add(new PeliculaActorDTO() { Id = actores.ActorId, Nombre = actores.Actor.Nombre, Foto = actores.Actor.Foto, Orden = actores.Orden, Personaje = actores.Personaje });
                }
            }

            return resultado;
        }

        private List<CineDTO> MapearPeliculasCines(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            List<CineDTO> resultado;

            resultado = new List<CineDTO>();

            if (pelicula.PeliculasGeneros != null)
            {
                foreach (PeliculasCines cines in pelicula.PeliculasCines)
                {
                    resultado.Add(new CineDTO() { Id = cines.CineId, Nombre = cines.Cine.Nombre, Latitud = cines.Cine.Ubicacion.X, Longitud = cines.Cine.Ubicacion.Y });
                }
            }

            return resultado;
        }

        private List<GeneroDTO> MapearPeliculaGeneros(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            List<GeneroDTO> resultado;

            resultado = new List<GeneroDTO>();

            if (pelicula.PeliculasGeneros != null)
            {
                foreach (PeliculasGeneros genero in pelicula.PeliculasGeneros)
                {
                    resultado.Add(new GeneroDTO() { Id = genero.GeneroId, Nombre = genero.Genero.Nombre});
                }
            }

            return resultado;
        } 

        private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            List<PeliculasActores> resultado;

            resultado = new List<PeliculasActores>();
            if (peliculaCreacionDTO.Actores == null) { return resultado; }

            foreach (ActorPeliculaCreacionDTO actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.Id, Personaje = actor.Personaje });
            }

            return resultado;
        }

        private List<PeliculasCines> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            List<PeliculasCines> resultado;

            resultado = new List<PeliculasCines>();
            if (peliculaCreacionDTO.CinesIds == null) { return resultado; }

            foreach (int id in peliculaCreacionDTO.CinesIds)
            {
                resultado.Add(new PeliculasCines() { CineId = id });
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            List<PeliculasGeneros> resultado;

            resultado = new List<PeliculasGeneros>();
            if (peliculaCreacionDTO.GenerosIds == null) { return resultado; }

            foreach (int id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new PeliculasGeneros() { GeneroId = id });
            }

            return resultado;
        }
    }
}
