using Microsoft.AspNetCore.Mvc;
using API.Models.Request;
using API.Models.Response;
using API.Services;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; 

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly LogicaUtilitarios _logicaUtilitarios;
        private readonly LogicaPersona _logica;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenHelper _jwtHelper;

        public PersonaController(LogicaPersona logica, LogicaUtilitarios logicaUtilitarios, IConfiguration configuration, JwtTokenHelper jwtHelper)
        {
            _configuration = configuration;
            _logica = logica;
            _logicaUtilitarios = logicaUtilitarios;
            _jwtHelper = jwtHelper;
        }


        /// <summary>
        /// Obtiene los datos de una persona a partir del ID proporcionado.
        /// </summary>
        /// <param name="req">Objeto que contiene el ID de la persona.</param>
        /// <returns>Respuesta con los datos de la persona o los errores ocurridos.</returns>
        // POST api/persona/obtenerPersona
        [Authorize]
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

        [Authorize]
        [HttpGet("obtenerMiPerfil")]
        public async Task<ActionResult<ResOptenerPersona>> obtenerMiPerfil()
        {
            // Obtener correo del token
            var correo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(correo))
                return Unauthorized("Token inválido o correo no encontrado.");

            var req = new ReqObtenerPersona
            {
                PersonaId = 0, // No usar id, se busca por correo
                Correo = correo
            };

            var resultado = await _logica.ObtenerPersonaPorCorreoAsync(req);

            if (!resultado.Resultado)
                return NotFound(resultado);

            return Ok(resultado);
        }


        /// <summary>
        /// Registra una nueva persona en el sistema. El administrador es quien crea la cuenta.
        /// </summary>
        /// <param name="req">Datos de la persona a registrar.</param>
        /// <returns>Resultado del registro incluyendo errores o el ID generado.</returns>
        // POST api/persona/registrarPersona
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<ResRestablecerContrasena>> ActualizarContrasena([FromBody] ReqRestablecerContrasena req)
        {
            if (req == null)
                return BadRequest(new ResRestablecerContrasena { Resultado = false, ListaDeErrores = new List<string> { "Request nulo" } });

            var res = await _logica.ActualizarContrasenaAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// Solicita la recuperación de contraseña para un correo registrado.
        /// </summary>
        /// <param name="req">Objeto que contiene el correo del usuario.</param>
        /// <returns>
        /// - 200 OK si se envió correctamente el correo con el enlace de recuperación.  
        /// - 400 BadRequest si el correo es nulo o vacío.  
        /// - 404 NotFound si el correo no está registrado.  
        /// - 500 InternalServerError si ocurrió un error durante la verificación o el envío del correo.
        /// </returns>
        [HttpPost("solicitar-recuperacion")]
        public async Task<IActionResult> SolicitarRecuperacion([FromBody] ReqCorreo req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Correo))
                return BadRequest("El correo es obligatorio.");

            var resExiste = await _logica.ExistePersonaPorCorreoAsync(req.Correo);

            if (!resExiste.Resultado)
                return StatusCode(500, "Error al verificar el correo.");

            if (!resExiste.Existe)
                return NotFound("Correo no registrado.");

            var token = _jwtHelper.GenerarTokenRestablecer(req.Correo);

            var urlRecuperacion = $"https://tusitio.com/restablecer?token={token}";

            bool emailEnviado = await _logicaUtilitarios.EnviarCorreoRecuperacionAsync(req.Correo, urlRecuperacion);

            if (!emailEnviado)
                return StatusCode(500, "Error enviando el correo de recuperación.");

            return Ok("Se ha enviado un enlace para restablecer su contraseña.");
        }


        /// <summary>
        /// Restablece la contraseña de un usuario utilizando un token JWT de recuperación.
        /// </summary>
        /// <param name="req">Objeto que contiene el token JWT y la nueva contraseña.</param>
        /// <returns>
        /// - 200 OK si la contraseña se actualizó correctamente.  
        /// - 400 BadRequest si faltan datos, el token es inválido o la contraseña es muy corta.  
        /// - 500 InternalServerError si ocurre un error al actualizar la contraseña.
        /// </returns>
        [HttpPost("restablecer-con-token")]
        public async Task<IActionResult> RestablecerConToken ([FromBody] ReqRestablecerConToken req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.Token) || string.IsNullOrWhiteSpace(req.NuevaContrasena))
                return BadRequest("Token y nueva contraseña son obligatorios.");

            try
            {
                var claims = _jwtHelper.ValidarToken(req.Token);
                var correo = claims.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(correo))
                    return BadRequest("Token inválido.");

                if (req.NuevaContrasena.Length < 8)
                    return BadRequest("La nueva contraseña debe tener al menos 8 caracteres.");

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


        /// <summary>
        /// Autentica a un usuario con correo y contraseña, y devuelve un token JWT si es válido.
        /// </summary>
        /// <param name="req">Objeto que contiene las credenciales del usuario (correo y contraseña).</param>
        /// <returns>
        /// Respuesta HTTP:
        /// - 200 OK con un objeto que incluye el token JWT y datos básicos del usuario en caso de éxito.
        /// - 401 Unauthorized si las credenciales son incorrectas.
        /// - 400 BadRequest si el request es nulo o inválido.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ReqLoginPersona req)
        {
            // 1. Validar que el request no sea nulo y que los campos no estén vacíos
            if (req == null || string.IsNullOrWhiteSpace(req.Correo) || string.IsNullOrWhiteSpace(req.Contrasena))
            {
                return BadRequest(new { message = "El correo y la contraseña son obligatorios." });
            }

            // 2. Llamar a la lógica para validar las credenciales mediante procedimiento almacenado
            var resLogin = await _logica.ValidarLoginAsync(req);

            // 3. Si la validación falla, responder con 401 Unauthorized y mensaje
            if (!resLogin.Resultado)
                return Unauthorized(new { message = resLogin.Mensaje });

            // Usar el nombre del rol directamente desde la base de datos
            string rolNombre = resLogin.Persona.NombreRol ?? "Usuario";

            // Generar token con correo y rol real
            string token = _jwtHelper.GenerarTokenLogin(resLogin.Persona.Correo, rolNombre);


            // Devolver respuesta 200 OK con el token y los datos relevantes del usuario
            return Ok(new
            {
                token,
                usuario = new
                {
                    resLogin.Persona.PersonaId,
                    resLogin.Persona.PrimerNombre,
                    resLogin.Persona.SegundoNombre,
                    resLogin.Persona.PrimerApellido,
                    resLogin.Persona.SegundoApellido,
                    resLogin.Persona.Correo,
                    resLogin.Persona.IdRol,
                    resLogin.Persona.Puesto,
                    resLogin.Persona.NombreRol // También lo puedes enviar si quieres

                }
            });
        }

        /// <summary>
        /// Endpoint para que un usuario autorizado actualice su perfil personal.
        /// Valida que el correo en el token coincida con el correo del request para evitar modificaciones no autorizadas.
        /// </summary>
        /// <param name="req">Datos del perfil a actualizar</param>
        /// <returns>Resultado de la operación</returns>
        [Authorize]
        [HttpPut("actualizarMiPerfil")]
        public async Task<IActionResult> ActualizarMiPerfil([FromBody] ReqActualizarPerfil req)
        {
            if (req == null)
                return BadRequest("Request nulo.");

            var correoToken = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(correoToken))
                return Unauthorized("Token inválido.");

            if (!string.Equals(req.Correo, correoToken, StringComparison.OrdinalIgnoreCase))
                return Forbid("No puede actualizar datos de otro usuario.");

            // ✅ Obtener PersonaId real desde el correo
            var personaActual = await _logica.ObtenerPersonaPorCorreoAsync(new ReqObtenerPersona
            {
                Correo = correoToken
            });

            if (!personaActual.Resultado || personaActual.Persona == null)
                return NotFound("No se encontró la persona asociada al token.");

            req.PersonaId = personaActual.Persona.PersonaId;

            var resultado = await _logica.ActualizarPerfilAsync(req);

            if (!resultado.Resultado)
                return BadRequest(resultado);

            return Ok(new { mensaje = "Perfil actualizado correctamente." });
        }


    }
}

