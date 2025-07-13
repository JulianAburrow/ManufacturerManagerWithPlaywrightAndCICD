namespace TestsPlaywright;

public static class GlobalValues
{
    public const string BaseUrl = "https://127.0.0.1:5245";
    public const bool IsHeadless = true;
    public static PageGotoOptions GetPageOptions() =>
        new()
        {
            WaitUntil = WaitUntilState.NetworkIdle
        };
}
