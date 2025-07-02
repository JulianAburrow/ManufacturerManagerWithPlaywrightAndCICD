namespace MMUserInterface.Shared.Components;

public partial class ManufacturerCreateUpdateComponent
{

    [Parameter] public ManufacturerDisplayModel ManufacturerDisplayModel { get; set; } = null!;

    [Parameter] public List<ManufacturerStatusModel> ManufacturerStatuses { get; set; } = null!;
}