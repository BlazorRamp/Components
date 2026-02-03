using BlazorRamp.DialogFramework.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.DialogFramework.Framework;

public class ModalDialogService(IJSRuntime jsRuntime)
{
    private IJSObjectReference? _jsModule;
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private readonly List<ModalDialogWindow> _dialogWindows = new List<ModalDialogWindow>();
    public IReadOnlyList<ModalDialogWindow> DialogWindows => _dialogWindows.AsReadOnly();

    internal event Action OnChanged = delegate { };//add one to the invocation list to stop null and compiler null warning

    public Task<ModalDialogResult> ShowDialog<TDialog>() => ShowDialog<TDialog>([], new());
    public Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogOptions dialogOptions) => ShowDialog<TDialog>([], dialogOptions);
    public Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogParameters<TDialog> dialogParameters) => ShowDialog<TDialog>(dialogParameters, new());
    public Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogParameters<TDialog> dialogParameters, ModalDialogOptions dialogOptions)
    {
        Type dialogType = typeof(TDialog);
        var windowID = Guid.NewGuid();

        if (false == typeof(ComponentBase).IsAssignableFrom(dialogType)) throw new ArgumentException($"{dialogType.FullName} must be a blazor component");

        dialogParameters ??= [];
        dialogOptions ??= new ModalDialogOptions();

        var dialogComponent = new ModalDialogWindow(windowID, dialogType, dialogParameters, dialogOptions);

        if (OnChanged.GetInvocationList().Length > 1)
        {
            _dialogWindows.Add(dialogComponent);
            /*
                * Just invoke one instance, not index zero as that is for our empty delegate. Initially I did use Add Remove event accessors but switched to this instead.
            */
            if (OnChanged.GetInvocationList()[1] is Action action) action.Invoke();
        }

        return dialogComponent.ShowDialogTask;

    }

    public async Task CloseDialog(ModalDialogResult dialogResult)
    {
        if (_dialogWindows.Count == 0) return;

        var dialogWindow = _dialogWindows.Last();

        _dialogWindows.Remove(dialogWindow);

        await (await GetJsModule(GlobalValues.JavaScript_File_Path)).InvokeVoidAsync(GlobalValues.JavaScript_Close_Modal_Func, dialogWindow.WindowID.ToString());

        if (OnChanged.GetInvocationList().Length > 1)
        {
            if (OnChanged.GetInvocationList()[1] is Action action) action.Invoke();
        }

        dialogWindow.TaskSource.SetResult(dialogResult);

    }
    public string GetAriaLabelledByID()

        => _dialogWindows.Count == 0 ? "dialog-" : "dialog-" + _dialogWindows.Last().WindowID.ToString();
    internal async Task JsOpenModalDialog()
    {
        if (_dialogWindows.Count == 0) return;

        var dialogWindow = _dialogWindows.Last();

        await (await GetJsModule(GlobalValues.JavaScript_File_Path)).InvokeVoidAsync(GlobalValues.JavaScript_Open_Modal_Func, dialogWindow.WindowID.ToString());
    }

    private async Task<IJSObjectReference> GetJsModule(string modulePath)

        => _jsModule ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", modulePath);

}
