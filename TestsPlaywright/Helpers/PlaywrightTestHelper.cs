namespace TestsPlaywright.Helpers;

public static class PlaywrightTestHelper
{
    public static async Task<IPage> CreatePageAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = GlobalValues.IsHeadless
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });
        return await context.NewPageAsync();
    }

    public static DbContextOptions<ManufacturerManagerContext> GetContextOptions()
    {
        return new DbContextOptionsBuilder<ManufacturerManagerContext>()
            .UseSqlServer("Server=localhost,11433;initial catalog=ManufacturerManagerWithMudBlazor;persist security info=True;User Id=sa;Password=Pwd12345!;multipleactiveresultsets=True;TrustServerCertificate=true")
            .Options;
    }
}