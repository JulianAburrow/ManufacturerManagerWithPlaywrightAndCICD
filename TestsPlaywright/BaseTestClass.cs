namespace TestsPlaywright;

[Collection("Sequential")]

public abstract class BaseTestClass
{
    protected ManufacturerManagerContext _context;

    protected BaseTestClass()
    {
        _context = new ManufacturerManagerContext(PlaywrightTestHelper.GetContextOptions());
        _context.Database.EnsureCreated();
    }
}
