using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    /// <summary>
    /// Entidad que representa la relación entre Foros y Roles.
    /// </summary>
    [Table("Foro_Roles")]
    public class ForoRol
    {
        [Column("idregistro")]
        public int IdRegistro { get; set; }

        [Column("id_foro")]
        public int ForoId { get; set; }

        [Column("id_rol")]
        public int RolId { get; set; }
    }

}
