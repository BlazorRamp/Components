using BlazorRamp.Core.Services;
using BlazorRamp.Disclosure.Common.Constants;
using BlazorRamp.Disclosure.Common.Utilities;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Disclosure.Components;

/// <summary>
/// An accessible disclosure: a button that shows and hides a single content region, using
/// <c>aria-expanded</c> and <c>aria-controls</c>. The content region is registered with the
/// Core auto-tabindex service, so it becomes keyboard focusable only when it overflows and
/// contains no focusable descendants.
/// </summary>
public partial class Disclosure: IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the content displayed in the expandable region.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; } = null;

    /// <summary>
    /// Gets or sets the text of the trigger button, which also provides its accessible name.
    /// Leading and trailing whitespace is trimmed. Required: an <see cref="ArgumentNullException"/>
    /// is thrown if it is <see langword="null"/>, empty or whitespace.
    /// </summary>
    [Parameter] public string TriggerText            { get; set; } = default!;


    /// <summary>
    /// Gets or sets a value indicating whether the contents element is given
    /// <c>role="region"</c>, making it a landmark navigable by screen readers.
    /// Only set to <see langword="true"/> when it contains sufficiently important
    /// content to warrant landmark navigation.
    /// Only visible as a region when the content is expanded.
    /// Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter] public bool ContentIsRegion { get; set; } = false;
    /// <summary>
    /// Gets or sets a value indicating whether the panel content is retained in the DOM
    /// when the content is collapsed. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter] public bool PersistContent { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the content is expanded on initial render.
    /// Supports two-way binding via <see cref="ExpandedChanged"/>. Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter] public bool Expanded { get; set; } = false;

    /// <summary>
    /// Gets or sets the callback invoked when the expanded state changes, supporting
    /// two-way binding with <c>@bind-Expanded</c>.
    /// </summary>
    [Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
    /// used as a mask-image icon next to the item text. Must begin with <c>--</c>.
    /// For example: <c>--svg-my-icon</c>.
    /// </summary>
    [Parameter] public string? SvgIcon { get; set; } = default;

    /// <summary>
    /// Gets or sets a value indicating whether the border and the inline (left and right) padding
    /// are removed from the trigger button, so it lines up with the surrounding content.
    /// Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter] public bool RemoveTriggerBorderPadding { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the border and the inline (left and right) padding
    /// are removed from the content element, so it lines up with the surrounding content.
    /// Defaults to <see langword="false"/>.
    /// </summary>
    [Parameter] public bool RemoveContentBorderPadding { get; set; } = false;

    /// <summary>
    /// Gets or sets the maximum height of the content element as a CSS length, for example <c>20rem</c>.
    /// Content taller than this scrolls within the element, and the Core service makes it keyboard
    /// focusable when it contains nothing focusable. Accepts a non-negative number followed by one of
    /// <c>rem</c>, <c>em</c>, <c>lh</c>, <c>ch</c>, <c>px</c>, <c>vh</c>, <c>dvh</c>, <c>svh</c>, <c>lvh</c>
    /// or <c>vw</c>; percentages and functions such as <c>calc()</c> are not supported.
    /// Relative units such as <c>rem</c> are recommended because they scale with the user's text size.
    /// Defaults to <see langword="null"/>, meaning no limit.
    /// </summary>
    /// <remarks>
    /// An <see cref="ArgumentException"/> is thrown if a value is supplied that isn't a supported length.
    /// </remarks>
    [Parameter] public string? ContentMaxHeight { get; set; } = null;

    /// <summary>
    /// Gets or sets additional attributes applied to the component's root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private ElementReference ContainerRef { get; set; } = default;


    /// <summary>
    /// The Core service that adds and removes <c>tabindex</c> on the content element when it becomes scrollable.
    /// </summary>
    [Inject] private ICoreUtilityService CoreUtilityService { get; set; } = default!;
    private string  _triggerText        = String.Empty;
    private bool    _isExpanded         = false;
    private bool    _persistContent     = false;
    private bool    _contentIsRegion    = false; 
    private string? _svgIcon            = null;
    private string  _contentID          = Guid.NewGuid().ToString();
    private string  _triggerID          = Guid.NewGuid().ToString();
    private bool    _lastExpanded       = false;
    private string? _contentHeightStyle = null;

    /// <summary>
    /// Validates <see cref="TriggerText"/>, copies the parameters into their working fields, and
    /// syncs the expanded state when the parent has changed <see cref="Expanded"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <see cref="TriggerText"/> is <see langword="null"/>, empty or whitespace.
    /// </exception>
    protected override void OnParametersSet()
    {
        if (true == String.IsNullOrWhiteSpace(TriggerText)) throw new ArgumentNullException(nameof(TriggerText), GlobalValues.Disclosure_Trigger_Text_Exception_Message);
        
        _svgIcon           = CheckSetSvgVariable(SvgIcon, GlobalValues.Disclosure_Trigger_Icon_Svg_Css_Variable_Name);
        _triggerText       = TriggerText.Trim();
        _persistContent    = PersistContent;
        _contentIsRegion   = ContentIsRegion;

        if (Expanded != _lastExpanded)
        {
            _isExpanded = Expanded;
            _lastExpanded = Expanded;
        }
        
        _contentHeightStyle = null;

        if (true == String.IsNullOrWhiteSpace(ContentMaxHeight)) return;

        var trimmedValue = ContentMaxHeight.Trim().Trim(';');
        if (false ==GeneralUtilities.IsValidCssLength(trimmedValue))  throw new ArgumentException(GlobalValues.Disclosure_Content_Max_Height_Exception_Message, nameof(ContentMaxHeight));
        
        _contentHeightStyle = $"{GlobalValues.Disclosure_Content_Height_Css_Variable_Name}:{trimmedValue};";
    }


    /// <summary>
    /// Toggles the expanded state of the disclosure.
    /// </summary>
    private async Task ToggleExpandedState()

        => await RaiseExpandedChanged(!_isExpanded);


    /// <summary>
    /// Updates the expanded state and invokes <see cref="ExpandedChanged"/> if a delegate
    /// is bound, otherwise requests a state update. Does nothing if the state is unchanged.
    /// </summary>
    private async Task RaiseExpandedChanged(bool expanded)
    {
        if (_isExpanded == expanded) return;

        _isExpanded = expanded;

        if (ExpandedChanged.HasDelegate)
            await ExpandedChanged.InvokeAsync(expanded);
        else
            await InvokeAsync(StateHasChanged);

    }

    /// <summary>
    /// Converts the name of a CSS custom property into an inline style declaration that assigns it
    /// to the component's internal icon variable.
    /// </summary>
    /// <param name="svgIcon">The custom property name supplied by the consumer, which must begin with <c>--</c>.</param>
    /// <param name="variableName">The name of the internal CSS variable to assign the icon to.</param>
    /// <returns>
    /// A declaration such as <c>--_br-svg-disclosure-trigger-icon:var(--svg-my-icon);</c>, or
    /// <see langword="null"/> if <paramref name="svgIcon"/> is empty, whitespace or doesn't begin with <c>--</c>.
    /// </returns>
    private static string? CheckSetSvgVariable(string? svgIcon, string variableName)
    {
        var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                ? null
                                : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

        return iconVariable is null ? null : $"{variableName}:{iconVariable};";
    }

    /// <summary>
    /// On first render, registers the content element with the Core service so it automatically receives a
    /// <c>tabindex</c> when it overflows and has no focusable descendants.
    /// </summary>
    /// <param name="firstRender">
    /// <see langword="true"/> if this is the first time the component has rendered; otherwise <see langword="false"/>.
    /// </param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(true == firstRender)
        {
            if (ContainerRef.Context is not null) await CoreUtilityService.RegisterContainerForAutoTabindex(ContainerRef, _triggerText);
        }
    }

    /// <summary>
    /// Expands or collapses the disclosure programmatically, for example through a component reference.
    /// Invokes <see cref="ExpandedChanged"/> if the state changes; does nothing if it is already in the requested state.
    /// </summary>
    /// <param name="expandedState"><see langword="true"/> to expand the content; <see langword="false"/> to collapse it.</param>
    public async Task SetExpandedState(bool expandedState)

        => await RaiseExpandedChanged(expandedState);


    /// <summary>
    /// Builds the CSS class string for the root element by combining the base disclosure
    /// class with any additional class passed via <see cref="AdditionalAttributes"/>.
    /// </summary>
    /// <param name="additionalAttributes">The unmatched attributes captured by the component, which may contain a <c>class</c> entry.</param>
    /// <returns>
    /// The disclosure class, followed by the additional class if one was supplied.
    /// </returns>
    private static string GetDisclosureClasses(IReadOnlyDictionary<string, object>? additionalAttributes)
    {
        var classData = additionalAttributes?.TryGetValue("class", out var extraClass) == true ? extraClass.ToString() : "";

        if (false == String.IsNullOrWhiteSpace(classData))
        {
            return $"{GlobalValues.Disclosure_Class} {classData}";
        }

        return GlobalValues.Disclosure_Class;
    }

    private static string GetTriggerContentClasses(bool removeBorderPadding, bool isTriggerElement)

        => isTriggerElement switch
        {
            true  => $"{GlobalValues.Disclosure_Trigger_Class}{(removeBorderPadding ? " " + GlobalValues.Disclosure_Trigger_Modifier_Class : "")}",
            false => $"{GlobalValues.Disclosure_Content_Class}{(removeBorderPadding ? " " + GlobalValues.Disclosure_Content_Modifier_Class : "")}"
        };
    

    /// <summary>
    /// Unregisters the content element from the Core auto-tabindex service. Any exception, such as
    /// a disconnected circuit, is ignored because the underlying observers go away with the element.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if(ContainerRef.Context is not null) await CoreUtilityService.UnregisterContainerForAutoTabindex(ContainerRef);
        }
        catch { }
    }
}
