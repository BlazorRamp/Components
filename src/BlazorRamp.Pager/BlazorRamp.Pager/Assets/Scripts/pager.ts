const elementFocusInMap = new WeakMap<HTMLElement, (e: FocusEvent) => void>();

const registerElementFocusInHandler = (element: HTMLElement, dotNetRef: any, callbackName: string): void => {

    if (!element) return;

    const handler = (e: FocusEvent): void => {

        if (element.contains(e.relatedTarget as Node)) return;

        dotNetRef.invokeMethodAsync(callbackName);
    };

    elementFocusInMap.set(element, handler);

    element.addEventListener("focusin", handler);
};

const unregisterElementFocusInHandler = (element: HTMLElement): void => {

    if (!element) return;

    const handler = elementFocusInMap.get(element);

    if (handler) {
        element.removeEventListener("focusin", handler);
        elementFocusInMap.delete(element);
    }
};

export { registerElementFocusInHandler, unregisterElementFocusInHandler };