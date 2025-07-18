using API.Models.Entities;
using API.Models.Request;
using API.Models.Response;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.Services
{
    /// <summary>
    /// Lógica de negocio relacionada con el manejo de publicaciones del foro.
    /// </summary>
    public class LogicaForo
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor que recibe la configuración para extraer la cadena de conexión.
        /// </summary>
        public LogicaForo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Crea un nuevo foro y asigna los roles correspondientes.
        /// </summary>
        /// <param name="req">Datos para crear el foro, incluyendo roles asignados.</param>
        /// <returns>Respuesta con resultado de la operación y el ID del foro creado.</returns>
        public async Task<ResCrearActualizarForo> CrearForoAsync(ReqCrearForo req)
        {
            var res = new ResCrearActualizarForo();

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("InsertarForo", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Parámetros del stored procedure
                cmd.Parameters.AddWithValue("@IdAdmin", req.AdminId);
                cmd.Parameters.AddWithValue("@Titulo", req.Titulo);
                cmd.Parameters.AddWithValue("@Descripcion", req.Descripcion);
                cmd.Parameters.AddWithValue("@FechaRegistro", req.FechaRegistro);
                cmd.Parameters.AddWithValue("@EsPublico", req.EsPublico);

                // Parámetros de salida
                var pError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var pMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                var pNuevoIdForo = new SqlParameter("@NuevoIdForo", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(pError);
                cmd.Parameters.Add(pMensaje);
                cmd.Parameters.Add(pNuevoIdForo);
                cmd.Parameters.Add(pResultado);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                res.Resultado = (bool)pResultado.Value;
                res.Mensaje = pMensaje.Value?.ToString();
                res.ForoId = (int)(pNuevoIdForo.Value ?? 0);

                // Si el foro NO es público, insertar los roles asignados en la tabla Foro_Roles
                if (res.Resultado && !req.EsPublico && req.RolesAsignados != null && req.RolesAsignados.Count > 0)
                {
                    foreach (var rolId in req.RolesAsignados)
                    {
                        using var cmdRoles = new SqlCommand("InsertarForoRol", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmdRoles.Parameters.AddWithValue("@IdForo", res.ForoId);
                        cmdRoles.Parameters.AddWithValue("@IdRol", rolId);

                        await cmdRoles.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error en InsertarForo: {ex.Message}";
            }

            return res;
        }

        /// <summary>
        /// Actualiza un foro y sus roles asignados.
        /// </summary>
        /// <param name="req">Datos para actualizar el foro, incluyendo roles.</param>
        /// <returns>Resultado con mensaje y estado.</returns>
        public async Task<ResCrearActualizarForo> ActualizarForoAsync(ReqActualizarForo req)
        {
            var res = new ResCrearActualizarForo();

            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                // 1. Actualizar el foro principal
                using (var cmd = new SqlCommand("ActualizarForo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ForoId", req.ForoId);
                    cmd.Parameters.AddWithValue("@IdAdmin", req.AdminId);
                    cmd.Parameters.AddWithValue("@Titulo", req.Titulo);
                    cmd.Parameters.AddWithValue("@Descripcion", req.Descripcion);
                    cmd.Parameters.AddWithValue("@EsPublico", req.EsPublico);

                    var pError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    var pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pError);
                    cmd.Parameters.Add(pMensaje);
                    cmd.Parameters.Add(pResultado);

                    await cmd.ExecuteNonQueryAsync();

                    res.Resultado = (int)pError.Value == 0 && (bool)pResultado.Value;
                    res.Mensaje = pMensaje.Value?.ToString() ?? "";

                    if (!res.Resultado)
                    {
                        return res; // Error al actualizar el foro
                    }
                }

                // 2. Actualizar roles del foro (solo si no es público)
                if (!req.EsPublico)
                {
                    // Para simplificar, pasamos los roles como CSV al SP de roles
                    var rolesCsv = req.RolesAsignados != null && req.RolesAsignados.Count > 0
                        ? string.Join(",", req.RolesAsignados)
                        : "";

                    using var cmdRoles = new SqlCommand("ActualizarForoRoles", conn);
                    cmdRoles.CommandType = CommandType.StoredProcedure;

                    //Parametros de entrada
                    cmdRoles.Parameters.AddWithValue("@IdForo", req.ForoId);
                    cmdRoles.Parameters.AddWithValue("@Roles", rolesCsv);

                    //Parametros de salida
                    var pErrorRoles = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pMensajeRoles = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };

                    cmdRoles.Parameters.Add(pErrorRoles);
                    cmdRoles.Parameters.Add(pMensajeRoles);

                    await cmdRoles.ExecuteNonQueryAsync();

                    if ((int)pErrorRoles.Value != 0)
                    {
                        res.Resultado = false;
                        res.Mensaje = $"Error actualizando roles: {pMensajeRoles.Value}";
                        return res;
                    }
                }
                else
                {
                    // Si pasamos de privado a público, eliminamos roles asignados para ese foro
                    using var cmdDeleteRoles = new SqlCommand("DELETE FROM Foro_Roles WHERE id_foro = @IdForo", conn);
                    cmdDeleteRoles.Parameters.AddWithValue("@IdForo", req.ForoId);
                    await cmdDeleteRoles.ExecuteNonQueryAsync();
                }

                res.ForoId = req.ForoId;
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error en lógica de actualización: {ex.Message}";
            }

            return res;
        }

        public async Task<ResObtenerForosPorRol> ObtenerForosPorRolAsync(ReqObtenerForosPorRol req)
        {
            var res = new ResObtenerForosPorRol
            {
                Foros = new List<Foro>()
            };

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("ObtenerForosPorRol", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRol", req.RolId);

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var foro = new Foro
                    {
                        ForoId = reader.GetInt32(reader.GetOrdinal("id_foro")),
                        AdminId = reader.GetInt32(reader.GetOrdinal("id_administrador")),
                        Titulo = reader.GetString(reader.GetOrdinal("titulo")),
                        Descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("fecha_registro")),
                        FechaUltimaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaUltimaModificacion"))
                            ? (DateTime?)null
                            : reader.GetDateTime(reader.GetOrdinal("FechaUltimaModificacion")),
                        EsPublico = reader.GetBoolean(reader.GetOrdinal("espublico")),
                    };

                    res.Foros.Add(foro);
                }

                res.Resultado = true;
                res.Mensaje = "Foros obtenidos correctamente.";
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error al obtener foros: {ex.Message}";
            }

            return res;
        }

        public async Task<ResEliminarForo> EliminarForoAsync(ReqEliminarForo req)
        {
            var res = new ResEliminarForo();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("EliminarForo", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@ForoId", req.ForoId);
                    cmd.Parameters.AddWithValue("@IdAdmin", req.AdminId);

                    // Parámetros de salida
                    var pError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    var pResultado = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pError);
                    cmd.Parameters.Add(pMensaje);
                    cmd.Parameters.Add(pResultado);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    // Leer valores de salida
                    var errorOccurred = (int)pError.Value;
                    var mensaje = pMensaje.Value?.ToString();
                    var resultado = (bool)pResultado.Value;

                    res.Resultado = resultado && errorOccurred == 0;
                    res.Mensaje = mensaje ?? (res.Resultado ? "Foro eliminado correctamente." : "Error desconocido.");

                    if (!res.Resultado && errorOccurred != 0)
                    {
                        res.ListaDeErrores = new List<string> { mensaje };
                    }
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error en servidor: {ex.Message}";
                res.ListaDeErrores = new List<string> { ex.ToString() };
            }

            return res;
        }

    }
}
