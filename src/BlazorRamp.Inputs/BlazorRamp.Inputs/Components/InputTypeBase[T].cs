using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace BlazorRamp.Inputs.Components
{
    public abstract class InputTypeBase<TValue> : InputBase<TValue>, IAsyncDisposable
    {
        [Parameter] public string LabelText     { get; set; } = String.Empty;
        [Parameter] public string ControlID     { get; set; } = String.Empty;
        [Parameter] public string HintText      { get; set; } = String.Empty;
        [Parameter] public bool   ReadOnly      { get; set; } = false;
        [Parameter] public bool   AriaDisabled  { get; set; } = false;
        [Parameter] public bool   Required      { get; set; } = true;
        [Parameter] public string ErrorsLabel   { get; set; } = GlobalValues.Default_Errors_label;

        [Parameter] public ValidationDisplayMode ValidationDisplayMode { get; set; } = ValidationDisplayMode.DescribedbyWithHint;


        /// <summary>
        /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
        /// used as a mask-image icon alongside the tab title. Must begin with <c>--</c>.
        /// For example: <c>--svg-my-icon</c>.
        /// </summary>
        [Parameter] public string? SvgIcon { get; set; } = default;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private IJSObjectReference? _jsModule = null;
        public ElementReference  ControlReference { get; set; }

        protected List<string> InvalidMessages  { get; private set; } = [];
        protected string  InputID           { get; private set; } = string.Empty;
        protected string  StateIconClasses  { get; private set; } = GlobalValues.Text_Input_State_Icon_Class;
        protected bool    IsDisabled        { get; private set; } = false;
        protected string  HintTextID        { get; } = Guid.NewGuid().ToString();
        protected string? HintNormalised    { get; private set; } = default;
        protected string  ErrorMessageID    { get; } = Guid.NewGuid().ToString();
        protected string? SvgVariable       { get; private set; } = null;
        protected bool    HasErrors         { get; private set; } = false;
        protected string? AriaDescribedByID { get; private set; } = String.Empty;
        protected string  ErrorsText        { get; set; } = GlobalValues.Default_Errors_label;
        protected  bool   TabbableError     { get; set; } = false;
        protected Dictionary<string, object> MutableAttributes { get; private set; } = [];

        private bool _disposed                  = false;
        private bool _disabledHandlerRegistered = false;

        protected override void OnParametersSet()
        {
            MutableAttributes = AdditionalAttributes?.ToDictionary(k => k.Key, v => v.Value) ?? [];

            IsDisabled = AriaDisabled && !ReadOnly;

            SvgVariable = CheckSetSvgVariable(SvgIcon);

            HintNormalised = String.IsNullOrWhiteSpace(HintText) ? null : (HintText.EndsWith('.') ? HintText : HintText.Trim() + ".");

            AriaDescribedByID = SetDescribedby(HasErrors,ValidationDisplayMode);

            TabbableError = (ValidationDisplayMode == ValidationDisplayMode.TabbableWithHint || ValidationDisplayMode == ValidationDisplayMode.TabbableHintSuppressed);

            base.OnParametersSet();
        }

        private string? SetDescribedby(bool hasErrors, ValidationDisplayMode validationMode)

            => (hasErrors, validationMode) switch
            {
                (true,  ValidationDisplayMode.DescribedbyWithHint)       => $"{ErrorMessageID} {HintTextID}",
                (true,  ValidationDisplayMode.DescribedbyHintSuppressed) => ErrorMessageID,
                (true,  ValidationDisplayMode.TabbableWithHint)          => HintTextID,
                (true,  ValidationDisplayMode.TabbableHintSuppressed)    => null,
                (false, ValidationDisplayMode.DescribedbyWithHint)       => HintTextID,
                (false, ValidationDisplayMode.DescribedbyHintSuppressed) => HintTextID,
                (false, ValidationDisplayMode.TabbableWithHint)          => HintTextID,
                (false, ValidationDisplayMode.TabbableHintSuppressed)    => HintTextID,

                _ => HintTextID 
            };

        
        protected override void OnInitialized()
        {
            base.OnInitialized(); 

            InputID   = String.IsNullOrWhiteSpace(ControlID) ? Guid.NewGuid().ToString() : ControlID;
            LabelText = String.IsNullOrWhiteSpace(LabelText) ? FieldIdentifier.FieldName : LabelText;
            ErrorsText = String.IsNullOrWhiteSpace(ErrorsLabel) ? GlobalValues.Default_Errors_label : ErrorsLabel.Trim();


            EditContext.OnValidationStateChanged += EditContext_OnValidationStateChanged;
            EditContext.OnValidationRequested += EditContext_OnValidationRequested;

        }

        private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
        {
            StateIconClasses  = GetStateIconClasses(EditContext.GetValidationMessages(FieldIdentifier).Any());
            InvalidMessages   = GetValidationMessages();
            HasErrors         = InvalidMessages.Count > 0;
            AriaDescribedByID = SetDescribedby(HasErrors, ValidationDisplayMode);
        }
        private void EditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        
            => OnValidationStateChanged();
        
        protected virtual void OnValidationStateChanged()
        {
            if(!EditContext.IsModified(FieldIdentifier)) return;

            var hasMessages   = EditContext.GetValidationMessages(FieldIdentifier).Any();
            StateIconClasses  = GetStateIconClasses(hasMessages);
            InvalidMessages   = hasMessages ? GetValidationMessages() : [];
            HasErrors         = InvalidMessages.Count > 0;
            AriaDescribedByID = SetDescribedby(HasErrors, ValidationDisplayMode);
        }
        public override Task SetParametersAsync(ParameterView parameters)

            => base.SetParametersAsync(parameters);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", GlobalValues.JS_Inputs_File_Path);

            if (IsDisabled && !_disabledHandlerRegistered)
            {
                await RegisterDisabledHandlers();
                _disabledHandlerRegistered = true;
                return;
            }

            if (!IsDisabled && _disabledHandlerRegistered)
            {
                await RegisterDisabledHandlers();
                _disabledHandlerRegistered = false;
            }
                
        }

        private string GetStateIconClasses(bool? invalid)
        {
            var classes = GlobalValues.Text_Input_State_Icon_Class;

            return invalid == null ? classes : $"{classes} {(invalid == true ? GlobalValues.Text_Input_State_Icon_Invalid_Modifier : GlobalValues.Text_Input_State_Icon_Valid_Modifier)}";
        }

        internal string? CheckSetSvgVariable(string? svgIcon)
        {
            var iconVariable = String.IsNullOrWhiteSpace(svgIcon)
                                    ? null
                                    : svgIcon.TrimStart().StartsWith("--") ? $"var({svgIcon!.Trim().TrimEnd(':')})" : null;

            return iconVariable is null ? null : $"{GlobalValues.Text_Input_Svg_Css_Variable_Name}:{iconVariable};";
        }

        private List<string> GetValidationMessages() => EditContext.GetValidationMessages(FieldIdentifier).Select(s => s.EndsWith('.') ? s : $"{s}.").ToList();

        private async Task RegisterDisabledHandlers()
        {
            if (_jsModule is not null) await _jsModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, ControlReference);
        }
        private async Task UnRegisterDisabledHandlers()
        {
            if (_jsModule is not null)  await _jsModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Aria_Disabled_Handlers, ControlReference);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EditContext.OnValidationStateChanged -= EditContext_OnValidationStateChanged;
                EditContext.OnValidationRequested -= EditContext_OnValidationRequested;
            }

            base.Dispose(disposing);
        }

        public async ValueTask DisposeAsync()
        {
            if (_jsModule == null || true == _disposed) return;

            try
            {
                await UnRegisterDisabledHandlers();
                await _jsModule.DisposeAsync();
            }
            catch { }

            Dispose(true);

            GC.SuppressFinalize(this);

        }
    }
}
