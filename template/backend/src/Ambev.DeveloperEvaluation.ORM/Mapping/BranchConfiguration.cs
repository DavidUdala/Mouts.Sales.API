using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branch");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(b => b.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(b => b.Address)
                .HasMaxLength(500);

            builder.HasData(
                new Branch { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Branch North",  Address = "North Avenue, 100"    },
                new Branch { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Branch South",    Address = "South Avenue, 200"      },
                new Branch { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Branch East",  Address = "East Street, 300"    },
                new Branch { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Branch West",  Address = "West Street, 400"    },
                new Branch { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Branch Central", Address = "Central Street, 500"  }
            );
        }
    }
}