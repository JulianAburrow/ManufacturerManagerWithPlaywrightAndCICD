namespace MMUserInterface.Components.Pages.Widgets;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        WidgetModel = await WidgetHandler.GetWidgetAsync(WidgetId);
        MainLayout.SetHeaderValue("View Widget");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetWidgetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(ViewTextForBreadcrumb),
        ]);
    }
}
