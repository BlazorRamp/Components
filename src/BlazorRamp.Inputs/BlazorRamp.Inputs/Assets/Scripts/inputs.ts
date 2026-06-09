const elementFocusOutMap = new WeakMap<HTMLElement, (e: FocusEvent) => void>();
const textAreaCounterMap = new WeakMap<HTMLTextAreaElement, () => void>();

const TIME_INPUT_COMPONENT_NAME = "TimeInput";
const DATE_INPUT_COMPONENT_NAME = "DateInput";
const TEXTAREA_INPUT_COMPONENT_NAME = "TextAreaInput";

const getDecimalSeparator = (): string => Intl.NumberFormat(navigator.language).format(1.1).charAt(1);

const preventClickAction = (e: MouseEvent): void => e.preventDefault();
const preventAction = (e) => e.preventDefault();

const ariaDisabledKeyHandler = (e: KeyboardEvent): void => {

    const navigationKeys = ["Tab", "Enter", "Escape", "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Home", "End", "PageUp", "PageDown"];

    const isNavigation = navigationKeys.includes(e.key);
    const isCopy = (e.ctrlKey || e.metaKey) && e.key.toLowerCase() === "c";

    if (isNavigation || isCopy) return;

    e.preventDefault();
};

const integerHandler = (e: Event): void => {

    const input = e.target as HTMLInputElement;

    let cleaned = input.value.replace(/[^0-9\-]/g, '');

    cleaned = cleaned.replace(/(?!^)-/g, '');

    if (input.value !== cleaned) input.value = cleaned;
};

const decimalHandler = (e: Event): void => {

    const input = e.target as HTMLInputElement;
    const separator = getDecimalSeparator();

    const escapedSeparator = separator === '.' ? '\\.' : separator;

    let cleaned = input.value.replace(new RegExp(`[^0-9\\-${escapedSeparator}]`, 'g'), '');

    cleaned = cleaned.replace(/(?!^)-/g, '');

    const parts = cleaned.split(separator);

    if (parts.length > 2) cleaned = parts[0] + separator + parts.slice(1).join('');

    if (input.value !== cleaned) input.value = cleaned;
};

const timeSegmentHandler = (e:Event): void => {

    const input = e.target as HTMLInputElement;

    let cleaned = input.value.replace(/[^0-9]/g, '');
    if (input.value !== cleaned) input.value = cleaned;
};

const dateSegmentHandler = (e: Event): void => {

    const input = e.target as HTMLInputElement;

    let cleaned = input.value.replace(/[^0-9]/g, '');
    if (input.value !== cleaned) input.value = cleaned;
};

const setInputValue = (inputElement: HTMLElement, value: string): void => {

    if (!inputElement) return;

    (inputElement as HTMLInputElement).value = value;
};

const setInputFocus = (elementId: string): void => {

    const element = document.getElementById(elementId) as HTMLInputElement;

    if (!element) return;

    const prefersReducedMotion: boolean = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    element.scrollIntoView({
        behavior: prefersReducedMotion ? "auto" : "smooth",
        block: "nearest",
        inline: "nearest"
    });

    if (element.getAttribute("data-br-component") === TIME_INPUT_COMPONENT_NAME || element.getAttribute("data-br-component") === DATE_INPUT_COMPONENT_NAME){

        const input = element.querySelector("input") as HTMLInputElement;

        if (input) {
            input.focus({ preventScroll: true });
            return;
        }
    }

    if (element.getAttribute('role') === 'radiogroup') {
        const firstRadio = element.querySelector('input[type="radio"]') as HTMLInputElement;
        if (firstRadio) {
            firstRadio.focus({ preventScroll: true });
            firstRadio.addEventListener("blur", () => firstRadio.removeAttribute("data-br-focused"), { once: true });
        }
        return;
    }

    if (element instanceof HTMLTextAreaElement) {
        element.focus({ preventScroll: true });
        try {
            if (element.value) element.setSelectionRange(element.value.length, element.value.length);
        } catch { }
        return;
    }

    element.focus({ preventScroll: true });

    switch (element.type) {
        case "text":
        case "password":
        case "email":
        case "tel":
        case "url":
        case "search":
            try {
                if (element.value) element.setSelectionRange(element.value.length, element.value.length);
            } catch { }
            break;
    }
};

const setSummaryFocus = (elementId: string): void => {

    const element = document.getElementById(elementId) as HTMLElement;

    if (!element) return;
    element.setAttribute("tabindex", "-1");
    element.focus();

    element.addEventListener("blur", () => element.removeAttribute("tabindex"), { once: true });

};

const formatDateForAnnouncement = (dateString: string): string => {

    try {
        const [year, month, day] = dateString.split('-').map(Number);
        const date = new Date(year, month - 1, day);

        return new Intl.DateTimeFormat(navigator.language, { dateStyle: 'long' }).format(date);

    } catch {
        return '';
    }
};

