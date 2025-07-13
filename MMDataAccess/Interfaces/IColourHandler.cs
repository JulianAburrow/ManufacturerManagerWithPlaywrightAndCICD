namespace MMDataAccess.Interfaces;

public interface IColourHandler
{
    Task<ColourModel> GetColourAsync(int colourId);

    Task<List<ColourModel>> GetColoursAsync();

    Task CreateColourAsync(ColourModel colour, bool callSaveChanges);

    Task UpdateColourAsync(ColourModel colour, bool callSaveChanges);

    Task DeleteColourAsync(int colourId, bool callSaveChanges);

    Task SaveChangesAsync();
}
