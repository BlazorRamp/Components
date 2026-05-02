using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace BlazorRamp.Inputs.Components
{
    /// <summary>
    /// Abstract base class for all of the basic BlazorRamp input components. Extends Blazor's
    /// <see cref="InputBase{TValue}"/> with accessibility support, validation state
    /// management, hint text normalisation, SVG icon support, and aria-disabled
    /// simulation via JavaScript interop.
    /// </summary>
    public abstract class InputTypeBase<TValue> : InputBase<TValue>, IAsyncDisposable
    {
        /// <summary>
        /// Gets or sets the name used for the input control label.
        /// If the value is null, empty, or whitespace then the field name is used.
        /// </summary>
        [Parameter] public string LabelText { get; set; } = String.Empty;
        /// <summary>
        /// Gets or sets the <c>id</c> attribute applied to the underlying <c>&lt;input&gt;</c>
        /// element. When null, empty, or whitespace a <see cref="Guid"/> string is generated
        /// automatically. Leading and trailing whitespace is trimmed before use.
        /// </summary>
        [Parameter] public string ControlID     { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the hint text rendered below the label and above the input field.
        /// When set, the text is normalised so it always ends with a full stop, and is
        /// associated to the input via <c>aria-describedby</c>. When null, empty, or
        /// whitespace no hint element is rendered.
        /// </summary>
        [Parameter] public string HintText      { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets whether the input is read-only. When <c>true</c>, the HTML
        /// <c>readonly</c> attribute is applied to the underlying <c>&lt;input&gt;</c>
        /// element. Takes precedence over <see cref="AriaDisabled"/> when both are set.
        /// </summary>
        [Parameter] public bool   ReadOnly      { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the input is aria-disabled. When <c>true</c>, the input
        /// is visually and functionally disabled via <c>aria-disabled="true"</c> without
        /// using the native <c>disabled</c> attribute, keeping the field focusable and
        /// readable by screen readers. JavaScript handlers block keyboard input and paste.
        /// Has no effect when <see cref="ReadOnly"/> is also <c>true</c>.
        /// </summary>
        [Parameter] public bool   AriaDisabled  { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the field is required. When <c>true</c>, <c>aria-required="true"</c>
        /// is applied to the input and a visually hidden asterisk is appended to the label.
        /// Defaults to <c>true</c>.
        /// </summary>
        [Parameter] public bool   Required      { get; set; } = true;

        /// <summary>
        /// Gets or sets the accessible label for the tabbable error region, used when
        /// <see cref="ValidationDisplayMode"/> is set to <see cref="ValidationDisplayMode.TabbableWithHint"/>.
        /// Defaults to <c>"Errors"</c>.
        /// </summary>
        [Parameter] public string ErrorsLabel   { get; set; } = GlobalValues.Default_Errors_label;

        /// <summary>
        /// Gets or sets the validation display mode which controls how <c>aria-describedby</c>
        /// is set and whether the error region is tabbable. Defaults to
        /// <see cref="ValidationDisplayMode.DescribedByWithHint"/>.
        /// </summary>
        [Parameter] public ValidationDisplayMode ValidationDisplayMode { get; set; } = ValidationDisplayMode.DescribedByWithHint;

        /// <summary>
        /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
        /// used as a mask-image icon alongside the tab title. Must begin with <c>--</c>.
        /// For example: <c>--svg-my-icon</c>.
        /// </summary>
        [Parameter] public string? SvgIcon { get; set; } = default;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        /// <summary>
        /// Gets the <see cref="ElementReference"/> for the underlying <c>&lt;input&gt;</c>
        /// element, available after the component has rendered.
        /// </summary>
        public ElementReference  ControlReference { get; set; }

        private IJSObjectReference? _jSModule = null;

        /// <summary>
        /// Gets the current list of normalised validation error messages for this field.
        /// Each message is guaranteed to end with a full stop.
        /// </summary>
        protected List<string> InvalidMessages  { get; private set; } = [];

        /// <summary>
        /// Gets the resolved <c>id</c> attribute value applied to the underlying input element.
        /// Either the trimmed <see cref="ControlID"/> parameter or a generated <see cref="Guid"/> string.
        /// </summary>
        protected string  InputID           { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the CSS class string applied to the state icon element, reflecting the
        /// current validation state of the field — neutral, valid, or invalid.
        /// </summary>
        protected string  StateIconClasses  { get; private set; } = GlobalValues.Text_Input_State_Icon_Class;

        /// <summary>
        /// Gets a value indicating whether the input is currently in the aria-disabled state.
        /// <c>true</c> when <see cref="AriaDisabled"/> is <c>true</c> and <see cref="ReadOnly"/> is <c>false</c>.
        /// </summary>
        protected bool    IsDisabled        { get; private set; } = false;

        /// <summary>
        /// Gets the unique <c>id</c> of the hint text element, used to associate hint text
        /// to the input via <c>aria-describedby</c>.
        /// </summary>
        protected string  HintTextID        { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the normalised hint text, guaranteed to end with a full stop, or <c>null</c>
        /// when <see cref="HintText"/> is null, empty, or whitespace.
        /// </summary>
        protected string? HintNormalised    { get; private set; } = default;

        /// <summary>
        /// Gets the unique <c>id</c> of the error message element, used to associate error
        /// messages to the input via <c>aria-describedby</c>.
        /// </summary>
        protected string  ErrorMessageID    { get; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets the resolved CSS custom property string for the SVG icon, or <c>null</c>
        /// when no valid <see cref="SvgIcon"/> has been set.
        /// </summary>
        protected string? SvgVariable       { get; private set; } = null;

        /// <summary>
        /// Gets a value indicating whether the field currently has validation errors.
        /// </summary>
        protected bool    HasErrors         { get; private set; } = false;

        /// <summary>
        /// Gets the space-separated <c>id</c> value(s) applied to <c>aria-describedby</c>
        /// on the input element, computed from the current validation state and
        /// <see cref="ValidationDisplayMode"/>.
        /// </summary>
        protected string? AriaDescribedByID { get; private set; } = String.Empty;

        /// <summary>
        /// Gets the resolved accessible label text for the error region, derived from
        /// <see cref="ErrorsLabel"/> or the default value when not set.
        /// </summary>
        protected string  ErrorsText        { get; private set; } = GlobalValues.Default_Errors_label;

        /// <summary>
        /// Gets the label name for the field when set, or the <see cref="Microsoft.AspNetCore.Components.Forms.FieldIdentifier.FieldName"/>
        /// as a fallback.
        /// </summary>
        protected string LabelNameText { get; private set; } = default!;

        /// <summary>
        /// Gets a value indicating whether the error region is rendered as a tabbable
        /// <c>role="region"</c> element. <c>true</c> when <see cref="ValidationDisplayMode"/>
        /// is <see cref="ValidationDisplayMode.TabbableWithHint"/>.
        /// </summary>
        protected bool   TabbableError      { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether <typeparamref name="TValue"/> is a nullable type.
        /// </summary>
        protected bool   IsNullableType     { get; }

        /// <summary>
        /// Gets the underlying non-nullable <see cref="Type"/> of <typeparamref name="TValue"/>,
        /// stripping any nullable wrapper. For example, returns <c>typeof(int)</c> for both
        /// <c>int</c> and <c>int?</c>.
        /// </summary>
        protected Type   DataType           { get; }

        private bool _disposed                  = false;
        private bool _disabledHandlerRegistered = false;


        /// <summary>
        /// Initialises the <see cref="IsNullableType"/> and <see cref="DataType"/> properties
        /// by inspecting <typeparamref name="TValue"/> for a nullable wrapper type.
        /// </summary>
        protected InputTypeBase()
        {
            IsNullableType = Nullable.GetUnderlyingType(typeof(TValue)) != null;
            DataType       = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        }


        /// <summary>
        /// Updates component state on each parameter change, resolving the disabled state,
        /// SVG variable, normalised hint text, <c>aria-describedby</c> value, and tabbable
        /// error flag.
        /// </summary>
        protected override void OnParametersSet()
        {

            IsDisabled = AriaDisabled && !ReadOnly;

            SvgVariable = CheckSetSvgVariable(SvgIcon);

            HintNormalised = String.IsNullOrWhiteSpace(HintText) ? null : (HintText.EndsWith('.') ? HintText : HintText.Trim() + ".");

            AriaDescribedByID = SetDescribedBy(HasErrors,ValidationDisplayMode);

            TabbableError = (ValidationDisplayMode == ValidationDisplayMode.TabbableWithHint);

            base.OnParametersSet();
        }

        private string? SetDescribedBy(bool hasErrors, ValidationDisplayMode validationMode)

            => (hasErrors, validationMode) switch
            {
                (true,  ValidationDisplayMode.DescribedByWithHint)       => $"{ErrorMessageID} {HintTextID}",
                (true,  ValidationDisplayMode.DescribedByHintSuppressed) => ErrorMessageID,
                (true,  ValidationDisplayMode.TabbableWithHint)          => HintTextID,
                (false, ValidationDisplayMode.DescribedByWithHint)       => HintTextID,
                (false, ValidationDisplayMode.DescribedByHintSuppressed) => HintTextID,
                (false, ValidationDisplayMode.TabbableWithHint)          => HintTextID,


                _ => HintTextID 
            };

        /// <summary>
        /// Resolves the input ID, display name text, and errors text from parameters, and
        /// subscribes to <see cref="EditContext.OnValidationStateChanged"/> and
        /// <see cref="EditContext.OnValidationRequested"/>.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized(); 

            InputID          = String.IsNullOrWhiteSpace(ControlID) ? Guid.NewGuid().ToString() : ControlID.Trim();
            LabelNameText  = String.IsNullOrWhiteSpace(LabelText) ? FieldIdentifier.FieldName : LabelText.Trim();
            ErrorsText       = String.IsNullOrWhiteSpace(ErrorsLabel) ? GlobalValues.Default_Errors_label : ErrorsLabel.Trim();

            EditContext.OnValidationStateChanged += EditContext_OnValidationStateChanged;
            EditContext.OnValidationRequested += EditContext_OnValidationRequested;

        }

        private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
        {
            StateIconClasses  = GetStateIconClasses(EditContext.GetValidationMessages(FieldIdentifier).Any());
            InvalidMessages   = GetValidationMessages();
            HasErrors         = InvalidMessages.Count > 0;
            AriaDescribedByID = SetDescribedBy(HasErrors, ValidationDisplayMode);
        }
        private void EditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        
            => OnValidationStateChanged();

        /// <summary>
        /// Called when the <see cref="EditContext"/> raises a validation state change.
        /// Clears validation state if the field is unmodified and has no validation messages,
        /// otherwise updates <see cref="InvalidMessages"/>, <see cref="HasErrors"/>, 
        /// <see cref="StateIconClasses"/>, and <see cref="AriaDescribedByID"/>.
        /// </summary>
        protected virtual void OnValidationStateChanged()
        {
            var hasMessages = EditContext.GetValidationMessages(FieldIdentifier).Any();

            if (!EditContext.IsModified(FieldIdentifier) && !hasMessages)
            {
                InvalidMessages = [];
                HasErrors = false;
                StateIconClasses = GlobalValues.Text_Input_State_Icon_Class;
                AriaDescribedByID = SetDescribedBy(false, ValidationDisplayMode);
                return;
            }

            StateIconClasses = GetStateIconClasses(hasMessages);
            InvalidMessages = hasMessages ? GetValidationMessages() : [];
            HasErrors = InvalidMessages.Count > 0;
            AriaDescribedByID = SetDescribedBy(HasErrors, ValidationDisplayMode);


        }

        /// <summary>
        /// Loads the JavaScript module on first render and registers or unregisters
        /// aria-disabled keyboard handlers as the <see cref="IsDisabled"/> state changes.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) _jSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);

            if (IsDisabled && !_disabledHandlerRegistered)
            {
                await RegisterDisabledHandlers();
                _disabledHandlerRegistered = true;
                return;
            }

            if (!IsDisabled && _disabledHandlerRegistered)
            {
                await UnRegisterDisabledHandlers();
                _disabledHandlerRegistered = false;
            }

        }

        private static string GetStateIconClasses(bool? invalid)
        {
            var classes = GlobalValues.Text_Input_State_Icon_Class;

            return invalid == null ? classes : $"{classes} {(invalid == true ? GlobalValues.Text_Input_State_Icon_Invalid_Modifier : GlobalValues.Text_Input_State_Icon_Valid_Modifier)}";
        }

        private static string? CheckSetSvgVariable(string? svgIcon)
        {
            var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                    ? null
                                    : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

            return iconVariable is null ? null : $"{GlobalValues.Input_Svg_Css_Variable_Name}:{iconVariable};";
        }

        private List<string> GetValidationMessages() 
            
            => [.. EditContext.GetValidationMessages(FieldIdentifier).Where(s => !String.IsNullOrWhiteSpace(s)).Select(s => s.EndsWith('.') ? s : $"{s}.")];

        private async Task RegisterDisabledHandlers()
        {
            if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, ControlReference);
        }
        private async Task UnRegisterDisabledHandlers()
        {
            if (_jSModule is not null)  await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Aria_Disabled_Handlers, ControlReference);
        }


        /// <summary>
        /// Unsubscribes from <see cref="EditContext.OnValidationStateChanged"/> and
        /// <see cref="EditContext.OnValidationRequested"/> to prevent memory leaks
        /// when the component is removed from the render tree.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EditContext.OnValidationStateChanged -= EditContext_OnValidationStateChanged;
                EditContext.OnValidationRequested -= EditContext_OnValidationRequested;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Unregisters aria-disabled JavaScript handlers, disposes the JavaScript module
        /// reference, and calls <see cref="Dispose(bool)"/> to release managed resources.
        /// </summary>
        public virtual async ValueTask DisposeAsync()
        {
            if (_disposed) return;
            
            if (_jSModule is not null)
            {
                try
                {
                    await UnRegisterDisabledHandlers();
                    await _jSModule.DisposeAsync();
                }
                catch { }
            }
            Dispose(true);

            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
