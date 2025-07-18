using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    /// <summary>
    /// Entidad que representa un Foro o post en el sistema.
    /// </summary>
    [Table("Foro")]
    public class Foro
    {
        [Column("id_foro")]
        public int ForoId { get; set; }

        [Column("id_administrador")]
        public int AdminId { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [Column("espublico")]
        public bool EsPublico { get; set; }

        [Column("FechaUltimaModificacion")]
        public DateTime? FechaUltimaModificacion { get; set; }

        /// <summary>
        /// Lista de roles asignados al foro (relación con foro_roles).
        /// </summary>
        public List<ForoRol>? RolesAsignados { get; set; }
    }

}
