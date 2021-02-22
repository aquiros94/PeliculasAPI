using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DTOs
{
    public class GeneroDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "El nombre tiene que tener como máximo 50 carácteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
