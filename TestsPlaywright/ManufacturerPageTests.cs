namespace TestsPlaywright;

//public class ManufacturerPageTests
//{
//    [Fact]
//    public async Task ManufacturerHomePageLoads()
//    {
//        using var playwright = await Playwright.CreateAsync();
//        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//        {
//            Headless = GlobalValues.IsHeadless
//        });
//        await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
//        {
//            IgnoreHTTPSErrors = true
//        });
//        var page = await context.NewPageAsync();

//        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index", new PageGotoOptions
//        {
//            WaitUntil = WaitUntilState.NetworkIdle
//        });
//        var title = await page.TitleAsync();

//        Assert.Equal("Manufacturers", title);
//    }

//    [Fact]
//    public async Task ManufacturerCreatePageLoads()
//    {
//        using var playwright = await Playwright.CreateAsync();
//        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//        {
//            Headless = GlobalValues.IsHeadless
//        });
//        await using var context = await browser.NewContextAsync(new BrowserNewContextOptions
//        {
//            IgnoreHTTPSErrors = true
//        });
//        var page = await context.NewPageAsync();
//        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/create", new PageGotoOptions
//        {
//            WaitUntil = WaitUntilState.NetworkIdle
//        });
//        var title = await page.TitleAsync();
//        Assert.Equal("Create Manufacturer", title);
//    }
//}
