using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Comprobante
    {
        public int ComprobanteId { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public byte[] Archivo { get; set; }
        public string Comentario { get; set; }
    }
}
