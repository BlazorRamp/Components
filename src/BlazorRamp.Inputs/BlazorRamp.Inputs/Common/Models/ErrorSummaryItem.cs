namespace BlazorRamp.Inputs.Common.Models;

internal record ErrorSummaryItem(List<string> ValidationErrors, string DisplayName, string ControlID);

