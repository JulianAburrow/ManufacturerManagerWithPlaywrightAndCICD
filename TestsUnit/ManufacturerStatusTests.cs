namespace TestsUnit;

public class ManufacturerStatusTests
{
    private readonly ManufacturerManagerContext _manufacturerManagerContext;
    private readonly IManufacturerStatusHandler _manufacturerStatusHandler;

    public ManufacturerStatusTests()
    {
        _manufacturerManagerContext = TestsUnitHelper.GetContextWithOptions();
        _manufacturerStatusHandler = new ManufacturerStatusHandler(_manufacturerManagerContext);
    }

    private readonly ManufacturerStatusModel ManufacturerStatus1 = new()
    {
        StatusName = PublicEnums.ManufacturerStatusEnum.Active.ToString(),
    };
    private readonly ManufacturerStatusModel ManufacturerStatus2 = new()
    {
        StatusName = PublicEnums.ManufacturerStatusEnum.Inactive.ToString(),
    };

    [Fact]
    public async Task GetManufacturerStatusesGetsManufacturerStatuses()
    {
        var initialCount = _manufacturerManagerContext.ManufacturerStatuses.Count();

        _manufacturerManagerContext.ManufacturerStatuses.Add(ManufacturerStatus1);
        _manufacturerManagerContext.ManufacturerStatuses.Add(ManufacturerStatus2);
        _manufacturerManagerContext.SaveChanges();

        var manufacturerStatusesReturned = await _manufacturerStatusHandler.GetManufacturerStatusesAsync();

        manufacturerStatusesReturned.Count.Should().Be(initialCount + 2);
    }
}
