using API.Models.Entities;

namespace API.Models.Response
{
    /// <summary>
    /// Respuesta para la consulta de una persona específica.
    /// Contiene la persona consultada, un mensaje, un estado de éxito y posibles errores.
    /// </summary>
    public class ResOptenerPersona : ResBase
    {
        /// <summary>
        /// Objeto con la información completa de la persona consultada.
        /// </summary>
        public Persona Persona { get; set; }
    }
}
