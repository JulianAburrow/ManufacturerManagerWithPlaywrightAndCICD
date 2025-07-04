namespace TestsPlaywright;

public class ManufacturerPageTests
{
    [Fact]
    public async Task ManufacturerHomePageLoads()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index");
        var title = await page.TitleAsync();

        Assert.Equal("Manufacturers", title);
    }

    [Fact]
    public async Task ManufacturerCreatePageLoads()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();
        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/create");
        var title = await page.TitleAsync();
        Assert.Equal("Create Manufacturer", title);
    }
}
