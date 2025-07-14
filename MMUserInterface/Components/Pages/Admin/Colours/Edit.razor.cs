namespace MMUserInterface.Components.Pages.Admin.Colours;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        ColourModel = await ColourHandler.GetColourAsync(ColourId);
        ColourDisplayModel.Name = ColourModel.Name;
        MainLayout.SetHeaderValue("Edit Colour");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(EditTextForBreadcrumb),
        ]);
    }

    private async Task UpdateColour()
    {
        ColourModel.Name = ColourDisplayModel.Name;

        try
        {
            await ColourHandler.UpdateColourAsync(ColourModel, true);
            Snackbar.Add($"Colour {ColourModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/colours/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating colour {ColourModel.Name}. Please try again.", Severity.Error);
        }
    }
}
