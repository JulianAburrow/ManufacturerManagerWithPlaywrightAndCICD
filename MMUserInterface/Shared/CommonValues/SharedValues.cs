namespace MMUserInterface.Shared.CommonValues;

public static class SharedValues
{
    public const int PleaseSelectValue = -1;
    public const string PleaseSelectText = "Please select";
    public const int NoneValue = -1;
    public const string NoneText = "None";

    public enum ObjectTypes
    {
        PleaseSelect = -1,
        Manufacturer = 1,
        Widget = 2
    }

    public enum Statuses
    {
        Any = 0,
        Active = 1,
        Inactive = 2,
    }
}
