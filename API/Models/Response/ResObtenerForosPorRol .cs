using API.Models.Entities;

namespace API.Models.Response
{
    /// <summary>
    /// Respuesta para obtener lista de foros.
    /// </summary>
    public class ResObtenerForosPorRol : ResBase
    {
        /// <summary>
        /// Lista de foros obtenidos.
        /// </summary>
        public List<Foro> Foros { get; set; } = new List<Foro>();
    }
}
