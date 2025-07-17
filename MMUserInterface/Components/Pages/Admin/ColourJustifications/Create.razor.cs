namespace MMUserInterface.Components.Pages.Admin.ColourJustifications;

public partial class Create
{
    protected override void OnInitialized()
    {
        MainLayout.SetHeaderValue("Create Colour Justification");
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourJustificationHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(CreateTextForBreadcrumb),
        ]);
    }

    private async Task CreateColourJustification()
    {
        ColourJustificationModel = new ColourJustificationModel
        {
            Justification = ColourJustificationDisplayModel.Justification,
        };

        try
        {
            await ColourJustificationHandler.CreateColourJustificationAsync(ColourJustificationModel, true);
            Snackbar.Add($"Colour Justification {ColourJustificationModel.Justification} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/colourjustifications/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating Colour Justification {ColourJustificationModel.Justification}. Please try again", Severity.Error);
        }
    }
}
