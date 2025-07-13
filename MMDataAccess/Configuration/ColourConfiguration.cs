namespace MMDataAccess.Configuration;

public class ColourConfiguration : IEntityTypeConfiguration<ColourModel>
{
    public void Configure(EntityTypeBuilder<ColourModel> builder)
    {
        builder.ToTable("Colour");
        builder.HasKey(nameof(ColourModel.ColourId));
    }
}
