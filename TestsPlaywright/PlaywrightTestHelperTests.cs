namespace TestsPlaywright;

public class PlaywrightTestHelperTests
{
    [Fact]
    public async Task CreatePageAsync_ReturnsValidPage()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        Assert.NotNull(page);
        Assert.NotNull(page.Context);
        Assert.NotNull(page.Context.Browser);

        await page.GotoAsync("about:blank");
        var url = page.Url;
        Assert.Equal("about:blank", url);

        await page.Context.Browser.CloseAsync();
        await page.Context.DisposeAsync();
    }
}