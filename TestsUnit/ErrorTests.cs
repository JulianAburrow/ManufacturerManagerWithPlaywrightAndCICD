namespace TestsUnit;

public class ErrorTests
{
    private readonly ManufacturerManagerContext _manufacturerManagerContext;
    private readonly IErrorHandler _errorHandler;

    public ErrorTests()
    {
        _manufacturerManagerContext = TestsUnitHelper.GetContextWithOptions();
        _errorHandler = new ErrorHandler(_manufacturerManagerContext);
    }

    private readonly ErrorModel Error1 = new()
    {
        ErrorDate = DateTime.Now,
        ErrorMessage = "Error1",
        StackTrace = "This is the stack trace for Error1",
        Resolved = false,
        ResolvedDate = null,
    };

    private readonly ErrorModel Error2 = new()
    {
        ErrorDate = DateTime.Now,
        ErrorMessage = "Error2",
        StackTrace = "This is the stack trace for Error2",
        Resolved = false,
        ResolvedDate = null,
    };

    [Fact]
    public async Task CreateErrorCreatesError()
    {
        var initialCount = _manufacturerManagerContext.Errors.Count();

        await _errorHandler.CreateErrorAsync(Error1, false);
        await _errorHandler.CreateErrorAsync(Error2, true);

        _manufacturerManagerContext.Errors.Count().Should().Be(initialCount + 2);
    }

    [Fact]
    public async Task GetErrorGetsError()
    {
        _manufacturerManagerContext.Errors.Add(Error1);
        _manufacturerManagerContext.SaveChanges();

        var returnedError = await _errorHandler.GetErrorAsync(Error1.ErrorId);
        returnedError.Should().NotBeNull();
        returnedError.ErrorMessage.Should().Be(Error1.ErrorMessage);
    }

    [Fact]
    public async Task GetErrorsGetsErrors()
    {
        var initialCount = _manufacturerManagerContext.Errors.Count();

        _manufacturerManagerContext.Errors.Add(Error1);
        _manufacturerManagerContext.Errors.Add(Error2);
        _manufacturerManagerContext.SaveChanges();

        var errors = await _errorHandler.GetErrorsAsync();

        errors.Count.Should().Be(initialCount + 2);
    }

    [Fact]
    public async Task UpdateErrorUpdatesError()
    {
        var newErrorMessage = "UpdatedError1";
        var resolvedDate = DateTime.UtcNow;

        _manufacturerManagerContext.Errors.Add(Error1);
        _manufacturerManagerContext.SaveChanges();

        var errorToUpdate = _manufacturerManagerContext.Errors.First(e => e.ErrorId == Error1.ErrorId);
        errorToUpdate.ErrorMessage = newErrorMessage;
        errorToUpdate.Resolved = true;
        errorToUpdate.ResolvedDate = resolvedDate;
        await _errorHandler.UpdateErrorAsync(errorToUpdate, true);

        var updatedError = _manufacturerManagerContext.Errors.First(e => e.ErrorId == Error1.ErrorId);
        updatedError.Should().NotBeNull();
        updatedError.ErrorMessage.Should().Be(newErrorMessage);
        updatedError.Resolved.Should().BeTrue();
        updatedError.ResolvedDate.Should().Be(resolvedDate);
    }

    [Fact]
    public async Task DeleteErrorDeletesError()
    {
        int errorId;

        _manufacturerManagerContext.Errors.Add(Error2);
        _manufacturerManagerContext.SaveChanges();
        errorId = Error2.ErrorId;

        await _errorHandler.DeleteErrorAsync(errorId, true);

        var deletedError = _manufacturerManagerContext.Errors.FirstOrDefault(e => e.ErrorId == errorId);

        deletedError.Should().BeNull();
    }
}
