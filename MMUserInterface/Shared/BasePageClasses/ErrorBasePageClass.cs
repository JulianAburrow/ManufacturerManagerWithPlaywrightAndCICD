namespace MMUserInterface.Shared.BasePageClasses;

public class ErrorBasePageClass : BasePageClass
{
    [Parameter] public int ErrorId { get; set; }

    protected ErrorModel ErrorModel { get; set; } = new();

    protected ErrorDisplayModel ErrorDisplayModel { get; set; } = new();

    protected string Error = "Error";

    protected string ErrorPlural = "Errors";

    protected BreadcrumbItem GetErrorHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new("Errors", "/errors/index", isDisabled);
    }

    protected void CopyModelToDisplayModel()
    {
        ErrorDisplayModel.ErrorDate = ErrorModel.ErrorDate;
        ErrorDisplayModel.ErrorMessage = ErrorModel.ErrorMessage;
        ErrorDisplayModel.Exception = ErrorModel.Exception;
        ErrorDisplayModel.InnerException = ErrorModel.InnerException;
        ErrorDisplayModel.StackTrace = ErrorModel.StackTrace;
        ErrorDisplayModel.Resolved = ErrorModel.Resolved;
        ErrorDisplayModel.ResolvedDate = ErrorModel.ResolvedDate;
    }

    protected void CopyDisplayModelToModel()
    {
        ErrorModel.ErrorDate = ErrorDisplayModel.ErrorDate;
        ErrorModel.ErrorMessage = ErrorDisplayModel.ErrorMessage;
        ErrorModel.Exception = ErrorDisplayModel.Exception;
        ErrorModel.InnerException = ErrorDisplayModel.InnerException;
        ErrorModel.StackTrace = ErrorDisplayModel.StackTrace;
        ErrorModel.Resolved = ErrorDisplayModel.Resolved;
        ErrorModel.ResolvedDate = ErrorDisplayModel.Resolved
            ? DateTime.Now
            : ErrorModel.ResolvedDate = null;
    }
}
