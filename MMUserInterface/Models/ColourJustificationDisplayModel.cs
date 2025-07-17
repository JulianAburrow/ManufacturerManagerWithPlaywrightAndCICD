namespace MMUserInterface.Models;

public class ColourJustificationDisplayModel
{
    public int ColourJustificationId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Justification { get; set; } = default!;
}
