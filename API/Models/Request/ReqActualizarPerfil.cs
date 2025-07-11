/// <summary>
/// Modelo para recibir datos para actualizar el perfil personal.
/// </summary>
public class ReqActualizarPerfil
{
    public int PersonaId { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public string Correo { get; set; }
    public string? Direccion { get; set; }
    public long Telefono1 { get; set; }
    public long? Telefono2 { get; set; }
}
