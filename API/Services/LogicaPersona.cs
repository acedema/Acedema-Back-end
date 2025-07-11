using API.Data;
using API.Models.Entities;
using API.Models.Request;
using API.Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace API.Services
{
    /// <summary>
    /// Lógica de negocio relacionada con la entidad Persona.
    /// Encapsula interacciones con procedimientos almacenados para registrar, obtener o actualizar datos de personas.
    /// </summary>
    public class LogicaPersona
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor que recibe la configuración del sistema.
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación, usada para obtener la cadena de conexión.</param>
        public LogicaPersona(IConfiguration configuration)
        {
            _configuration = configuration;               
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Obtiene la información detallada de una persona a partir de su ID.
        /// </summary>
        /// <param name="req">Objeto con el ID de la persona a consultar.</param>
        /// <returns>
        /// Objeto <see cref="ResOptenerPersona"/> que contiene los datos de la persona,
        /// un mensaje de estado, el resultado de la operación y posibles errores.
        /// </returns>
        /// <remarks>
        /// Este método ejecuta el procedimiento almacenado 'TraerInfoUsuario' y procesa
        /// sus parámetros de salida. También maneja excepciones y errores personalizados
        /// definidos en el SP.
        /// </remarks>
        public async Task<ResOptenerPersona> ObtenerPersonaAsync(ReqObtenerPersona req)
        {
            var res = new ResOptenerPersona();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("TraerInfoUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetro de entrada
                    cmd.Parameters.AddWithValue("@id_persona", req.PersonaId);

                    // Parámetros de salida
                    var paramError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var paramMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    var paramIdReturn = new SqlParameter("@idReturn", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var paramResultado = new SqlParameter("@resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(paramError);
                    cmd.Parameters.Add(paramMensaje);
                    cmd.Parameters.Add(paramIdReturn);
                    cmd.Parameters.Add(paramResultado);

                    await conn.OpenAsync();

                    // Leer resultado del procedimiento si hay datos
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                res.Persona = new Persona
                                {
                                    PersonaId = reader.IsDBNull(reader.GetOrdinal("id_persona")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_persona")),
                                    NumCedula = reader.IsDBNull(reader.GetOrdinal("num_cedula")) ? 0 : reader.GetInt32(reader.GetOrdinal("num_cedula")),
                                    FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("fecha_nacimiento")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento")),
                                    PrimerNombre = reader.IsDBNull(reader.GetOrdinal("primer_nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("primer_nombre")),
                                    SegundoNombre = reader.IsDBNull(reader.GetOrdinal("segundo_nombre")) ? null : reader.GetString(reader.GetOrdinal("segundo_nombre")),
                                    PrimerApellido = reader.IsDBNull(reader.GetOrdinal("primer_apellido")) ? string.Empty : reader.GetString(reader.GetOrdinal("primer_apellido")),
                                    SegundoApellido = reader.IsDBNull(reader.GetOrdinal("segundo_apellido")) ? string.Empty : reader.GetString(reader.GetOrdinal("segundo_apellido")),
                                    Correo = reader.IsDBNull(reader.GetOrdinal("correo")) ? string.Empty : reader.GetString(reader.GetOrdinal("correo")),
                                    Password = reader.IsDBNull(reader.GetOrdinal("contraseña")) ? null : reader.GetString(reader.GetOrdinal("contraseña")),
                                    Direccion = reader.IsDBNull(reader.GetOrdinal("direccion")) ? string.Empty : reader.GetString(reader.GetOrdinal("direccion")),
                                    Telefono1 = reader.IsDBNull(reader.GetOrdinal("telefono_1")) ? 0 : reader.GetInt32(reader.GetOrdinal("telefono_1")),
                                    Telefono2 = reader.IsDBNull(reader.GetOrdinal("telefono_2")) ? 0 : reader.GetInt32(reader.GetOrdinal("telefono_2")),
                                    IdRol = reader.IsDBNull(reader.GetOrdinal("id_Rol")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_Rol")),
                                    Puesto = reader.IsDBNull(reader.GetOrdinal("puesto")) ? string.Empty : reader.GetString(reader.GetOrdinal("puesto")),
                                    CedulaResponsable = reader.IsDBNull(reader.GetOrdinal("cedula_responsable")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("cedula_responsable"))
                                };


                            }
                        }
                    }

                    // Leer los valores de salida del SP
                    int errorCode = Convert.ToInt32(paramError.Value);
                    string mensaje = paramMensaje.Value?.ToString();
                    bool resultado = paramResultado.Value != DBNull.Value && Convert.ToBoolean(paramResultado.Value);

                    if (errorCode != 0 || !resultado)
                    {
                        res.Resultado = false;
                        res.Mensaje = "No se pudo obtener la persona.";
                        if (!string.IsNullOrEmpty(mensaje))
                            res.ListaDeErrores.Add(mensaje);
                        return res;
                    }

                    res.Resultado = true;
                    res.Mensaje = "Persona encontrada correctamente.";
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = "Error inesperado al obtener la persona.";
                res.ListaDeErrores.Add(ex.Message);
            }

            return res;
        }


        /// <summary>
        /// Registra una nueva persona en la base de datos. También se envía una contraseña temporal por correo.
        /// </summary>
        /// <param name="req">Datos de la persona a registrar.</param>
        /// <returns>Resultado del registro, incluyendo el ID de la nueva persona y errores si los hubiera.</returns>
        public async Task<ResRegistrarPersona> RegistrarPersonaAsync(ReqRegistrarPersona req)
        {
            var res = new ResRegistrarPersona();

            try
            {
                // Validación de datos
                var error = LogicaUtilitarios.ValidarPersona(req);
                if (error != null)
                {
                    res.Resultado = false;
                    res.Mensaje = error;
                    return res;
                }

                var utilitarios = new LogicaUtilitarios(_configuration);

                // Generar y encriptar la contraseña temporal
                string passwordPlano = utilitarios.GenerarPassword(12);
                string passwordHash = utilitarios.Encriptar(passwordPlano);

                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Registrar_Persona", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@num_cedula", req.Persona.NumCedula);
                    cmd.Parameters.AddWithValue("@fecha_nacimiento", req.Persona.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@primer_nombre", req.Persona.PrimerNombre);
                    cmd.Parameters.AddWithValue("@segundo_nombre", req.Persona.SegundoNombre);
                    cmd.Parameters.AddWithValue("@primer_apellido", req.Persona.PrimerApellido);
                    cmd.Parameters.AddWithValue("@segundo_apellido", req.Persona.SegundoApellido);
                    cmd.Parameters.AddWithValue("@correo", req.Persona.Correo);
                    cmd.Parameters.AddWithValue("@contraseña", passwordHash);
                    cmd.Parameters.AddWithValue("@direccion", req.Persona.Direccion);
                    cmd.Parameters.AddWithValue("@telefono_1", req.Persona.Telefono1);
                    cmd.Parameters.AddWithValue("@telefono_2", req.Persona.Telefono2);
                    cmd.Parameters.AddWithValue("@id_Rol", req.Persona.IdRol);
                    cmd.Parameters.AddWithValue("@puesto", req.Persona.Puesto);
                    cmd.Parameters.AddWithValue("@cedula_responsable", req.Persona.CedulaResponsable);

                    // Parámetros de salida
                    var paramError = new SqlParameter("@ErrorOccurred", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramError);

                    var paramMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramMensaje);

                    var paramIdReturn = new SqlParameter("@idReturn", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramIdReturn);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    int errorCode = Convert.ToInt32(paramError.Value);
                    string mensajeSP = paramMensaje.Value?.ToString();

                    object idReturnObj = paramIdReturn.Value;
                    int? idReturn = idReturnObj != DBNull.Value ? Convert.ToInt32(idReturnObj) : null;

                    if (errorCode != 0 || idReturn == null)
                    {
                        res.Resultado = false;
                        res.Mensaje = $"Error en SP o envío de correo: {mensajeSP}";
                        return res;
                    }

                    // Envío de contraseña por correo
                    bool emailEnviado = await utilitarios.EnviarPasswordAsync(
                        req.Persona.PrimerNombre,
                        req.Persona.Correo,
                        passwordPlano
                    );

                    if (!emailEnviado)
                    {
                        res.Resultado = false;
                        res.Mensaje = "Persona registrada, pero falló el envío del correo.";
                        return res;
                    }

                    res.Resultado = true;
                    res.Mensaje = "Persona registrada y contraseña enviada exitosamente.";
                    res.Persona = req.Persona;
                    res.Persona.PersonaId = idReturn.Value;
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error general: {ex.Message}";
            }

            return res;
        }

        /// <summary>
        /// Actualiza la contraseña de un usuario autenticado que ingresó con la temporal.
        /// </summary>
        /// <param name="req">Contiene el correo, la contraseña actual y la nueva contraseña.</param>
        /// <returns>Resultado de la operación incluyendo errores si los hay.</returns>
        public async Task<ResRestablecerContrasena> ActualizarContrasenaAsync(ReqRestablecerContrasena req)
        {
            var res = new ResRestablecerContrasena();
            var utilitarios = new LogicaUtilitarios(_configuration);

            try
            {
                // Encriptar ambas contraseñas
                string passActualEncriptada = utilitarios.Encriptar(req.ContrasenaActual);
                string nuevaPassEncriptada = utilitarios.Encriptar(req.NuevaContrasena);

                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Actualizar_Contrasena", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@correo", req.Correo);
                    cmd.Parameters.AddWithValue("@contrasena_actual", passActualEncriptada);
                    cmd.Parameters.AddWithValue("@nueva_contrasena", nuevaPassEncriptada);

                    // Parámetros de salida
                    var paramError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var paramMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    var paramResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(paramError);
                    cmd.Parameters.Add(paramMensaje);
                    cmd.Parameters.Add(paramResultado);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    int errorCode = Convert.ToInt32(paramError.Value);
                    string mensaje = paramMensaje.Value?.ToString();
                    bool resultado = paramResultado.Value != DBNull.Value && Convert.ToBoolean(paramResultado.Value);

                    if (errorCode != 0 || !resultado)
                    {
                        res.Resultado = false;
                        res.Mensaje = "No se pudo actualizar la contraseña.";
                        if (!string.IsNullOrEmpty(mensaje))
                            res.ListaDeErrores.Add(mensaje);
                        return res;
                    }

                    res.Resultado = true;
                    res.Mensaje = "Contraseña actualizada correctamente.";
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = "Error inesperado al actualizar la contraseña.";
                res.ListaDeErrores.Add(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Verifica si existe una persona con el correo electrónico proporcionado.
        /// </summary>
        /// <param name="correo">Correo electrónico a verificar.</param>
        /// <returns>
        /// Objeto <see cref="ResExisteCorreo"/> indicando si existe una persona con ese correo,
        /// así como posibles errores ocurridos durante la operación.
        /// </returns>

        public async Task<ResExisteCorreo> ExistePersonaPorCorreoAsync(string correo)
        {
            var res = new ResExisteCorreo();

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("SELECT COUNT(1) FROM Persona WHERE correo = @correo", conn);
                cmd.Parameters.AddWithValue("@correo", correo);
                await conn.OpenAsync();
                int count = (int)await cmd.ExecuteScalarAsync();
                res.Existe = count > 0;
                res.Resultado = true;
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.ListaDeErrores.Add(ex.Message);
                res.Existe = false;
            }

            return res;
        }

        /// <summary>
        /// Actualiza la contraseña de una persona utilizando su correo electrónico.
        /// </summary>
        /// <param name="correo">Correo electrónico de la persona.</param>
        /// <param name="nuevaContrasenaHash">Nueva contraseña en formato hash.</param>
        /// <returns>
        /// Objeto <see cref="ResActualizarContrasena"/> con el resultado de la operación,
        /// incluyendo si fue exitosa y mensajes de error si aplica.
        /// </returns>

        public async Task<ResActualizarContrasena> ActualizarContrasenaPorCorreoAsync(string correo, string nuevaContrasenaHash)
        {
            var res = new ResActualizarContrasena();

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("UPDATE Persona SET contraseña = @contraseña WHERE correo = @correo", conn);
                cmd.Parameters.AddWithValue("@contraseña", nuevaContrasenaHash);
                cmd.Parameters.AddWithValue("@correo", correo);
                await conn.OpenAsync();

                int rows = await cmd.ExecuteNonQueryAsync();

                res.Actualizado = rows > 0;
                res.Resultado = res.Actualizado;
                res.Mensaje = res.Actualizado ? "Contraseña actualizada correctamente." : "No se encontró el correo para actualizar.";
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = "Error al actualizar la contraseña.";
                res.ListaDeErrores.Add(ex.Message);
                res.Actualizado = false;
            }

            return res;
        }

        /// <summary>
        /// Valida las credenciales de inicio de sesión de una persona.
        /// </summary>
        /// <param name="req">Objeto <see cref="ReqLoginPersona"/> que contiene el correo y la contraseña en texto plano.</param>
        /// <returns>
        /// Objeto <see cref="ResLoginPersona"/> con los datos de la persona si el login es exitoso,
        /// además de los mensajes y errores correspondientes.
        /// </returns>

        public async Task<ResLoginPersona> ValidarLoginAsync(ReqLoginPersona req)
        {
            var res = new ResLoginPersona();
            var utilitarios = new LogicaUtilitarios(_configuration);

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("Login_Persona", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Encriptar la contraseña antes de enviarla
                string passEncriptada = utilitarios.Encriptar(req.Contrasena);

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@email", req.Correo);
                cmd.Parameters.AddWithValue("@password", passEncriptada);

                // Parámetros de salida
                var paramErrorOccurred = new SqlParameter("@ErrorOccurred", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var paramErrorMensaje = new SqlParameter("@ErrorMensaje", System.Data.SqlDbType.VarChar, 255) { Direction = System.Data.ParameterDirection.Output };
                var paramIdReturn = new SqlParameter("@idReturn", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
                var paramResultado = new SqlParameter("@resultado", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output };

                cmd.Parameters.Add(paramErrorOccurred);
                cmd.Parameters.Add(paramErrorMensaje);
                cmd.Parameters.Add(paramIdReturn);
                cmd.Parameters.Add(paramResultado);

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    res.Persona = new Persona
                    {
                        PersonaId = reader.GetInt32(reader.GetOrdinal("id_persona")),
                        NumCedula = reader.IsDBNull(reader.GetOrdinal("num_cedula")) ? 0 : reader.GetInt32(reader.GetOrdinal("num_cedula")),
                        FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("fecha_nacimiento")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento")),
                        PrimerNombre = reader.IsDBNull(reader.GetOrdinal("primer_nombre")) ? "" : reader.GetString(reader.GetOrdinal("primer_nombre")),
                        SegundoNombre = reader.IsDBNull(reader.GetOrdinal("segundo_nombre")) ? null : reader.GetString(reader.GetOrdinal("segundo_nombre")),
                        PrimerApellido = reader.IsDBNull(reader.GetOrdinal("primer_apellido")) ? "" : reader.GetString(reader.GetOrdinal("primer_apellido")),
                        SegundoApellido = reader.IsDBNull(reader.GetOrdinal("segundo_apellido")) ? "" : reader.GetString(reader.GetOrdinal("segundo_apellido")),
                        Correo = reader.IsDBNull(reader.GetOrdinal("correo")) ? "" : reader.GetString(reader.GetOrdinal("correo")),
                        Direccion = reader.IsDBNull(reader.GetOrdinal("direccion")) ? "" : reader.GetString(reader.GetOrdinal("direccion")),
                        Telefono1 = reader.IsDBNull(reader.GetOrdinal("telefono_1")) ? 0 : reader.GetInt32(reader.GetOrdinal("telefono_1")),
                        Telefono2 = reader.IsDBNull(reader.GetOrdinal("telefono_2")) ? 0 : reader.GetInt32(reader.GetOrdinal("telefono_2")),
                        IdRol = reader.IsDBNull(reader.GetOrdinal("id_Rol")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_Rol")),
                        Puesto = reader.IsDBNull(reader.GetOrdinal("puesto")) ? "" : reader.GetString(reader.GetOrdinal("puesto")),
                        CedulaResponsable = reader.IsDBNull(reader.GetOrdinal("cedula_responsable")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("cedula_responsable")),
                        NombreRol = reader.IsDBNull(reader.GetOrdinal("nombre")) ? "" : reader.GetString(reader.GetOrdinal("nombre")),
                    };
                }

                await reader.CloseAsync();

                // Leer parámetros de salida
                int errorOccurred = Convert.ToInt32(paramErrorOccurred.Value);
                string errorMensaje = paramErrorMensaje.Value?.ToString();
                bool resultado = paramResultado.Value != DBNull.Value && Convert.ToBoolean(paramResultado.Value);

                if (errorOccurred != 0 || !resultado)
                {
                    res.Resultado = false;
                    res.Mensaje = string.IsNullOrEmpty(errorMensaje) ? "Error en login." : errorMensaje;
                    return res;
                }

                res.Resultado = true;
                res.Mensaje = "Login exitoso.";
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = "Error inesperado en el login.";
                res.ListaDeErrores.Add(ex.Message);
            }

            return res;
        }

        /// <summary>
        /// Obtiene la información completa de una persona a partir de su ID o correo electrónico.
        /// </summary>
        /// <param name="req">Objeto <see cref="ReqObtenerPersona"/> con el ID y/o correo a consultar.</param>
        /// <returns>
        /// Objeto <see cref="ResOptenerPersona"/> con la información de la persona, si fue encontrada,
        /// junto con el estado de la operación y errores si existieran.
        /// </returns>

        public async Task<ResOptenerPersona> ObtenerPersonaPorCorreoAsync(ReqObtenerPersona req)
        {
            var response = new ResOptenerPersona();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("TraerInfoUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Se pasan parámetros: Id y correo (correo puede ser null)
                cmd.Parameters.AddWithValue("@id_persona", req.PersonaId == 0 ? (object)DBNull.Value : req.PersonaId);

                // Si agregas Correo al ReqObtenerPersona, úsalo aquí:
                cmd.Parameters.AddWithValue("@correo", string.IsNullOrEmpty(req.Correo) ? (object)DBNull.Value : req.Correo);

                // Parámetros OUTPUT
                var errorOccurredParam = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var errorMsgParam = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                var idReturnParam = new SqlParameter("@idReturn", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var resultadoParam = new SqlParameter("@resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(errorOccurredParam);
                cmd.Parameters.Add(errorMsgParam);
                cmd.Parameters.Add(idReturnParam);
                cmd.Parameters.Add(resultadoParam);

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        response.Persona = new Persona
                        {
                            PersonaId = reader.GetInt32(reader.GetOrdinal("id_persona")),
                            NumCedula = reader.GetInt32(reader.GetOrdinal("num_cedula")),
                            FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento")),
                            PrimerNombre = reader.GetString(reader.GetOrdinal("primer_nombre")),
                            SegundoNombre = reader.IsDBNull(reader.GetOrdinal("segundo_nombre")) ? null : reader.GetString(reader.GetOrdinal("segundo_nombre")),
                            PrimerApellido = reader.GetString(reader.GetOrdinal("primer_apellido")),
                            SegundoApellido = reader.IsDBNull(reader.GetOrdinal("segundo_apellido")) ? null : reader.GetString(reader.GetOrdinal("segundo_apellido")),
                            Correo = reader.GetString(reader.GetOrdinal("correo")),
                            Password = reader.IsDBNull(reader.GetOrdinal("contraseña")) ? null : reader.GetString(reader.GetOrdinal("contraseña")),
                            Direccion = reader.IsDBNull(reader.GetOrdinal("direccion")) ? null : reader.GetString(reader.GetOrdinal("direccion")),
                            Telefono1 = reader.IsDBNull(reader.GetOrdinal("telefono_1")) ? 0 : reader.GetInt32(reader.GetOrdinal("telefono_1")),
                            Telefono2 = reader.IsDBNull(reader.GetOrdinal("telefono_2")) ? 0 : reader.GetInt32(reader.GetOrdinal("telefono_2")),
                            IdRol = reader.GetInt32(reader.GetOrdinal("id_Rol")),
                            Puesto = reader.IsDBNull(reader.GetOrdinal("puesto")) ? null : reader.GetString(reader.GetOrdinal("puesto")),
                            CedulaResponsable = reader.IsDBNull(reader.GetOrdinal("cedula_responsable")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("cedula_responsable")),
                            NombreRol = reader.IsDBNull(reader.GetOrdinal("nombre_rol")) ? "" : reader.GetString(reader.GetOrdinal("nombre_rol"))

                        };
                    }
                }

                response.Resultado = (bool)resultadoParam.Value;
                response.ListaDeErrores = new List<string>();
                if ((int)errorOccurredParam.Value == 1)
                    response.ListaDeErrores.Add(errorMsgParam.Value.ToString() ?? "Error desconocido");
            }

            return response;
        }

        /// <summary>
        /// Actualiza los datos del perfil de la persona en la base de datos.
        /// Utiliza procedimiento almacenado para realizar la actualización.
        /// </summary>
        /// <param name="req">Datos con la información a actualizar</param>
        /// <returns>Objeto con resultado y lista de errores si ocurren</returns>
        public async Task<ResActualizarPerfil> ActualizarPerfilAsync(ReqActualizarPerfil req)
        {
            var res = new ResActualizarPerfil();

            try
            {
                using (var con = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("dbo.ActualizarPerfilPersona", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros al comando SQL
                    cmd.Parameters.AddWithValue("@id_persona", req.PersonaId);
                    cmd.Parameters.AddWithValue("@fecha_nacimiento", req.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@primer_nombre", req.PrimerNombre);
                    cmd.Parameters.AddWithValue("@segundo_nombre", (object?)req.SegundoNombre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@primer_apellido", req.PrimerApellido);
                    cmd.Parameters.AddWithValue("@segundo_apellido", (object?)req.SegundoApellido ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@correo", req.Correo);
                    cmd.Parameters.AddWithValue("@direccion", (object?)req.Direccion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@telefono_1", req.Telefono1);
                    cmd.Parameters.AddWithValue("@telefono_2", (object?)req.Telefono2 ?? DBNull.Value);

                    // Parámetros de salida para manejar errores
                    var paramErrorOccurred = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var paramErrorMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(paramErrorOccurred);
                    cmd.Parameters.Add(paramErrorMensaje);

                    // Ejecutar el procedimiento almacenado
                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    int errorOccurred = (int)paramErrorOccurred.Value;
                    string errorMensaje = paramErrorMensaje.Value?.ToString() ?? "";

                    // Evaluar si hubo error
                    if (errorOccurred == 1)
                    {
                        res.Resultado = false;
                        res.ListaDeErrores.Add(errorMensaje);
                    }
                    else
                    {
                        res.Resultado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.ListaDeErrores.Add(ex.Message);
            }

            return res;
        }



    }
}

