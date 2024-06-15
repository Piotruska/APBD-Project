using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class CompanyClientConfiguration : IEntityTypeConfiguration<CompanyClient>
{
    public void Configure(EntityTypeBuilder<CompanyClient> builder)
    {
        builder
            .HasKey(x => x.IdClient);
        
        builder
            .Property(x => x.IdClient)
            .IsRequired();
        
        builder
            .Property(x => x.ComapnyName)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.KRS)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .ToTable("CompanyClient", "APBDProject");
    }
}