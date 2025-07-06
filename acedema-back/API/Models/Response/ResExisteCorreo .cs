namespace API.Models.Response
{
    /// <summary>
    /// Respuesta para verificación de existencia de correo.
    /// </summary>
    public class ResExisteCorreo : ResBase
    {
        /// <summary>
        /// Indica si el correo existe en la base de datos.
        /// </summary>
        public bool Existe { get; set; }
    }
}
