namespace MMDataAccess.Models;

public class ManufacturerModel
{
    public int ManufacturerId { get; set; }

    public string Name { get; set; } = default!;

    public int StatusId { get; set; }

    public ManufacturerStatusModel Status { get; set; } = null!;

    public ICollection<WidgetModel> Widgets { get; set; } = null!;
}
