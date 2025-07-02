using MMDataAccess.Models;

namespace MMUserInterface.Shared.Components;

public partial class ManufacturerGridViewComponent
{
    [Parameter] public List<ManufacturerModel> Manufacturers { get; set; } = null!;
}