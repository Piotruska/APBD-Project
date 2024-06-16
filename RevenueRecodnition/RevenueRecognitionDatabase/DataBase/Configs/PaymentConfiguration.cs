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
             .Property(x => x.Amount)
             .HasPrecision(10,2)
             .IsRequired();

         
        builder
            .Property(x => x.IdContract)
            .IsRequired(false);
        
        builder
            .Property(x => x.IdSubscription)
            .IsRequired(false);
        
        builder
            .HasOne(x => x.Subscription)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.IdSubscription)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.Contract)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.IdContract)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .ToTable("Payment", "APBDProject");
    }
}