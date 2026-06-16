using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DebounceFilter.Common.Models;


public record DebounceConfiguration<TValue>(TValue  BlazorCallBackRef, string CallBackName, ElementReference MessageElement, ElementReference StateIconElement, int DelayMs, string SystemErrorMessage, string? RegexPattern = null, string? ValidationMessage = null);

public record DebouncedFilterResult(string FilterValue, bool IsValid, bool ClearCalled, string? ExceptionMessage = null);