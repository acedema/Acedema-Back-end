using API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Models.Request
{
    public class ReqRegistrarPersona
    {
        public Persona Persona { get; set; } = null!;
    }
}
