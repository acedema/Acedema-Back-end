using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class ArchivosAplicar
    {
        public int IdArchivoAplicar { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public byte[] Archivo { get; set; }
    }
}

