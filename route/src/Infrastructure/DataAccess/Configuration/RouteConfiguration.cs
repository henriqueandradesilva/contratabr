using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configuration;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        // Primary key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        // Property Entity
        builder.Property(c => c.DateCreated)
               .HasColumnType("timestamp with time zone")
               .IsRequired()
               .HasDefaultValueSql("now()");

        builder.Property(c => c.DateUpdated)
               .HasColumnType("timestamp with time zone")
               .IsRequired(false);

        // Property
        builder.Property(c => c.Origem)
               .HasColumnType("varchar(100)")
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(c => c.Destino)
               .HasColumnType("varchar(150)")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(c => c.Valor)
               .HasColumnType("double precision")
               .IsRequired();
    }
}