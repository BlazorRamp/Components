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
    /// Attempts to retrieve the item data associated with the clicked button.
    /// </summary>
    /// <param name="itemData">The associated data, or <see langword="default"/> if none was supplied.</param>
    /// <returns><see langword="true"/> if data was supplied; otherwise <see langword="false"/>.</returns>
    public bool TryGetData(out TData? itemData)
    {
        itemData = _payload ?? default(TData);
        return _payload is not null;
    }
}
