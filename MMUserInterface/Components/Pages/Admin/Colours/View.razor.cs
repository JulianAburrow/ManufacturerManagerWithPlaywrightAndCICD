namespace MMUserInterface.Components.Pages.Admin.Colours;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        ColourModel = await ColourHandler.GetColourAsync(ColourId);
        MainLayout.SetHeaderValue("View Colour");
        OkToDelete = true;
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(ViewTextForBreadcrumb),
        ]);
    }
}
