using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Prestamos
    {
        public int IdPrestamo { get; set; }
        public Instrumento Instrumento { get; set; }
        public Persona Estudiante { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime EstadoPrestamo { get; set; }
        public Persona Administrador { get; set; }
    }
}
