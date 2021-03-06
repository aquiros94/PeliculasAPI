﻿using PeliculasApi.Validaciones;
using PeliculasAPI.Entidades;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50, ErrorMessage = "El nombre tiene que tener como máximo 50 carácteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}
