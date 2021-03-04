using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;
using PeliculasAPI.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Controllers
{
    [Route("api/cines")]
    [ApiController]
    public class CinesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CinesController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.context = applicationDbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CineDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            IQueryable<Cine> cines;

            cines = context.Cines.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(cines);

            return mapper.Map<List<CineDTO>>(await cines.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync());
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<CineDTO>> Get(int id)
        {
            Cine cineEncontrado;
            cineEncontrado = await context.Cines.FirstOrDefaultAsync(x => x.Id == id);

            if (cineEncontrado == null)
            {
                return NotFound();
            }
            return mapper.Map<CineDTO>(cineEncontrado);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDTO cine)
        {
            context.Cines.Add(mapper.Map<Cine>(cine));
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CineCreacionDTO cineModificar)
        {
            Cine cineEncontrado;
            cineEncontrado = await context.Cines.FirstOrDefaultAsync(x => x.Id == id);

            if (cineEncontrado == null)
            {
                return NotFound();
            }

            cineEncontrado = mapper.Map(cineModificar, cineEncontrado);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            Cine cineEncontrado;
            cineEncontrado = await context.Cines.FirstOrDefaultAsync(x => x.Id == id);

            if (cineEncontrado == null)
            {
                return NotFound();
            }

            context.Cines.Remove(cineEncontrado);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
