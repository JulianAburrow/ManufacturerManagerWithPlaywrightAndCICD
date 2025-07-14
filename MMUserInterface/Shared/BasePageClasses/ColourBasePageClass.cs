namespace MMUserInterface.Shared.BasePageClasses;

public class ColourBasePageClass : BasePageClass
{
    [Inject] protected IColourHandler ColourHandler { get; set; } = default!;

    [Parameter] public int ColourId { get; set; }

    protected ColourModel ColourModel { get; set; } = new();

    protected ColourDisplayModel ColourDisplayModel { get; set; } = new();

    protected string Colour = "Colour";

    protected string ColourPlural = "Colours";

    protected BreadcrumbItem GetColourHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new ("Colours", "/colours/index", isDisabled);
    }
}
