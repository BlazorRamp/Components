using BlazorRamp.Inputs.Common.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Runtime.InteropServices;

namespace BlazorRamp.Inputs.Components
{
    public abstract class InputTypeBase<TValue> : InputBase<TValue>, IAsyncDisposable
    {
        [Parameter] public string ControlID     { get; set; } = String.Empty;
        [Parameter] public string HintText      { get; set; } = String.Empty;
        [Parameter] public bool   ReadOnly      { get; set; } = false;
        [Parameter] public bool   AriaDisabled  { get; set; } = false;
        [Parameter] public bool   Required      { get; set; } = true;
        [Parameter] public string ErrorsLabel   { get; set; } = GlobalValues.Default_Errors_label;



        [Parameter] public ValidationDisplayMode ValidationDisplayMode { get; set; } = ValidationDisplayMode.DescribedByWithHint;


        /// <summary>
        /// Gets or sets the name of a CSS custom property that resolves to an SVG data URI,
        /// used as a mask-image icon alongside the tab title. Must begin with <c>--</c>.
        /// For example: <c>--svg-my-icon</c>.
        /// </summary>
        [Parameter] public string? SvgIcon { get; set; } = default;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private IJSObjectReference? _jSModule = null;

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
        protected string  ErrorsText        { get; private set; } = GlobalValues.Default_Errors_label;


        protected string DisplayNameText    { get; private set; }
        protected bool   TabbableError      { get; set; } = false;
        protected bool   IsNullableType     { get; } 
        protected Type   DataType           { get; }

        private bool _disposed                  = false;
        private bool _disabledHandlerRegistered = false;

        protected InputTypeBase()
        {
            IsNullableType = Nullable.GetUnderlyingType(typeof(TValue)) != null;
            DataType       = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        }

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


        protected override void OnInitialized()
        {
            base.OnInitialized(); 

            InputID         = String.IsNullOrWhiteSpace(ControlID) ? Guid.NewGuid().ToString() : ControlID.Trim();
            DisplayNameText       = String.IsNullOrWhiteSpace(DisplayName) ? FieldIdentifier.FieldName : DisplayName.Trim();
            ErrorsText      = String.IsNullOrWhiteSpace(ErrorsLabel) ? GlobalValues.Default_Errors_label : ErrorsLabel.Trim();

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
        
        protected virtual void OnValidationStateChanged()
        {
            if (!EditContext.IsModified(FieldIdentifier))
            {
                InvalidMessages = [];
                HasErrors = false;
                StateIconClasses = GlobalValues.Text_Input_State_Icon_Class;
                AriaDescribedByID = SetDescribedBy(false, ValidationDisplayMode);
                return;
            }

            var hasMessages   = EditContext.GetValidationMessages(FieldIdentifier).Any();
            StateIconClasses  = GetStateIconClasses(hasMessages);
            InvalidMessages   = hasMessages ? GetValidationMessages() : [];
            HasErrors         = InvalidMessages.Count > 0;
            AriaDescribedByID = SetDescribedBy(HasErrors, ValidationDisplayMode);
        }

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

            return iconVariable is null ? null : $"{GlobalValues.Text_Input_Svg_Css_Variable_Name}:{iconVariable};";
        }

        private List<string> GetValidationMessages() 
            
            => EditContext.GetValidationMessages(FieldIdentifier).Where(s => !String.IsNullOrWhiteSpace(s)).Select(s => s.EndsWith('.') ? s : $"{s}.").ToList();

        private async Task RegisterDisabledHandlers()
        {
            if (_jSModule is not null) await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Register_Aria_Disabled_Handlers, ControlReference);
        }
        private async Task UnRegisterDisabledHandlers()
        {
            if (_jSModule is not null)  await _jSModule.InvokeVoidAsync(GlobalValues.JS_Inputs_Unregister_Aria_Disabled_Handlers, ControlReference);
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
