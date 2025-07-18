namespace MMDataAccess.Interfaces;

public interface IWidgetStatusHandler
{
    Task<List<WidgetStatusModel>> GetWidgetStatusesAsync();
}
