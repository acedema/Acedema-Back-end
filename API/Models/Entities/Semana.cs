using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Semana
    {
        public int SemanaId { get; set; }
        public int IdCronograma { get; set; }
        public DateTime Fecha { get; set; }
        public string Contenido { get; set; }
        public List<Bitacora> Bitacoras { get; set; }

    }
}
