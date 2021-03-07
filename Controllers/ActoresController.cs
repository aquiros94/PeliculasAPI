using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Utilidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [Route("api/actores")]
    [ApiController()]
    public class ActoresController : Controller
    {
        private const string CONTENEDOR = "actores";

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;


        public ActoresController(ApplicationDbContext applicationDbContext, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = applicationDbContext;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            IQueryable<Actor> actores;

            actores = context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(actores);

            return mapper.Map<List<ActorDTO>>(await actores.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync());
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            Actor actorEncontrado;
            actorEncontrado = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actorEncontrado == null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(actorEncontrado);
        }

        [HttpPost()]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO nuevoActor)
        {
            Actor actor;

            actor = mapper.Map<Actor>(nuevoActor);
            if (nuevoActor.Foto != null)
            {
                actor.Foto = await almacenadorArchivos.GuardarArchivo(CONTENEDOR, nuevoActor.Foto);
            }

            context.Actores.Add(actor);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("BuscarPorNombre")]
        public async Task<ActionResult<List<PeliculaActorDTO>>> BuscarPorNombre([FromBody]string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return new List<PeliculaActorDTO>();
            }

            return await context.Actores.Where(x => x.Nombre.Contains(nombre)).Select(x => new PeliculaActorDTO { Id = x.Id, Nombre = x.Nombre, Foto = x.Foto }).Take(5).ToListAsync();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorModificar)
        {
            Actor actorEncontrado;
            actorEncontrado = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actorEncontrado == null)
            {
                return NotFound();
            }

            if (actorModificar.Foto != null)
            {
                actorEncontrado.Foto = await almacenadorArchivos.EditarArchivo(CONTENEDOR, actorModificar.Foto, actorEncontrado.Foto);
            }

            actorEncontrado = mapper.Map(actorModificar, actorEncontrado);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Actor actorEncontrado;

            actorEncontrado = await context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actorEncontrado == null)
            {
                return NotFound();
            }

            if (actorEncontrado.Foto != null)
            {
                await almacenadorArchivos.BorrarArchivo(actorEncontrado.Foto, CONTENEDOR);
            }

            context.Actores.Remove(actorEncontrado);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
