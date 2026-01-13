using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication4.Models;

namespace WebApplication4.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Departament>
{
    public void Configure(EntityTypeBuilder<Departament> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);

    }
}
