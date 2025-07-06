using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.Request
{
    public class ReqLoginPersona
    {
        public string Correo { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
}
