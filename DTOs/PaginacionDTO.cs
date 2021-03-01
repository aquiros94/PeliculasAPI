namespace PeliculasAPI.DTOs
{
    public class PaginacionDTO
    {
        private const int CANTIDAD_MAXIMA_REGISTROS_PAGINA = 10;

        public int Pagina { get; set; } = 1;
        public int registrosPorPagina = 10;
        public int RegistrosPorPagina
        {
            get
            {
                return registrosPorPagina;
            }
            set
            {
                registrosPorPagina = (value > CANTIDAD_MAXIMA_REGISTROS_PAGINA) ? CANTIDAD_MAXIMA_REGISTROS_PAGINA : value;
            }
        }
    }
}
