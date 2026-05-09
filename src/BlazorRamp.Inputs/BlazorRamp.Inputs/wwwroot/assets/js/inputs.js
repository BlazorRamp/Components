const getDecimalSeparator = () => Intl.NumberFormat(navigator.language).format(1.1).charAt(1);
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
const setInputValue = (inputElement, value) => {
    if (!inputElement)
        return;
    inputElement.value = value;
};
const setInputFocus = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element)
        return;
    element.focus();
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
        // date, time, number, checkbox, radio etc - just focus, no cursor manipulation
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
const registerAriaDisabledHandlers = (inputElement) => {
    if (!inputElement)
        return;
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
    inputElement.removeEventListener("keydown", ariaDisabledKeyHandler);
    inputElement.removeEventListener("cut", preventAction);
    inputElement.removeEventListener("paste", preventAction);
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
export { registerAriaDisabledHandlers, unregisterAriaDisabledHandlers, registerNumericHandlers, unregisterNumericHandlers, setInputValue, setInputFocus, setSummaryFocus };
//# sourceMappingURL=inputs.js.map