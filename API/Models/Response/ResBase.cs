using System.Collections.Generic;

namespace API.Models.Response
{
    /// <summary>
    /// Clase base de respuestas que incluye estructura común para operaciones del sistema.
    /// </summary>
    public class ResBase
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Resultado { get; set; }

        /// <summary>
        /// Lista de errores que pueden haber ocurrido durante la operación.
        /// </summary>
        public List<string> ListaDeErrores { get; set; } = new List<string>();

        /// <summary>
        /// Mensaje de resultado general (éxito o error) para mostrar al usuario o depurar.
        /// </summary>
        public string Mensaje { get; set; } = string.Empty;
    }
}
