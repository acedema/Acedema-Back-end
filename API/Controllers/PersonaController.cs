using Microsoft.AspNetCore.Mvc;
using API.Models.Request;
using API.Models.Response;
using API.Services;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly LogicaUtilitarios _logicaUtilitarios;
        private readonly LogicaPersona _logica;
        private readonly IConfiguration _configuration;

        public PersonaController(LogicaPersona logica, LogicaUtilitarios logicaUtilitarios, IConfiguration configuration)
        {
            _configuration = configuration;
            _logica = logica;
            _logicaUtilitarios = logicaUtilitarios;
        }

        // POST api/persona/obtenerPersona
        /// <summary>
        /// Obtiene los datos de una persona a partir del ID proporcionado.
        /// </summary>
        /// <param name="req">Objeto que contiene el ID de la persona.</param>
        /// <returns>Respuesta con los datos de la persona o los errores ocurridos.</returns>
        [HttpPost("obtenerPersona")]
        public async Task<ActionResult<ResOptenerPersona>> ObtenerPersona([FromBody] ReqObtenerPersona req)
        {
            if (req is null)
            {
                return BadRequest(new ResOptenerPersona
                {
                    Resultado = false,
                    ListaDeErrores = new List<string> { "Request nulo" }
                });
            }

            var result = await _logica.ObtenerPersonaAsync(req);

            if (!result.Resultado)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Registra una nueva persona en el sistema. El administrador es quien crea la cuenta.
        /// </summary>
        /// <param name="req">Datos de la persona a registrar.</param>
        /// <returns>Resultado del registro incluyendo errores o el ID generado.</returns>
        // POST api/persona/registrarPersona
        [HttpPost("registrarPersona")]
        public async Task<ActionResult<ResRegistrarPersona>> RegistrarPersona([FromBody] ReqRegistrarPersona req)
        {
            if (req is null)
                return BadRequest(new ResRegistrarPersona {
                    Resultado = false,
                    ListaDeErrores = new List<string> { "Request nulo" }
                });

            var result = await _logica.RegistrarPersonaAsync(req);
            if (!result.Resultado)
                return BadRequest(result);
            return Ok(result);

        }

        /// <summary>
        /// Permite a un usuario actualizar su contraseña una vez iniciada sesión con la temporal.
        /// </summary>
        /// <param name="req">Correo, contraseña actual (temporal) y nueva contraseña.</param>
        /// <returns>Resultado del proceso de actualización de la contraseña.</returns>
        [HttpPost("actualizarContrasena")]
        public async Task<ActionResult<ResRestablecerContrasena>> ActualizarContrasena([FromBody] ReqRestablecerContrasena req)
        {
            if (req == null)
                return BadRequest(new ResRestablecerContrasena { Resultado = false, ListaDeErrores = new List<string> { "Request nulo" } });

            var res = await _logica.ActualizarContrasenaAsync(req);
            return Ok(res);
        }
        

        [HttpPost("solicitar-recuperacion")]
        public async Task<IActionResult> SolicitarRecuperacion([FromBody] ReqCorreo req)
        {
            if (string.IsNullOrWhiteSpace(req.Correo))
                return BadRequest("El correo es obligatorio.");

            var resExiste = await _logica.ExistePersonaPorCorreoAsync(req.Correo);

            // Verifica si hubo error al consultar
            if (!resExiste.Resultado)
                return StatusCode(500, "Error al verificar el correo.");

            // Verifica si realmente existe
            if (!resExiste.Existe)
                return NotFound("Correo no registrado.");

            var jwtHelper = new JwtTokenHelper(_configuration);
            var token = jwtHelper.GenerarTokenRestablecer(req.Correo);

            var urlRecuperacion = $"https://tusitio.com/restablecer?token={token}";

            bool emailEnviado = await _logicaUtilitarios.EnviarCorreoRecuperacionAsync(req.Correo, urlRecuperacion);
            if (!emailEnviado)
                return StatusCode(500, "Error enviando el correo de recuperación.");

            return Ok("Se ha enviado un enlace para restablecer su contraseña.");
        }



        [HttpPost("restablecer-con-token")]
        public async Task<IActionResult> RestablecerConToken([FromBody] ReqRestablecerConToken req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Token) || string.IsNullOrWhiteSpace(req.NuevaContrasena))
                return BadRequest("Token y nueva contraseña son obligatorios.");
            var jwtHelper = new JwtTokenHelper(_configuration);

            try
            {
                var claims = jwtHelper.ValidarToken(req.Token);
                var correo = claims.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(correo))
                    return BadRequest("Token inválido.");

                // Validar contraseña (puedes crear método aparte para esto)
                if (req.NuevaContrasena.Length < 8) // ejemplo simple
                    return BadRequest("La nueva contraseña debe tener al menos 8 caracteres.");

                // Encriptar contraseña
                var utilitarios = new LogicaUtilitarios(_configuration);
                string nuevaPassHash = utilitarios.Encriptar(req.NuevaContrasena);

                var resultado = await _logica.ActualizarContrasenaPorCorreoAsync(correo, nuevaPassHash);
                if (!resultado.Resultado)
                    return StatusCode(500, $"Error actualizando la contraseña: {string.Join(", ", resultado.ListaDeErrores)}");

                return Ok("Contraseña restablecida con éxito.");
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest($"Token inválido o expirado: {ex.Message}");
            }
        }



    }
}

