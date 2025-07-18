using Microsoft.AspNetCore.Mvc;
using API.Models.Request;
using API.Models.Response;
using API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Controlador para gestionar operaciones relacionadas con la matrícula escolar.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculaController : ControllerBase
    {
        private readonly LogicaMatricula _logica;

        /// <summary>
        /// Constructor del controlador de matrícula.
        /// </summary>
        /// <param name="logica">Instancia de la lógica de negocio para matrícula.</param>
        public MatriculaController(LogicaMatricula logica)
        {
            _logica = logica;
        }

        /// <summary>
        /// Obtiene la información de matrícula de un estudiante según su ID.
        /// </summary>
        /// <param name="req">Objeto con el ID de la persona para buscar matrícula.</param>
        /// <returns>
        /// - 200 OK con la información de matrícula si se encuentra.  
        /// - 400 BadRequest si el request es nulo o hay error en la búsqueda.
        /// </returns>
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

        /// <summary>
        /// Realiza el proceso de matrícula para un estudiante.
        /// </summary>
        /// <param name="req">Objeto con los datos necesarios para matricular al estudiante.</param>
        /// <returns>
        /// - 200 OK con la matrícula creada si todo sale bien.  
        /// - 400 BadRequest si el request es nulo o la matrícula falla.
        /// </returns>
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
