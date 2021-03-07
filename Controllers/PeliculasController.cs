using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Entidades;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [Route("api/peliculas")]
    [ApiController()]
    public class PeliculasController : ControllerBase
    {
        private const string CONTENEDOR = "peliculas";

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        public PeliculasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            Pelicula pelicula;
            PeliculaDTO peliculaDTO;

            pelicula = await context.Peliculas.Include(x => x.PeliculasGeneros).ThenInclude(x => x.Genero)
                .Include(x => x.PeliculasActores).ThenInclude(x => x.Actor)
                .Include(x => x.PeliculasCines).ThenInclude(x => x.Cine)
                .FirstAsync(x => x.Id == id);

            if (pelicula == null) { return NotFound(); }

            peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            peliculaDTO.Actores = peliculaDTO.Actores.OrderBy(x => x.Orden).ToList();

            return peliculaDTO;
        }

        [HttpPost()]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacion)
        {
            Pelicula pelicula;

            pelicula = mapper.Map<Pelicula>(peliculaCreacion);

            if (peliculaCreacion.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(CONTENEDOR, peliculaCreacion.Poster);
            }

            OrdenActores(pelicula);

            context.Add(pelicula);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("postget")]
        public async Task<ActionResult<PeliculaPostGetDTO>> PostGet()
        {
            return new PeliculaPostGetDTO() { Generos = mapper.Map<List<GeneroDTO>>(await context.Generos.ToListAsync()), Cines = mapper.Map<List<CineDTO>>(await context.Cines.ToListAsync()) };
        }

        private void OrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }
    }
}
