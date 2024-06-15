using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.DataBase.Configs;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(x => x.IdUser);
        
        builder
            .Property(x => x.IdUser)
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .Property(x => x.Username)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .Property(x => x.Password)
            .HasMaxLength(200)
            .IsRequired();
        
        builder
            .ToTable("User", "APBDProject");
    }
}