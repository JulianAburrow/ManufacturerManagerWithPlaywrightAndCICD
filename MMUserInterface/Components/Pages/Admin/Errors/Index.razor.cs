namespace MMUserInterface.Components.Pages.Admin.Errors;

public partial class Index
{
    List<ErrorModel> Errors { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Errors = await ErrorHandler.GetErrorsAsync();
        Snackbar.Add($"{Errors.Count} item(s) found", Errors.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("Errors");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetErrorHomeBreadcrumbItem(true),
        ]);
    }
}