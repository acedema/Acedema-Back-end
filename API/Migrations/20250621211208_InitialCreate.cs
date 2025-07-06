using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clases",
                columns: table => new
                {
                    ClasesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreClase = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<bool>(type: "INTEGER", nullable: true),
                    Modalidad = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clases", x => x.ClasesId);
                });

            migrationBuilder.CreateTable(
                name: "Cronograma",
                columns: table => new
                {
                    CronogramaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LinkTeams = table.Column<string>(type: "TEXT", nullable: false),
                    IdClaseDocente = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cronograma", x => x.CronogramaId);
                });

            migrationBuilder.CreateTable(
                name: "Matriculas",
                columns: table => new
                {
                    MatriculaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdEstudiante = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matriculas", x => x.MatriculaId);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    PersonaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumCedula = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrimerNombre = table.Column<string>(type: "TEXT", nullable: false),
                    SegundoNombre = table.Column<string>(type: "TEXT", nullable: false),
                    Correo = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Direccion = table.Column<string>(type: "TEXT", nullable: false),
                    Telefono1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Telefono2 = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdRol = table.Column<int>(type: "INTEGER", nullable: false),
                    Puesto = table.Column<string>(type: "TEXT", nullable: false),
                    CedulaResponsable = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.PersonaId);
                });

            migrationBuilder.CreateTable(
                name: "Semana",
                columns: table => new
                {
                    SemanaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdCronograma = table.Column<int>(type: "INTEGER", nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Contenido = table.Column<string>(type: "TEXT", nullable: false),
                    CronogramaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semana", x => x.SemanaId);
                    table.ForeignKey(
                        name: "FK_Semana_Cronograma_CronogramaId",
                        column: x => x.CronogramaId,
                        principalTable: "Cronograma",
                        principalColumn: "CronogramaId");
                });

            migrationBuilder.CreateTable(
                name: "ClaseDocente",
                columns: table => new
                {
                    ClaseDocenteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClasesId = table.Column<int>(type: "INTEGER", nullable: false),
                    DocentePersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Estado = table.Column<bool>(type: "INTEGER", nullable: false),
                    Capacidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Miembros = table.Column<int>(type: "INTEGER", nullable: false),
                    CronogramaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseDocente", x => x.ClaseDocenteId);
                    table.ForeignKey(
                        name: "FK_ClaseDocente_Clases_ClasesId",
                        column: x => x.ClasesId,
                        principalTable: "Clases",
                        principalColumn: "ClasesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaseDocente_Cronograma_CronogramaId",
                        column: x => x.CronogramaId,
                        principalTable: "Cronograma",
                        principalColumn: "CronogramaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaseDocente_Personas_DocentePersonaId",
                        column: x => x.DocentePersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bitacora",
                columns: table => new
                {
                    BitacoraId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdSemana = table.Column<int>(type: "INTEGER", nullable: false),
                    Comentario = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    FechaHoraRegistro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SemanaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacora", x => x.BitacoraId);
                    table.ForeignKey(
                        name: "FK_Bitacora_Semana_SemanaId",
                        column: x => x.SemanaId,
                        principalTable: "Semana",
                        principalColumn: "SemanaId");
                });

            migrationBuilder.CreateTable(
                name: "ClaseEstudiante",
                columns: table => new
                {
                    ClaseEstudianteId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EstudiantePersonaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaseDocenteId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatriculaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaseEstudiante", x => x.ClaseEstudianteId);
                    table.ForeignKey(
                        name: "FK_ClaseEstudiante_ClaseDocente_ClaseDocenteId",
                        column: x => x.ClaseDocenteId,
                        principalTable: "ClaseDocente",
                        principalColumn: "ClaseDocenteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaseEstudiante_Matriculas_MatriculaId",
                        column: x => x.MatriculaId,
                        principalTable: "Matriculas",
                        principalColumn: "MatriculaId");
                    table.ForeignKey(
                        name: "FK_ClaseEstudiante_Personas_EstudiantePersonaId",
                        column: x => x.EstudiantePersonaId,
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_SemanaId",
                table: "Bitacora",
                column: "SemanaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseDocente_ClasesId",
                table: "ClaseDocente",
                column: "ClasesId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseDocente_CronogramaId",
                table: "ClaseDocente",
                column: "CronogramaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseDocente_DocentePersonaId",
                table: "ClaseDocente",
                column: "DocentePersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseEstudiante_ClaseDocenteId",
                table: "ClaseEstudiante",
                column: "ClaseDocenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseEstudiante_EstudiantePersonaId",
                table: "ClaseEstudiante",
                column: "EstudiantePersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaseEstudiante_MatriculaId",
                table: "ClaseEstudiante",
                column: "MatriculaId");

            migrationBuilder.CreateIndex(
                name: "IX_Semana_CronogramaId",
                table: "Semana",
                column: "CronogramaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bitacora");

            migrationBuilder.DropTable(
                name: "ClaseEstudiante");

            migrationBuilder.DropTable(
                name: "Semana");

            migrationBuilder.DropTable(
                name: "ClaseDocente");

            migrationBuilder.DropTable(
                name: "Matriculas");

            migrationBuilder.DropTable(
                name: "Clases");

            migrationBuilder.DropTable(
                name: "Cronograma");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}
