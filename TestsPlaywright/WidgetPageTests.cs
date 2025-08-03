namespace TestsPlaywright;

public class WidgetPageTests : BaseTestClass
{
    [Fact]
    public async Task WidgetHomePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/", GlobalValues.GetPageOptions());
        var homeTitle = await page.TitleAsync();
        Assert.Equal("Manufacturer Manager", homeTitle);

        var widgetsLink = page.GetByRole(AriaRole.Link, new() { Name = "Widgets" });
        if (await widgetsLink.CountAsync() == 0)
        {
            widgetsLink = page.GetByText("Widgets", new() { Exact = false });
            Assert.True(await widgetsLink.CountAsync() > 0, "Widgets link not found in navmenu");
        }
        await widgetsLink.First.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Widgets'");
    }

    [Fact]
    public async Task CreateButtonOnIndexPageNavigatesToCreatePage()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/widgets/index", GlobalValues.GetPageOptions());
        var widgetsTitle = await page.TitleAsync();
        Assert.Equal("Widgets", widgetsTitle);

        var createButton = page.GetByRole(AriaRole.Button, new() { Name = "Create Widget" });
        if (await createButton.CountAsync() == 0)
        {
            createButton = page.GetByText("Create Widget", new() { Exact = false });
            Assert.True(await createButton.CountAsync() > 0, "Create Widget button not found on Widgets page");
        }
        await createButton.First.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Create Widget'");
    }

    [Fact]
    public async Task ViewButtonOnIndexPageNavigatesToViewWidgetPage()
    {
        var manufacturerId = AddManufacturer();
        var widgetId = AddWidget(manufacturerId);
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/widgets/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Widgets'");

            var viewButton = page.GetByRole(AriaRole.Link, new() { Name = "View" });
            if (await viewButton.CountAsync() == 0)
            {
                viewButton = page.GetByText("View", new() { Exact = false });
                Assert.True(await viewButton.CountAsync() > 0, "View button not found on Widgets index page.");
            }
            await viewButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'View Widget'");
        }
        finally
        {
            RemoveWidget(widgetId);
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task EditButtonOnIndexPageNavigatesToEditWidgetPage()
    {
        var manufacturerId = AddManufacturer();
        var widgetId = AddWidget(manufacturerId);
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/widgets/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Widgets'");

            var editButton = page.GetByRole(AriaRole.Link, new() { Name = "Edit" });
            if (await editButton.CountAsync() == 0)
            {
                editButton = page.GetByText("Edit", new() { Exact = false });
                Assert.True(await editButton.CountAsync() > 0, "Edit button not found on Widgets index page.");
            }
            await editButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Edit Widget'");
        }
        finally
        {
            RemoveWidget(widgetId);
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task CanCreateWidget()
    {
        var manufacturerId = AddManufacturer();
        var widget = new WidgetModel();

        try
        {
            // database will have been seeded with colours and colour justifications.
            var colourName = "Red";
            var colourJustificationName = "Customer request";
            var statusName = PublicEnums.WidgetStatusEnum.Active.ToString();
            var costPrice = 10m;
            var retailPrice = 20m;
            var stockLevel = 5;
            var initialCount = _context.Widgets.Count();
            var manufacturer = _context.Manufacturers.First(m => m.ManufacturerId == manufacturerId);

            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/widget/create", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Create Widget'");
            var widgetName = $"Test Widget {Guid.NewGuid()}";
            await page.GetByLabel("Name").FillAsync(widgetName);
            await SelectDropdownOption(page, "manufacturer-select", manufacturer.Name);
            await SelectDropdownOption(page, "colour-select", colourName);
            await SelectDropdownOption(page, "colour-justification-select", colourJustificationName);
            await SelectDropdownOption(page, "status-select", statusName);
            await page.GetByLabel("Cost Price").FillAsync(costPrice.ToString());
            await page.GetByLabel("Retail Price").FillAsync(retailPrice.ToString());
            await page.GetByLabel("Stock Level").FillAsync(stockLevel.ToString());

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Create Widget page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Widgets'");

            Assert.Equal(initialCount + 1, _context.Widgets.Count());

            widget = await _context.Widgets
                .Include(w => w.Manufacturer)
                .Include(w => w.Colour)
                .Include(w => w.ColourJustification)
                .FirstOrDefaultAsync(w => w.Name == widgetName);
            Assert.NotNull(widget);
            Assert.Equal(widgetName, widget.Name);
            Assert.Equal(manufacturer.Name, widget.Manufacturer.Name);
            Assert.Equal(colourName, widget.Colour.Name);
            Assert.Equal(colourJustificationName, widget.ColourJustification.Justification);
            Assert.Equal((int)PublicEnums.WidgetStatusEnum.Active, widget.StatusId);
            Assert.Equal(costPrice, widget.CostPrice);
            Assert.Equal(retailPrice, widget.RetailPrice);
            Assert.Equal(stockLevel, widget.StockLevel);
        }
        finally
        {
            if (widget != null)
            {
                RemoveWidget(widget.WidgetId);
            }
            _context.ChangeTracker.Clear();
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task CanEditWidget()
    {
        var manufacturerId = AddManufacturer();
        var newManufacturerId = AddManufacturer();
        var widgetId = AddWidget(manufacturerId);

        try
        {
            // database will have been seeded with colours and colour justifications.
            var colourName = "Red";
            var colourJustificationName = "Customer request";
            var statusName = "Inactive";
            var costPrice = 10m;
            var retailPrice = 20m;
            var stockLevel = 5;
            var newManufacturer = _context.Manufacturers.First(m => m.ManufacturerId == newManufacturerId);

            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/widget/edit/{widgetId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Widget'");

            var updatedWidgetName = $"Updated Widget {Guid.NewGuid()}";
            await page.GetByLabel("Name").FillAsync(updatedWidgetName);
            await SelectDropdownOption(page, "manufacturer-select", newManufacturer.Name);
            await SelectDropdownOption(page, "colour-select", colourName);
            await SelectDropdownOption(page, "colour-justification-select", colourJustificationName);
            await SelectDropdownOption(page, "status-select", statusName);
            await page.GetByLabel("Cost Price").FillAsync(costPrice.ToString());
            await page.GetByLabel("Retail Price").FillAsync(retailPrice.ToString());
            await page.GetByLabel("Stock Level").FillAsync(stockLevel.ToString());

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Edit Widget page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Widgets'");

            var updatedWidget = await WaitForWidgetUpdate(widgetId, updatedWidgetName, 5000);
            Assert.NotNull(updatedWidget);
            Assert.Equal(updatedWidgetName, updatedWidget.Name);
            Assert.Equal(newManufacturer.Name, updatedWidget.Manufacturer.Name);
            Assert.Equal(colourName, updatedWidget.Colour.Name);
            Assert.Equal(colourJustificationName, updatedWidget.ColourJustification.Justification);
            Assert.Equal((int)PublicEnums.WidgetStatusEnum.Inactive, updatedWidget.StatusId);
            Assert.Equal(costPrice, updatedWidget.CostPrice);
            Assert.Equal(retailPrice, updatedWidget.RetailPrice);
            Assert.Equal(stockLevel, updatedWidget.StockLevel);
        }
        finally
        {
            RemoveWidget(widgetId);
            _context.ChangeTracker.Clear();
            RemoveManufacturer(manufacturerId);
            RemoveManufacturer(newManufacturerId);
        }
    }

    [Fact]
    public async Task CancelButtonOnCreatePageNavigatesToIndex()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/widget/create", GlobalValues.GetPageOptions());
        await page.WaitForFunctionAsync("document.title === 'Create Widget'");

        var cancelButton = page.GetByRole(AriaRole.Link, new() { Name = "Cancel" });
        if (await cancelButton.CountAsync() == 0)
        {
            cancelButton = page.GetByText("Cancel", new() { Exact = false });
            Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Create page.");
        }
        await cancelButton.First.ClickAsync();
        await page.WaitForFunctionAsync("document.title === 'Widgets'");
    }

    [Fact]
    public async Task CancelButtonOnEditPageNavigatesToIndex()
    {
        var manufacturerId = AddManufacturer();
        var widgetId = AddWidget(manufacturerId);
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/widget/edit/{widgetId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Widget'");

            var cancelButton = page.GetByRole(AriaRole.Link, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Edit page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Widgets'");
        }
        finally
        {
            RemoveWidget(widgetId);
            _context.ChangeTracker.Clear();
            RemoveManufacturer(manufacturerId);
        }
    }

    [Fact]
    public async Task CancelButtonOnViewPageNavigatesToIndex()
    {
        var manufacturerId = AddManufacturer();
        var widgetId = AddWidget(manufacturerId);
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/widget/view/{widgetId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'View Widget'");

            var backToListButton = page.GetByRole(AriaRole.Link, new() { Name = "Back to list" });
            if (await backToListButton.CountAsync() == 0)
            {
                backToListButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await backToListButton.CountAsync() > 0, "Back to list button not found on View page.");
            }
            await backToListButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Widgets'");
        }
        finally
        {
            RemoveWidget(widgetId);
            _context.ChangeTracker.Clear();
            RemoveManufacturer(manufacturerId);
        }
    }

    private int AddWidget(int manufacturerId)
    {
        var newWidget = new WidgetModel
        {
            Name = $"Test Widget {Guid.NewGuid()}",
            ManufacturerId = manufacturerId,
            StatusId = (int)PublicEnums.WidgetStatusEnum.Active,
        };
        _context.Widgets.Add(newWidget);
        _context.SaveChanges();
        return newWidget.WidgetId;
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

    private void RemoveWidget(int widgetId)
    {
        var widget = _context.Widgets.Find(widgetId);
        if (widget != null)
        {
            _context.Widgets.Remove(widget);
            _context.SaveChanges();
        }
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

    private async Task<WidgetModel?> WaitForWidgetUpdate(int widgetId, string expectedName, int timeoutMs)
    {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            var widget = await _context.Widgets
                .Include(w => w.Manufacturer)
                .Include(w => w.Colour)
                .Include(w => w.ColourJustification)
                .Include(w => w.Status)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.WidgetId == widgetId);
            if (widget != null && widget.Name == expectedName)
                return widget;
            await Task.Delay(100);
        }
        return await _context.Widgets.FindAsync(widgetId);
    }

    private static async Task SelectDropdownOption(IPage page, string dropdownTestId, string itemText)
    {
        await page.GetByTestId(dropdownTestId).Locator("..").ClickAsync();
        await page.ClickAsync($"div.mud-popover div.mud-list-item:has-text('{itemText}')");
    }
}
