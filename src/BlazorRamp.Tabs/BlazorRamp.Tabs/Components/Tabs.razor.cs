using BlazorRamp.Tabs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorRamp.Tabs.Components;

/// <summary>
/// A Blazor component that renders an accessible tabs widget, managing a set of
/// <see cref="Tab"/> child components with full keyboard navigation, roving tabindex,
/// and optional automatic panel activation.
/// </summary>
public partial class Tabs : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the child content of the component, expected to contain one or more
    /// <see cref="Tab"/> components.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the ID of an external element that provides an accessible label for
    /// the tab list via <c>aria-labelledby</c>. When set, <see cref="AriaLabel"/> is ignored.
    /// </summary>
    [Parameter] public string? AriaLabelledBy { get; set; }

    /// <summary>
    /// Gets or sets the accessible label applied to the tab list via <c>aria-label</c>.
    /// Ignored when <see cref="AriaLabelledBy"/> is set. Defaults to <c>"Tabs"</c>.
    /// </summary>
    [Parameter] public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the active tab index changes,
    /// providing the zero-based index of the newly selected tab.
    /// </summary>
    [Parameter] public EventCallback<int> ActiveTabIndexChanged { get; set; }

    /// <summary>
    /// Gets or sets the zero-based index of the initially active tab. Defaults to <c>0</c>.
    /// </summary>
    [Parameter] public int ActiveTabIndex { get; set; } = 0;

    /// <summary>
    /// Gets or sets a value indicating whether a tab panel is activated automatically
    /// when its button receives focus during keyboard navigation. When <see langword="false"/>,
    /// the user must press Enter or Space to activate. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool AutoActivatePanel { get; set; } = true;

    /// <summary>
    /// Gets or sets the position of the icon relative to the tab title for all tabs.
    /// Defaults to <see cref="TabIconPosition.Left"/>.
    /// </summary>
    [Parameter] public TabIconPosition TabIconPosition { get; set; } = TabIconPosition.Left;

    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jsModule;

    internal Tab?   ActiveTab  { get; set; } = null;

    private Dictionary<Tab, ElementReference> _tabButtonRefs = [];
    private List<Tab>                         _tabs          = [];

    private string  _tabsListID     = $"tablist-{Guid.NewGuid()}";
    private int     _activeTabIndex = -1;
    private int     _rovingIndex    = 0;
    private string? _ariaLabelledBy = null;
    private string? _ariaLabel      = null;

    /// <summary>
    /// Registers a <see cref="Tab"/> with the parent <see cref="Tabs"/> component.
    /// Called automatically during <see cref="Tab"/> initialisation.
    /// </summary>
    /// <param name="tab">The <see cref="Tab"/> instance to register.</param>
    internal void AddTab(Tab tab)
    {
        if (_tabs.Contains(tab)) return;

        _tabs.Add(tab);
        _tabButtonRefs.TryAdd(tab, new ElementReference());
    }

    /// <summary>
    /// Removes a <see cref="Tab"/> from the tab list. Called automatically when a
    /// <see cref="Tab"/> component is disposed.
    /// </summary>
    /// <param name="tab">The <see cref="Tab"/> instance to remove.</param>
    public void RemoveTab(Tab tab)
    {
        _tabButtonRefs.Remove(tab);
        _tabs.Remove(tab);
    }
    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        if (_tabs.Count == 0) return;
        if (ActiveTabIndex != _activeTabIndex) await RaiseTabChanged(ActiveTabIndex);
    }
    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        _ariaLabelledBy = String.IsNullOrWhiteSpace(AriaLabelledBy) ? null : AriaLabelledBy.Trim();
        _ariaLabel = _ariaLabelledBy != null ? null
                                             : (string.IsNullOrWhiteSpace(AriaLabel) ? GlobalValues.Tabs_Default_ACC_Name : AriaLabel.Trim());
    }
    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Module_File_Path);
            await _jsModule.InvokeVoidAsync(GlobalValues.JS_Register_Tabs_Func, _tabsListID);

            await RaiseTabChanged(ActiveTabIndex);
            if (false == ActiveTabIndexChanged.HasDelegate) await InvokeAsync(StateHasChanged);
        }
    }

    private async Task RaiseTabChanged(int tabIndex)
    {
        if (_tabs.Count == 0) return;

        // Cycle index if out of range
        tabIndex = tabIndex switch
        {
            _ when tabIndex >= _tabs.Count => 0,
            _ when tabIndex < 0 => _tabs.Count - 1,
            _ => tabIndex
        };

        _activeTabIndex = _rovingIndex = tabIndex;
        ActiveTab = _tabs[tabIndex];

        if (true == ActiveTabIndexChanged.HasDelegate) await ActiveTabIndexChanged.InvokeAsync(tabIndex);
    
    }

    
    private async Task CheckRaiseTabChanged(int tabIndex)
    {        
        if (true == AutoActivatePanel) await RaiseTabChanged(tabIndex);
    }   

    private async Task HandleOnKeyDown(KeyboardEventArgs keyArgs)
    {
        int tabCount     = _tabs.Count - 1;
        var focusToIndex = -1;

        focusToIndex = keyArgs.Key switch
        {
            GlobalValues.KeyBoard_Left_Arrow_Key => (_rovingIndex > 0 ? _rovingIndex - 1 : tabCount),

            GlobalValues.KeyBoard_Right_Arrow_Key => (_rovingIndex < tabCount ? _rovingIndex + 1 : 0),
            
            GlobalValues.KeyBoard_Home_Key => 0,
            GlobalValues.KeyBoard_End_Key => tabCount,
            _ => -1
        };

        if (focusToIndex > -1 && focusToIndex != _rovingIndex)
        {
            var targetTab = _tabs[focusToIndex];
            _rovingIndex  = focusToIndex;
            await _tabButtonRefs[targetTab].FocusAsync();
        }

        if (focusToIndex == -1 && keyArgs.Key == GlobalValues.KeyBoard_Tab_Key) _rovingIndex = _activeTabIndex;
    }

    private string GetTabContentClasses(TabIconPosition tabIconPosition)
    {
        var modifierClass = tabIconPosition switch
        {
            TabIconPosition.Top    => GlobalValues.Tabs_Tab_Content_Icon_Top_Mdoifier,
            TabIconPosition.Right  => GlobalValues.Tabs_Tab_Content_Icon_Right_Mdoifier,
            TabIconPosition.Bottom => GlobalValues.Tabs_Tab_Content_Icon_Bottom_Mdoifier,
            _                      => GlobalValues.Tabs_Tab_Content_Icon_Left_Mdoifier

        };

        return $"{GlobalValues.Tabs_Tab_Content_Class} {modifierClass}";
    }


    /// <summary>
    /// Programmatically activates a tab by index and moves browser focus to its
    /// button so that screen readers announce the change correctly.
    /// </summary>
    public async Task SetActiveTab(int tabIndex)
    {
        if (tabIndex < 0 || tabIndex > _tabs.Count - 1 || _activeTabIndex == tabIndex) return;

        var targetTab = _tabs[tabIndex];

        await RaiseTabChanged(tabIndex);

        await _tabButtonRefs[targetTab].FocusAsync();
    }

    /// <summary>
    /// Cleans up the component by unregistering the tab list keyboard listener
    /// and disposing the JavaScript module reference.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync(GlobalValues.JS_UnRegister_Tabs_Func, _tabsListID);
                await _jsModule.DisposeAsync();
            }
               
        }
        catch { }
    }
}
