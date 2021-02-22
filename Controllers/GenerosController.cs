using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeliculasApi.Entidades;
using PeliculasAPI.DTOs;
using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            return mapper.Map<List<GeneroDTO>>(await context.Generos.ToListAsync());
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Genero>> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO genero)
        {
            context.Generos.Add(mapper.Map<Genero>(genero));
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }


    }
}
