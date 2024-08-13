using Microsoft.EntityFrameworkCore;
using MINIMAL_API.Models; // Adicione esta linha para garantir que o namespace correto está sendo utilizado

namespace MINIMAL_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure o mapeamento do modelo aqui, se necessário.
        }
    }
}
