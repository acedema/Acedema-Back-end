using System;
using System.Collections.Generic;

namespace API.Models.Entities
{
    public class Foro
    {
        public int IdForo { get; set; }
        public Persona Administrador { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRegistro { get; set; }

        public List<Rol> ListaRoles { get; set; }

    }
}
