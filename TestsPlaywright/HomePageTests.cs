namespace TestsPlaywright;

public class HomePageTests
{
    [Fact]
    public async Task HomePageLoads()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = GlobalValues.IsHeadless
        });
        await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        });

        var page = await context.NewPageAsync();
        await page.GotoAsync(GlobalValues.BaseUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        var title = await page.TitleAsync();

        Assert.Equal("Manufacturer Manager", title);
    }
}
