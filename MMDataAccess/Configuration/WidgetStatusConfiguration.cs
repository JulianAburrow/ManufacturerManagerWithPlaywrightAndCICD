namespace MMDataAccess.Configuration;

public class WidgetStatusConfiguration : IEntityTypeConfiguration<WidgetStatusModel>
{
    public void Configure(EntityTypeBuilder<WidgetStatusModel> builder)
    {
        builder.ToTable("WidgetStatus");
        builder.HasKey(nameof(WidgetStatusModel.StatusId));
    }
}
