namespace TestsPlaywright;

public class HomePageTests
{
    [Fact]
    public async Task HomePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync(GlobalValues.BaseUrl, GlobalValues.GetPageOptions());
        var title = await page.TitleAsync();

        Assert.Equal("Manufacturer Manager", title);
    }
}
