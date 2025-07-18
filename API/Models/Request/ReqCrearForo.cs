namespace API.Models.Request
{   
    /// <summary>
    /// Solicitud para crear un nuevo foro.
    /// </summary>
    public class ReqCrearForo
    {
        /// <summary>
        /// Identificador del administrador que crea el foro.
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// Título del foro.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción o contenido del foro.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de creación del foro.
        /// </summary>
        public DateTime FechaRegistro { get; set; }

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
