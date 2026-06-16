using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRamp.DebounceFilter.Common.Models;

/// <summary>
/// Holds the configuration passed to the JavaScript debounce handler on registration,
/// including the .NET callback reference, element references, delay, and optional validation settings.
/// </summary>
/// <typeparam name="TValue">The type of the .NET callback reference, typically a <see cref="DotNetObjectReference{T}"/>.</typeparam>
/// <param name="BlazorCallBackRef">The .NET object reference used to invoke the managed callback from JavaScript.</param>
/// <param name="CallBackName">The name of the .NET method to invoke on the callback reference.</param>
/// <param name="MessageElement">A reference to the element used to display validation messages.</param>
/// <param name="StateIconElement">A reference to the element used to display the valid/invalid state icon.</param>
/// <param name="DelayMs">The debounce delay in milliseconds before the callback is invoked.</param>
/// <param name="SystemErrorMessage">The message displayed when the regex pattern fails to compile.</param>
/// <param name="RegexPattern">The optional regular expression pattern used to validate input.</param>
/// <param name="ValidationMessage">The optional message displayed when the regex pattern is not matched.</param>

public record DebounceConfiguration<TValue>(TValue  BlazorCallBackRef, string CallBackName, ElementReference MessageElement, ElementReference StateIconElement, 
                                            int DelayMs, string SystemErrorMessage, string? RegexPattern = null, string? ValidationMessage = null);

/// <summary>
/// Represents the result raised after each debounced input event, containing the current
/// filter value, validity state, and any exception details from regex evaluation.
/// </summary>
/// <param name="FilterValue">The current value of the filter input at the time the debounce fired.</param>
/// <param name="IsValid">Indicates whether the input passed regex validation, or <c>true</c> if no pattern is configured.</param>
/// <param name="ClearCalled">Indicates whether the result was raised as a result of the filter being programmatically cleared.</param>
/// <param name="ExceptionMessage">The exception message if the regex pattern failed to compile, otherwise <c>null</c>.</param>

public record DebouncedFilterResult(string FilterValue, bool IsValid, bool ClearCalled, string? ExceptionMessage = null);