namespace MMDataAccess.Interfaces;

public interface IWidgetHandler
{
    Task<WidgetModel> GetWidgetAsync(int widgetId);

    Task<List<WidgetModel>> GetWidgetsAsync();

    Task CreateWidgetAsync(WidgetModel widget, bool callSaveChanges);

    Task UpdateWidgetAsync(WidgetModel widget, bool callSaveChanges);

    Task DeleteWidgetAsync(int widgetId, bool callSaveChanges);

    Task SaveChangesAsync();
}
