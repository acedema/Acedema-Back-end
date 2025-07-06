namespace API.Models.Request
{
    public class ReqRestablecerContrasena
    {
        public string Correo { get; set; }
        public string ContrasenaActual { get; set; }
        public string NuevaContrasena { get; set; }
    }
}
