using BlazorRamp.DialogFramework.Common.Constants;
using BlazorRamp.DialogFramework.Common.Utilities;
using System.Collections;
using System.Linq.Expressions;

namespace BlazorRamp.DialogFramework.Framework;

/// <summary>
/// Defines the positioning and sizing options for a modal dialog window.
/// </summary>
/// <param name="horizontalAlignment">The horizontal alignment of the dialog window relative to the screen. Defaults to <see cref="HorizontalAlignment.Centre"/>.</param>
/// <param name="verticalAlignment">The vertical alignment of the dialog window relative to the screen. Defaults to <see cref="VerticalAlignment.Centre"/>.</param>
/// <param name="maxWidthPercent">The maximum width of the dialog window as a percentage of the viewport. Capped at 100. Defaults to 70.</param>
public class ModalDialogOptions (HorizontalAlignment horizontalAlignment = HorizontalAlignment.Centre, VerticalAlignment verticalAlignment = VerticalAlignment.Centre, int maxWidthPercent = 70)
{
    /// <summary
    /// >Gets the CSS flexbox alignment value for horizontal positioning.
    /// </summary>
    public string HorizonalPosition { get; } = horizontalAlignment switch { HorizontalAlignment.Left => "start", HorizontalAlignment.Centre => "center", HorizontalAlignment.Right => "end", _ => "center" };

    /// <summary>
    /// Gets the CSS flexbox alignment value for vertical positioning.
    /// </summary>
    public string VerticalPosition  { get; } = verticalAlignment switch { VerticalAlignment.Top => "start", VerticalAlignment.Centre => "center", VerticalAlignment.Bottom => "end", _ => "center" };

    /// <summary>
    /// Gets the maximum width of the dialog window as a CSS percentage string.
    /// </summary>
    public string MaxWidth          { get; } = maxWidthPercent > 100 ? "100%" : maxWidthPercent + "%";

}

/// <summary>
/// Base class for passing parameters to a modal dialog component.
/// Implement via <see cref="ModalDialogParameters{TDialog}"/> for type-safe parameter binding.
/// </summary>
public abstract class ModalDialogParameters : IEnumerable<KeyValuePair<string, object>>
{
    Dictionary<string, Object> _parameters = [];

    /// <summary>
    /// Adds a parameter by name and value. Duplicate keys are silently ignored.
    /// </summary>
    /// <param name="key">The parameter name.</param>
    /// <param name="value">The parameter value.</param>
    protected void Add(string key, object value)                     => _parameters.TryAdd(key, value);

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() =>  _parameters.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()                          =>  GetEnumerator();
}

/// <summary>
/// Provides type-safe parameter binding for a modal dialog component of type <typeparamref name="TDialog"/>.
/// </summary>
public class ModalDialogParameters<TDialog> : ModalDialogParameters
{
    /// <summary>
    /// Adds a parameter value bound to a specific component property.
    /// </summary>
    /// <typeparam name="TData">The type of the data being passed. Must match the component parameter type.</typeparam>
    /// <param name="dialogParam">A lambda expression identifying the component parameter property, e.g. <c>x => x.MyParam</c>.</param>
    /// <param name="data">The data value to pass to the component parameter.</param>
    /// <exception cref="ArgumentException">Thrown if the data type does not match the component parameter type.</exception>

    public void Add<TData>(Expression<Func<TDialog, object>> dialogParam, TData? data)
    {
        var (paramName, paramType) = GeneralUtilities.GetModalDialogParamType(dialogParam);

        if (paramType != typeof(TData)) throw new ArgumentException("The data type does not match the component parameter type");

        this.Add(paramName, data!);
    }
}
/// <summary>
/// Represents the result returned when a modal dialog is closed, including which button was clicked
/// and any data returned by the dialog.
/// </summary>
public class ModalDialogResult
{
    /// <summary
    /// >Gets which button was clicked to close the dialog.
    /// </summary>
    public DialogResultButtons ButtonClicked { get; }
    /// <summary>
    /// Gets the data returned by the dialog, if any. Cast using <see cref="DataType"/>.
    /// </summary>
    public object Data       { get; }

