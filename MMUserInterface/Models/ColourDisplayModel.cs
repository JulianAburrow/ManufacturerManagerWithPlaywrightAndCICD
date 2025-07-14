namespace MMUserInterface.Models;

public class ColourDisplayModel
{
    public int ColourId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(20, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Name { get; set; } = default!;
}
