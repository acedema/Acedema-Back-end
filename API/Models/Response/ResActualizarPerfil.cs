using API.Models.Entities;
using System.Collections.Generic;

namespace API.Models.Response
{
    /// <summary>
    /// Respuesta para la operación de actualizar persona.
    /// </summary>
    public class ResActualizarPerfil : ResBase
    {
        public Persona Persona { get; set; }
    }
}
