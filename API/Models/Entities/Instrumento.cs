using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Instrumento
    {
        public int IdInstrumento {  get; set; }
        public string NombreInstrumento { get; set; }
        public string CategoriaInstrumento { get; set; }
        public string EstadoInstrumento { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
