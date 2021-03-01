using Microsoft.EntityFrameworkCore;
using PeliculasApi.Entidades;
using PeliculasAPI.Entidades;

namespace PeliculasAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
    }
}
