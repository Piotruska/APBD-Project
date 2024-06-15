using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder
            .HasKey(x => x.IdPayement);
        
        builder
            .Property(x => x.IdPayement)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
         builder
                    .Property(x => x.DatePayed)
                    .IsRequired();

        builder
            .Property(x => x.IdContract)
            .IsRequired();
        
        builder
            .Property(x => x.IdSubscription)
            .IsRequired();
        
        builder
            .HasOne(x => x.Subscription)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.IdSubscription);
        
        builder
            .HasOne(x => x.Contract)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.IdContract);
        
        builder
            .ToTable("Payment", "APBDProject");
    }
}