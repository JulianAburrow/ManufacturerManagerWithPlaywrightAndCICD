namespace PlaywrightTests;

public class HomePageTests
{
    [Fact]
    public async Task HomePageLoads()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();
        await page.GotoAsync(GlobalValues.BaseUrl);
        var title = await page.TitleAsync();

        Assert.Equal("Manufacturer Manager", title);
    }
}
