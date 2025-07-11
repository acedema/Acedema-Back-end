namespace API.Models.Request
{
    public class ReqObtenerPersona
    {
        public int? PersonaId { get; set; }  // nullable para que pueda no enviarse
        public string? Correo { get; set; }  // nullable para buscar por correo
    }
}
