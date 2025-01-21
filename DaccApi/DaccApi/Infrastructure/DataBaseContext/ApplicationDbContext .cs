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

                entity.Property(e => e.Name)
                      .HasColumnName("Nome")
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasColumnName("Email")
                      .IsRequired();

                entity.Property(e => e.Password)
                      .HasColumnName("Senha")
                      .IsRequired();

                entity.Property(e => e.TypeId)
                      .HasColumnName("TipoUsuarioID")
                      .IsRequired();

                entity.Property(e => e.RegistrationDate)
                      .HasColumnName("DataCadastro")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UsuSitua)
                      .HasColumnName("Situacao")
                      .IsRequired();
            });
        }
    }
}
