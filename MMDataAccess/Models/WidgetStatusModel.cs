namespace MMDataAccess.Models;

public class WidgetStatusModel
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = default!;

    public ICollection<WidgetModel> Widgets { get; set; } = null!;
}
