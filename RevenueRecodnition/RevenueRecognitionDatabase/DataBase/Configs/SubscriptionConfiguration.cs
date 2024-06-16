using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder
            .HasKey(x => x.IdSubscription);
        
        builder
            .Property(x => x.IdSubscription)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .Property(x => x.RenewalPeriod)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .Property(x => x.Price)
            .HasPrecision(10,2)
            .IsRequired();
        
        builder
            .Property(x => x.StartDateRenewalPayement)
            .IsRequired();
        
        builder
            .Property(x => x.EndDateRenewalPayement)
            .IsRequired();
        
        builder
            .Property(x => x.Canceled)
            .IsRequired();
        
        builder
            .Property(x => x.IdClient)
            .IsRequired();
        
        builder
            .Property(x => x.IdProduct)
            .IsRequired();

        builder
            .HasOne(x => x.Client)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.IdClient);
        
        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.IdProduct);
        
        builder
            .ToTable("Subscription", "APBDProject");
    }
}