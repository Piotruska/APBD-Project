using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class DiscountConfiguration: IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder
            .HasKey(x => x.IdDiscount);
        
        builder
            .Property(x => x.IdDiscount)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .Property(contract => contract.StartDate)
            .IsRequired();
        
        builder
            .Property(contract => contract.EndDate)
            .IsRequired();
        
        builder
            .Property(contract => contract.Percentage)
            .HasPrecision(5,2)
            .IsRequired();
        
        builder
            .ToTable("Discount", "APBDProject");
    }
    


}