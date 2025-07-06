using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class AsistenciaEstudiante
    {
        public int IdAsistenciaEstudiante { get; set; }
        public ClaseEstudiante claseEstudiante { get; set; }
        public string Asistencia { get; set; }
        public DateTime FechaRegistro { get; set; }

    }
}
