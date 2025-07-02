namespace MMUserInterface.Shared.BasePageClasses;

public class ManufacturerBasePageClass : BasePageClass
{
    [Inject] protected IManufacturerHandler ManufacturerHandler { get; set; } = default!;

    [Inject] protected IManufacturerStatusHandler ManufacturerStatusHandler { get; set; } = default!;

    [Parameter] public int ManufacturerId { get; set; }

    protected ManufacturerModel ManufacturerModel = new();

    protected ManufacturerDisplayModel ManufacturerDisplayModel = new();

    public required List<ManufacturerStatusModel> ManufacturerStatuses { get; set; }

    protected string Manufacturer = "Manufacturer";

    protected string ManufacturerPlural = "Manufacturers";

    protected void CopyDisplayModelToModel()
    {
        ManufacturerModel.Name = ManufacturerDisplayModel.Name;
        ManufacturerModel.StatusId = ManufacturerDisplayModel.StatusId;
    }

    protected BreadcrumbItem GetManufacturerHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new BreadcrumbItem("Manufacturers", "/manufacturers/index", isDisabled);
    }
}
