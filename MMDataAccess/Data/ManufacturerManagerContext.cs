namespace MMDataAccess.Data;

public class ManufacturerManagerContext : DbContext
{
    public ManufacturerManagerContext(DbContextOptions<ManufacturerManagerContext> options)
        : base(options)
    {
    }
    public DbSet<ManufacturerModel> Manufacturers { get; set; }
    public DbSet<ManufacturerStatusModel> ManufacturerStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
            .Where(p => p.ClrType == typeof(string))))
        {
            property.SetIsUnicode(false);
        }

        builder.ApplyConfiguration(new ManufacturerConfiguration());
        builder.ApplyConfiguration(new ManufacturerStatusConfiguration());
    }
}
