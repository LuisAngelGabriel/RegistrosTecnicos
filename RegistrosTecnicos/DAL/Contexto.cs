using RegistrosTecnicos.Models;
 using Microsoft.EntityFrameworkCore;

namespace RegistrosTecnicos.DAL
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }
        public DbSet<Tecnico> Tecnicos { get; set; }

    }
}
