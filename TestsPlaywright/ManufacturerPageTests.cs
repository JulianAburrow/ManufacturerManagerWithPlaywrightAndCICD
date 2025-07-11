using Microsoft.EntityFrameworkCore;

namespace TestsPlaywright;

public class ManufacturerPageTests
{
    private readonly ManufacturerManagerContext _context;

    public ManufacturerPageTests()
    {
        _context = new ManufacturerManagerContext(PlaywrightTestHelper.GetContextOptions());
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task ManufacturerHomePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        // 1. Load the home page
        await page.GotoAsync($"{GlobalValues.BaseUrl}/", GlobalValues.GetPageOptions());
        var homeTitle = await page.TitleAsync();
        Assert.Equal("Manufacturer Manager", homeTitle);

        // 2. Find and click the 'Manufacturers' link
        var manufacturersLink = page.GetByRole(AriaRole.Link, new() { Name = "Manufacturers" });
        if (await manufacturersLink.CountAsync() == 0)
        {
            manufacturersLink = page.GetByText("Manufacturers", new() { Exact = false });
            Assert.True(await manufacturersLink.CountAsync() > 0, "Manufacturers link not found in navmenu.");
        }
        await manufacturersLink.First.ClickAsync();

        // 3. Wait for navigation and assert the manufacturers page has loaded
        await page.WaitForURLAsync($"{GlobalValues.BaseUrl}/manufacturers/index", new PageWaitForURLOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        await page.WaitForFunctionAsync("document.title === 'Manufacturers'");
    }

    [Fact]
    public async Task ManufacturerCreatePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        // 1. Load the Manufacturers index page
        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index", GlobalValues.GetPageOptions());
        var manufacturersTitle = await page.TitleAsync();
        Assert.Equal("Manufacturers", manufacturersTitle);

        // 2. Find and click the 'Create' button
        var createButton = page.GetByRole(AriaRole.Button, new() { Name = "Create" });
        if (await createButton.CountAsync() == 0)
        {
            createButton = page.GetByText("Create", new() { Exact = false });
            Assert.True(await createButton.CountAsync() > 0, "Create button not found on Manufacturers page.");
        }
        await createButton.First.ClickAsync();

        // 3. Wait for navigation to the create page and assert the title
        await page.WaitForURLAsync($"{GlobalValues.BaseUrl}/manufacturer/create", new PageWaitForURLOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        await page.WaitForFunctionAsync("document.title === 'Create Manufacturer'");
    }

    [Fact]
    public async Task ManufacturerEditPageLoads()
    {
        // Add manufacturer to ensure we have at least one to edit
        var newManufacturer = new ManufacturerModel
        {
            Name = $"Test Manufacturer {Guid.NewGuid()}",
            StatusId = 1, // 1 is the ID for 'Active' status
        };
        _context.Manufacturers.Add(newManufacturer);
        _context.SaveChanges();

        var page = await PlaywrightTestHelper.CreatePageAsync();

        // 1. Load the Manufacturers index page
        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/edit/{newManufacturer.ManufacturerId}", GlobalValues.GetPageOptions());
        var editManufacturerTitle = await page.TitleAsync();
        Assert.Equal("Edit Manufacturer", editManufacturerTitle);

        _context.Manufacturers.Remove(newManufacturer);
        _context.SaveChanges();
    }

    [Fact]
    public async Task CanAddManufacturer()
    {
        var initialCount = _context.Manufacturers.Count();

        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index", GlobalValues.GetPageOptions());
        await page.WaitForFunctionAsync("document.title === 'Manufacturers'");

        var createButton = page.GetByRole(AriaRole.Button, new() { Name = "Create" });
        if (await createButton.CountAsync() == 0)
        {
            createButton = page.GetByText("Create", new() { Exact = false });
            Assert.True(await createButton.CountAsync() > 0, "Create button not found on Manufacturers page.");
        }
        await createButton.First.ClickAsync();

        await page.WaitForURLAsync($"{GlobalValues.BaseUrl}/manufacturer/create", new PageWaitForURLOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
        await page.WaitForFunctionAsync("document.title === 'Create Manufacturer'");

        // 5. Fill out the form
        var manufacturerName = $"Test Manufacturer {Guid.NewGuid()}";
        await page.GetByLabel("Name").FillAsync(manufacturerName);
        await page.ClickAsync("div[class='mud-input-control mud-input-text-with-label mud-select'] div[class='mud-input-control-input-container']");
        await page.ClickAsync("div.mud-popover div.mud-list-item:has-text('Active')");

        // 6. Submit the form
        var saveButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
        if (await saveButton.CountAsync() == 0)
        {
            saveButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            Assert.True(await saveButton.CountAsync() > 0, "Submit button not found on Create Manufacturer page.");
        }
        await saveButton.First.ClickAsync();

        // 7. Wait for redirect back to index and confirm manufacturer count increased
        await page.WaitForURLAsync($"{GlobalValues.BaseUrl}/manufacturers/index", new PageWaitForURLOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });

        Assert.Equal(_context.Manufacturers.Count(), initialCount + 1);

        var manufacturer = await _context.Manufacturers.FirstOrDefaultAsync(m => m.Name == manufacturerName);
        Assert.NotNull(manufacturer);

        _context.Manufacturers.Remove(manufacturer);
        _context.SaveChanges();
    }
}