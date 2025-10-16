using API.Models.Entities;
using API.Models.Request;
using API.Models.Response;
using Dapper;
using Npgsql;

namespace API.Services;

/// <summary>
///     Lógica de negocio para procesos relacionados con matrícula.
/// </summary>
public class LogicaMatricula
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly ILogger<LogicaMatricula> _logger;

    /// <summary>
    ///     Inicializa la lógica con la cadena de conexión.
    /// </summary>
    /// <param name="configuration">Configuración para obtener la cadena de conexión.</param>
    public LogicaMatricula(IConfiguration configuration, ILogger<LogicaMatricula> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    ///     Busca la matrícula asociada a un estudiante dado su ID.
    /// </summary>
    /// <param name="req">Objeto con el ID del estudiante.</param>
    /// <returns>
    ///     - Resultado con objeto matrícula si se encontró.
    ///     - Lista de errores y mensaje en caso de fallo.
    /// </returns>
    public async Task<ResOptenerMatricula> BuscarMatriculaAsync(ReqOptenerMatricula req)
    {
        _logger.LogDebug("Buscar Matrícula Inicio :: {}", req.PersonaId);

        var res = new ResOptenerMatricula
        {
            ListaDeErrores = new List<string>()
        };

        const string sql = """
                           SELECT 
                               m.id_matricula AS MatriculaId,
                               m.id_estudiante AS IdEstudiante,
                               m.fecha_inicio AS FechaInicio,
                               m.fecha_fin AS FechaFin
                           FROM matricula m
                           WHERE m.id_estudiante = @PersonaId
                           LIMIT 1
                           """;

        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);

            res.Matricula = await conn.QueryFirstOrDefaultAsync<Matricula>(sql, new { req.PersonaId });

            res.Resultado = res.Matricula != null;
            res.Mensaje = res.Resultado
                ? "Matrícula encontrada correctamente."
                : "No se encontró ninguna matrícula.";

            if (res.Resultado)
                _logger.LogDebug("Matrícula encontrada correctamente para personaId :: {}", req.PersonaId);
            else
                _logger.LogWarning("No se encontró matrícula para personaId :: {}", req.PersonaId);
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(ex, "Error de base de datos al buscar matrícula.");
            res.Resultado = false;
            res.Mensaje = "Error de base de datos al buscar matrícula.";
            res.ListaDeErrores.Add(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al buscar matrícula.");
            res.Resultado = false;
            res.Mensaje = "Error inesperado al buscar matrícula.";
            res.ListaDeErrores.Add(ex.Message);
        }

        _logger.LogDebug("Buscar Matrícula Finalizada :: {}", req.PersonaId);
        return res;
    }

    /// <summary>
    ///     Realiza la matrícula para un estudiante dado su ID.
    /// </summary>
    /// <param name="req">Objeto con datos para crear la matrícula.</param>
    /// <returns>
    ///     - Resultado con la matrícula creada si éxito.
    ///     - Lista de errores y mensaje si falla la operación.
    /// </returns>
    public async Task<ResMatricular> MatricularAsync(ReqMatricular req)
    {
        _logger.LogDebug("Matricular Inicio :: {}", req.PersonaId);

        var res = new ResMatricular
        {
            ListaDeErrores = new List<string>()
        };

        const string sql = """
                           INSERT INTO matricula (id_estudiante, fecha_inicio, fecha_fin)
                           VALUES (@PersonaId, CURRENT_DATE, CURRENT_DATE + INTERVAL '6 months')
                           RETURNING 
                               id_matricula AS MatriculaId,
                               id_estudiante AS IdEstudiante,
                               fecha_inicio AS FechaInicio,
                               fecha_fin AS FechaFin;
                           """;

        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);

            res.Matricula = await conn.QueryFirstOrDefaultAsync<Matricula>(sql, new { req.PersonaId });

            res.Resultado = res.Matricula != null;
            res.Mensaje = res.Resultado
                ? "Matrícula creada correctamente."
                : "No se pudo crear la matrícula.";

            if (res.Resultado)
                _logger.LogDebug("Matrícula creada correctamente para personaId :: {}", req.PersonaId);
            else
                _logger.LogWarning("No se pudo crear matrícula para personaId :: {}", req.PersonaId);
        }
        catch (PostgresException ex)
        {
            _logger.LogError(ex, "Error de base de datos al crear matrícula.");
            res.Resultado = false;
            res.Mensaje = "Error de base de datos al crear matrícula.";
            res.ListaDeErrores.Add(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al crear matrícula.");
            res.Resultado = false;
            res.Mensaje = "Error inesperado al crear matrícula.";
            res.ListaDeErrores.Add(ex.Message);
        }

        _logger.LogDebug("Matricular Finalizada :: {}", req.PersonaId);
        return res;
    }
}