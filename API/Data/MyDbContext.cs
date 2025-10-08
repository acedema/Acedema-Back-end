using Microsoft.EntityFrameworkCore;
using API.Models.Entities;

namespace API.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> opts) : base(opts) { }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Matricula> Matricula { get; set; }
    }
}
