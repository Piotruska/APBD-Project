using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder
            .HasKey(x => x.IdClient);
        
        builder
            .Property(x => x.IdClient)
            .ValueGeneratedOnAdd()
            .IsRequired();
        builder
            .Property(x => x.Address)
            .HasMaxLength(200)
            .IsRequired();
        builder
            .Property(x => x.Email)
            .HasMaxLength(200)
            .IsRequired();
        builder
            .Property(x => x.PhoneNumber)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .HasOne(x => x.IndividualClient)
            .WithOne(x => x.Client)
            .HasForeignKey<IndividualClient>(x=>x.IdClient);
        
        builder
            .HasOne(x => x.CompanyClient)
            .WithOne(x => x.Client)
            .HasForeignKey<CompanyClient>(x=>x.IdClient);
        
        builder
            .ToTable("Client", "APBDProject");
    }
}