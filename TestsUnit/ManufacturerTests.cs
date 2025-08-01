﻿namespace TestsUnit;

public class ManufacturerTests
{
    private readonly ManufacturerManagerContext _manufacturerManagerContext;
    private readonly IManufacturerHandler _manufacturerHandler;

    public ManufacturerTests()
    {
        _manufacturerManagerContext = TestsUnitHelper.GetContextWithOptions();
        _manufacturerHandler = new ManufacturerHandler(_manufacturerManagerContext);
    }

    private readonly ManufacturerModel Manufacturer1 = new()
    {
        Name = "Manufacturer1",
        StatusId = (int)PublicEnums.ManufacturerStatusEnum.Active,
    };
    private readonly ManufacturerModel Manufacturer2 = new()
    {
        Name = "Manufacturer2",
        StatusId = (int)PublicEnums.ManufacturerStatusEnum.Active,
    };
    private readonly ManufacturerModel Manufacturer3 = new()
    {
        Name = "Manufacturer3",
        StatusId = (int)PublicEnums.ManufacturerStatusEnum.Inactive,
    };
    private readonly ManufacturerModel Manufacturer4 = new()
    {
        Name = "Manufacturer4",
        StatusId = (int)PublicEnums.ManufacturerStatusEnum.Inactive,
    };

    [Fact]
    public async Task CreateManufacturerCreatesManufacturer()
    {
        var initialCount = _manufacturerManagerContext.Manufacturers.Count();

        await _manufacturerHandler.CreateManufacturerAsync(Manufacturer1, false);
        await _manufacturerHandler.CreateManufacturerAsync(Manufacturer2, false);
        await _manufacturerHandler.CreateManufacturerAsync(Manufacturer3, false);
        await _manufacturerHandler.CreateManufacturerAsync(Manufacturer4, true);

        _manufacturerManagerContext.Manufacturers.Count().Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetManufacturerGetsManufacturer()
    {
        _manufacturerManagerContext.Manufacturers.Add(Manufacturer4);
        _manufacturerManagerContext.SaveChanges();

        var returnedManufacturer = await _manufacturerHandler.GetManufacturerAsync(Manufacturer4.ManufacturerId);
        returnedManufacturer.Should().NotBeNull();
        Assert.Equal(Manufacturer4.Name, returnedManufacturer.Name);
    }

    [Fact]
    public async Task GetManufacturersGetsManufacturers()
    {
        var initialCount = _manufacturerManagerContext.Manufacturers.Count();

        _manufacturerManagerContext.Manufacturers.Add(Manufacturer1);
        _manufacturerManagerContext.Manufacturers.Add(Manufacturer2);
        _manufacturerManagerContext.Manufacturers.Add(Manufacturer3);
        _manufacturerManagerContext.Manufacturers.Add(Manufacturer4);
        _manufacturerManagerContext.SaveChanges();

        var manufacturersReturned = await _manufacturerHandler.GetManufacturersAsync();

        manufacturersReturned.Count.Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task UpdateManufacturerUpdatesManufacturer()
    {
        var newManufacturer = "AceWidgets";

        _manufacturerManagerContext.Manufacturers.Add(Manufacturer3);
        _manufacturerManagerContext.SaveChanges();

        var manufacturerToUpdate = _manufacturerManagerContext.Manufacturers.First(m => m.ManufacturerId == Manufacturer3.ManufacturerId);
        manufacturerToUpdate.Name = newManufacturer;
        await _manufacturerHandler.UpdateManufacturerAsync(manufacturerToUpdate, true);

        var updatedManufacturer = _manufacturerManagerContext.Manufacturers.First(m => m.ManufacturerId == Manufacturer3.ManufacturerId);
        updatedManufacturer.Name.Should().Be(newManufacturer);
    }

    [Fact]
    public async Task DeleteManufacturerDeletesManufacturer()
    {
        _manufacturerManagerContext.Manufacturers.Add(Manufacturer1);
        _manufacturerManagerContext.SaveChanges();
        var manufacturerId = Manufacturer1.ManufacturerId;
        await _manufacturerHandler.DeleteManufacturerAsync(manufacturerId, true);

        Func<Task> act = async () => await _manufacturerHandler.GetManufacturerAsync(manufacturerId);
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task SetManufacturerInactiveSetsWidgetsForManufacturerInactive()
    {
        _manufacturerManagerContext.Manufacturers.Add(Manufacturer1);
        _manufacturerManagerContext.SaveChanges();
        var widget1 = new WidgetModel
        {
            Name = "Widget1",
            ManufacturerId = Manufacturer1.ManufacturerId,
            ColourId = 1,
            StatusId = (int)PublicEnums.WidgetStatusEnum.Active
        };
        _manufacturerManagerContext.Widgets.Add(widget1);
        _manufacturerManagerContext.SaveChanges();
        Manufacturer1.StatusId = (int)PublicEnums.ManufacturerStatusEnum.Inactive;
        await _manufacturerHandler.UpdateManufacturerAsync(Manufacturer1, true);
        var updatedWidgets = _manufacturerManagerContext.Widgets.Where(w => w.WidgetId == widget1.WidgetId);
        foreach (var updatedWidget in updatedWidgets)
        {
            updatedWidget.StatusId.Should().Be((int)PublicEnums.WidgetStatusEnum.Inactive);
        }
    }
}
