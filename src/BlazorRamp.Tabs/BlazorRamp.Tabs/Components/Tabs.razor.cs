using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.Tabs.Components;

public partial class Tabs
{

    [Parameter] public RenderFragment?    ChildContent          { get; set; }
    [Parameter] public EventCallback<int> ActiveTabIndexChanged { get; set; }
    [Parameter] public int                ActiveTabIndex        { get; set; } = 0;
    [Parameter] public bool               AutoActivatePanel     { get; set; } = false;

    internal Tab?   ActiveTab  { get; set; } = null;

    private Dictionary<Tab, ElementReference> _tabButtonRefs = [];
    private List<Tab>                         _tabs          = [];

    private int _activeTabIndex = -1;
    private int _rovingIndex    = 0;


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
        // Cycle index if out of range
        tabIndex = tabIndex switch
        {
            _ when tabIndex >= _tabs.Count => 0,
            _ when tabIndex < 0 => _tabs.Count - 1,
            _ => tabIndex
        };

        if (_tabs.Count == 0) return;

        _activeTabIndex = _rovingIndex = tabIndex;
        ActiveTab = _tabs[tabIndex];

        if (true == ActiveTabIndexChanged.HasDelegate) await ActiveTabIndexChanged.InvokeAsync(tabIndex);
    
    }
    private async Task CheckRaiseTabChanged(int tabIndex)
    {
        if (true == AutoActivatePanel) await RaiseTabChanged(tabIndex);
    }   

}
