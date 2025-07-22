namespace MMUserInterface.Components.Pages.Admin.Colours;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        ColourModel = await ColourHandler.GetColourAsync(ColourId);
        MainLayout.SetHeaderValue("Delete Colour");
        OkToDelete = ColourModel.Widgets.Count == 0;
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(DeleteTextForBreadcrumb),
        ]);
    }

    private async Task DeleteColour()
    {
        try
        {
            await ColourHandler.DeleteColourAsync(ColourId, true);
            Snackbar.Add($"Colour {ColourModel.Name} successfully deleted", Severity.Success);
            NavigationManager.NavigateTo("/colours/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting colour {ColourModel.Name}. Please try again", Severity.Error);
        }
    }
}
