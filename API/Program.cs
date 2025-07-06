using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Carga configuración de appsettings.json y variables de entorno
builder.Configuration
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

// 1) Agrega servicios al contenedor
builder.Services.AddControllers();

// 2) Registra tu DbContext con SQL Server
builder.Services.AddDbContext<MyDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 3) Registra la lógica de negocio para inyección de dependencias
builder.Services.AddScoped<LogicaPersona>();
builder.Services.AddScoped<LogicaMatricula>();
builder.Services.AddScoped<LogicaUtilitarios>();


// 4) Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ─── NUEVO: Configura CORS ─────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")   // Cambia al origen de tu frontend
            .AllowAnyHeader()
            .AllowAnyMethod();
            // .AllowCredentials(); // si necesitas enviar cookies o auth
    });
});
// ───────────────────────────────────────────────────────────────────────────────

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ─── NUEVO: Habilita CORS ──────────────────────────────────────────────────────
app.UseCors("AllowFrontend");
// ───────────────────────────────────────────────────────────────────────────────

app.UseAuthorization();

// Mapea únicamente tus controllers
app.MapControllers();

app.Run();

