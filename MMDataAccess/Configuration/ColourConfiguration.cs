namespace MMDataAccess.Configuration;

public class ColourConfiguration : IEntityTypeConfiguration<ColourModel>
{
    public void Configure(EntityTypeBuilder<ColourModel> builder)
    {
        builder.ToTable("Colour");
        builder.HasKey(nameof(ColourModel.ColourId));
        builder.HasMany(e => e.Widgets)
            .WithOne(e => e.Colour)
            .HasForeignKey(e => e.ColourId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
