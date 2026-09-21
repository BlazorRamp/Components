const _tooltipRegistry = new WeakMap();
const _openTooltips = new Set();
let _escapeHandlerRegistered = false;
const closeOpenTooltips = () => {
    _openTooltips.forEach(tooltip => tooltip.hidePopover());
    _openTooltips.clear();
};
const showTooltip = (containerElement, tooltipElement) => {
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
        event.preventDefault(); // stop this same Escape also reaching the dialog's own close watcher
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
    const mouseEnterHandler = () => showTooltip(containerElement, tooltipElement);
    const mouseLeaveHandler = () => hideTooltip(tooltipElement);
    const focusInHandler = () => showTooltip(containerElement, tooltipElement);
    const focusOutHandler = () => hideTooltip(tooltipElement);
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
export { registerTooltip, unregisterTooltip };
//# sourceMappingURL=tooltip.js.map