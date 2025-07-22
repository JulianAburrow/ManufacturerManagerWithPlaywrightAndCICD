namespace MMUserInterface.Models;

public class WidgetDisplayModel
{
    public int WidgetId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Name { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
    [Display(Name = "Manufacturer")]
    public int ManufacturerId { get; set; }

    public int? ColourId { get; set; }

    public int? ColourJustificationId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
    [Display(Name = "Status")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Cost Price")]
    public decimal CostPrice { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Retail Price")]
    public decimal RetailPrice { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Stock Level")]
    public int StockLevel {  get; set; }

    public byte[]? WidgetImage { get; set; }

    public ManufacturerModel Manufacturer { get; set; } = null!;
}
