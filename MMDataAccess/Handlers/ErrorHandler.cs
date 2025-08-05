namespace MMDataAccess.Handlers;

public class ErrorHandler(ManufacturerManagerContext context) : IErrorHandler
{
    private readonly ManufacturerManagerContext _context = context;

    public async Task CreateErrorAsync(Exception ex, bool callSaveChanges)
    {
        var errorModel = new ErrorModel
        {
            ErrorDate = DateTime.Now,
            ErrorMessage = ex.Message,
            Exception = ex.GetType().ToString(),
            InnerException = ex.InnerException?.Message ?? "No inner exception",
            StackTrace = ex.StackTrace,
        };
        _context.Errors.Add(errorModel);
        if (callSaveChanges)
            await SaveChangesAsync();
    }

    public async Task DeleteErrorAsync(int errorId, bool callSaveChanges)
    {
        var errorToDelete = _context.Errors.SingleOrDefault(e => e.ErrorId == errorId);
        if (errorToDelete == null)
            return;
        _context.Errors.Remove(errorToDelete);
            if (callSaveChanges)
                await SaveChangesAsync();        
    }

    public async Task<ErrorModel> GetErrorAsync(int errorId) =>
        await _context.Errors
            .AsNoTracking()
        .SingleOrDefaultAsync(e => e.ErrorId == errorId)
        ?? throw new ArgumentNullException(nameof(errorId), "Error not found");

    public async Task<List<ErrorModel>> GetErrorsAsync() =>
        await _context.Errors
        .AsNoTracking()
        .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateErrorAsync(ErrorModel error, bool callSaveChanges)
    {
        var errorToUpdate = _context.Errors.SingleOrDefault(e => e.ErrorId == error.ErrorId);
        if (errorToUpdate == null)
            return;

        errorToUpdate.Resolved = error.Resolved;
        errorToUpdate.ResolvedDate = error.ResolvedDate;
        if (callSaveChanges)
            await SaveChangesAsync();
    }
}
