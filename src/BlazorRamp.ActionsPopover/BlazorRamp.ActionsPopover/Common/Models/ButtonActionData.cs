namespace BlazorRamp.ActionsPopover.Common.Models;

public class ButtonActionData<TData>(string buttonText, TData? payload)
{
    private readonly TData? _payload = payload;

    public string ButtonText { get; } = String.IsNullOrWhiteSpace(buttonText) ? String.Empty : buttonText.Trim();

    public bool TryGetData(out TData? itemData)
    {
        itemData = _payload ?? default(TData);
        return _payload is not null;
    }
}
