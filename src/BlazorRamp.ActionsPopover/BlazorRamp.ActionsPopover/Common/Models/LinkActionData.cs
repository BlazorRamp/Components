using BlazorRamp.ActionsPopover.Common.Constants;

namespace BlazorRamp.ActionsPopover.Common.Models;

public class LinkActionData<TData>(string linkText, TData? payload, TargetType targetType = TargetType.Self, string path = "/")
{
    private readonly TData? _payload = payload;
    public string?    Path       { get; } = String.IsNullOrWhiteSpace(path) ? String.Empty : path.Trim();
    public string     LinkText   { get; } = String.IsNullOrWhiteSpace(linkText) ? String.Empty : linkText.Trim();
    public TargetType TargetType { get; } = targetType;

    public bool TryGetData(out TData? itemData)
    {
        itemData = _payload ?? default(TData);
        return _payload is not null;
    }
}