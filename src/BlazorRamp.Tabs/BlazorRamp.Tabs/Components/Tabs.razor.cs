using BlazorRamp.Tabs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Tabs.Components;

public partial class Tabs
{

    [Parameter] public RenderFragment?    ChildContent          { get; set; }
    [Parameter] public string?            AriaLabelledBy        { get; set; }
    [Parameter] public string?            AriaLabel             { get; set; }
    [Parameter] public EventCallback<int> ActiveTabIndexChanged { get; set; }
    [Parameter] public int                ActiveTabIndex        { get; set; } = 0;
    [Parameter] public bool               AutoActivatePanel     { get; set; } = false;
    [Parameter] public TabIconPosition    TabIconPosition       { get; set; } = TabIconPosition.Left;

    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    internal Tab?   ActiveTab  { get; set; } = null;

    private Dictionary<Tab, ElementReference> _tabButtonRefs = [];
    private List<Tab>                         _tabs          = [];

    private int     _activeTabIndex = -1;
    private int     _rovingIndex    = 0;
    private string? _ariaLabelledBy = null;
    private string? _ariaLabel      = null;

    internal void AddTab(Tab tab)
    {
        if (_tabs.Contains(tab)) return;

        _tabs.Add(tab);
        _tabButtonRefs.TryAdd(tab, new ElementReference());
    }
    public void RemoveTab(Tab tab)
    {
        _tabButtonRefs.Remove(tab);
        _tabs.Remove(tab);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_tabs.Count == 0) return;
        if (ActiveTabIndex != _activeTabIndex) await RaiseTabChanged(ActiveTabIndex);
    }

    protected override void OnInitialized()
    {
        _ariaLabelledBy = String.IsNullOrWhiteSpace(AriaLabelledBy) ? null : AriaLabelledBy.Trim();
        _ariaLabel = _ariaLabelledBy != null ? null
                                             : (string.IsNullOrWhiteSpace(AriaLabel) ? GlobalValues.Tabs_Default_ACC_Name : AriaLabel.Trim());
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
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
            GlobalValues.KeyBoard_Left_Arrow_Key or 
            GlobalValues.KeyBoard_Up_Arrow_Key => (_rovingIndex > 0 ? _rovingIndex - 1 : tabCount),

            GlobalValues.KeyBoard_Right_Arrow_Key or 
            GlobalValues.KeyBoard_Down_Arrow_Key  => (_rovingIndex < tabCount ? _rovingIndex + 1 : 0),
            
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


}
