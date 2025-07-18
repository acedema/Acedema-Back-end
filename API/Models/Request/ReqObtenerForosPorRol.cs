namespace API.Models.Request
{
    /// <summary>
    /// Solicitud para obtener foros según el rol del usuario.
    /// </summary>
    public class ReqObtenerForosPorRol
    {
        /// <summary>
        /// Identificador del rol para filtrar los foros.
        /// </summary>
        public int RolId { get; set; }
    }
}
