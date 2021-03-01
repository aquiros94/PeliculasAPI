using PeliculasAPI.DTOs;
using System.Linq;

namespace PeliculasAPI.Utilidades
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> lista, PaginacionDTO paginacionDTO)
        {
            return lista.Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RegistrosPorPagina).Take(paginacionDTO.RegistrosPorPagina);
        }
    }
}
