namespace MMUserInterface.Components.Pages.Widgets;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        WidgetStatuses = await WidgetStatusHandler.GetWidgetStatusesAsync();
        WidgetStatuses.Insert(0, new()
        {
            StatusId = SharedValues.PleaseSelectValue,
            StatusName = SharedValues.PleaseSelectText,
        });
        WidgetDisplayModel.StatusId = SharedValues.PleaseSelectValue;
        Colours = await ColourHandler.GetColoursAsync();
        ColourJustifications = await ColourJustificationHandler.GetColourJustificationsAsync();
        Manufacturers = await ManufacturerHandler.GetManufacturersAsync();
        Manufacturers.Insert(0, new()
        {
            ManufacturerId = SharedValues.PleaseSelectValue,
            Name = SharedValues.PleaseSelectText,
        });
        WidgetDisplayModel.ManufacturerId = SharedValues.PleaseSelectValue;
        Colours.Insert(0, new()
        {
            ColourId = SharedValues.NoneValue,
            Name = SharedValues.NoneText,
        });
        WidgetDisplayModel.ColourId = SharedValues.PleaseSelectValue;
        ColourJustifications.Insert(0, new()
        {
            ColourJustificationId = SharedValues.NoneValue,
            Justification = SharedValues.NoneText,
        });
        WidgetDisplayModel.ColourJustificationId = SharedValues.PleaseSelectValue;

        MainLayout.SetHeaderValue("Create Widget");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetWidgetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(CreateTextForBreadcrumb),
        ]);
    }

    private async Task CreateWidget()
    {
        try
        {
            CopyDisplayModelToModel();
            await WidgetHandler.CreateWidgetAsync(WidgetModel, true);
            Snackbar.Add($"Widget {WidgetModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/widgets/index");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"An error occurred creating Widget {WidgetModel.Name}. Please try again.", Severity.Error);
            await ErrorHandler.CreateErrorAsync(ex, true);
        }
    }
}
