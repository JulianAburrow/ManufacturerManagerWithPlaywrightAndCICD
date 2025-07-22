namespace MMUserInterface.Components.Pages.Widgets;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        WidgetStatuses = await WidgetStatusHandler.GetWidgetStatusesAsync();
        WidgetStatuses.Insert(0, new WidgetStatusModel
        {
            StatusId = SharedValues.PleaseSelectValue,
            StatusName = SharedValues.PleaseSelectText,
        });
        Colours = await ColourHandler.GetColoursAsync();
        Colours.Insert(0, new ColourModel
        {
            ColourId = SharedValues.NoneValue,
            Name = SharedValues.NoneText,
        });
        ColourJustifications = await ColourJustificationHandler.GetColourJustificationsAsync();
        ColourJustifications.Insert(0, new ColourJustificationModel
        {
            ColourJustificationId = SharedValues.NoneValue,
            Justification = SharedValues.NoneText,
        });
        Manufacturers = await ManufacturerHandler.GetManufacturersAsync();

        WidgetModel = await WidgetHandler.GetWidgetAsync(WidgetId);
        WidgetDisplayModel.WidgetId = WidgetId;
        WidgetDisplayModel.Name = WidgetModel.Name;
        WidgetDisplayModel.ManufacturerId = WidgetModel.ManufacturerId;
        WidgetDisplayModel.ColourId = WidgetModel.ColourId != null
            ? WidgetModel.ColourId
            : SharedValues.NoneValue;
        WidgetDisplayModel.ColourJustificationId = WidgetModel.ColourJustificationId != null
            ? WidgetModel.ColourJustificationId
            : SharedValues.NoneValue;
        WidgetDisplayModel.StatusId = WidgetModel.StatusId;
        WidgetDisplayModel.Manufacturer = WidgetModel.Manufacturer;
        WidgetDisplayModel.WidgetImage = WidgetModel.WidgetImage;
        WidgetDisplayModel.CostPrice = WidgetModel.CostPrice;
        WidgetDisplayModel.RetailPrice = WidgetModel.RetailPrice;
        WidgetDisplayModel.StockLevel = WidgetModel.StockLevel;

        ManufacturerIsInactive = WidgetModel.Manufacturer.StatusId == (int)ManufacturerStatusEnum.Inactive;

        MainLayout.SetHeaderValue("Edit Widget");
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadcrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetWidgetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(EditTextForBreadcrumb),
        ]);
    }

    private async void UpdateWidget()
    {
        try
        {
            CopyDisplayModelToModel();
            await WidgetHandler.UpdateWidgetAsync(WidgetModel, true);
            Snackbar.Add($"Widget {WidgetModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/widgets/index");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating {WidgetModel.Name}. Please try again.", Severity.Error);
        }
    }
}
