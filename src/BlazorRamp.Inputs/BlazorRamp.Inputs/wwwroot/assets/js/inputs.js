const _preventAction = (e) => e.preventDefault();
const _ariaDisabledKeyHandler = (e) => {
    const navigationKeys = ["Tab", "Enter", "Escape", "ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight", "Home", "End", "PageUp", "PageDown"];
    const isNavigation = navigationKeys.includes(e.key);
    const isCopy = (e.ctrlKey || e.metaKey) && e.key.toLowerCase() === "c";
    if (isNavigation || isCopy)
        return;
    e.preventDefault();
};
const registerAriaDisabledHandlers = (inputElement) => {
    if (!inputElement)
        return;
    inputElement.removeEventListener("keydown", _ariaDisabledKeyHandler);
    inputElement.addEventListener("keydown", _ariaDisabledKeyHandler);
    inputElement.removeEventListener("paste", _preventAction);
    inputElement.addEventListener("paste", _preventAction);
    inputElement.removeEventListener("cut", _preventAction);
    inputElement.addEventListener("cut", _preventAction);
};
const unregisterAriaDisabledHandlers = (inputElement) => {
    if (!inputElement)
        return;
    inputElement.removeEventListener("keydown", _ariaDisabledKeyHandler);
    inputElement.removeEventListener("cut", _preventAction);
    inputElement.removeEventListener("paste", _preventAction);
};
export { registerAriaDisabledHandlers, unregisterAriaDisabledHandlers };
//# sourceMappingURL=inputs.js.map