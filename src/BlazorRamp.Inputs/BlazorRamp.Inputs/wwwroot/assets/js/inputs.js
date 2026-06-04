const elementFocusOutMap = new WeakMap();
const TIME_INPUT_COMPONENT_NAME = "TimeInput";
const DATE_INPUT_COMPONENT_NAME = "DateInput";
const getDecimalSeparator = () => Intl.NumberFormat(navigator.language).format(1.1).charAt(1);
const preventClickAction = (e) => e.preventDefault();
const preventAction = (e) => e.preventDefault();
const ariaDisabledKeyHandler = (e) => {
    const navigationKeys = ["Tab", "Enter", "Escape", "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Home", "End", "PageUp", "PageDown"];
    const isNavigation = navigationKeys.includes(e.key);
    const isCopy = (e.ctrlKey || e.metaKey) && e.key.toLowerCase() === "c";
    if (isNavigation || isCopy)
        return;
    e.preventDefault();
};
const integerHandler = (e) => {
    const input = e.target;
    let cleaned = input.value.replace(/[^0-9\-]/g, '');
    cleaned = cleaned.replace(/(?!^)-/g, '');
    if (input.value !== cleaned)
        input.value = cleaned;
};
const decimalHandler = (e) => {
    const input = e.target;
    const separator = getDecimalSeparator();
    const escapedSeparator = separator === '.' ? '\\.' : separator;
    let cleaned = input.value.replace(new RegExp(`[^0-9\\-${escapedSeparator}]`, 'g'), '');
    cleaned = cleaned.replace(/(?!^)-/g, '');
    const parts = cleaned.split(separator);
    if (parts.length > 2)
        cleaned = parts[0] + separator + parts.slice(1).join('');
    if (input.value !== cleaned)
        input.value = cleaned;
};
const timeSegmentHandler = (e) => {
    const input = e.target;
    let cleaned = input.value.replace(/[^0-9]/g, '');
    if (input.value !== cleaned)
        input.value = cleaned;
};
const dateSegmentHandler = (e) => {
    const input = e.target;
    let cleaned = input.value.replace(/[^0-9]/g, '');
    if (input.value !== cleaned)
        input.value = cleaned;
};
const setInputValue = (inputElement, value) => {
    if (!inputElement)
        return;
    inputElement.value = value;
};
const setInputFocus = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element)
        return;
    const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    element.scrollIntoView({
        behavior: prefersReducedMotion ? "auto" : "smooth",
        block: "nearest",
        inline: "nearest"
    });
    if (element.getAttribute("data-br-component") === TIME_INPUT_COMPONENT_NAME || element.getAttribute("data-br-component") === DATE_INPUT_COMPONENT_NAME) {
        const input = element.querySelector("input");
        if (input) {
            input.focus({ preventScroll: true });
            return;
        }
    }
    if (element.getAttribute('role') === 'radiogroup') {
        const firstRadio = element.querySelector('input[type="radio"]');
        if (firstRadio) {
            firstRadio.focus({ preventScroll: true });
            firstRadio.addEventListener("blur", () => firstRadio.removeAttribute("data-br-focused"), { once: true });
        }
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
                if (element.value)
                    element.setSelectionRange(element.value.length, element.value.length);
            }
            catch { }
            break;
    }
};
const setSummaryFocus = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element)
        return;
    element.setAttribute("tabindex", "-1");
    element.focus();
    element.addEventListener("blur", () => element.removeAttribute("tabindex"), { once: true });
};
const formatDateForAnnouncement = (dateString) => {
    try {
        const [year, month, day] = dateString.split('-').map(Number);
        const date = new Date(year, month - 1, day);
        return new Intl.DateTimeFormat(navigator.language, { dateStyle: 'long' }).format(date);
    }
    catch {
        return '';
    }
};
const registerAriaDisabledHandlers = (inputElement) => {
    if (!inputElement)
        return;
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
    if (inputElement.type === "checkbox" || inputElement.type === "time") {
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
const unregisterAriaDisabledHandlers = (inputElement) => {
    if (!inputElement)
        return;
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
const selectReadOnlyKeyHandler = (e) => {
    const blockedKeys = [" ", "ArrowUp", "ArrowDown", "Enter", "F4"];
    if (blockedKeys.includes(e.key))
        e.preventDefault();
};
const registerReadOnlyHandlers = (inputElement) => {
    if (!inputElement)
        return;
    inputElement.removeEventListener("click", preventClickAction);
    inputElement.addEventListener("click", preventClickAction);
};
const unregisterReadOnlyHandlers = (inputElement) => {
    if (!inputElement)
        return;
    inputElement.removeEventListener("click", preventClickAction);
};
const registerNumericHandlers = (inputElement, isWholeNumber) => {
    if (!inputElement)
        return;
    const handler = isWholeNumber ? integerHandler : decimalHandler;
    inputElement.removeEventListener("input", handler);
    inputElement.addEventListener("input", handler);
};
const unregisterNumericHandlers = (inputElement, isWholeNumber) => {
    if (!inputElement)
        return;
    const handler = isWholeNumber ? integerHandler : decimalHandler;
    inputElement.removeEventListener("input", handler);
};
const registerSelectReadOnlyDisabledHandlers = (inputElement) => {
    if (!inputElement)
        return;
    inputElement.removeEventListener("mousedown", preventAction);
    inputElement.addEventListener("mousedown", preventAction);
    inputElement.removeEventListener("keydown", selectReadOnlyKeyHandler);
    inputElement.addEventListener("keydown", selectReadOnlyKeyHandler);
};
const unregisterSelectReadOnlyDisabledHandlers = (inputElement) => {
    if (!inputElement)
        return;
    inputElement.removeEventListener("mousedown", preventAction);
    inputElement.removeEventListener("keydown", selectReadOnlyKeyHandler);
};
const registerTimeSegmentHandlers = (hoursElement, minutesElement, secondsElement) => {
    if (!hoursElement || !minutesElement)
        return;
    hoursElement.addEventListener("input", timeSegmentHandler);
    minutesElement.addEventListener("input", timeSegmentHandler);
    if (secondsElement) {
        secondsElement.addEventListener("input", timeSegmentHandler);
    }
};
const unregisterTimeSegmentHandlers = (hoursElement, minutesElement, secondsElement) => {
    if (!hoursElement || !minutesElement)
        return;
    hoursElement.removeEventListener("input", timeSegmentHandler);
    minutesElement.removeEventListener("input", timeSegmentHandler);
    if (secondsElement) {
        secondsElement.removeEventListener("input", timeSegmentHandler);
    }
};
const registerDateSegmentHandlers = (yearsElement, monthsElement, daysElement) => {
    if (!yearsElement || !monthsElement || !daysElement)
        return;
    yearsElement.addEventListener("input", dateSegmentHandler);
    monthsElement.addEventListener("input", dateSegmentHandler);
    daysElement.addEventListener("input", dateSegmentHandler);
};
const unregisterDateSegmentHandlers = (yearsElement, monthsElement, daysElement) => {
    if (!yearsElement || !monthsElement || !daysElement)
        return;
    yearsElement.removeEventListener("input", dateSegmentHandler);
    monthsElement.removeEventListener("input", dateSegmentHandler);
    daysElement.removeEventListener("input", dateSegmentHandler);
};
const registerElementFocusOutHandler = (element, dotNetRef, callBackName) => {
    if (!element)
        return;
    const handler = (e) => {
        if (element.contains(e.relatedTarget))
            return;
        dotNetRef.invokeMethodAsync(callBackName);
    };
    elementFocusOutMap.set(element, handler);
    element.addEventListener("focusout", handler);
};
const unregisterElementFocusOutHandler = (element) => {
    if (!element)
        return;
    const handler = elementFocusOutMap.get(element);
    if (handler) {
        element.removeEventListener("focusout", handler);
        elementFocusOutMap.delete(element);
    }
};
export { registerAriaDisabledHandlers, unregisterAriaDisabledHandlers, registerNumericHandlers, unregisterNumericHandlers, setInputValue, setInputFocus, setSummaryFocus, registerReadOnlyHandlers, unregisterReadOnlyHandlers, registerSelectReadOnlyDisabledHandlers, unregisterSelectReadOnlyDisabledHandlers, registerTimeSegmentHandlers, unregisterTimeSegmentHandlers, registerElementFocusOutHandler, unregisterElementFocusOutHandler, registerDateSegmentHandlers, unregisterDateSegmentHandlers, formatDateForAnnouncement };
//# sourceMappingURL=inputs.js.map