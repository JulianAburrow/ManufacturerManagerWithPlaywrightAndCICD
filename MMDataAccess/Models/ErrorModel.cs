namespace MMDataAccess.Models;

public class ErrorModel
{
    public int ErrorId { get; set; }

    public DateTime ErrorDate { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string Exception { get; set; } = null!;

    public string InnerException { get; set; } = null!;

    public string? StackTrace { get; set; }

    public bool Resolved { get; set; }

    public DateTime? ResolvedDate { get; set; }
}
