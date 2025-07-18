namespace MMDataAccess.Handlers;

public class WidgetHandler(ManufacturerManagerContext context) : IWidgetHandler
{
    private readonly ManufacturerManagerContext _context = context;

    public async Task CreateWidgetAsync(WidgetModel widget, bool callSaveChanges)
    {
        _context.Widgets.Add(widget);
        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task DeleteWidgetAsync(int widgetId, bool callSaveChanges)
    {
        var widgetToDelete = _context.Widgets.SingleOrDefault(w => w.WidgetId == widgetId);
        if (widgetToDelete == null)
            return;
        _context.Widgets.Remove(widgetToDelete);
        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task<WidgetModel> GetWidgetAsync(int widgetId) =>
        await _context.Widgets
            .Include(w => w.Colour)
            .Include(w => w.ColourJustification)
            .Include(w => w.Manufacturer)
            .Include(w => w.Status)
            .AsNoTracking()
            .SingleOrDefaultAsync(w => w.WidgetId == widgetId)
            ?? throw new ArgumentNullException(nameof(widgetId), "Widget not found");

    public async Task<List<WidgetModel>> GetWidgetsAsync() =>
        await _context.Widgets
        .Include(w => w.Manufacturer)
        .Include(w => w.Colour)
        .Include(w => w.Status)
        .OrderBy(w => w.Name)
        .AsNoTracking()
        .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateWidgetAsync(WidgetModel widget, bool callSaveChanges)
    {
        var widgetToUpdate = _context.Widgets.SingleOrDefault(w => w.WidgetId == widget.WidgetId);
        if (widgetToUpdate == null)
            return;
        widgetToUpdate.Name = widget.Name;
        widgetToUpdate.ColourId = widget.ColourId;
        widgetToUpdate.ColourJustificationId = widget.ColourJustificationId;
        widgetToUpdate.ManufacturerId = widget.ManufacturerId;
        widgetToUpdate.StatusId = widget.StatusId;
        widgetToUpdate.CostPrice = widget.CostPrice;
        widgetToUpdate.RetailPrice = widget.RetailPrice;
        widgetToUpdate.StockLevel = widget.StockLevel;
        widgetToUpdate.WidgetImage = widget.WidgetImage;

        if (callSaveChanges)
            await SaveChangesAsync();
    }
}
