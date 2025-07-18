namespace API.Models.Request
{
    /// <summary>
    /// Solicitud para eliminar un foro.
    /// </summary>
    public class ReqEliminarForo
    {
        /// <summary>
        /// Identificador del foro a eliminar.
        /// </summary>
        public int ForoId { get; set; }

        /// <summary>
        /// Identificador del administrador que solicita la eliminación.
        /// </summary>
        public int AdminId { get; set; }
    }
}
