using Microsoft.AspNetCore.Mvc;
using API.Models.Request;
using API.Models.Response;
using API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculaController : ControllerBase
    {
        private readonly LogicaMatricula _logica;

        public MatriculaController(LogicaMatricula logica)
        {
            _logica = logica;
        }

        [HttpPost("obtenerMatricula")]
        public async Task<ActionResult<ResOptenerMatricula>> ObtenerMatricula([FromBody] ReqOptenerMatricula req)
        {
            if (req == null)
            {
                return BadRequest(new ResOptenerMatricula
                {
                    Resultado = false,
                    ListaDeErrores = new List<string> { "Request nulo" }
                });
            }

            var result = await _logica.BuscarMatriculaAsync(req);
            if (!result.Resultado)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("realizarMatricula")]
        public async Task<ActionResult<ResMatricular>> RealizarMatricula([FromBody] ReqMatricular req)
        {
            if (req == null)
            {
                return BadRequest(new ResMatricular
                {
                    Resultado = false,
                    ListaDeErrores = new List<string> { "Request nulo" }
                });
            }

            var result = await _logica.MatricularAsync(req);
            if (!result.Resultado)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

