const elementFocusOutMap = new WeakMap();
const preventClickAction = (e) => e.preventDefault();
const hidePopover = (elementId) => {
    const popoverElement = document.getElementById(elementId);
    if (popoverElement)
        popoverElement.hidePopover();
};
const registerFocusOutHandler = (parentElement, popoverElement) => {
    if (!parentElement || !popoverElement)
        return;
    const handler = (e) => {
        if (!parentElement.contains(e.relatedTarget)) {
            popoverElement.hidePopover();
            return;
        }
    };
    elementFocusOutMap.set(parentElement, handler);
    parentElement.addEventListener("focusout", handler);
};
const unregisterFocusOutHandler = (parentElement, popoverElement) => {
    if (!parentElement || !popoverElement)
        return;
    const handler = elementFocusOutMap.get(parentElement);
    if (handler) {
        parentElement.removeEventListener("focusout", handler);
        elementFocusOutMap.delete(parentElement);
    }
};
const registerPreventClickAction = (anchorElement) => {
    if (!anchorElement)
        return;
    unregisterPreventClickAction(anchorElement);
    anchorElement.addEventListener("click", preventClickAction);
};
const unregisterPreventClickAction = (anchorElement) => {
    if (!anchorElement)
        return;
    anchorElement.removeEventListener("click", preventClickAction);
};
export { registerFocusOutHandler, unregisterFocusOutHandler, registerPreventClickAction, unregisterPreventClickAction, hidePopover };
//# sourceMappingURL=actions-popover.js.map