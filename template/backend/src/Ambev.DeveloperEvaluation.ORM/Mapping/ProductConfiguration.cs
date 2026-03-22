using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.HasData(
                new Product { Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Skol 350ml",           Description = "Cerveja Skol lata 350ml" },
                new Product { Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Brahma 350ml",         Description = "Cerveja Brahma lata 350ml" },
                new Product { Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Antarctica 600ml",     Description = "Cerveja Antarctica garrafa 600ml" },
                new Product { Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "Stella Artois 550ml",  Description = "Cerveja Stella Artois garrafa 550ml" },
                new Product { Id = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Budweiser 350ml",      Description = "Cerveja Budweiser lata 350ml" },
                new Product { Id = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Original 600ml",       Description = "Cerveja Original garrafa 600ml" },
                new Product { Id = new Guid("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Bohemia 600ml",        Description = "Cerveja Bohemia garrafa 600ml" },
                new Product { Id = new Guid("22222222-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Corona 330ml",         Description = "Cerveja Corona long neck 330ml" },
                new Product { Id = new Guid("33333333-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Guaraná Antarctica 2L",Description = "Refrigerante Guaraná Antarctica garrafa 2L" },
                new Product { Id = new Guid("44444444-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "H2OH! Limão 500ml",    Description = "Bebida com gás sabor limão 500ml" }
            );
        }
    }
}