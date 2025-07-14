namespace MMUserInterface.Shared.Components;

public partial class CannotDeleteComponent
{
    [Parameter]
    public string ObjectType { get; set; } = default!;

    [Parameter]
    public string ObjectName { get; set; } = default!;

    [Parameter]
    public int WidgetCount { get; set; }

    private string CancelURL => $"{ObjectType.Replace(" ", "").ToLower()}s/index";
}
