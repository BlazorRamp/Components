const _tooltipRegistry = new WeakMap();
const _openTooltips = new Set();
let _escapeHandlerRegistered = false;
const closeOpenTooltips = () => {
    _openTooltips.forEach(tooltip => tooltip.hidePopover());
    _openTooltips.clear();
};
const showTooltip = (tooltipElement) => {
    closeOpenTooltips();
    tooltipElement.showPopover();
    _openTooltips.add(tooltipElement);
};
const hideTooltip = (tooltipElement) => {
    tooltipElement.hidePopover();
    _openTooltips.delete(tooltipElement);
};
const registerEscapeHandler = () => {
    if (_escapeHandlerRegistered)
        return;
    document.addEventListener("keydown", (event) => {
        if (event.key !== "Escape" || _openTooltips.size === 0)
            return;
        closeOpenTooltips();
        event.preventDefault();
    });
    _escapeHandlerRegistered = true;
};
const removeTooltipListeners = (containerElement) => {
    const existingHandlers = _tooltipRegistry.get(containerElement);
    if (!existingHandlers)
        return;
    containerElement.removeEventListener("mouseenter", existingHandlers.mouseEnterHandler);
    containerElement.removeEventListener("mouseleave", existingHandlers.mouseLeaveHandler);
    containerElement.removeEventListener("focusin", existingHandlers.focusInHandler);
    containerElement.removeEventListener("focusout", existingHandlers.focusOutHandler);
    _tooltipRegistry.delete(containerElement);
};
const registerTooltip = (containerId, tooltipId) => {
    const containerElement = document.getElementById(containerId);
    const tooltipElement = document.getElementById(tooltipId);
    if (!containerElement || !tooltipElement)
        return;
    removeTooltipListeners(containerElement);
    // Per-tooltip state — has to live here, not at module level, since each
    // tooltip on the page needs its own independent pointer/focus tracking.
    let isPointerOver = false;
    let isFocused = false;
    const hideIfNeitherActive = () => {
        if (isPointerOver || isFocused)
            return;
        hideTooltip(tooltipElement);
    };
    const mouseEnterHandler = () => { isPointerOver = true; showTooltip(tooltipElement); };
    const mouseLeaveHandler = () => { isPointerOver = false; hideIfNeitherActive(); };
    const focusInHandler = () => { isFocused = true; showTooltip(tooltipElement); };
    const focusOutHandler = () => { isFocused = false; hideIfNeitherActive(); };
    containerElement.addEventListener("mouseenter", mouseEnterHandler);
    containerElement.addEventListener("mouseleave", mouseLeaveHandler);
    containerElement.addEventListener("focusin", focusInHandler);
    containerElement.addEventListener("focusout", focusOutHandler);
    _tooltipRegistry.set(containerElement, { mouseEnterHandler, mouseLeaveHandler, focusInHandler, focusOutHandler });
    registerEscapeHandler();
};
const unregisterTooltip = (containerId) => {
    const containerElement = document.getElementById(containerId);
    if (!containerElement)
        return;
    removeTooltipListeners(containerElement);
};
export { registerTooltip, unregisterTooltip, closeOpenTooltips };
//# sourceMappingURL=tooltip.js.map