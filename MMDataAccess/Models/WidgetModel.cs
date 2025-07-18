namespace MMDataAccess.Models;

public class WidgetModel
{
    public int WidgetId { get; set; }

    public string Name { get; set; } = default!;

    public int ManufacturerId { get; set; }

    public int? ColourId { get; set; }

    public int? ColourJustificationId { get; set; }

    public int StatusId { get; set; }

    public decimal CostPrice { get; set; }

    public decimal RetailPrice { get; set; }

    public int StockLevel {  get; set; }

    public byte[]? WidgetImage { get; set; }

    public ManufacturerModel Manufacturer { get; set; } = null!;

    public WidgetStatusModel Status { get; set; } = null!;

    public ColourModel Colour { get; set; } = null!;

    public ColourJustificationModel ColourJustification { get; set; } = null!;
}
