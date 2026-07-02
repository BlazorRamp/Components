const elementFocusInMap = new WeakMap();
const registerElementFocusInHandler = (element, dotNetRef, callbackName) => {
    if (!element)
        return;
    const handler = (e) => {
        if (element.contains(e.relatedTarget))
            return;
        dotNetRef.invokeMethodAsync(callbackName);
    };
    elementFocusInMap.set(element, handler);
    element.addEventListener("focusin", handler);
};
const unregisterElementFocusInHandler = (element) => {
    if (!element)
        return;
    const handler = elementFocusInMap.get(element);
    if (handler) {
        element.removeEventListener("focusin", handler);
        elementFocusInMap.delete(element);
    }
};
export { registerElementFocusInHandler, unregisterElementFocusInHandler };
//# sourceMappingURL=pager.js.map