namespace MMUserInterface.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSqlConnections(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<ManufacturerManagerContext>(
            options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ManufacturerManagerWithMudBlazor")));

    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddTransient<IColourHandler, ColourHandler>();
        services.AddTransient<IColourJustificationHandler, ColourJustificationHandler>();
        services.AddTransient<IManufacturerHandler, ManufacturerHandler>();
        services.AddTransient<IManufacturerStatusHandler, ManufacturerStatusHandler>();
        services.AddTransient<IWidgetHandler, WidgetHandler>();
        services.AddTransient<IWidgetStatusHandler, WidgetStatusHandler>();
    }
}
