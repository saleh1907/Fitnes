using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication4.Models;

namespace WebApplication4.Configurations;

public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
{
    public void Configure(EntityTypeBuilder<Trainer> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x=>x.Description).IsRequired().HasMaxLength(1024);
        builder.Property(x=>x.ImagePath).IsRequired().HasMaxLength(1024);


        builder.HasOne(x => x.Departament).WithMany(x => x.Trainers).HasForeignKey(x => x.DepartamenId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);

    }
}
