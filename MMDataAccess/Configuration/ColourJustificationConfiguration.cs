namespace MMDataAccess.Configuration;

public class ColourJustificationConfiguration : IEntityTypeConfiguration<ColourJustificationModel>
{
    public void Configure(EntityTypeBuilder<ColourJustificationModel> builder)
    {
        builder.ToTable("ColourJustification");
        builder.HasKey(nameof(ColourJustificationModel.ColourJustificationId));
        builder.HasMany(e => e.Widgets)
            .WithOne(e => e.ColourJustification)
            .HasForeignKey(e => e.ColourJustificationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}