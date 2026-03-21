using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
    {
        public void Configure(EntityTypeBuilder<SaleItem> builder)
        {
            builder.ToTable("SaleItem");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(i => i.SaleId)
                .IsRequired();

            builder.Property(i => i.ProductId)
                .IsRequired();

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.UnitPrice)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(i => i.Discount)
                .HasColumnType("numeric(18,2)")
                .HasDefaultValue(0m)
                .IsRequired();

            builder.Property(i => i.TotalAmount)
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            builder.Property(i => i.IsCancelled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(i => i.Product)
                .WithMany(p => p.SaleItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
