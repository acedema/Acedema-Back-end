using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class ClaseEstudiante
    {
        public int ClaseEstudianteId { get; set; }
        public Persona Estudiante { get; set; }
        public ClaseDocente claseDocente { get; set; }
 
    }
}
