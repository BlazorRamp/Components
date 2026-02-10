using Microsoft.AspNetCore.Components;

namespace BlazorRamp.DialogFramework.Framework;

public class ModalDialogWindow(Guid windowID, Type dialogType, ModalDialogParameters dialogParameters, ModalDialogOptions dialogOptions)
{
    internal TaskCompletionSource<ModalDialogResult> TaskSource { get; } = new TaskCompletionSource<ModalDialogResult>();
    public Task<ModalDialogResult> ShowDialogTask => TaskSource.Task;

    public EscapeTrigger         EscapeTrigger      { get; } = new();
    public Type                  DialogType         { get; } = dialogType;
    public ModalDialogParameters DialogParameters   { get; } = dialogParameters;
    public ModalDialogOptions    DialogOptions      { get; } = dialogOptions;
    public Guid                  WindowID           { get; } = windowID;
    public RenderFragment        DialogContents

        => builder =>
        {
            int sequence = 0;

            builder.OpenComponent(sequence++, DialogType);

            if (DialogParameters.Any()) builder.AddMultipleAttributes(sequence++, DialogParameters);

            builder.CloseComponent();
        };

}
