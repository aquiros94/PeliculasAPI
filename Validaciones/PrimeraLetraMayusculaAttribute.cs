using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.Validaciones
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string primeraLetra;

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            primeraLetra = value.ToString()[0].ToString();

            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra tiene que ser mayúscula");
            }
            return ValidationResult.Success;
        }
    }
}
