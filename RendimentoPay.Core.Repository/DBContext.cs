using Microsoft.EntityFrameworkCore;

namespace RendimentoPay.Core.Domain.Request.Dominio
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<LogEndPoint> LogEndPoint { get; set; }
    //public DbSet<Log> Log{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      /*
      // Configurações adicionais, como chaves primárias, índices, etc.
      modelBuilder.Entity<LogEndPoint>()
          .HasData(new LogEndPoint { Id = 1, Name = "Initial Data" }); // Exemplo de dados iniciais
      */

      // Proibir exclusão de tabelas com dados
      modelBuilder.Entity<LogEndPoint>()
          .HasAnnotation("SqlServer:MemoryOptimized", true);

    }
  }
}