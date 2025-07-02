namespace MMUserInterface.Components.Pages.Manufacturers;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        ManufacturerStatuses = await ManufacturerStatusHandler.GetManufacturerStatusesAsync();
        ManufacturerStatuses.Insert(0, new ManufacturerStatusModel
        {
            StatusId = SharedValues.PleaseSelectValue,
            StatusName = SharedValues.PleaseSelectText,
        });
        ManufacturerDisplayModel.StatusId = -1;
        MainLayout.SetHeaderValue("Create Manufacturer");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(new List<BreadcrumbItem>
        {
            GetHomeBreadcrumbItem(),
            GetManufacturerHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(CreateTextForBreadcrumb)
        });
    }

    private async Task CreateManufacturer()
    {
        try
        {
            CopyDisplayModelToModel();
            await ManufacturerHandler.CreateManufacturerAsync(ManufacturerModel, true);
            Snackbar.Add($"Manufacturer {ManufacturerModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/manufacturers/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating manufacturer {ManufacturerModel.Name}. Please try again.", Severity.Error);
        }
    }
}