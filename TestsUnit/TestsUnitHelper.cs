namespace TestsUnit;

public static class TestsUnitHelper
{
    public static ManufacturerManagerContext GetContextWithOptions()
    {
        var options = new DbContextOptionsBuilder<ManufacturerManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new ManufacturerManagerContext(options);
    }
}
