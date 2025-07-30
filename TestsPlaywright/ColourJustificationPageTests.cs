namespace TestsPlaywright;

public class ColourJustificationPageTests : BaseTestClass
{
    [Fact]
    public async Task ColourJustificationHomePageLoads()
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

        // This should have revealed the 'Colour Justifications' link in the admin menu.
        var colourJustificationsLink = page.GetByRole(AriaRole.Link, new() { Name = "Colour Justifications" });
        if (await colourJustificationsLink.CountAsync() == 0)
        {
            colourJustificationsLink = page.GetByText("Colour Justifications", new() { Exact = false });
            Assert.True(await colourJustificationsLink.CountAsync() > 0, "Colour Justifications link not found in admin menu.");
        }
        await colourJustificationsLink.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");
    }

    [Fact]
    public async Task CreateButtonOnIndexPageNavigatesToCreateColourJustificationPage()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustifications/index", GlobalValues.GetPageOptions());
        await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");
        Assert.Equal("Colour Justifications", await page.TitleAsync());

        var createButton = page.GetByRole(AriaRole.Button, new() { Name = "Create" });
        if (await createButton.CountAsync() == 0)
        {
            createButton = page.GetByText("Create", new() { Exact = false });
            Assert.True(await createButton.CountAsync() > 0, "Create button not found on Colour Justifications index page.");
        }
        await createButton.First.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Create Colour Justification'");
    }

    [Fact]
    public async Task ViewButtonOnIndexPageNavigatesToViewColourJustificationPage()
    {
        var colourJustificationId = AddColourJustification();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustifications/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");

            var viewButton = page.GetByRole(AriaRole.Button, new() { Name = "View" });
            if (await viewButton.CountAsync() == 0)
            {
                viewButton = page.GetByText("View", new() { Exact = false });
                Assert.True(await viewButton.CountAsync() > 0, "View button not found on Colour Justifications index page.");
            }
            await viewButton.First.ClickAsync();
            await page.WaitForFunctionAsync("document.title === 'View Colour Justification'");
        }
        finally
        {
            RemoveColourJustification(colourJustificationId);
        }
    }

    [Fact]
    public async Task EditButtonOnIndexPageNavigatesToEditColourJustificationPage()
    {
        var colourJustificationId = AddColourJustification();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustifications/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");

            var editButton = page.GetByRole(AriaRole.Button, new() { Name = "Edit" });
            if (await editButton.CountAsync() == 0)
            {
                editButton = page.GetByText("Edit", new() { Exact = false });
                Assert.True(await editButton.CountAsync() > 0, "Edit button not found on Colour Justifications index page.");
            }
            await editButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Edit Colour Justification'");
        }
        finally
        {
            RemoveColourJustification(colourJustificationId);
        }
    }

    [Fact]
    public async Task DeleteButtonOnIndexPageNavigatesToDeleteColourJustificationPage()
    {
        var colourJustificationId = AddColourJustification();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustifications/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");

            var deleteButton = page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
            if (await deleteButton.CountAsync() == 0)
            {
                deleteButton = page.GetByText("Delete", new() { Exact = false });
                Assert.True(await deleteButton.CountAsync() > 0, "Delete button not found on Colour Justifications index page.");
            }
            await deleteButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Delete Colour Justification'");
        }
        finally
        {
            RemoveColourJustification(colourJustificationId);
        }
    }

    [Fact]
    public async Task CanCreateColourJustification()
    {
        var colourJustification = new ColourJustificationModel();
        try
        {
            var initialCount = _context.ColourJustifications.Count();

            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustification/create", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Create Colour Justification'");

            var colourJustificationJustification = $"Colour Justification {Guid.NewGuid()}";
            await page.GetByLabel("Justification").FillAsync(colourJustificationJustification);

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Create Colour Justification page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");

            Assert.Equal(initialCount + 1, _context.ColourJustifications.Count());

            colourJustification = await _context.ColourJustifications.FirstOrDefaultAsync(c => c.Justification == colourJustificationJustification);
            Assert.NotNull(colourJustification);
            Assert.Equal(colourJustificationJustification, colourJustification.Justification);
        }
        finally
        {
            if (colourJustification != null)
            {
                RemoveColourJustification(colourJustification.ColourJustificationId);
            }
        }
    }

    [Fact]
    public async Task CanEditColourJustification()
    {
        var colourJustificationId = AddColourJustification();

        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();
            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustification/edit/{colourJustificationId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Colour Justification'");

            var updatedColourJustificationJustification = $"Updated Colour Justification {Guid.NewGuid()}";
            await page.GetByLabel("Justification").FillAsync(updatedColourJustificationJustification);

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Edit Colour Justification page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");

            var updatedColourJustification = await WaitForColourJustificationUpdate(colourJustificationId, updatedColourJustificationJustification, 2000);
            Assert.NotNull(updatedColourJustification);
            Assert.Equal(updatedColourJustificationJustification, updatedColourJustification.Justification);
        }
        finally
        {
            RemoveColourJustification(colourJustificationId);
        }
    }

    [Fact]
    public async Task CanDeleteColourJustification()
    {
        var colourJustificationId = AddColourJustification();
        var shouldReattemptDelete = false;
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustification/delete/{colourJustificationId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Delete Colour Justification'");

            var deleteButton = page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
            if (await deleteButton.CountAsync() == 0)
            {
                deleteButton = page.GetByText("Delete", new() { Exact = false });
                Assert.True(await deleteButton.CountAsync() > 0, "Delete button not found on Delete Colour Justification page.");
            }
            await deleteButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");
            
            var deletedColourJustification = await _context.ColourJustifications
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ColourJustificationId == colourJustificationId);
            Assert.Null(deletedColourJustification);

            if (deletedColourJustification != null)
            {
                shouldReattemptDelete = true;
            }
        }
        finally
        {
            if (shouldReattemptDelete)
            {
                RemoveColourJustification(colourJustificationId);
            }            
        }
    }

    [Fact]
    public async Task CancelButtonOnCreatePageNavigatesToIndex()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustification/create", GlobalValues.GetPageOptions());
        await page.WaitForFunctionAsync("document.title === 'Create Colour Justification'");

        var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
        if (await cancelButton.CountAsync() == 0)
        {
            cancelButton = page.GetByText("Cancel", new() { Exact = false });
            Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Create Colour Justification page.");
        }
        await cancelButton.First.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");
    }

    [Fact]
    public async Task CancelButtonOnEditPageNavigatesToIndex()
    {
        var colourJustificationId = AddColourJustification();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustification/edit/{colourJustificationId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Colour Justification'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Edit Colour Justification page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");
        }
        finally
        {
            RemoveColourJustification(colourJustificationId);
        }
    }

    [Fact]
    public async Task CancelButtonOnViewPageNavigatesToIndex()
    {
        var colourJustificationId = AddColourJustification();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustification/view/{colourJustificationId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'View Colour Justification'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on View Colour Justification page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");
        }
        finally
        {
            RemoveColourJustification(colourJustificationId);
        }
    }

    [Fact]
    public async Task CancelButtonOnDeletePageNavigatesToIndex()
    {
        var colourJustificationId = AddColourJustification();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/colourjustification/delete/{colourJustificationId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Delete Colour Justification'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Delete Colour Justification page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Colour Justifications'");
        }
        finally
        {
            RemoveColourJustification(colourJustificationId);
        }
    }

    private int AddColourJustification()
    {
        var newColourJustification = new ColourJustificationModel
        {
            Justification = $"Colour Justification {Guid.NewGuid()}"
        };
        _context.ColourJustifications.Add(newColourJustification);
        _context.SaveChanges();
        return newColourJustification.ColourJustificationId;
    }

    private void RemoveColourJustification(int colourJustificationId)
    {
        var colourJustification = _context.ColourJustifications.Find(colourJustificationId);
        if (colourJustification != null)
        {
            _context.ColourJustifications.Remove(colourJustification);
            _context.SaveChanges();
        }
    }

    private async Task<ColourJustificationModel?> WaitForColourJustificationUpdate(int colourJustificationId, string expectedJustification, int timeoutMS)
    {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < timeoutMS)
        {
            var colourJustification = await _context.ColourJustifications
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ColourJustificationId ==colourJustificationId);
            if (colourJustification != null && colourJustification.Justification == expectedJustification)
            {
                return colourJustification;
            }
            await Task.Delay(100);
        }
        return await _context.ColourJustifications.FindAsync(colourJustificationId);
    }
}
