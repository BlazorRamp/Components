using Microsoft.AspNetCore.Components;

namespace BlazorRamp.Core.Services;

internal interface ICoreUtilityService
{
    /// <summary>
    /// Registers a scrollable container so it automatically receives a <c>tabindex="0"</c> whenever it overflows
    /// and has no focusable descendants, and has it removed again once neither condition applies. Works around
    /// Safari not making scrollable regions keyboard-focusable by default.
    /// </summary>
    /// <param name="container">The scrollable element to observe.</param>
    /// <param name="labelName">The accessible name to apply if a role and name need to be added.</param>
    /// <param name="addRoleAndLabel">
    /// When <c>true</c> (the default), a missing <c>role="group"</c> and/or accessible name is added alongside the
    /// tab stop, per WCAG 4.1.2. Existing roles, <c>aria-label</c> or <c>aria-labelledby</c> are never overwritten.
    /// Pass <c>false</c> only when the element already carries its own accessible name by other means.
    /// </param>
    public ValueTask RegisterContainerForAutoTabindex(ElementReference container, string labelName = "Scroll region", bool addRoleAndLabel = true);

    /// <summary>
    /// Unregisters a container previously passed to <see cref="RegisterContainerForAutoTabindex"/>, disconnecting
    /// its observers and restoring any tabindex, role or aria-label that had been added automatically.
    /// </summary>
    /// <param name="container">The element to stop observing.</param>
    public ValueTask UnregisterContainerForAutoTabindex(ElementReference container);
}

