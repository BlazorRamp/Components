const _tabFocusElements = 'a[href], area[href], input:not([disabled]):not([type="hidden"]),select:not([disabled]), textarea:not([disabled]),button:not([disabled]),iframe, object,embed, [contenteditable],[tabindex]:not([tabindex="-1"]),audio[controls],video[controls],summay';
const ANNOUNCEMENT_COMPONENTS_ID = "blazor-ramp-announcement-history-components";
const BS_COMPONENT_NAME = "Busy Indicator";
const AH_COMPONENT_NAME = "Announcement History";
const INERT_ATTRIBUTE = "inert";
const BUSY_INDICATOR_TIMEOUT = 30000;
const _indicatorMap = new WeakMap();
const _ahCompsElement = document.getElementById(ANNOUNCEMENT_COMPONENTS_ID);
const _ahCompsOriginalParent = _ahCompsElement?.parentElement ?? null;
/*
    * To simplify things I moved the live regions into the announcment history container so everything in this container does not
    * get the inert attribute added. I also chack and move everything to the body element.
*/
const getSetInertElements = (busyElement) => {
    const inertElements = [];
    if (!busyElement || !busyElement.parentElement)
        return inertElements;
    Array.from(busyElement.parentElement.children).forEach(child => {
        const tagName = child.tagName.toLowerCase();
        if (child instanceof HTMLElement
            && child !== busyElement
            && !child.hasAttribute(INERT_ATTRIBUTE)
            && !child.getAttribute("aria-live")
            && child.getAttribute("data-br-component") !== BS_COMPONENT_NAME
            && child.id !== ANNOUNCEMENT_COMPONENTS_ID
            && child.getAttribute("data-br-component") !== AH_COMPONENT_NAME
            && child.getAttribute("role") !== "alert" && tagName !== "script") {
            child.setAttribute(INERT_ATTRIBUTE, "true");
            inertElements.push(child);
        }
    });
    return inertElements;
};
const checkIsInsideDialog = (busyElement) => {
    if (!busyElement)
        return [null, false];
    const dialogTagName = "dialog";
    let parentElement = busyElement.parentElement;
    while (parentElement) {
        if (parentElement.tagName.toLowerCase() === dialogTagName) {
            return [parentElement, true];
        }
        parentElement = parentElement.parentElement;
    }
    return [null, false];
};
const getParentElement = (busyElement, overlay) => {
    const [dialogElement, inDialog] = checkIsInsideDialog(busyElement);
    if (overlay.toLowerCase() === "container")
        return [busyElement.parentElement, inDialog];
    if (dialogElement)
        return [dialogElement, inDialog];
    return [document.body, false];
};
const startBusyIndicator = (busyElement, displayModifier, timeout = BUSY_INDICATOR_TIMEOUT, overlay = "container") => {
    if (!busyElement || !busyElement.parentElement)
        return;
    let [targetParent, inDialog] = getParentElement(busyElement, overlay);
    targetParent = targetParent ?? busyElement.parentElement; //should be no nulls here but keep typscript happy
    let indicatorData = _indicatorMap.get(busyElement);
    if (!indicatorData) {
        indicatorData = { element: busyElement, originalParent: busyElement.parentElement, displayModifier: displayModifier, inModalDialog: inDialog };
        _indicatorMap.set(busyElement, indicatorData);
    }
    const { element, displayModifier: modifier } = indicatorData;
    if (!element.classList.contains(modifier))
        element.classList.add(modifier);
    if (element.parentElement !== targetParent)
        targetParent.appendChild(element);
    indicatorData.activatingElement = document?.activeElement;
    element.firstElementChild.focus();
    indicatorData.inertElements = getSetInertElements(element);
    if (indicatorData.timerId)
        clearTimeout(indicatorData.timerId);
    const timerId = setTimeout(() => {
        stopBusyIndicator(element);
    }, timeout);
    indicatorData.timerId = timerId;
};
const stopBusyIndicator = (busyElement) => {
    const indicatorData = _indicatorMap.get(busyElement);
    if (!indicatorData)
        return;
    const { element, originalParent, timerId, displayModifier, inertElements, inModalDialog, activatingElement } = indicatorData;
    if (timerId) {
        clearTimeout(timerId);
        indicatorData.timerId = undefined;
    }
    inertElements?.forEach(inertElement => inertElement.removeAttribute(INERT_ATTRIBUTE));
    indicatorData.inertElements = undefined;
    if (element.parentElement !== originalParent) {
        originalParent.appendChild(element);
    }
    if (activatingElement && activatingElement.isConnected && !activatingElement.hasAttribute('disabled') && activatingElement.getAttribute('aria-disabled') !== 'true') {
        const currentActiveElement = document.activeElement;
        if (currentActiveElement === activatingElement || currentActiveElement == document.body
            || currentActiveElement === busyElement.firstElementChild)
            activatingElement.focus();
    }
    element.classList.remove(displayModifier);
    _indicatorMap.delete(busyElement);
};
export { startBusyIndicator, stopBusyIndicator };
//# sourceMappingURL=busy-indicator.js.map