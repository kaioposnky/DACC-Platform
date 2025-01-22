using DaccApi.Model;
using Microsoft.EntityFrameworkCore;

namespace DaccApi.Infrastructure.DataBaseContext
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Produtos");


                entity.Property(e => e.Id)
                      .HasColumnName("ProdutoId")
                      .IsRequired();

                entity.Property(e => e.Name)
                      .HasColumnName("Nome")
                      .IsRequired();

                entity.Property(e => e.Description)
                      .HasColumnName("Descricao")
                      .IsRequired();

                entity.Property(e => e.Price)
                      .HasColumnName("Preco")
                      .IsRequired();

                entity.Property(e => e.ReleaseDate)
                      .HasColumnName("DataDeLancamento")
                      .IsRequired();

                entity.Property(e => e.ImageUrl)
                      .HasColumnName("ImagemProduto")
                      .IsRequired();

            });
        }
    }
}
