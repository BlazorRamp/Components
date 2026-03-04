using BlazorRamp.Tabs.Common.Constants;
using BlazorRamp.Tabs.Common.Models;
using Microsoft.AspNetCore.Components;
using System.Reflection.PortableExecutable;

namespace BlazorRamp.Tabs.Components;

public partial class Tab : IDisposable
{
    [CascadingParameter]        public Tabs?           ParentControl   { get; set; } = default!;
    [Parameter]                 public RenderFragment? TabPanelContent { get; set; } = null;
    [Parameter, EditorRequired] public string          TabTitle        { get; set; } = default!;
    [Parameter]                 public bool            PersistContent  { get; set; } = true;
    [Parameter]                 public string?         SvgIcon         { get; set; } = default;



    internal string TabPanelID   { get; } = $"tab-panel-{Guid.NewGuid().ToString()}";
    internal string TabID        { get; } = $"tab-{Guid.NewGuid().ToString()}";
    internal string? SvgVariable { get; private set;}  = null;

    private string _cssClasses = GlobalValues.Tabs_Tab_Panel_Class;

    private bool _isActiveTab    = false;
    private bool _performRender  = false;

    private string? _svgIcon = null;

    protected override void OnParametersSet()
    {
       
        if (String.IsNullOrWhiteSpace(TabTitle)) throw new ArgumentNullException(nameof(TabTitle),GlobalValues.Error_Message_Tab_Title);

        SvgVariable = CheckSetSvgVariable(SvgIcon);

        _isActiveTab = ParentControl!.ActiveTab == this;

        if (_performRender == false && _isActiveTab == true) _performRender = true;

        if (true == _isActiveTab)
        {
            _performRender = true;
        }
        else if (false == PersistContent)
        {
            _performRender = false;
        }

        _cssClasses = _isActiveTab ? $"{GlobalValues.Tabs_Tab_Panel_Class} {GlobalValues.Tabs_Tab_Panel_Active_Modifier}"
                                   : GlobalValues.Tabs_Tab_Panel_Class ;
       
    }

    protected override void OnInitialized()
    {
        if (ParentControl == null) throw new ArgumentNullException(GlobalValues.Error_Message_Needs_Tabs_Componenent);

        ParentControl.AddTab(this);
    }

    internal string? CheckSetSvgVariable(string? svgIcon)
    {
        var iconVariable = String.IsNullOrWhiteSpace(SvgIcon) 
                                ? null
                                : SvgIcon.TrimStart().StartsWith("--") ? $"var({SvgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{GlobalValues.Tab_Svg_Css_Variable_Name}:{iconVariable};";
    }

    public void Dispose()
    {
        try
        {
            ParentControl?.RemoveTab(this);
        }
        catch { }
    }
}
