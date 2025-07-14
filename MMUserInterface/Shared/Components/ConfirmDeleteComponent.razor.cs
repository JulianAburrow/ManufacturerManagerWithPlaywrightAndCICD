namespace MMUserInterface.Shared.Components;

public partial class ConfirmDeleteComponent
{
    [Parameter]
    public string ObjectType { get; set; } = default!;

    [Parameter]
    public string ObjectName { get; set; } = default!;
}
