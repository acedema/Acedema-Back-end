namespace API.Models.Request
{
    public class ReqRestablecerContrasena
    {
        public required string Correo { get; set; }
        public required string ContrasenaActual { get; set; }
        public required string NuevaContrasena { get; set; }
    }
}
