namespace MMDataAccess.Models;

public class ManufacturerStatusModel
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = default!;

    public ICollection<ManufacturerModel> Manufacturers { get; set; } = null!;
}
