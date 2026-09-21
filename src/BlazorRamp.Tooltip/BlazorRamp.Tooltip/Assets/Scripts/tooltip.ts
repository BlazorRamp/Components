interface TooltipHandlers {
    mouseEnterHandler: EventListener;
    mouseLeaveHandler: EventListener;
    focusInHandler: EventListener;
    focusOutHandler: EventListener;
}

const _tooltipRegistry = new WeakMap<HTMLElement, TooltipHandlers>();
const _openTooltips = new Set<HTMLElement>();
let _escapeHandlerRegistered = false;

const closeOpenTooltips = (): void => {

    _openTooltips.forEach(tooltip => tooltip.hidePopover());
    _openTooltips.clear();
};

const showTooltip = (containerElement: HTMLElement, tooltipElement: HTMLElement): void => {

    closeOpenTooltips();
    tooltipElement.showPopover();
    _openTooltips.add(tooltipElement);
};

const hideTooltip = (tooltipElement: HTMLElement): void => {

    tooltipElement.hidePopover();
    _openTooltips.delete(tooltipElement);
};

const registerEscapeHandler = (): void => {

    if (_escapeHandlerRegistered) return;

    document.addEventListener("keydown", (event: KeyboardEvent) => {

        if (event.key !== "Escape" || _openTooltips.size === 0) return;

        closeOpenTooltips();
        event.preventDefault(); // stop this same Escape also reaching the dialog's own close watcher
    });

    _escapeHandlerRegistered = true;
};

const removeTooltipListeners = (containerElement: HTMLElement): void => {

    const existingHandlers = _tooltipRegistry.get(containerElement);

    if (!existingHandlers) return;

    containerElement.removeEventListener("mouseenter", existingHandlers.mouseEnterHandler);
    containerElement.removeEventListener("mouseleave", existingHandlers.mouseLeaveHandler);
    containerElement.removeEventListener("focusin", existingHandlers.focusInHandler);
    containerElement.removeEventListener("focusout", existingHandlers.focusOutHandler);

    _tooltipRegistry.delete(containerElement);
};

const registerTooltip = (containerId: string, tooltipId: string): void => {

    const containerElement = document.getElementById(containerId) as HTMLElement;
    const tooltipElement = document.getElementById(tooltipId) as HTMLElement;

    if (!containerElement || !tooltipElement) return;

    removeTooltipListeners(containerElement);

    const mouseEnterHandler: EventListener = () => showTooltip(containerElement, tooltipElement);
    const mouseLeaveHandler: EventListener = () => hideTooltip(tooltipElement);
    const focusInHandler: EventListener = () => showTooltip(containerElement, tooltipElement);
    const focusOutHandler: EventListener = () => hideTooltip(tooltipElement);

    containerElement.addEventListener("mouseenter", mouseEnterHandler);
    containerElement.addEventListener("mouseleave", mouseLeaveHandler);
    containerElement.addEventListener("focusin", focusInHandler);
    containerElement.addEventListener("focusout", focusOutHandler);

    _tooltipRegistry.set(containerElement, { mouseEnterHandler, mouseLeaveHandler, focusInHandler, focusOutHandler });

    registerEscapeHandler();
};

const unregisterTooltip = (containerId: string): void => {

    const containerElement = document.getElementById(containerId) as HTMLElement;

    if (!containerElement) return;

    removeTooltipListeners(containerElement);
};

export { registerTooltip, unregisterTooltip };