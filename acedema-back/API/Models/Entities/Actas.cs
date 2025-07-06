using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Actas
    {
        public int IdActa { get; set; }
        public Persona Administrador { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public List<Rol> ListaRoles { get; set; }
    }
}

