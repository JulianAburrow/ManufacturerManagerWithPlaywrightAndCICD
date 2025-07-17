namespace MMUserInterface.Shared.BasePageClasses;

public class ColourJustificationBasePageClass : BasePageClass
{
    [Inject] protected IColourJustificationHandler ColourJustificationHandler { get; set; } = default!;

    [Parameter] public int ColourJustificationId {  get; set; }

    protected ColourJustificationModel ColourJustificationModel { get; set; } = new();

    protected ColourJustificationDisplayModel ColourJustificationDisplayModel { get; set; } = new();

    protected string ColourJustification = "Colour Justification";

    protected string ColourJustificationPlural = "Colour Justifications";

    protected BreadcrumbItem GetColourJustificationHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new ("ColourJustifications", "/colourjustifications/index", isDisabled);
    }
}
