using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class ArchivosForo
    {
        public int IdArchivoForo { get; set; }
        public int IdForo { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public byte[] Archivo { get; set; }
    }
}
