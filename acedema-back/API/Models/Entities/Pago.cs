using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Pago
    {
        public int IdPago { get; set; }
        public Persona Estudiante { get; set; }
        public Comprobante Comprobante { get; set; }
        public Matricula Matricula { get; set; }
        public string EstadoPago { get; set; }
        public string TipoPago { get; set; }
        public Persona Administrador { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
