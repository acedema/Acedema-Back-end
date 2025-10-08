using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    [Table("Persona")]
    public class Persona
    {
        [Column("id_persona")]
        public int PersonaId { get; set; }
        [Column("num_cedula")]
        public int NumCedula { get; set; }
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        [Column("primer_nombre")]        
        public string PrimerNombre { get; set; }
        [Column("segundo_nombre")]
        public string SegundoNombre { get; set; }
        [Column("primer_apellido")]
        public string PrimerApellido { get; set; }
        [Column("segundo_apellido")]
        public string SegundoApellido { get; set; }
        [Column("correo")]
        public string Correo {  get; set; }
        [Column("contraseña")]
        public string? Password { get; set; }
        [Column("Direccion")]
        public string Direccion { get; set; }
        [Column("telefono_1")]
        public int Telefono1 { get; set; }
        [Column("telefono_2")]
        public int Telefono2 { get; set; }
        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }
        [Column("id_Rol")]
        public int IdRol { get; set; }
        [Column("puesto")]
        public string Puesto { get; set; }
        [Column("cedula_responsable")]
        public int? CedulaResponsable { get; set; }

        // Propiedad adicional que NO está en la tabla Persona, sino se obtiene en la consulta con JOIN
        public string NombreRol { get; set; } = string.Empty;

    }
}
