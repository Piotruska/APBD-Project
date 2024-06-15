using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class IndividualClientConfiguration : IEntityTypeConfiguration<IndividualClient>
{
    public void Configure(EntityTypeBuilder<IndividualClient> builder)
    {
        builder
            .HasKey(x => x.IdClient);
        
        builder
            .Property(x => x.IdClient)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .Property(x => x.FirstName)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.LastName)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .Property(x => x.PESEL)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .ToTable("IndividualClient", "APBDProject");
    }
}