using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Bitacora
    {
        public int BitacoraId { get; set; }
        public int IdSemana { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
    }
}
