using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Cronograma
    {
        public int CronogramaId { get; set; }
        public string LinkTeams { get; set; }
        public int IdClaseDocente { get; set; }
        public DateTime FechaRegistro { get; set; }
        public List<Semana> Semanas { get; set; }   
    }
}
