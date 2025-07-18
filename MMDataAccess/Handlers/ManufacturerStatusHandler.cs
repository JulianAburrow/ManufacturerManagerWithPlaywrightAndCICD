namespace MMDataAccess.Handlers;

public class ManufacturerStatusHandler(ManufacturerManagerContext context) : IManufacturerStatusHandler
{
    private readonly ManufacturerManagerContext _context = context;

    public async Task<List<ManufacturerStatusModel>> GetManufacturerStatusesAsync() =>
        await _context.ManufacturerStatuses
            .AsNoTracking()
            .ToListAsync();
}
