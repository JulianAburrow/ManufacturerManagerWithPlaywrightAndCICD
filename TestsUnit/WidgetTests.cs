namespace TestsUnit;
public class WidgetTests
{
    private readonly ManufacturerManagerContext _manufacturerManagerContext;
    private readonly IWidgetHandler _widgetHandler;

    public WidgetTests()
    {
        _manufacturerManagerContext = TestsUnitHelper.GetContextWithOptions();
        _widgetHandler = new WidgetHandler(_manufacturerManagerContext);
    }

    private readonly WidgetModel Widget1 = new()
    {
        Name = "Widget1",
        Manufacturer = new ManufacturerModel()
        {
            Name = "Manufacturer1",
            StatusId = (int)PublicEnums.ManufacturerStatusEnum.Active
        },
        StatusId = (int)PublicEnums.WidgetStatusEnum.Active,
        Status = new WidgetStatusModel()
        {
            StatusId = (int)PublicEnums.WidgetStatusEnum.Active,
            StatusName = PublicEnums.WidgetStatusEnum.Active.ToString()
        },
    };

    private readonly WidgetModel Widget2 = new()
    {
        Name = "Widget2",
        Manufacturer = new ManufacturerModel()
        {
            Name = "Manufacturer2",
            StatusId = (int)PublicEnums.ManufacturerStatusEnum.Inactive
        },
        StatusId = (int)PublicEnums.WidgetStatusEnum.Inactive,
        Status = new WidgetStatusModel()
        {
            StatusId = (int)PublicEnums.WidgetStatusEnum.Inactive,
            StatusName = PublicEnums.WidgetStatusEnum.Inactive.ToString()
        },
    };

    [Fact]
    public async Task CreateWidgetCreatesWidget()
    {
        var initialCount = _manufacturerManagerContext.Widgets.Count();

        await _widgetHandler.CreateWidgetAsync(Widget1, false);
        await _widgetHandler.CreateWidgetAsync(Widget2, true);

        _manufacturerManagerContext.Widgets.Count().Should().Be(initialCount + 2);
    }

    [Fact]
    public async Task GetWidgetGetsWidget()
    {
        _manufacturerManagerContext.Widgets.Add(Widget2);
        _manufacturerManagerContext.SaveChanges();

        var returnedWidget = await _widgetHandler.GetWidgetAsync(Widget2.WidgetId);
        returnedWidget.Should().NotBeNull();
        Assert.Equal(Widget2.Name, returnedWidget.Name);
        Assert.Equal(Widget2.Manufacturer.Name, returnedWidget.Manufacturer.Name);
    }

    [Fact]
    public async Task GetWidgetsGetsWidgets()
    {
        var initialCount = _manufacturerManagerContext.Widgets.Count();

        _manufacturerManagerContext.Widgets.Add(Widget1);
        _manufacturerManagerContext.Widgets.Add(Widget2);
        _manufacturerManagerContext.SaveChanges();

        var widgetsReturned = await _widgetHandler.GetWidgetsAsync();

        widgetsReturned.Count.Should().Be(initialCount + 2);
    }

    [Fact]
    public async Task UpdateWidgetUpdatesWidget()
    {
        var newWidget = "AcmeWidget";

        _manufacturerManagerContext.Widgets.Add(Widget1);
        _manufacturerManagerContext.SaveChanges();

        var widgetToUpdate = _manufacturerManagerContext.Widgets.First(w => w.WidgetId == Widget1.WidgetId);
        widgetToUpdate.Name = newWidget;
        await _widgetHandler.UpdateWidgetAsync(widgetToUpdate, true);

        var updatedWidget = _manufacturerManagerContext.Widgets.First(w => w.WidgetId == Widget1.WidgetId);
        updatedWidget.Name.Should().Be(newWidget);

    }
}
