using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [Route("api/actores")]
    [ApiController()]
    public class ActoresController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ActoresController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.context = applicationDbContext;
            this.mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> Post([FromBody] ActorCreacionDTO nuevoActor)
        {
            context.Actores.Add(mapper.Map<Actor>(nuevoActor));
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
