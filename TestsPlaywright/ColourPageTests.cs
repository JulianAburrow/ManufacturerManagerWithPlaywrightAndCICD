namespace TestsPlaywright;

public class ColourPageTests : BaseTestClass
{
    [Fact]
    public async Task ColourHomePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        // In this test only we will load the application home page and then click the
        // Admin menu item to expand the dropdownlist, then click 'Colours' to navigate
        // to the Colours page (which will always contain something as there are colours
        // created when the database is built).

        await page.GotoAsync($"{GlobalValues.BaseUrl}/", GlobalValues.GetPageOptions());
        var homeTitle = await page.TitleAsync();
        Assert.Equal("Manufacturer Manager", homeTitle);

        var adminLink = page.GetByRole(AriaRole.Link, new() { Name = "Admin" });
        if (await adminLink.CountAsync() == 0)
        {
            adminLink = page.GetByText("Admin", new() { Exact = false });
            Assert.True(await adminLink.CountAsync() > 0, "Admin link not found on home page.");
        }
        await adminLink.ClickAsync();

        // This should have revealed the 'Colours' link in the Admin menu.
        var coloursLink = page.GetByRole(AriaRole.Link, new() { Name = "Colours" });
        if (await coloursLink.CountAsync() == 0)
        {
            coloursLink = page.GetByText("Colours", new() { Exact = false });
            Assert.True(await coloursLink.CountAsync() > 0, "Colours link not found in Admin menu.");
        }
        await coloursLink.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Colours'");
    }

    [Fact]
    public async Task CreateButtonOnIndexPageNavigatesToCreateColourPage()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/colours/index", GlobalValues.GetPageOptions());
        var coloursTitle = await page.TitleAsync();
        Assert.Equal("Colours", coloursTitle);

        var createButton = page.GetByRole(AriaRole.Button, new() { Name = "Create" });
        if (await createButton.CountAsync() == 0)
        {
            createButton = page.GetByText("Create", new() { Exact = false });
            Assert.True(await createButton.CountAsync() > 0, "Create button not found on Colours index page.");
        }
        await createButton.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Create Colour'");
    }

    [Fact]
    public async Task ViewButtonOnIndexPageNavigatesToViewColourPage()
    {
        var colourId = AddColour();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colours/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Colours'");

            var viewButton = page.GetByRole(AriaRole.Button, new() { Name = "View" });
            if (await viewButton.CountAsync() == 0)
            {
                viewButton = page.GetByText("View", new() { Exact = false });
                Assert.True(await viewButton.CountAsync() > 0, "View button not found on Colours index page.");
            }
            await viewButton.First.ClickAsync();
            await page.WaitForFunctionAsync("document.title === 'View Colour'");
        }
        finally
        {
            RemoveColour(colourId);
        }
    }

    [Fact]
    public async Task EditButtonOnIndexPageNavigatesToEditColourPage()
    {
        var colourId = AddColour();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colours/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Colours'");

            var editButton = page.GetByRole(AriaRole.Button, new() { Name = "Edit" });
            if (await editButton.CountAsync() == 0)
            {
                editButton = page.GetByText("Edit", new() { Exact = false });
                Assert.True(await editButton.CountAsync() > 0, "Edit button not found on Colours index page.");
            }
            await editButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Edit Colour'");
        }
        finally
        {
            RemoveColour(colourId);
        }
    }

    [Fact]
    public async Task DeleteButtonOnIndexPageNavigatesToDeleteColourPage()
    {
        var colourId = AddColour();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colours/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Colours'");

            var deleteButton = page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
            if (await deleteButton.CountAsync() == 0)
            {
                deleteButton = page.GetByText("Delete", new() { Exact = false });
                Assert.True(await deleteButton.CountAsync() > 0, "Delete button not found on Colours index page.");
            }
            await deleteButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Delete Colour'");
        }
        finally
        {
            RemoveColour(colourId);
        }
    }

    [Fact]
    public async Task CanCreateColour()
    {
        var colour = new ColourModel();

        try
        {
            var initialCount = _context.Colours.Count();

            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/colour/create", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Create Colour'");

            var colourName = "Test Colour 123456";
            await page.GetByLabel("Name").FillAsync(colourName);

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Create Colour page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colours'");

            Assert.Equal(initialCount + 1, _context.Colours.Count());
            
            colour = await _context.Colours.FirstOrDefaultAsync(c => c.Name == colourName);
            Assert.NotNull(colour);
            Assert.Equal(colourName, colour.Name);
        }
        finally
        {
            if (colour != null)
            {
                RemoveColour(colour.ColourId);
            }
        }
    }

    [Fact]
    public async Task CanEditColour()
    {
        var colourId = AddColour();

        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/colour/edit/{colourId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Colour'");

            var updatedColourName = "Updated Colour 654321";
            await page.GetByLabel("Name").FillAsync(updatedColourName);

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Edit Colour page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colours'");

            var updatedColour = await WaitForColourUpdate(colourId, updatedColourName, 5_000);
            Assert.NotNull(updatedColour);
            Assert.Equal(updatedColourName, updatedColour.Name);
        }
        finally
        {
            RemoveColour(colourId);
        }    
    }

    [Fact]
    public async Task CanDeleteColour()
    {
        var colourId = AddColour();
        var shouldReattemptDelete = false;
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colour/delete/{colourId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Delete Colour'");
            
            var deleteButton = page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
            if (await deleteButton.CountAsync() == 0)
            {
                deleteButton = page.GetByText("Delete", new() { Exact = false });
                Assert.True(await deleteButton.CountAsync() > 0, "Delete button not found on Delete Colour page.");
            }
            await deleteButton.First.ClickAsync();
            
            await page.WaitForFunctionAsync("document.title === 'Colours'");
            
            var deletedColour = await _context.Colours
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ColourId == colourId);
            Assert.Null(deletedColour);
            
            if (deletedColour != null)
            {
                shouldReattemptDelete = true;
            }
        }
        finally
        {
            if (shouldReattemptDelete)
            {
                RemoveColour(colourId);
            }            
        }
    }

    [Fact]
    public async Task CancelButtonOnCreatePageNavigatesToIndex()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/colour/create", GlobalValues.GetPageOptions());
        await page.WaitForFunctionAsync("document.title === 'Create Colour'");
        
        var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
        if (await cancelButton.CountAsync() == 0)
        {
            cancelButton = page.GetByText("Cancel", new() { Exact = false });
            Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Create Colour page.");
        }
        await cancelButton.First.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Colours'");
    }

    [Fact]
    public async Task CancelButtonOnEditPageNavigatesToIndex()
    {
        var colourId = AddColour();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colour/edit/{colourId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Colour'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Edit Colour page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colours'");
        }
        finally
        {
            RemoveColour(colourId);
        }
    }

    [Fact]
    public async Task CancelButtonOnViewPageNavigatesToIndex()
    {
        var colourId = AddColour();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colour/view/{colourId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'View Colour'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on View Colour page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colours'");
        }
        finally
        {
            RemoveColour(colourId);
        }
    }

    [Fact]
    public async Task CancelButtonOnDeletePageNavigatesToIndex()
    {
        var colourId = AddColour();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colour/delete/{colourId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Delete Colour'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Delete Colour page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colours'");
        }
        finally
        {
            RemoveColour(colourId);
        }
    }

    private int AddColour()
    {
        var newColour = new ColourModel
        {
            Name = "Test Colour 123546",
        };
        _context.Colours.Add(newColour);
        _context.SaveChanges();
        return newColour.ColourId;
    }

    private void RemoveColour(int colourId)
    {
        var colour = _context.Colours.Find(colourId);
        if (colour != null)
        {
            _context.Colours.Remove(colour);
            _context.SaveChanges();
        }
    }

    private async Task<ColourModel?> WaitForColourUpdate(int colourId, string expectedName, int timeoutMs)
    {
        var sw = Stopwatch.StartNew();  
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            var colour = await _context.Colours
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ColourId == colourId);
            if (colour != null && colour.Name == expectedName)
            {
                return colour;
            }
            await Task.Delay(100);
        }
        return await _context.Colours.FindAsync(colourId);
    }
}
