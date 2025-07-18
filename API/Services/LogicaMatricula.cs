using API.Models.Request;
using API.Models.Response;
using API.Models.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TuProyecto.Servicios;

namespace API.Services
{
    /// <summary>
    /// Lógica de negocio para procesos relacionados con matrícula.
    /// </summary>
    public class LogicaMatricula
    {
        private readonly string _connectionString;
        private readonly BlobStorageService _blobStorageService;

        /// <summary>
        /// Inicializa la lógica con la cadena de conexión.
        /// </summary>
        /// <param name="configuration">Configuración para obtener la cadena de conexión.</param>
        public LogicaMatricula(IConfiguration configuration, BlobStorageService blobStorageService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Busca la matrícula asociada a un estudiante dado su ID.
        /// </summary>
        /// <param name="req">Objeto con el ID del estudiante.</param>
        /// <returns>
        /// - Resultado con objeto matrícula si se encontró.  
        /// - Lista de errores y mensaje en caso de fallo.
        /// </returns>
        public async Task<ResOptenerMatricula> BuscarMatriculaAsync(ReqOptenerMatricula req)
        {
            var res = new ResOptenerMatricula();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Buscar_Matricula", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetro de entrada: id de persona
                    cmd.Parameters.AddWithValue("@id_persona", req.PersonaId);

                    // Parámetros de salida para controlar errores y resultado
                    var pError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    var pIdReturn = new SqlParameter("@idReturn", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pResultado = new SqlParameter("@resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pError);
                    cmd.Parameters.Add(pMensaje);
                    cmd.Parameters.Add(pIdReturn);
                    cmd.Parameters.Add(pResultado);

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                res.Matricula = new Matricula
                                {
                                    MatriculaId = reader.GetInt32(reader.GetOrdinal("id_matricula")),
                                    IdEstudiante = reader.GetInt32(reader.GetOrdinal("id_estudiante")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                    FechaFin = reader.GetDateTime(reader.GetOrdinal("fecha_fin"))
                                };
                            }
                        }
                    }

                    // Evaluar resultado y mensaje del SP
                    res.Resultado = (bool)pResultado.Value;
                    res.Mensaje = pMensaje.Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error en SP: {ex.Message}";
            }

            return res;
        }

        /// <summary>
        /// Realiza la matrícula para un estudiante dado su ID.
        /// </summary>
        /// <param name="req">Objeto con datos para crear la matrícula.</param>
        /// <returns>
        /// - Resultado con la matrícula creada si éxito.  
        /// - Lista de errores y mensaje si falla la operación.
        /// </returns>
        public async Task<ResMatricular> MatricularAsync(ReqMatricular req)
        {
            var res = new ResMatricular();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Crear_Matricular", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetro de entrada: id persona a matricular
                    cmd.Parameters.AddWithValue("@id_persona", req.PersonaId);

                    // Parámetros de salida para control de errores y resultados
                    var pError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    var pIdReturn = new SqlParameter("@idReturn", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pError);
                    cmd.Parameters.Add(pMensaje);
                    cmd.Parameters.Add(pIdReturn);

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            res.Matricula = new Matricula
                            {
                                MatriculaId = reader.GetInt32(reader.GetOrdinal("MatriculaId")),
                                IdEstudiante = reader.GetInt32(reader.GetOrdinal("IdEstudiante")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin"))
                            };
                        }

                        // El stored procedure retorna OUTPUT además de SELECT, cerrar reader para poder acceder
                        reader.Close();
                    }

                    res.Resultado = (int)pError.Value == 0;
                    res.Mensaje = pMensaje.Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                res.Resultado = false;
                res.Mensaje = $"Error en SP: {ex.Message}";
            }

            return res;
        }


    }
}
