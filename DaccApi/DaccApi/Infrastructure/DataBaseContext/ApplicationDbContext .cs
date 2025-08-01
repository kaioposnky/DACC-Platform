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
                entity.ToTable("usuarios");

                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                      .HasColumnName("nome")
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasColumnName("email")
                      .IsRequired();
    
                entity.Property(e => e.Telefone)
                    .HasColumnName("telefone")
                    .IsRequired();
    
                entity.Property(e => e.ImagemUrl)
                    .HasColumnName("imagem_url");
    
                entity.Property(e => e.Ativo)
                    .HasColumnName("ativo");
        
                entity.Property(e => e.DataCriacao)
                      .HasColumnName("data_criacao") 
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
          
                entity.Property(e => e.DataAtualizacao)
                      .HasColumnName("data_atualizacao")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
    
                entity.Property(e => e.Cargo)
                      .HasColumnName("tipo_usuario_id")
                      .HasConversion<int>()
                      .IsRequired();
            });
        }
    }
}