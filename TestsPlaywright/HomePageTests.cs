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
        var page = await browser.NewPageAsync();
        await page.GotoAsync(GlobalValues.BaseUrl, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        var title = await page.TitleAsync();

        Assert.Equal("Manufacturer Manager", title);
    }
}
