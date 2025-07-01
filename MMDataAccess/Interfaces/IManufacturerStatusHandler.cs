namespace MMDataAccess.Interfaces;

public interface IManufacturerStatusHandler
{
    Task<List<ManufacturerStatusModel>> GetManufacturerStatusesAsync();
}
