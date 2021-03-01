using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeliculasApi.Entidades;
using PeliculasAPI.DTOs;
using PeliculasAPI.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [Route("api/generos")]
    [ApiController()]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController : Controller
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ILogger<GenerosController> logger, ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            IQueryable<Genero> generos;

            generos = context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(generos);

            return mapper.Map<List<GeneroDTO>>(await generos.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync());
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            Genero generoEncontrado;
            generoEncontrado = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if (generoEncontrado == null)
            {
                return NotFound();
            }
            return mapper.Map<GeneroDTO>(generoEncontrado);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO genero)
        {
            context.Generos.Add(mapper.Map<Genero>(genero));
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoModificar)
        {
            Genero generoEncontrado;
            generoEncontrado = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if (generoEncontrado == null)
            {
                return NotFound();
            }

            generoEncontrado = mapper.Map(generoModificar, generoEncontrado);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Genero generoEncontrado;
            generoEncontrado = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if (generoEncontrado == null)
            {
                return NotFound();
            }

            context.Generos.Remove(generoEncontrado);
            await context.SaveChangesAsync();

            return NoContent();
        }


    }
}
