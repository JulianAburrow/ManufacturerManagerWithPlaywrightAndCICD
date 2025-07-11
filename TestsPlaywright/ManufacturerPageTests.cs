namespace TestsPlaywright;

public class ManufacturerPageTests
{
    [Fact]
    public async Task ManufacturerHomePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        var title = await page.TitleAsync();

        Assert.Equal("Manufacturers", title);
    }

    [Fact]
    public async Task ManufacturerCreatePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/create", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        var title = await page.TitleAsync();
        Assert.Equal("Create Manufacturer", title);
    }
}
