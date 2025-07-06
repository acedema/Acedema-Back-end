using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class ArchivosActa
    {
        public int IdArchivoActa { get; set; }
        public int IdActa { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public byte[] Archivo { get; set; }
    }
}