const registerAriaDisabledHandlers = (inputElement: HTMLElement): void => {

    if (!inputElement) return;

    if (inputElement.getAttribute('role') === 'group') {
        const inputs = inputElement.querySelectorAll('input');
       
        inputs.forEach(input => {
            input.removeEventListener("keydown", ariaDisabledKeyHandler);
            input.addEventListener("keydown", ariaDisabledKeyHandler);
            input.removeEventListener("paste", preventAction);
            input.addEventListener("paste", preventAction);
            input.removeEventListener("cut", preventAction);
            input.addEventListener("cut", preventAction);
        });
        return;
    }


    if ((inputElement as HTMLInputElement).type === "checkbox") {
        inputElement.removeEventListener("click", preventClickAction);
        inputElement.addEventListener("click", preventClickAction);
    }

    inputElement.removeEventListener("keydown", ariaDisabledKeyHandler);
    inputElement.addEventListener("keydown", ariaDisabledKeyHandler);

    inputElement.removeEventListener("paste", preventAction);
    inputElement.addEventListener("paste", preventAction);

    inputElement.removeEventListener("cut", preventAction);
    inputElement.addEventListener("cut", preventAction);
};

const unregisterAriaDisabledHandlers = (inputElement: HTMLElement): void => {

    if (!inputElement) return;

    if (inputElement.getAttribute('role') === 'group') {
        const inputs = inputElement.querySelectorAll('input');

        inputs.forEach(input => {
            input.removeEventListener("keydown", ariaDisabledKeyHandler);
            input.removeEventListener("paste", preventAction);
            input.removeEventListener("cut", preventAction);
        });
        return;
    }


    inputElement.removeEventListener("keydown", ariaDisabledKeyHandler);
    inputElement.removeEventListener("cut", preventAction);
    inputElement.removeEventListener("paste", preventAction);
    inputElement.removeEventListener("click", preventClickAction);
};

const selectReadOnlyKeyHandler = (e: KeyboardEvent): void => {
    const blockedKeys = [" ", "ArrowUp", "ArrowDown", "Enter", "F4"];
    if (blockedKeys.includes(e.key)) e.preventDefault();
};

const registerReadOnlyHandlers = (inputElement: HTMLElement): void => {


    if (!inputElement) return;
    inputElement.removeEventListener("click", preventClickAction);
    inputElement.addEventListener("click", preventClickAction);
};

const unregisterReadOnlyHandlers = (inputElement: HTMLElement): void => {

    if (!inputElement) return;
    inputElement.removeEventListener("click", preventClickAction);
};

const registerNumericHandlers = (inputElement: HTMLElement, isWholeNumber: boolean): void => {

    if (!inputElement) return;

    const handler = isWholeNumber ? integerHandler : decimalHandler;

    inputElement.removeEventListener("input", handler);
    inputElement.addEventListener("input", handler);

};

const unregisterNumericHandlers = (inputElement: HTMLElement, isWholeNumber: boolean): void => {

    if (!inputElement) return;

    const handler = isWholeNumber ? integerHandler : decimalHandler;
    inputElement.removeEventListener("input", handler);
};

const registerSelectReadOnlyDisabledHandlers = (inputElement: HTMLElement): void => {

    if (!inputElement) return;

    inputElement.removeEventListener("mousedown", preventAction);
    inputElement.addEventListener("mousedown", preventAction);
    inputElement.removeEventListener("keydown", selectReadOnlyKeyHandler);
    inputElement.addEventListener("keydown", selectReadOnlyKeyHandler);
};


const unregisterSelectReadOnlyDisabledHandlers = (inputElement: HTMLElement): void => {

    if (!inputElement) return;

    inputElement.removeEventListener("mousedown", preventAction);
    inputElement.removeEventListener("keydown", selectReadOnlyKeyHandler);

};

const registerTimeSegmentHandlers = (hoursElement: HTMLElement, minutesElement: HTMLElement, secondsElement: HTMLElement | null): void => {
    if (!hoursElement || !minutesElement) return;

    hoursElement.addEventListener("input", timeSegmentHandler);
    minutesElement.addEventListener("input", timeSegmentHandler);

    if (secondsElement) {
        secondsElement.addEventListener("input", timeSegmentHandler);
    }
};

const unregisterTimeSegmentHandlers = (hoursElement: HTMLElement, minutesElement: HTMLElement, secondsElement: HTMLElement | null): void => {
    if (!hoursElement || !minutesElement) return;

    hoursElement.removeEventListener("input", timeSegmentHandler);
    minutesElement.removeEventListener("input", timeSegmentHandler);

    if (secondsElement) {
        secondsElement.removeEventListener("input", timeSegmentHandler);
    }
};