    /// <summary>
    /// Gets the <see cref="Type"/> of the returned data. Will be <see cref="NoReturnValue"/> if no data was returned.
    /// </summary>
    public Type   DataType   { get; }

    /// <summary>
    /// Gets the string indentifier the button that was clicked.
    /// </summary>
    public string ButtonText { get; }


    private ModalDialogResult (DialogResultButtons buttonClicked, Type dataType, object data, string buttonText)
    
        => (ButtonClicked, DataType, Data,ButtonText) = (buttonClicked, dataType, data, buttonText);

    /// <summary>
    /// Creates an OK result with data returned from the dialog.
    /// </summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="data">The data to return. Must not be null, empty, or whitespace.</param>
    /// <returns>A <see cref="ModalDialogResult"/> representing a successful confirmation with data.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="data"/> is null, empty, or whitespace.</exception>
    public static ModalDialogResult OK<T>(T data)

        => new(DialogResultButtons.Ok, typeof(T), GeneralUtilities.ThrowIfNullEmptyOrWhitespace(data)!, "OK");

    /// <summary>
    /// Creates an Other result with a custom button label and data returned from the dialog.
    /// </summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="buttonText">The label of the custom button that was clicked.</param>
    /// <param name="data">The data to return. Must not be null, empty, or whitespace.</param>
    /// <returns>A <see cref="ModalDialogResult"/> representing a custom button action with data.</returns>
    public static ModalDialogResult Other<T>(string buttonText, T data)

        => new(DialogResultButtons.Other, typeof(T), GeneralUtilities.ThrowIfNullEmptyOrWhitespace(data)!, buttonText);

    /// <summary>
    /// Creates an OK result with no data returned.
    /// </summary>
    /// <returns>
    /// A <see cref="ModalDialogResult"/> representing a successful confirmation.
    /// </returns>
    public static ModalDialogResult OK()                     => new(DialogResultButtons.Ok, typeof(NoReturnValue), NoReturnValue.Value, "OK");

    /// <summary>
    /// Creates a Cancel result with no data returned.
    /// </summary>
    /// <returns>A <see cref="ModalDialogResult"/>
    /// representing a cancelled dialog.
    /// </returns>
    public static ModalDialogResult Cancel()                 => new(DialogResultButtons.Cancel, typeof(NoReturnValue), NoReturnValue.Value, "Cancelled");

    /// <summary>
    /// Creates an Other result with a custom button label and no data returned.
    /// </summary>
    /// <param name="buttonText">The label of the custom button that was clicked.</param>
    /// <returns>A <see cref="ModalDialogResult"/> representing a custom button action.</returns>
    public static ModalDialogResult Other(string buttonText) => new(DialogResultButtons.Other, typeof(NoReturnValue), NoReturnValue.Value, buttonText);

}

/// <summary>
/// A Unit type used as the data type for <see cref="ModalDialogResult"/> instances that return no data.
/// Use <see cref="Value"/> to access the singleton instance.
/// </summary>
public record NoReturnValue
{
    /// <summary>
    /// Gets the singleton instance of <see cref="NoReturnValue"/>.
    /// </summary>
    public static readonly NoReturnValue Value = new();
    private NoReturnValue() { }

    /// <summary>
    /// Overrides the ToString method returing the empty set symbol.
    /// </summary>
    public override string ToString() => "Ø";
}

internal class EscapeTrigger
{
    private readonly List<WeakReference<Func<Task>>> _handlers = new();

    public bool HasHandlers => _handlers.Any(weakRef => weakRef.TryGetTarget(out _));

    public void Subscribe(Func<Task> handler)

        => _handlers.Add(new WeakReference<Func<Task>>(handler));

    public void Unsubscribe(Func<Task> handler)
    
        => _handlers.RemoveAll(weakRef => weakRef.TryGetTarget(out var target) && target == handler);
    

    public async Task RaiseEscapeKeyPressed()
    {
        for (int index = _handlers.Count - 1; index >= 0; index--)
        {
            if (_handlers[index].TryGetTarget(out var handler))
            {
                try
                {
                    await handler.Invoke();
                }
                catch { } //squash any errors bubbled up from client 
            }
            else
            {
                _handlers.RemoveAt(index); // Clean up "dead" references
            }
        }
    }
}