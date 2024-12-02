using BancoChu.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoChu.API.Data
{
    public class BancoChuContext : DbContext
    {
        public BancoChuContext(DbContextOptions<BancoChuContext> options)
            : base(options)
        { }

        public DbSet<Conta> Contas { get; set; }
        public DbSet<Transferencia> Transferencias { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura as propriedades decimal
            modelBuilder.Entity<Conta>()
                .Property(c => c.Saldo)
                .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<Transferencia>()
                .Property(t => t.Valor)
                .HasColumnType("decimal(18,2)");  

            base.OnModelCreating(modelBuilder);
        }
    }
}
