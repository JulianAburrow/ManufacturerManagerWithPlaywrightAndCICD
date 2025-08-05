namespace TestsPlaywright;

public class ErrorPageTests : BaseTestClass
{
    [Fact]
    public async Task ErrorHomePageLoads()
    {
        var page = await PlaywrightTestHelper.CreatePageAsync();

        // In this test only we will load the application home page and then click the
        // Admin menu item to expand the dropdownlist, then click 'Errors' to navigate
        // to the Errors page

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

        // This should have revealed the 'Errors' link in the Admin menu.
        var errorsLink = page.GetByRole(AriaRole.Link, new() { Name = "Errors" });
        if (await errorsLink.CountAsync() == 0)
        {
            errorsLink = page.GetByText("Errors", new() { Exact = false });
            Assert.True(await errorsLink.CountAsync() > 0, "Errors link not found in Admin menu.");
        }
        await errorsLink.ClickAsync();

        await page.WaitForFunctionAsync("document.title === 'Errors'");
    }

    [Fact]
    public async Task ViewButtonOnIndexPageNavigatesToViewErrorPage()
    {
        var newError = AddError();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/errors/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Errors'");

            var viewButton = page.GetByRole(AriaRole.Button, new() { Name = "View" });
            if (await viewButton.CountAsync() == 0)
            {
                viewButton = page.GetByText("View", new() { Exact = false });
                Assert.True(await viewButton.CountAsync() > 0, "View button not found on Errors index page.");
            }
            await viewButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'View Error'");
        }
        finally
        {
            RemoveError(newError.ErrorId);
        }
    }

    [Fact]
    public async Task EditButtonOnIndexPageNavigatesToEditErrorPage()
    {
        var newError = AddError();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/errors/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Errors'");

            var editButton = page.GetByRole(AriaRole.Button, new() { Name = "Edit" });
            if (await editButton.CountAsync() == 0)
            {
                editButton = page.GetByText("Edit", new() { Exact = false });
                Assert.True(await editButton.CountAsync() > 0, "Edit button not found on Errors index page.");
            }
            await editButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Edit Error'");
        }
        finally
        {
            RemoveError(newError.ErrorId);
        }
    }

    [Fact]
    public async Task DeleteButtonOnIndexPageNavigatesToDeleteErrorPage()
    {
        var newError = AddError();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/errors/index", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Errors'");
            
            var deleteButton = page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
            if (await deleteButton.CountAsync() == 0)
            {
                deleteButton = page.GetByText("Delete", new() { Exact = false });
                Assert.True(await deleteButton.CountAsync() > 0, "Delete button not found on Errors index page.");
            }
            await deleteButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Delete Error'");
        }
        finally
        {
            RemoveError(newError.ErrorId);
        }
    }

    [Fact]
    public async Task CanEditError()
    {
        var newError = AddError();

        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/error/edit/{newError.ErrorId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Error'");

            var errorResolvedCheckBox = page.Locator("[data-testid=\"error-resolved-checkbox\"]").First;
            await errorResolvedCheckBox.ClickAsync();

            var resolvedField = page.Locator("[data-testid=\"resolved-date-label\"]").First;
            string fieldText = await resolvedField.InnerTextAsync();
            Assert.False(string.IsNullOrWhiteSpace(fieldText));

            var submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
            if (await submitButton.CountAsync() == 0)
            {
                submitButton = page.GetByText("Submit", new() { Exact = false });
                Assert.True(await submitButton.CountAsync() > 0, "Submit button not found on Edit Error page.");
            }
            await submitButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Errors'");

            var updatedError = await WaitForErrorUpdate(newError.ErrorId, newError.ErrorMessage, 5_000);
            Assert.NotNull(updatedError);
            Assert.True(updatedError.Resolved);
            Assert.NotNull(updatedError.ResolvedDate);
        }
        finally
        {
            RemoveError(newError.ErrorId);
        }
    }

    [Fact]
    public async Task CanDeleteError()
    {
        var newError = AddError();
        var shouldReattemptDelete = false;
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/error/delete/{newError.ErrorId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Delete Error'");

            var deleteButton = page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
            if (await deleteButton.CountAsync() == 0)
            {
                deleteButton = page.GetByText("Delete", new() { Exact = false });
                Assert.True(await deleteButton.CountAsync() > 0, "Delete button not found on Delete Error page.");
            }
            await deleteButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Errors'");

            var deletedError = await _context.Errors
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ErrorId == newError.ErrorId);
            Assert.Null(deletedError);

            if (deletedError != null)
            {
                shouldReattemptDelete = true;
            }
        }
        finally
        {
            if (shouldReattemptDelete)
            {
                RemoveError(newError.ErrorId);
            }
        }
    }

    [Fact]
    public async Task CancelButtonOnEditPageNavigatesToIndex()
    {
        var newError = AddError();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/error/edit/{newError.ErrorId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Edit Error'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Edit Error page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Errors'");
        }
        finally
        {
            RemoveError(newError.ErrorId);
        }
    }

    [Fact]
    public async Task CancelButtonOnViewPageNavigatesToIndex()
    {
        var newError = AddError();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/error/view/{newError.ErrorId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'View Error'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on View Error page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Errors'");
        }
        finally
        {
            RemoveError(newError.ErrorId);
        }
    }

    [Fact]
    public async Task CancelButtonOnDeletePageNavigatesToIndex()
    {
        var newError = AddError();
        try
        {
            var page = await PlaywrightTestHelper.CreatePageAsync();

            await page.GotoAsync($"{GlobalValues.BaseUrl}/error/delete/{newError.ErrorId}", GlobalValues.GetPageOptions());
            await page.WaitForFunctionAsync("document.title === 'Delete Error'");

            var cancelButton = page.GetByRole(AriaRole.Button, new() { Name = "Cancel" });
            if (await cancelButton.CountAsync() == 0)
            {
                cancelButton = page.GetByText("Cancel", new() { Exact = false });
                Assert.True(await cancelButton.CountAsync() > 0, "Cancel button not found on Delete Error page.");
            }
            await cancelButton.First.ClickAsync();

            await page.WaitForFunctionAsync("document.title === 'Errors'");
        }
        finally
        {
            RemoveError(newError.ErrorId);
        }
    }

    private ErrorModel AddError()
    {
        var newError = new ErrorModel
        {
            ErrorDate = DateTime.Now,
            ErrorMessage = $"Error {Guid.NewGuid()}",
            Exception = "Test exception",
            InnerException = "Test inner exception",
            StackTrace = "Test stack trace",
            Resolved = false
        };
        _context.Errors.Add(newError);
        _context.SaveChanges();
        return newError;
    }

    private void RemoveError(int errorId)
    {
        var error = _context.Errors.Find(errorId);
        if (error != null)
        {
            _context.Errors.Remove(error);
            _context.SaveChanges();
        }
    }

    private async Task<ErrorModel?> WaitForErrorUpdate(int errorId, string expectedMessage, int timeoutMs)
    {
        var sw = new Stopwatch();
        while (sw.ElapsedMilliseconds < timeoutMs)
        {
            var error = await _context.Errors
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ErrorId == errorId);
            if (error != null && error.ErrorMessage == expectedMessage)
            {
                return error;
            }
            await Task.Delay(100);
        }
        return await _context.Errors.FindAsync(errorId);
    }
}