const registerDateSegmentHandlers = (yearsElement: HTMLElement, monthsElement: HTMLElement, daysElement: HTMLElement): void => {
    if (!yearsElement || !monthsElement || !daysElement) return;

    yearsElement.addEventListener("input", dateSegmentHandler);
    monthsElement.addEventListener("input", dateSegmentHandler);
    daysElement.addEventListener("input", dateSegmentHandler);
};

const unregisterDateSegmentHandlers = (yearsElement: HTMLElement, monthsElement: HTMLElement, daysElement: HTMLElement): void => {
    if (!yearsElement || !monthsElement || !daysElement) return;

    yearsElement.removeEventListener("input", dateSegmentHandler);
    monthsElement.removeEventListener("input", dateSegmentHandler);
    daysElement.removeEventListener("input", dateSegmentHandler);
};


const registerElementFocusOutHandler = (element: HTMLElement, dotNetRef: any, callBackName: string): void => {

    if (!element) return;

    const handler = (e: FocusEvent): void => {
      
        if (element.contains(e.relatedTarget as Node)) return;
        dotNetRef.invokeMethodAsync(callBackName);
    };

    elementFocusOutMap.set(element, handler);

    element.addEventListener("focusout", handler);
};

const unregisterElementFocusOutHandler = (element: HTMLElement): void => {

    if (!element) return;

    const handler = elementFocusOutMap.get(element);

    if (handler) {
        element.removeEventListener("focusout", handler);
        elementFocusOutMap.delete(element);
    }
};


const formatCountMessage = (remainingMessage: string, overlimitMessage: string, currentLength: number, maxLength: number): string => {

    const countLength = Math.abs(maxLength - currentLength);
    const template    = currentLength <= maxLength ? remainingMessage : overlimitMessage;

    return template.replace(/{count}/g, countLength.toString());
}

const textAreaCountHandler = (dotNetRef: any, callBackName: string, textAreaElement: HTMLTextAreaElement, messageElement: HTMLSpanElement,
                              remainingMessage: string, overlimitMessage: string, overClass:string, maxCharacters: number): void => {

    if (!dotNetRef || !callBackName || !textAreaElement || !messageElement) return;

    const currentLength: number = textAreaElement.value?.length ?? 0;
    const isOver = currentLength > maxCharacters;
    const message = formatCountMessage(remainingMessage, overlimitMessage, currentLength, maxCharacters);

    messageElement.textContent = message;

    if (isOver) {messageElement.classList.add(overClass)}
    else { messageElement.classList.remove(overClass) }
        

    if (maxCharacters <= 0) return;

    const percentage = (currentLength / maxCharacters) * 100;

    if (percentage <= 70) return;

    dotNetRef.invokeMethodAsync(callBackName, currentLength);

};

const registerTextAreaCharacterCountHandler = (dotNetRef: any, callBackName: string, textAreaElement: HTMLTextAreaElement, messageElement: HTMLSpanElement,
                                               remainingMessage: string, overlimitMessage: string, overClass: string,  maxCharacters: number): void => {

    if (!dotNetRef || !callBackName || !textAreaElement || !messageElement || !remainingMessage || !overlimitMessage) return;

    const handler = () => textAreaCountHandler(dotNetRef, callBackName, textAreaElement, messageElement, remainingMessage, overlimitMessage, overClass, maxCharacters);

    const currentLength: number = textAreaElement.value?.length ?? 0;
    const isOver = currentLength > maxCharacters;

    messageElement.textContent = formatCountMessage(remainingMessage, overlimitMessage, currentLength, maxCharacters);

    if (isOver) { messageElement.classList.add(overClass) }
    else { messageElement.classList.remove(overClass) }

    textAreaElement.removeEventListener("input", handler);
    textAreaElement.addEventListener("input", handler);

    textAreaCounterMap.set(textAreaElement, handler);
};

const unregisterTextAreaCharacterCountHandler = (textAreaElement: HTMLTextAreaElement): void => {

    if (!textAreaElement) return;

    const handler = textAreaCounterMap.get(textAreaElement);

    if (handler) {
        textAreaElement.removeEventListener("input", handler);
        textAreaCounterMap.delete(textAreaElement);
    }
};

export {
    registerAriaDisabledHandlers, unregisterAriaDisabledHandlers, registerNumericHandlers, unregisterNumericHandlers,
    setInputValue, setInputFocus, setSummaryFocus, registerReadOnlyHandlers, unregisterReadOnlyHandlers,
    registerSelectReadOnlyDisabledHandlers, unregisterSelectReadOnlyDisabledHandlers, registerTimeSegmentHandlers,
    unregisterTimeSegmentHandlers, registerElementFocusOutHandler, unregisterElementFocusOutHandler,
    registerDateSegmentHandlers, unregisterDateSegmentHandlers, formatDateForAnnouncement,
    registerTextAreaCharacterCountHandler, unregisterTextAreaCharacterCountHandler
};