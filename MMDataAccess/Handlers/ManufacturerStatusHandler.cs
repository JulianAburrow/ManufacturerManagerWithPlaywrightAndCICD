namespace MMDataAccess.Handlers;

public class ManufacturerStatusHandler : IManufacturerStatusHandler
{
    private readonly ManufacturerManagerContext _context;

    public ManufacturerStatusHandler(ManufacturerManagerContext context) =>
        _context = context;

    public async Task<List<ManufacturerStatusModel>> GetManufacturerStatusesAsync()
    {
        return await _context.ManufacturerStatuses
            .AsNoTracking()
            .ToListAsync();
    }
}
