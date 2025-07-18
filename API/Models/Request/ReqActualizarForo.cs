namespace API.Models.Request
{
    /// <summary>
    /// Solicitud para actualizar un foro existente.
    /// </summary>
    public class ReqActualizarForo
    {
        /// <summary>
        /// Identificador del foro a actualizar.
        /// </summary>
        public int ForoId { get; set; }

        /// <summary>
        /// Identificador del administrador que edita el foro.
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Nuevo título del foro.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Nueva descripción o contenido del foro.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el foro es público (true) o privado (false).
        /// </summary>
        public bool EsPublico { get; set; }

        /// <summary>
        /// Lista de roles a los que se asignará el foro (opcional si es público).
        /// </summary>
        public List<int>? RolesAsignados { get; set; }
    }
}
