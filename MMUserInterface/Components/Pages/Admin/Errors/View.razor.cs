
namespace MMUserInterface.Components.Pages.Admin.Errors;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        ErrorModel = await ErrorHandler.GetErrorAsync(ErrorId);
        MainLayout.SetHeaderValue("View Error");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetErrorHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(ViewTextForBreadcrumb),
        ]);
    }
}