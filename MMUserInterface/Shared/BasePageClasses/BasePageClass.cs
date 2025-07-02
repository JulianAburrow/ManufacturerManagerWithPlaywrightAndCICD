namespace MMUserInterface.Shared.BasePageClasses;

public class BasePageClass : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    protected BreadcrumbItem GetHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new("Home", "#", isDisabled, icon: Icons.Material.Filled.Home);
    }

    protected BreadcrumbItem GetCustomBreadcrumbItem(string text)
    {
        return new(text, null, true);
    }

    protected string CreateTextForBreadcrumb = "Create";

    protected string DeleteTextForBreadcrumb = "Delete";

    protected string EditTextForBreadcrumb = "Edit";

    protected string ViewTextForBreadcrumb = "View";

    protected bool OkToDelete { get; set; }
}
