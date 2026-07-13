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
    /// Gets the item data associated with the clicked link, or <paramref name="fallback"/>
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