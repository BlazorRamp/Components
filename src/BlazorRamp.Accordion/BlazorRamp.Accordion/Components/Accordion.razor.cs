using BlazorRamp.Accordion.Common.Constants;
using BlazorRamp.Accordion.Common.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorRamp.Accordion.Components;

/// <summary>
/// A Blazor component that renders an accessible accordion component, managing a set of
/// <see cref="AccordionItem"/> child components with full keyboard navigation and
/// configurable expand behaviour.
/// </summary>
public partial class Accordion : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the child content of the component, expected to contain one or more
    /// <see cref="AccordionItem"/> components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the expand mode controlling whether one or multiple panels can be
    /// open simultaneously. Defaults to <see cref="ExpandMode.Multiple"/>.
    /// </summary>
    [Parameter] public ExpandMode      ExpandMode   { get; set; } = ExpandMode.Multiple;

    /// <summary>
    /// Gets or sets the semantic heading level rendered for each accordion item trigger.
    /// Defaults to <see cref="HeadingLevel.H3"/>.
    /// </summary>
    [Parameter] public HeadingLevel    HeadingLevel { get; set; } = HeadingLevel.H3;

    /// <summary>
    /// Gets or sets the callback invoked when an accordion item heading is clicked,
    /// providing an <see cref="ItemHeadingData"/> payload containing the item index,
    /// heading text, and expanded state.
    /// </summary>
    [Parameter] public EventCallback<ItemHeadingData> OnAccordionItemHeadingClicked { get; set; }

    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private List<AccordionItem> _accordionItems = [];
    private IJSObjectReference? _jsModule       = null;

    private string _accordionID     = $"accordion-{Guid.NewGuid()}";
    private bool   _disposed        = false;
    internal int   _focusIndex      = -1; //Made internal so its easy to test the keyboard navigation 
    private bool   _triggerHasFocus = false;

    /// <summary>
    /// Initialises the JavaScript module on first render and enforces single expand mode
    /// start state if any items were pre-expanded.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);
            await _jsModule.InvokeVoidAsync(GlobalValues.JS_Register_Handler_Func, _accordionID);

            await CheckSetStartState(ExpandMode, _accordionItems);
        }
    }

    /// <summary>
    /// Ensures only one panel is expanded on initial render when <see cref="ExpandMode"/>
    /// is <see cref="ExpandMode.Single"/>, collapsing any additional pre-expanded items.
    /// </summary>
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

    /// <summary>
    /// Registers an <see cref="AccordionItem"/> with this accordion. Called automatically
    /// by each child item during initialisation.
    /// </summary>
    internal void AddAccordionItem(AccordionItem accordionItem)
    {
        if (false == _accordionItems.Contains(accordionItem)) _accordionItems.Add(accordionItem); 
    }

    /// <summary>
    /// Removes an <see cref="AccordionItem"/> from this accordion. Called automatically
    /// when a child item is disposed.
    /// </summary>
    internal void RemoveAccordionItem(AccordionItem accordionItem)

        => _accordionItems.Remove(accordionItem);

    /// <summary>
    /// Handles a heading click from a child <see cref="AccordionItem"/>, toggling its
    /// expanded state and enforcing single expand mode if required.
    /// </summary>
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

    /// <summary>
    /// Invokes <see cref="OnAccordionItemHeadingClicked"/> if a delegate is bound,
    /// otherwise requests a state update.
    /// </summary>
    private async Task RaiseOnAccordionItemHeadingClicked(int itemIndex, string headingText, bool isExpanded)
    {
        if (OnAccordionItemHeadingClicked.HasDelegate)
        {
            await OnAccordionItemHeadingClicked.InvokeAsync(new(itemIndex, headingText, isExpanded));
            return;
        }
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Expands all accordion panels irrespective of the <see cref="ExpandMode"/> .
    /// </summary>
    public async Task ExpandAllPanels()
    {
        foreach (var accordionItem in _accordionItems)
        {
            if (false == accordionItem.IsExpanded) await accordionItem.ToggleExpandedState();
        }
    }

    /// <summary>
    /// Collapses all accordion panels.
    /// </summary>
    public async Task CollapseAllPanels()
    {
        foreach(var accordionItem in _accordionItems)
        {
            if (true == accordionItem.IsExpanded) await accordionItem.ToggleExpandedState();
        }
          
    }

    /// <summary>
    /// Expands the accordion panel at the specified zero-based index.
    /// In <see cref="ExpandMode.Single"/> mode, all other panels are collapsed first.
    /// Does nothing if the index is out of range.
    /// </summary>
    /// <param name="index">The zero-based index of the panel to expand.</param>
    /// <param name="codeFocused">
    /// When <see langword="true"/>, programmatically focuses the trigger button and applies
    /// the code-focused visual indicator. Defaults to <see langword="false"/>.
    /// </param>
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

    /// <summary>
    /// Updates <see cref="_focusIndex"/> to the position of the item matching the given ID.
    /// </summary>
    private void UpdateFocusIndex(Guid accordionItemID)
    {
        var accordionItem = _accordionItems.Where(a => a.AccordionItemID == accordionItemID).SingleOrDefault();

        if (accordionItem is not null) _focusIndex = _accordionItems.IndexOf(accordionItem);
    }

    /// <summary>
    /// Notifies the accordion that a heading trigger has received focus, updating the
    /// focus index and setting the trigger focus guard.
    /// </summary>
    internal void NotifyHeadingFocusIn(Guid accordionItemID)
    {
        UpdateFocusIndex(accordionItemID);
        _triggerHasFocus = true;
    }

    /// <summary>
    /// Notifies the accordion that a heading trigger has lost focus, clearing the
    /// trigger focus guard to prevent keyboard navigation from firing.
    /// </summary>
    internal void NotifyHeadingFocusOut()
        
        => _triggerHasFocus = false;

    /// <summary>
    /// Moves programmatic focus to the accordion item at the specified index.
    /// </summary>
    private async ValueTask SetItemFocus(int itemIndex, bool codeFocused = false)
    {
        _focusIndex = itemIndex;
        await _accordionItems[itemIndex].SetFocus(codeFocused);
    }

    /// <summary>
    /// Handles the Home and End keyboard keys, moving focus to the first or last item.
    /// </summary>
    private async Task HandleHomeEndKeys(bool isHome)
    {
        var focusIndex = isHome ? 0 : _accordionItems.Count - 1;

        await SetItemFocus(focusIndex);
    }

    /// <summary>
    /// Handles the Arrow Up and Arrow Down keyboard keys, moving focus to the
    /// previous or next item with wrapping at the boundaries.
    /// </summary>
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

    /// <summary>
    /// Handles keydown events on the accordion wrapper, routing Arrow, Home, and End keys
    /// to the appropriate navigation handlers. Ignored when no trigger has focus.
    /// </summary>
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
    /// <summary>
    /// Unregisters the JavaScript key handler and disposes the JS module.
    /// </summary>
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
