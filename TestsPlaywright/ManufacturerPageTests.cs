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

        await page.GotoAsync($"{GlobalValues.BaseUrl}/", GlobalValues.GetPageOptions());
        var homeTitle = await page.TitleAsync();
        Assert.Equal("Manufacturer Manager", homeTitle);

        var manufacturersLink = page.GetByRole(AriaRole.Link, new() { Name = "Manufacturers" });
        if (await manufacturersLink.CountAsync() == 0)
        {
            manufacturersLink = page.GetByText("Manufacturers", new() { Exact = false });
            Assert.True(await manufacturersLink.CountAsync() > 0, "Manufacturers link not found in navmenu.");
        }
        await manufacturersLink.First.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Manufacturers'");
    }

    [Fact]
    public async Task CreateButtonOnIndexPageNavigatesToCreateManufacturerPage()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index", GlobalValues.GetPageOptions());
        var manufacturersTitle = await page.TitleAsync();
        Assert.Equal("Manufacturers", manufacturersTitle);

        var createButton = page.GetByRole(AriaRole.Button, new() { Name = "Create Manufacturer" });
        if (await createButton.CountAsync() == 0)
        {
            createButton = page.GetByText("Create", new() { Exact = false });
            Assert.True(await createButton.CountAsync() > 0, "Create Manufacturer button not found on Manufacturers page.");
        }
        await createButton.First.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Create Manufacturer'");
    }

    [Fact]
    public async Task ViewButtonOnIndexPageNavigatesToViewManufacturerPage()
    {
        var manufacturerId = AddManufacturer();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Manufacturers'");

            var viewButton = page.GetByRole(AriaRole.Button, new() { Name = "View" });
            if (await viewButton.CountAsync() == 0)
            {
                viewButton = page.GetByText("View", new() { Exact = false });
                Assert.True(await viewButton.CountAsync() > 0, "View button not found on Manufacturers index page.");
            }
            await viewButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'View Manufacturer'");
        }
        finally
        {
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task EditButtonOnIndexPageNavigatesToEditManufacturerPage()
    {
        var manufacturerId = AddManufacturer();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturers/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Manufacturers'");

            var editButton = page.GetByRole(AriaRole.Button, new() { Name = "Edit" });
            if (await editButton.CountAsync() == 0)
            {
                editButton = page.GetByText("Edit", new() { Exact = false });
                Assert.True(await editButton.CountAsync() > 0, "Edit button not found on Manufacturers index page.");
            }
            await editButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Edit Manufacturer'");
        }
        finally
        {
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task CanCreateManufacturer()
    {
        var manufacturer = new ManufacturerModel();
        try
        {
            var initialCount = _context.Manufacturers.Count();

            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/create", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Create Manufacturer'");

            var manufacturerName = $"Test Manufacturer {Guid.NewGuid()}";
            await page.GetByLabel("Name").FillAsync(manufacturerName);
            await page.ClickAsync("div[class*='mud-select'] div[class*='mud-input-control-input-container']");
            await page.ClickAsync("div.mud-popover div.mud-list-item:has-text('Active')");

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Create Manufacturer page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Manufacturers'");

            Assert.Equal(initialCount + 1, _context.Manufacturers.Count());

            manufacturer = await _context.Manufacturers.FirstOrDefaultAsync(m => m.Name == manufacturerName);
            Assert.NotNull(manufacturer);
        }
        finally
        {
            if (manufacturer != null)
            {
                RemoveManufacturer(manufacturer.ManufacturerId);
            }                
        }
        
    }

    [Fact]
    public async Task CanEditManufacturer()
    {
        var manufacturerId = AddManufacturer();

        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/edit/{manufacturerId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Manufacturer'");

            var updatedManufacturerName = $"Updated Manufacturer {Guid.NewGuid()}";
            await page.GetByLabel("Name").FillAsync(updatedManufacturerName);

            await page.ClickAsync("div[class*='mud-select'] div[class*='mud-input-control-input-container']");
            await page.ClickAsync("div.mud-popover div.mud-list-item:has-text('Inactive')");

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Edit Manufacturer page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Manufacturers'");

            var updatedManufacturer = await WaitForManufacturerUpdate(manufacturerId, updatedManufacturerName, (int)Enums.StatusEnum.Inactive, 2_000);
            Assert.NotNull(updatedManufacturer);
            Assert.Equal(updatedManufacturerName, updatedManufacturer.Name);
            Assert.Equal((int)Enums.StatusEnum.Inactive, updatedManufacturer.StatusId);
        }
        finally
        {
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task CancelButtonOnCreatePageNavigatesToIndex()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/create", GlobalValues.GetPageOptions());
        await page.WaitForFunctionAsync("document.title === 'Create Manufacturer'");

        var cancelButton = page.GetByRole(AriaRole.Link, new() { Name = "Cancel" });
        if (await cancelButton.CountAsync() == 0)
        {
            cancelButton = page.GetByText("Cancel", new() { Exact = false });
            Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Create page.");
        }
        await cancelButton.First.ClickAsync();
        await page.WaitForFunctionAsync("document.title === 'Manufacturers'");
    }

    [Fact]
    public async Task CancelButtonOnEditPageNavigatesToIndex()
    {
        var manufacturerId = AddManufacturer();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/edit/{manufacturerId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Manufacturer'");

            var cancelButton = page.GetByRole(AriaRole.Link, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Edit page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Manufacturers'");
        }
        finally
        {
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task CancelButtonOnViewPageNavigatesToIndex()
    {
        var manufacturerId = AddManufacturer();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/manufacturer/view/{manufacturerId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'View Manufacturer'");

            var backToListButton = page.GetByRole(AriaRole.Link, new() { Name = "Back to list" });
            if (await backToListButton.CountAsync() == 0)
            {
                backToListButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await backToListButton.CountAsync() > 0, "Back to list button not found on View page.");
            }
            await backToListButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Manufacturers'");
        }
        finally
        {
            RemoveManufacturer(manufacturerId);
        }
    }

    private int AddManufacturer()
    {
        var newManufacturer = new ManufacturerModel
        {
            Name = $"Test Manufacturer {Guid.NewGuid()}",
            StatusId = (int)Enums.StatusEnum.Active,
        };
        _context.Manufacturers.Add(newManufacturer);
        _context.SaveChanges();
        return newManufacturer.ManufacturerId;
    }

    private void RemoveManufacturer(int manufacturerId)
    {
        var manufacturer = _context.Manufacturers.Find(manufacturerId);
        if (manufacturer != null)
        {
            _context.Manufacturers.Remove(manufacturer);
            _context.SaveChanges();
        }
    }

    private async Task<ManufacturerModel?> WaitForManufacturerUpdate(int manufacturerId, string expectedName, int expectedStatusId, int timeoutMs)
    {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            var manufacturer = await _context.Manufacturers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ManufacturerId == manufacturerId);
            if (manufacturer != null && manufacturer.Name == expectedName && manufacturer.StatusId == expectedStatusId)
                return manufacturer;
            await Task.Delay(100);
        }
        return await _context.Manufacturers.FindAsync(manufacturerId);
    }
}