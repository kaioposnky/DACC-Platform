using Microsoft.EntityFrameworkCore;
using DaccApi.Model;

namespace DaccApi.Infrastructure.DataBaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");


                entity.Property(e => e.Id)
                      .HasColumnName("UsuarioID")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                      .HasColumnName("Nome")
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasColumnName("Email")
                      .IsRequired();

                entity.Property(e => e.Senha)
                      .HasColumnName("Senha")
                      .IsRequired();

                entity.Property(e => e.TipoUsuario)
                      .HasColumnName("TipoUsuarioID")
                      .IsRequired();

                entity.Property(e => e.DataCadastro)
                      .HasColumnName("DataCadastro")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            });
        }
    }
}
