namespace MMDataAccess.Configuration;

public class ColourJustificationConfiguration : IEntityTypeConfiguration<ColourJustificationModel>
{
    public void Configure(EntityTypeBuilder<ColourJustificationModel> builder)
    {
        builder.ToTable("ColourJustification");
        builder.HasKey(nameof(ColourJustificationModel.ColourJustificationId));
    }
}