namespace MMDataAccess.Models;

public class ColourModel
{
    public int ColourId { get; set; }

    public string Name { get; set; } = default!;

    public ICollection<WidgetModel> Widgets { get; set; } = null!;
}
