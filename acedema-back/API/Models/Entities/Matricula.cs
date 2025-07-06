using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    [Table("Matricula")]

    public class Matricula
    {
        [Column("id_matricula")]
        public int MatriculaId { get; set; }

        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateTime FechaFin { get; set; }
        public List<ClaseEstudiante> Matriculados { get; set; }
    
    }
}
