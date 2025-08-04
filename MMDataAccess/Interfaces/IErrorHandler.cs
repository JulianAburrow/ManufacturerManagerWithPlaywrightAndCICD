namespace MMDataAccess.Interfaces;

public interface IErrorHandler
{
    Task<ErrorModel> GetErrorAsync(int errorId);

    Task<List<ErrorModel>> GetErrorsAsync();

    Task CreateErrorAsync(Exception ex, bool callSaveChanges);

    Task UpdateErrorAsync(ErrorModel error, bool callSaveChanges);

    Task DeleteErrorAsync(int errorId, bool callSaveChanges);

    Task SaveChangesAsync();
}
