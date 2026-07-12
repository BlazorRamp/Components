const elementFocusOutMap = new WeakMap<HTMLElement, (e: FocusEvent) => void>();

const preventClickAction = (e: MouseEvent): void => e.preventDefault();


const hidePopover = (elementId:string): void => {

    const popoverElement = document.getElementById(elementId) as HTMLElement;

    if (popoverElement) popoverElement.hidePopover();
};

const registerFocusOutHandler = (parentElement: HTMLElement, popoverElement: HTMLElement): void => {

    if (!parentElement || !popoverElement) return;

    const handler = (e: FocusEvent): void => {

        if (!parentElement.contains(e.relatedTarget as Node)) {
            popoverElement.hidePopover();
            return;
        }
    };
    
    elementFocusOutMap.set(parentElement, handler);

    parentElement.addEventListener("focusout", handler);
};

const unregisterFocusOutHandler = (parentElement: HTMLElement, popoverElement: HTMLElement): void => {

    if (!parentElement || !popoverElement) return;

    const handler = elementFocusOutMap.get(parentElement);

    if (handler) {
        parentElement.removeEventListener("focusout", handler);
        elementFocusOutMap.delete(parentElement);
    }
};

const registerPreventClickAction = (anchorElement: HTMLAnchorElement): void => {

    if (!anchorElement) return;

    unregisterPreventClickAction(anchorElement);
    anchorElement.addEventListener("click", preventClickAction);
};

const unregisterPreventClickAction = (anchorElement: HTMLAnchorElement): void => {
    if (!anchorElement) return;
    anchorElement.removeEventListener("click", preventClickAction);
};

export { registerFocusOutHandler, unregisterFocusOutHandler, registerPreventClickAction, unregisterPreventClickAction, hidePopover };