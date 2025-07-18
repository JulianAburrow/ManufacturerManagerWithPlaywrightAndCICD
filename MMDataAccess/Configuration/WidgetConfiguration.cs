namespace MMDataAccess.Configuration;

public class WidgetConfiguration : IEntityTypeConfiguration<WidgetModel>
{
    public void Configure(EntityTypeBuilder<WidgetModel> builder)
    {
        builder.ToTable("Widget");
        builder.HasKey(nameof(WidgetModel.WidgetId));
        builder.Property(e => e.CostPrice)
            .HasColumnType("decimal")
            .HasPrecision(18, 2);
        builder.Property(e => e.RetailPrice)
            .HasColumnType("decimal")
            .HasPrecision(18, 2);
        builder.HasOne(e => e.Status)
            .WithMany(e => e.Widgets)
            .HasForeignKey(e => e.StatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Manufacturer)
            .WithMany(e => e.Widgets)
            .HasForeignKey(e => e.ManufacturerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
