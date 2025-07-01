using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MMDataAccess.Configuration;

public class ManufacturerConfiguration : IEntityTypeConfiguration<ManufacturerModel>
{
    public void Configure(EntityTypeBuilder<ManufacturerModel> builder)
    {
        builder.ToTable("Manufacturer");
        builder.HasKey(nameof(ManufacturerModel.ManufacturerId));
        builder.HasOne(e => e.Status)
            .WithMany(e => e.Manufacturers)
            .HasForeignKey(e => e.StatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
