using API.Data;
using API.Models.Entities;
using API.Models.Request;
using API.Models.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class LogicaMatricula
    {
        private readonly string _connectionString;

        public LogicaMatricula(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ResOptenerMatricula> BuscarMatriculaAsync(ReqOptenerMatricula req)
        {
            var res = new ResOptenerMatricula();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Buscar_Matricula", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Entradas
                    cmd.Parameters.AddWithValue("@id_persona", req.PersonaId);

                    // Salidas
                    var pError = new SqlParameter("@ErrorOccurred", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pMensaje = new SqlParameter("@ErrorMensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output };
                    var pIdReturn = new SqlParameter("@idReturn", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var pResultado = new SqlParameter("@resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pError);
                    cmd.Parameters.Add(pMensaje);
                    cmd.Parameters.Add(pIdReturn);
                    cmd.Parameters.Add(pResultado);

                    await conn.OpenAsync();

                    //Aqui guarda la respuesta
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                // Ejemplo: crea aquí un objeto Matricula según los campos reales que devuelve el SP
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

                    // Salidas
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

        public async Task<ResMatricular> MatricularAsync(ReqMatricular req)
        {
            var res = new ResMatricular();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Crear_Matricular", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Entradas
                    cmd.Parameters.AddWithValue("@id_persona", req.PersonaId);

                    // Salidas
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

                        // El SP retorna parámetros OUTPUT además del SELECT,
                        // por lo que se debe cerrar el reader para poder leerlos.
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

