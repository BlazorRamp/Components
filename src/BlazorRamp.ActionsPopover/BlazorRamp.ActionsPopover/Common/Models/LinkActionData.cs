using BlazorRamp.ActionsPopover.Common.Constants;
using BlazorRamp.ActionsPopover.Components;

namespace BlazorRamp.ActionsPopover.Common.Models;

/// <summary>
/// Represents the data supplied to an <see cref="ActionPopoverLink{TData}.OnClick"/> callback.
/// </summary>
public class LinkActionData<TData>(string linkText, TData? payload, TargetType targetType = TargetType.Self, string path = "/")
{
    private readonly TData? _payload = payload;

    /// <summary>
    /// Gets the trimmed text of the link that was clicked.
    /// </summary>
    public string LinkText { get; } = String.IsNullOrWhiteSpace(linkText) ? String.Empty : linkText.Trim();

    /// <summary>
    /// Gets the trimmed <c>href</c> path for the clicked link.
    /// </summary>
    public string?    Path       { get; } = String.IsNullOrWhiteSpace(path) ? String.Empty : path.Trim();

    /// <summary>
    /// Gets the browsing context the link was opened in.
    /// </summary>
    public TargetType TargetType { get; } = targetType;

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