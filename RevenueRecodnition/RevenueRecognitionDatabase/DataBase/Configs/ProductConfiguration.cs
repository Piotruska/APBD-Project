using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .HasKey(x => x.IdProduct);
        
        builder
            .Property(x => x.IdProduct)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .Property(x => x.CurrentVersion)
            .HasMaxLength(200)
            .IsRequired();
        builder
            .Property(x => x.Category)
            .HasMaxLength(200)
            .IsRequired();
        builder
            .Property(x => x.BasePrice)
            .HasPrecision(10,2)
            .IsRequired();
        
        builder
            .ToTable("Product", "APBDProject");
    }
}