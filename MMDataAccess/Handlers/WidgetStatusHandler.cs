namespace MMDataAccess.Handlers;

public class WidgetStatusHandler : IWidgetStatusHandler
{
    private readonly ManufacturerManagerContext _context;

    public WidgetStatusHandler(ManufacturerManagerContext context) =>
        _context = context;

    public async Task<List<WidgetStatusModel>> GetWidgetStatusesAsync()
    {
        return await _context.WidgetStatuses
            .AsNoTracking()
            .ToListAsync();
    }
}
