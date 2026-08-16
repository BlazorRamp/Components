namespace BlazorRamp.DocSite.Common.Utilities;

public static class GeneralHelper
{
    public static string? CheckSetColourVariable(string? colourValue, string variableName)
    {
        var value = String.IsNullOrWhiteSpace(colourValue) ? null : colourValue.Trim().TrimEnd(';');
        return value is null ? null : $"{variableName}:{value};";
    }
}
