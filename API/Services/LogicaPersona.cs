using API.Data;
using API.Models.Entities;
using API.Models.Request;
using API.Models.Response;
using Npgsql; // Changed from Microsoft.Data.SqlClient
using System.Data;

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
                // Changed to NpgsqlConnection and NpgsqlCommand
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("TraerInfoUsuario", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // For calling PostgreSQL Functions

                    // Parámetro de entrada
                    cmd.Parameters.AddWithValue("@id_persona", req.PersonaId);
                    
                    // Parámetros de salida de MSSQL han sido removidos.
                    // En PostgreSQL, los status se leen como columnas del resultado de la función.
                    
                    await conn.OpenAsync();
                    
                    // Leer resultado del procedimiento que ahora retorna un solo set de datos con la Persona + Status
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) // Lee la única fila que contiene Persona data + Status columns
                        {
                            // Leer datos de Persona
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
                                IdRol = reader.IsDBNull(reader.GetOrdinal("id_rol")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_rol")),
                                Puesto = reader.IsDBNull(reader.GetOrdinal("puesto")) ? string.Empty : reader.GetString(reader.GetOrdinal("puesto")),
                                CedulaResponsable = reader.IsDBNull(reader.GetOrdinal("cedula_responsable")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("cedula_responsable"))
                            };
                            
                            // Leer los valores de Status/Error desde las mismas columnas de la fila
                            int errorCode = reader.IsDBNull(reader.GetOrdinal("erroroccurred")) ? 0 : reader.GetInt32(reader.GetOrdinal("erroroccurred"));
                            string mensaje = reader.IsDBNull(reader.GetOrdinal("errormensaje")) ? "" : reader.GetString(reader.GetOrdinal("errormensaje"));
                            bool resultado = reader.IsDBNull(reader.GetOrdinal("resultado")) ? false : reader.GetBoolean(reader.GetOrdinal("resultado"));

                            if (errorCode != 0 || !resultado)
                            {
                                res.Resultado = false;
                                res.Mensaje = "No se pudo obtener la persona.";
                                if (!string.IsNullOrEmpty(mensaje))
                                    res.ListaDeErrores.Add(mensaje);
                                return res;
                            }
                        }
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

                // Changed to NpgsqlConnection and NpgsqlCommand
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("Registrar_Persona", conn))
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
                    
                    // Parámetros de salida de MSSQL han sido removidos.
                    // La función de PostgreSQL retorna una tabla con los campos de status + idReturn.

                    await conn.OpenAsync();
                    
                    // Usar ExecuteReaderAsync para leer la fila de estado/ID retornada por la función.
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int errorCode = 1; // Default error
                        string mensajeSP = "No se pudo registrar la persona.";
                        int? idReturn = null;
                        
                        if (await reader.ReadAsync())
                        {
                            // Lee la única fila que contiene el ID y los campos de status.
                            errorCode = reader.IsDBNull(reader.GetOrdinal("erroroccurred")) ? 0 : reader.GetInt32(reader.GetOrdinal("erroroccurred"));
                            mensajeSP = reader.IsDBNull(reader.GetOrdinal("errormensaje")) ? "" : reader.GetString(reader.GetOrdinal("errormensaje"));
                            
                            // El idReturn puede ser NULL si hay un error en el SP (e.g., cédula duplicada)
                            object idReturnObj = reader.IsDBNull(reader.GetOrdinal("idReturn")) ? (object)DBNull.Value : reader.GetValue(reader.GetOrdinal("idReturn"));
                            idReturn = idReturnObj != DBNull.Value ? Convert.ToInt32(idReturnObj) : null;
                        }
                        
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

                // Changed to NpgsqlConnection and NpgsqlCommand
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("Actualizar_Contrasena", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@correo", req.Correo);
                    cmd.Parameters.AddWithValue("@contrasena_actual", passActualEncriptada);
                    cmd.Parameters.AddWithValue("@nueva_contrasena", nuevaPassEncriptada);

                    // Parámetros de salida de MSSQL han sido removidos.
                    // La función de PostgreSQL retorna una tabla con los campos de status + resultado.

                    await conn.OpenAsync();
                    
                    // Usar ExecuteReaderAsync para leer la fila de estado/ID retornada por la función.
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int errorCode = 1;
                        string mensaje = "No se pudo actualizar la contraseña (No response from DB).";
                        bool resultado = false;
                        
                        if (await reader.ReadAsync())
                        {
                            // Lee la única fila que contiene los campos de status.
                            errorCode = reader.IsDBNull(reader.GetOrdinal("erroroccurred")) ? 0 : reader.GetInt32(reader.GetOrdinal("erroroccurred"));
                            mensaje = reader.IsDBNull(reader.GetOrdinal("errormensaje")) ? "" : reader.GetString(reader.GetOrdinal("errormensaje"));
                            resultado = reader.IsDBNull(reader.GetOrdinal("resultado")) ? false : reader.GetBoolean(reader.GetOrdinal("resultado"));
                        }
                        
                        if (errorCode != 0 || !resultado)
                        {
                            res.Resultado = false;
                            res.Mensaje = "No se pudo actualizar la contraseña.";
                            if (!string.IsNullOrEmpty(mensaje))
                                res.ListaDeErrores.Add(mensaje);
                            return res;
                        }
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
                // Changed to NpgsqlConnection and NpgsqlCommand
                using var conn = new NpgsqlConnection(_connectionString);
                // PostgreSQL uses $1, $2, etc., for positional parameters in raw SQL, but Npgsql allows @name.
                using var cmd = new NpgsqlCommand("SELECT COUNT(1) FROM Persona WHERE correo = @correo", conn);
                cmd.Parameters.AddWithValue("@correo", correo);
                await conn.OpenAsync();
                // Npgsql returns a long for COUNT()
                long count = (long)await cmd.ExecuteScalarAsync();
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
                // Changed to NpgsqlConnection and NpgsqlCommand
                using var conn = new NpgsqlConnection(_connectionString);
                using var cmd = new NpgsqlCommand("UPDATE Persona SET contraseña = @contraseña WHERE correo = @correo", conn);
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
                // Changed to NpgsqlConnection and NpgsqlCommand
                using var conn = new NpgsqlConnection(_connectionString);
                using var cmd = new NpgsqlCommand("SELECT * FROM login_persona(@p_email, @p_password)", conn);
                cmd.CommandType = CommandType.Text;

                // Encriptar la contraseña antes de enviarla
                string passEncriptada = utilitarios.Encriptar(req.Contrasena);

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("p_email", NpgsqlTypes.NpgsqlDbType.Varchar, req.Correo);
                cmd.Parameters.AddWithValue("p_password", NpgsqlTypes.NpgsqlDbType.Varchar, passEncriptada);

                // Parámetros de salida de MSSQL han sido removidos.
                // La función de PostgreSQL retorna una tabla con los campos de persona + status.

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                int errorOccurred = 1; // Default error
                string errorMensaje = "No se obtuvo respuesta de la base de datos.";
                bool resultado = false;
                
                if (await reader.ReadAsync())
                {
                    // Lee los datos de la persona
                    res.Persona = new Persona
                    {
                        PersonaId = reader.IsDBNull(reader.GetOrdinal("id_persona")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_persona")),
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
                        IdRol = reader.IsDBNull(reader.GetOrdinal("id_rol")) ? 0 : reader.GetInt32(reader.GetOrdinal("id_rol")),
                        Puesto = reader.IsDBNull(reader.GetOrdinal("puesto")) ? "" : reader.GetString(reader.GetOrdinal("puesto")),
                        CedulaResponsable = reader.IsDBNull(reader.GetOrdinal("cedula_responsable")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("cedula_responsable")),
                        NombreRol = reader.IsDBNull(reader.GetOrdinal("nombre_rol")) ? "" : reader.GetString(reader.GetOrdinal("nombre_rol")),
                    };
                    
                    // Lee los parámetros de salida (Status) de la misma fila
                    errorOccurred = reader.IsDBNull(reader.GetOrdinal("erroroccurred")) ? 0 : reader.GetInt32(reader.GetOrdinal("erroroccurred"));
                    errorMensaje = reader.IsDBNull(reader.GetOrdinal("errormensaje")) ? "" : reader.GetString(reader.GetOrdinal("errormensaje"));
                    resultado = reader.IsDBNull(reader.GetOrdinal("resultado")) ? false : reader.GetBoolean(reader.GetOrdinal("resultado"));
                }
                
                // Procesar Status
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
                Console.WriteLine(ex.Message + "|" + ex.StackTrace);
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
            
            // Changed to NpgsqlConnection and NpgsqlCommand
            using (var conn = new NpgsqlConnection(_connectionString))
            using (var cmd = new NpgsqlCommand("TraerInfoUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Se pasan parámetros: Id y correo (correo puede ser null)
                cmd.Parameters.AddWithValue("@id_persona", req.PersonaId == 0 ? (object)DBNull.Value : req.PersonaId);
                cmd.Parameters.AddWithValue("@correo", string.IsNullOrEmpty(req.Correo) ? (object)DBNull.Value : req.Correo);

                // Parámetros OUTPUT de MSSQL han sido removidos.
                // La función de PostgreSQL retorna una tabla con los campos de persona + status.

                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int errorOccurred = 1;
                    string errorMsg = "No se obtuvo respuesta de la base de datos.";
                    bool resultado = false;
                    
                    if (await reader.ReadAsync())
                    {
                        // Lee los datos de la persona
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
                            IdRol = reader.GetInt32(reader.GetOrdinal("id_rol")),
                            Puesto = reader.IsDBNull(reader.GetOrdinal("puesto")) ? null : reader.GetString(reader.GetOrdinal("puesto")),
                            CedulaResponsable = reader.IsDBNull(reader.GetOrdinal("cedula_responsable")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("cedula_responsable")),
                            NombreRol = reader.IsDBNull(reader.GetOrdinal("nombre_rol")) ? "" : reader.GetString(reader.GetOrdinal("nombre_rol"))

                        };
                        
                        // Lee los parámetros de salida (Status) de la misma fila
                        errorOccurred = reader.IsDBNull(reader.GetOrdinal("erroroccurred")) ? 0 : reader.GetInt32(reader.GetOrdinal("erroroccurred"));
                        errorMsg = reader.IsDBNull(reader.GetOrdinal("errormensaje")) ? "" : reader.GetString(reader.GetOrdinal("errormensaje"));
                        resultado = reader.IsDBNull(reader.GetOrdinal("resultado")) ? false : reader.GetBoolean(reader.GetOrdinal("resultado"));
                    }
                    
                    response.Resultado = resultado;
                    response.ListaDeErrores = new List<string>();
                    if (errorOccurred == 1)
                        response.ListaDeErrores.Add(errorMsg ?? "Error desconocido");
                }
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
                // Changed to NpgsqlConnection and NpgsqlCommand
                using (var con = new NpgsqlConnection(_connectionString))
                // Note: The original code used 'dbo.ActualizarPerfilPersona'. PostgreSQL does not use 'dbo.' schema prefix.
                using (var cmd = new NpgsqlCommand("ActualizarPerfilPersona", con))
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

                    // Parámetros de salida de MSSQL han sido removidos.
                    // La función de PostgreSQL retorna una tabla con los campos de status.

                    // Ejecutar la función y leer el resultado
                    await con.OpenAsync();
                    
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int errorOccurred = 1;
                        string errorMensaje = "No se obtuvo respuesta de la base de datos.";
                        
                        if (await reader.ReadAsync())
                        {
                            // Lee la única fila que contiene los campos de status.
                            errorOccurred = reader.IsDBNull(reader.GetOrdinal("erroroccurred")) ? 0 : reader.GetInt32(reader.GetOrdinal("erroroccurred"));
                            errorMensaje = reader.IsDBNull(reader.GetOrdinal("errormensaje")) ? "" : reader.GetString(reader.GetOrdinal("errormensaje"));
                        }
                        
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