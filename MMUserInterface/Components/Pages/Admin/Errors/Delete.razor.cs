namespace MMUserInterface.Components.Pages.Admin.Errors;

public partial class Delete
{
    protected override async Task OnInitializedAsync()
    {
        ErrorModel = await ErrorHandler.GetErrorAsync(ErrorId);
        MainLayout.SetHeaderValue("Delete Error");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetErrorHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(DeleteTextForBreadcrumb),
        ]);
    }

    private async Task DeleteError()
    {
        try
        {
            await ErrorHandler.DeleteErrorAsync(ErrorId, true);
            Snackbar.Add($"Error {ErrorModel.ErrorMessage} successfully deleted", Severity.Success);
            NavigationManager.NavigateTo("/errors/index");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"An error occurred deleting error {ErrorModel.ErrorMessage}. Please try again", Severity.Error);
            await ErrorHandler.CreateErrorAsync(ex, true);
        }
    }
}