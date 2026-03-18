using BlazorRamp.Accordion.Common.Constants;
using BlazorRamp.Accordion.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorRamp.Accordion.Components;

public partial class Accordion : IAsyncDisposable
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public ExpandMode      ExpandMode   { get; set; } = ExpandMode.Multiple;
    [Parameter] public HeadingLevel    HeadingLevel { get; set; } = HeadingLevel.H3;

    [Parameter] public EventCallback<ItemHeadingData> OnAccordionItemHeadingClicked { get; set; }

    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private List<AccordionItem> _accordionItems = [];
    private IJSObjectReference? _jsModule       = null;

    private string _accordionID = $"accordion-{Guid.NewGuid()}";
    private bool   _disposed    = false;
    private int    _focusIndex = -1;
    private bool   _triggerHasFocus = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);
            await _jsModule.InvokeVoidAsync(GlobalValues.JS_Register_Handler_Func, _accordionID);

            await CheckSetStartState(ExpandMode, _accordionItems);
        }
    }

    private async Task CheckSetStartState(ExpandMode expandMode, List<AccordionItem> accordionItems)
    {
        if (expandMode != ExpandMode.Single) return;

        int openCount = 0;

        foreach (var accordionItem in accordionItems)
        {
            openCount = accordionItem.IsExpanded ? openCount + 1 : openCount;

            if (openCount > 1 && true == accordionItem.IsExpanded) await accordionItem.ToggleExpandedState();
        }
    }
    internal void AddAccordionItem(AccordionItem accordionItem)
    {
        if (false == _accordionItems.Contains(accordionItem)) _accordionItems.Add(accordionItem); 
    }
    
    internal void RemoveAccordionItem(AccordionItem accordionItem)

        => _accordionItems.Remove(accordionItem);


    internal async Task NotifyHeadingClicked(Guid accordionItemID)
    {
        var accordionItem = _accordionItems.Where(a => a.AccordionItemID == accordionItemID).SingleOrDefault();

        if (accordionItem is null) return;

        var itemWasExpanded = accordionItem.IsExpanded;

        _focusIndex = _accordionItems.IndexOf(accordionItem);

        if (ExpandMode == ExpandMode.Single) await CollapseAllPanels();

        if ((false == itemWasExpanded && ExpandMode == ExpandMode.Single) || (ExpandMode == ExpandMode.Multiple)) await accordionItem.ToggleExpandedState();

        await RaiseOnAccordionItemHeadingClicked(_focusIndex, accordionItem.HeadingText, accordionItem.IsExpanded);
    }

    private async Task RaiseOnAccordionItemHeadingClicked(int itemIndex, string headingText, bool isExpanded)
    {
        if (OnAccordionItemHeadingClicked.HasDelegate)
        {
            await OnAccordionItemHeadingClicked.InvokeAsync(new(itemIndex, headingText, isExpanded));
            return;
        }
        await InvokeAsync(StateHasChanged);
    }

    public async Task ExpandAllPanels()
    {
        foreach (var accordionItem in _accordionItems)
        {
            if (false == accordionItem.IsExpanded) await accordionItem.ToggleExpandedState();
        }
    }

    public async Task CollapseAllPanels()
    {
        foreach(var accordionItem in _accordionItems)
        {
            if (true == accordionItem.IsExpanded) await accordionItem.ToggleExpandedState();
        }
          
    }

    public async Task ExpandPanel(int index, bool codeFocused = false)
    {
        if (index < 0 || index > _accordionItems.Count - 1) return;

        var accordionItem = _accordionItems[index];

        if (false == accordionItem.IsExpanded)
        {
            if (ExpandMode == ExpandMode.Single) await CollapseAllPanels();
            await accordionItem.ToggleExpandedState();
        }

       if (true == codeFocused) await SetItemFocus(index,codeFocused);
    }
    private void UpdateFocusIndex(Guid accordionItemID)
    {
        var accordionItem = _accordionItems.Where(a => a.AccordionItemID == accordionItemID).SingleOrDefault();

        if (accordionItem is not null) _focusIndex = _accordionItems.IndexOf(accordionItem);
    }

    internal void NotifyHeadingFocusIn(Guid accordionItemID)
    {
        UpdateFocusIndex(accordionItemID);
        _triggerHasFocus = true;
    }

    internal void NotifyHeadingFocusOut()
        
        => _triggerHasFocus = false;

    private async ValueTask SetItemFocus(int itemIndex, bool codeFocused = false)
    {
        _focusIndex = itemIndex;
        await _accordionItems[itemIndex].SetFocus(codeFocused);
    }

    private async Task HandleHomeEndKeys(bool isHome)
    {
        var focusIndex = isHome ? 0 : _accordionItems.Count - 1;

        await SetItemFocus(focusIndex);
    }
    private async Task HandleArrowKeys(Direction direction, int focusIndex)
    {
        int maxIndex = _accordionItems.Count - 1;

        focusIndex = direction switch
        {
            Direction.Down when focusIndex < maxIndex => focusIndex + 1,
            Direction.Down when focusIndex >= maxIndex => 0,
            Direction.Up when focusIndex > 0 => focusIndex - 1,
            Direction.Up when focusIndex <= 0 => maxIndex,
            _ => focusIndex
        };

        await SetItemFocus(focusIndex);
    }

    private async Task HandleKeyDown(KeyboardEventArgs keyArgs)
    {
        if (_focusIndex < 0 || false == _triggerHasFocus) return;

        var keyboardTask = keyArgs.Key switch
        {
            GlobalValues.KeyBoard_Down_Arrow_Key => HandleArrowKeys(Direction.Down, _focusIndex),
            GlobalValues.KeyBoard_Up_Arrow_Key   => HandleArrowKeys(Direction.Up, _focusIndex),
            GlobalValues.KeyBoard_Home_Key       => HandleHomeEndKeys(true),
            GlobalValues.KeyBoard_End_Key        => HandleHomeEndKeys(false),
            _ => Task.CompletedTask
        };
        await keyboardTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule == null || true == _disposed) return;

        try
        {
            await _jsModule.InvokeVoidAsync(GlobalValues.JS_UnRegister_Handler_Func, _accordionID);
            await _jsModule.DisposeAsync();
        }
        catch { }

        _disposed = true;
    }
}
