namespace MMUserInterface.Models;

public class ManufacturerDisplayModel
{
    public int ManufacturerId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Name { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
    [Display(Name = "Status")]
    public int StatusId { get; set; }
}
