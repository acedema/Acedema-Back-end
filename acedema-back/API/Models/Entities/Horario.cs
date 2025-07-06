using System;
using System.Collections.Generic;


namespace API.Models.Entities
{
    public class Horario
    {
        public int IdHorario { get; set; }
        public int IdClaseDocente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinal { get; set; }
    }
}
