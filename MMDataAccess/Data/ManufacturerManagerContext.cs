namespace MMDataAccess.Data;

public class ManufacturerManagerContext(DbContextOptions<ManufacturerManagerContext> options) : DbContext(options)
{
    public DbSet<ColourJustificationModel> ColourJustifications { get; set; }
    public DbSet<ColourModel> Colours { get; set; }
    public DbSet<ManufacturerModel> Manufacturers { get; set; }
    public DbSet<ManufacturerStatusModel> ManufacturerStatuses { get; set; }
    public DbSet<WidgetStatusModel> WidgetStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
            .Where(p => p.ClrType == typeof(string))))
        {
            property.SetIsUnicode(false);
        }

        builder.ApplyConfiguration(new ColourConfiguration());
        builder.ApplyConfiguration(new ColourJustificationConfiguration());
        builder.ApplyConfiguration(new ManufacturerConfiguration());
        builder.ApplyConfiguration(new ManufacturerStatusConfiguration());
        builder.ApplyConfiguration(new WidgetStatusConfiguration());
    }
}
