using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class AsistenciaDocente
    {
        public int IdAsistenciaDocente { get; set; }
        public int IdClaseDocente { get; set; }
        public string Asistencia { get; set; }
        public DateTime FechaRegistro { get; set; }

    }
}
