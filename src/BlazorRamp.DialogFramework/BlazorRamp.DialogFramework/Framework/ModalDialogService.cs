using BlazorRamp.DialogFramework.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorRamp.DialogFramework.Framework;

/// <summary>
/// Service for displaying and managing modal dialogs.
/// Register via <c>services.AddBlazorRampDialogService()</c> and inject into components.
/// </summary>
public class ModalDialogService(IJSRuntime jsRuntime)
{
    private IJSObjectReference? _jsModule;
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private readonly List<ModalDialogWindow> _dialogWindows = new();
    internal IReadOnlyList<ModalDialogWindow> DialogWindows => _dialogWindows.AsReadOnly();

    internal event Action OnChanged = delegate { };//add one to the invocation list to stop null and compiler null warning

    internal void SubscribeToUpdates(Action updateHandler) => OnChanged += updateHandler;
    internal void UnsubscribeFromUpdates(Action updateHandler) => OnChanged -= updateHandler;

    /// <summary>
    /// Shows a modal dialog using the specified component as content, with default options and no parameters.
    /// </summary>
    /// <typeparam name="TDialog">
    /// The Blazor component type to render as the dialog content.
    /// </typeparam>
    /// <returns>
    /// A <see cref="Task{ModalDialogResult}"/> that completes when the dialog is closed.
    /// </returns>
    public Task<ModalDialogResult> ShowDialog<TDialog>() => ShowDialog<TDialog>([], new());

    /// <summary>
    /// Shows a modal dialog using the specified component as content, with custom positioning and sizing options.
    /// </summary>
    /// <typeparam name="TDialog">
    /// The Blazor component type to render as the dialog content.
    /// </typeparam>
    /// <param name="dialogOptions">
    /// Options controlling the position and maximum width of the dialog.
    /// </param>
    /// <returns>
    /// A <see cref="Task{ModalDialogResult}"/> that completes when the dialog is closed.
    /// </returns>
    public Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogOptions dialogOptions) => ShowDialog<TDialog>([], dialogOptions);

    /// <summary>
    /// Shows a modal dialog using the specified component as content, with typed parameters passed to the component.
    /// </summary>
    /// <typeparam name="TDialog">The Blazor component type to render as the dialog content.</typeparam>
    /// <param name="dialogParameters">
    /// The parameters to pass to the dialog component.
    /// </param>
    /// <returns>
    /// A <see cref="Task{ModalDialogResult}"/>
    /// that completes when the dialog is closed.
    /// </returns>
    public Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogParameters<TDialog> dialogParameters) => ShowDialog<TDialog>(dialogParameters, new());

    /// <summary>
    /// Shows a modal dialog using the specified component as content, with typed parameters and custom options.
    /// </summary>
    /// <typeparam name="TDialog">
    /// The Blazor component type to render as the dialog content.
    /// </typeparam>
    /// <param name="dialogParameters">T
    /// he parameters to pass to the dialog component.
    /// </param>
    /// <param name="dialogOptions">
    /// Options controlling the position and maximum width of the dialog.
    /// </param>
    /// <returns>A <see cref="Task{ModalDialogResult}"/>
    /// that completes when the dialog is closed.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if <typeparamref name="TDialog"/> is not a Blazor component.
    /// </exception>
    public Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogParameters<TDialog> dialogParameters, ModalDialogOptions dialogOptions)
    {
        Type dialogType = typeof(TDialog);
        var windowID = Guid.NewGuid();

        if (false == typeof(ComponentBase).IsAssignableFrom(dialogType)) throw new ArgumentException($"{dialogType.FullName} must be a blazor component");

        dialogParameters ??= [];
        dialogOptions ??= new ModalDialogOptions();

        var dialogComponent = new ModalDialogWindow(windowID, dialogType, dialogParameters, dialogOptions);

        _dialogWindows.Add(dialogComponent);

        NotifyStateChanged();

        return dialogComponent.ShowDialogTask;

    }
    /// <summary>
    /// Closes the topmost open dialog and returns the specified result to the caller awaiting <see cref="ShowDialog{TDialog}()"/>.
    /// </summary>
    /// <param name="dialogResult">
    /// The result to return. Use <see cref="ModalDialogResult.Cancel()"/>, <see cref="ModalDialogResult.OK()"/>, or their overloads.
    /// </param>
    public async Task CloseDialog(ModalDialogResult dialogResult)
    {
        if (_dialogWindows.Count == 0) return;

        var dialogWindow = _dialogWindows.Last();
        _dialogWindows.Remove(dialogWindow);

        await (await GetJsModule(GlobalValues.JavaScript_File_Path)).InvokeVoidAsync(GlobalValues.JavaScript_Close_Modal_Func, dialogWindow.WindowID.ToString());

        NotifyStateChanged();
        dialogWindow.TaskSource.TrySetResult(dialogResult);

    }
    /// <summary>
    /// Returns the <c>aria-labelledby</c> ID value for the topmost open dialog.
    /// Use this in your dialog component to set the <c>id</c> attribute on the heading element
    /// that labels the dialog, ensuring screen readers announce it correctly when opened.
    /// </summary>
    /// <returns>The ID string to assign to the dialog's heading element.</returns>
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
    
    /// <summary>
    /// Registers an async handler to be called when the escape key is pressed on the topmost dialog.
    /// If no handler is registered, the dialog will close automatically with a <see cref="ModalDialogResult.Cancel()"/> result.
    /// Register a handler when you need to intercept escape, for example to warn users of unsaved changes.
    /// Always pair with <see cref="UnregisterEscapeHandler"/> in your component's <c>Dispose</c> method.
    /// </summary>
    /// <param name="handler">
    /// The async function to invoke when escape is pressed.
    /// </param>
    public void RegisterEscapeHandler(Func<Task> handler)
    {
        var topmostWindow = _dialogWindows.LastOrDefault();
        topmostWindow?.EscapeTrigger.Subscribe(handler);
    }

    /// <summary>
    /// Unregisters a previously registered escape key handler.
    /// Always call this when the component is disposed to prevent memory leaks.
    /// </summary>
    /// <param name="handler">
    /// The handler previously passed to <see cref="RegisterEscapeHandler"/>.
    /// </param>
    public void UnregisterEscapeHandler(Func<Task> handler)
    {
        foreach (var window in _dialogWindows)
        {
            window.EscapeTrigger.Unsubscribe(handler);
        }
    }
    private void NotifyStateChanged()
    {
        if (OnChanged.GetInvocationList().Length > 1)
        {
            if (OnChanged.GetInvocationList()[1] is Action action) action.Invoke();
        }
    }
}
