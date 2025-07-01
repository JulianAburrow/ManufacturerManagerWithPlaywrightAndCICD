namespace MMDataAccess.Configuration;

public class ManufacturerStatusConfiguration : IEntityTypeConfiguration<ManufacturerStatusModel>
{
    public void Configure(EntityTypeBuilder<ManufacturerStatusModel> builder)
    {
        builder.ToTable("ManufacturerStatus");
        builder.HasKey(nameof(ManufacturerStatusModel.StatusId));
        builder.HasMany(e => e.Manufacturers)
            .WithOne(e => e.Status)
            .HasForeignKey(e => e.StatusId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
