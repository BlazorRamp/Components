using BlazorRamp.ActionsPopover.Components;

namespace BlazorRamp.ActionsPopover.Common.Models;


/// <summary>
/// Represents the data supplied to an <see cref="ActionPopoverButton{TData}.OnClick"/> callback.
/// </summary>
public class ButtonActionData<TData>(string buttonText, TData? payload)
{
    private readonly TData? _payload = payload;

    /// <summary>
    /// Gets the trimmed text of the button that was clicked.
    /// </summary>
    public string ButtonText { get; } = String.IsNullOrWhiteSpace(buttonText) ? String.Empty : buttonText.Trim();

    /// <summary>
    /// Gets the item data associated with the clicked button, or <paramref name="fallback"/>
    /// if no data was supplied.
    /// </summary>
    /// <remarks>
    /// For non-nullable value-typed <typeparamref name="TData"/> (e.g. <c>int</c>, <c>bool</c>),
    /// a default value (e.g. <c>0</c>) is indistinguishable from "no value was supplied" and is
    /// always treated as a real value — <paramref name="fallback"/> will never be returned in that
    /// case. Use a nullable value type (e.g. <c>int?</c>) as <typeparamref name="TData"/> if this
    /// distinction matters to you.
    /// </remarks>
    /// <param name="fallback">The value to return if no data was supplied.</param>
    public TData? GetValueOr(TData? fallback) => _payload ?? fallback;
}
