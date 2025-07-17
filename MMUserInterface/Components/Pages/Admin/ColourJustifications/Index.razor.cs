namespace MMUserInterface.Components.Pages.Admin.ColourJustifications;

public partial class Index
{
    List<ColourJustificationModel> ColourJustifications { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        ColourJustifications = await ColourJustificationHandler.GetColourJustificationsAsync();
        Snackbar.Add($"{ColourJustifications.Count} item(s) found", ColourJustifications.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("Colour Justifications");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourJustificationHomeBreadcrumbItem(true),
        ]);
    }
}