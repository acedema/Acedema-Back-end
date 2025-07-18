using API.Models.Entities;

namespace API.Models.Response
{
    /// <summary>
    /// Respuesta para creación o actualización de foro.
    /// </summary>
    public class ResCrearActualizarForo : ResBase
    {
        /// <summary>
        /// Id del foro creado o actualizado.
        /// </summary>
        public int ForoId { get; set; }

        /// <summary>
        /// Objeto Foro actualizado (o creado).
        /// </summary>
        public Foro? ForoActualizado { get; set; }
    }
}
