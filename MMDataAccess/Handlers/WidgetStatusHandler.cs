namespace MMDataAccess.Handlers;

public class WidgetStatusHandler(ManufacturerManagerContext context) : IWidgetStatusHandler
{
    private readonly ManufacturerManagerContext _context = context;

    public async Task<List<WidgetStatusModel>> GetWidgetStatusesAsync() =>
        await _context.WidgetStatuses
            .AsNoTracking()
            .ToListAsync();
}
