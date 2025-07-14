namespace MMUserInterface.Components.Pages.Admin.Colours;

public partial class Index
{
    List<ColourModel> Colours { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Colours = await ColourHandler.GetColoursAsync();
        Snackbar.Add($"{Colours.Count} item(s) found", Colours.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("Colours");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourHomeBreadcrumbItem(true),
        ]);
    }
}
