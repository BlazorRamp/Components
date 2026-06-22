using BlazorRamp.Pager.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Pager.Components;

public partial class Pager
{
    [Parameter] public PagerSelectorType    PagerSelectorType    { get; set; } = PagerSelectorType.Button;
    [Parameter] public PagerAnnouncmentType PagerAnnouncmentType { get; set; } = PagerAnnouncmentType.WithAnnouncement;

    [Parameter] public string NextText      { get; set; } = GlobalValues.Pager_Selector_Next_Text;
    [Parameter] public string PreviousText  { get; set; } = GlobalValues.Pager_Selector_Prev_Text;
    [Parameter] public string FirstText     { get; set; } = GlobalValues.Pager_Selector_First_Text;
    [Parameter] public string LastText      { get; set; } = GlobalValues.Pager_Selector_Last_Text;


    private PagerSelectorType    _pagerSelectorType    = PagerSelectorType.Button;
    private PagerAnnouncmentType _pagerAnnouncmentType = PagerAnnouncmentType.WithAnnouncement;
    
    private string _nextText  = GlobalValues.Pager_Selector_Next_Text;
    private string _prevText  = GlobalValues.Pager_Selector_Prev_Text;
    private string _firstText = GlobalValues.Pager_Selector_First_Text;
    private string _lastText  = GlobalValues.Pager_Selector_Last_Text;

    private string _informationText = GlobalValues.Pager_No_Records_Text;

    protected override void OnParametersSet()
    {
        _pagerAnnouncmentType = PagerAnnouncmentType;
        _pagerSelectorType    = PagerSelectorType;
        _nextText             = String.IsNullOrWhiteSpace(NextText)     ? GlobalValues.Pager_Selector_Next_Text  : NextText.Trim();
        _prevText             = String.IsNullOrWhiteSpace(PreviousText) ? GlobalValues.Pager_Selector_Prev_Text  : PreviousText.Trim();
        _lastText             = String.IsNullOrWhiteSpace(LastText)     ? GlobalValues.Pager_Selector_Last_Text  : LastText.Trim();
        _firstText            = String.IsNullOrWhiteSpace(FirstText)    ? GlobalValues.Pager_Selector_First_Text : FirstText.Trim();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }


    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        return base.OnAfterRenderAsync(firstRender);
    }
}
