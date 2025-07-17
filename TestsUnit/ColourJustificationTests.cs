namespace TestsUnit;

public class ColourJustificationTests
{
    private readonly ManufacturerManagerContext _manufacturerManagerContext;
    private readonly IColourJustificationHandler _colourJustificationHandler;

    public ColourJustificationTests()
    {
        var options = new DbContextOptionsBuilder<ManufacturerManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _manufacturerManagerContext = new ManufacturerManagerContext(options);
        _colourJustificationHandler = new ColourJustificationHandler(_manufacturerManagerContext);
    }

    private readonly ColourJustificationModel ColourJustification1 = new()
    {
        Justification = "Justification1",
    };
    private readonly ColourJustificationModel ColourJustification2 = new()
    {
        Justification = "Justification2",
    };
    private readonly ColourJustificationModel ColourJustification3 = new()
    {
        Justification = "Justification3",
    };
    private readonly ColourJustificationModel ColourJustification4 = new()
    {
        Justification = "Justification4",
    };

    [Fact]
    public async Task CreateColourJustificationCreatesColourJustification()
    {
        var initialCount = _manufacturerManagerContext.ColourJustifications.Count();

        await _colourJustificationHandler.CreateColourJustificationAsync(ColourJustification1, false);
        await _colourJustificationHandler.CreateColourJustificationAsync(ColourJustification2, false);
        await _colourJustificationHandler.CreateColourJustificationAsync(ColourJustification3, false);
        await _colourJustificationHandler.CreateColourJustificationAsync(ColourJustification4, true);

        _manufacturerManagerContext.ColourJustifications.Count().Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task GetColourJustificationGetsColourJustification()
    {
        _manufacturerManagerContext.ColourJustifications.Add(ColourJustification1);
        _manufacturerManagerContext.SaveChanges();

        var returnedColourJustification = await _colourJustificationHandler.GetColourJustificationAsync(ColourJustification1.ColourJustificationId);
        returnedColourJustification.Should().NotBeNull();
        returnedColourJustification.Justification.Should().Be(ColourJustification1.Justification);
    }

    [Fact]
    public async Task GetColourJustificationsGetsColourJustifications()
    {
        var initialCount = _manufacturerManagerContext.ColourJustifications.Count();

        _manufacturerManagerContext.ColourJustifications.Add(ColourJustification1);
        _manufacturerManagerContext.ColourJustifications.Add(ColourJustification2);
        _manufacturerManagerContext.ColourJustifications.Add(ColourJustification3);
        _manufacturerManagerContext.ColourJustifications.Add(ColourJustification4);
        _manufacturerManagerContext.SaveChanges();

        var colourJustifications = await _colourJustificationHandler.GetColourJustificationsAsync();

        colourJustifications.Count().Should().Be(initialCount + 4);
    }

    [Fact]
    public async Task UpdateColourJustificationUpdatesColourJustification()
    {
        var newJustification = "newJustification";

        _manufacturerManagerContext.ColourJustifications.Add(ColourJustification4);
        _manufacturerManagerContext.SaveChanges();

        var colourJustificationToUpdate = _manufacturerManagerContext.ColourJustifications.First(c => c.ColourJustificationId == ColourJustification4.ColourJustificationId);
        colourJustificationToUpdate.Justification = newJustification;
        await _colourJustificationHandler.UpdateColourJustificationAsync(ColourJustification4, true);

        var updatedColourJustification = _manufacturerManagerContext.ColourJustifications.First(c => c.ColourJustificationId == ColourJustification4.ColourJustificationId);
        updatedColourJustification.Justification.Should().Be(newJustification);
    }

    [Fact]
    public async Task DeleteColourJustificationDeletesColourJustification()
    {
        int colourJustificationId;

        _manufacturerManagerContext.ColourJustifications.Add(ColourJustification3);
        _manufacturerManagerContext.SaveChanges();
        colourJustificationId = ColourJustification3.ColourJustificationId;

        await _colourJustificationHandler.DeleteColourJustificationAsync(colourJustificationId, true);

        var deletedColourJustification = _manufacturerManagerContext.ColourJustifications.FirstOrDefault(c => c.ColourJustificationId == colourJustificationId);

        deletedColourJustification.Should().BeNull();
    }
}
