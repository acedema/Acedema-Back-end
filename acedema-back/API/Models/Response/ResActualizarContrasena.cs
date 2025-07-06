namespace API.Models.Response
{
    /// <summary>
    /// Respuesta para actualización de contraseña.
    /// </summary>
    public class ResActualizarContrasena : ResBase
    {
        /// <summary>
        /// Indica si la actualización fue exitosa.
        /// </summary>
        public bool Actualizado { get; set; }
    }
}
