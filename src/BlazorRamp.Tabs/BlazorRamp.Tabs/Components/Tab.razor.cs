using BlazorRamp.Tabs.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Tabs.Components;

public partial class Tab : IDisposable
{
    [CascadingParameter]        public Tabs?           ParentControl   { get; set; } = default!;
    [Parameter]                 public RenderFragment? TabPanelContent { get; set; } = null;
    [Parameter, EditorRequired] public string          TabTitle        { get; set; } = default!;


    internal string TabPanelID { get; } = $"tab-panel-{Guid.NewGuid().ToString()}";
    internal string TabID      { get; } = $"tab-{Guid.NewGuid().ToString()}";

    private string _cssClasses = GlobalValues.Tabs_Tab_Panel_Class;

    protected override void OnParametersSet()
    {
        if (String.IsNullOrWhiteSpace(TabTitle)) throw new ArgumentNullException(nameof(TabTitle),GlobalValues.Error_Message_Tab_Title);

        _cssClasses = ParentControl!.ActiveTab == this 
                        ? $"{GlobalValues.Tabs_Tab_Panel_Class} {GlobalValues.Tabs_Tab_Panel_Active_Modifier}"
                            : GlobalValues.Tabs_Tab_Panel_Class ;

        base.OnParametersSet();
    }

    protected override void OnInitialized()
    {
        if (ParentControl == null) throw new ArgumentNullException(GlobalValues.Error_Message_Needs_Tabs_Componenent);

        ParentControl.AddTab(this);
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
