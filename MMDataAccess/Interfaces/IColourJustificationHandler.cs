namespace MMDataAccess.Interfaces;

public interface IColourJustificationHandler
{
    Task<ColourJustificationModel> GetColourJustificationAsync(int colourJustificationId);

    Task<List<ColourJustificationModel>> GetColourJustificationsAsync();

    Task CreateColourJustificationAsync(ColourJustificationModel colourJustification, bool callSaveChanges);

    Task UpdateColourJustificationAsync(ColourJustificationModel colourJustification, bool callSaveChanges);

    Task DeleteColourJustificationAsync(int colourJustificationId, bool callSaveChanges);

    Task SaveChangesAsync();
}
