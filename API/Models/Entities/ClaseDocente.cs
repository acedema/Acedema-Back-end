using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class ClaseDocente
    {
        public int ClaseDocenteId { get; set; }
        public Clases clase { get; set; }
        public Persona Docente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Boolean Estado { get; set; }
        public int Capacidad { get; set; }
        public int Miembros { get; set; }
        public Cronograma Cronograma { get; set; }
    }
}
