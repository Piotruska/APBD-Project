using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class ContractConfigfuration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder
            .HasKey(x => x.IdContract);
        
        builder
            .Property(x => x.IdContract)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(contract => contract.StartDate)
            .IsRequired();
        builder
            .Property(contract => contract.EndDate)
            .IsRequired();
        builder
            .Property(contract => contract.Price)
            .HasPrecision(5,2)
            .IsRequired();
        builder
            .Property(contract => contract.IsSigned)
            .IsRequired();
        builder
            .Property(contract => contract.IdClient)
            .IsRequired();
        builder
            .Property(contract => contract.IdProduct)
            .IsRequired();

        builder.HasOne(c => c.Client)
            .WithMany(c => c.Contracts)
            .HasForeignKey(c => c.IdClient);

        builder.HasOne(c => c.Product)
            .WithMany(p => p.Contracts)
            .HasForeignKey(c => c.IdProduct);
        
        builder
            .ToTable("Contract", "APBDProject");

    }
}