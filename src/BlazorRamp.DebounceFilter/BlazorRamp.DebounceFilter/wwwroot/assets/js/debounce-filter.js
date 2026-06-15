;
;
const _handlerMap = new WeakMap();
const hasValue = (value) => (value !== null && value.trim().length > 0);
const raiseDebounceInterval = (inputElement, debounceConfiguration) => {
    if (!debounceConfiguration || !debounceConfiguration.blazorCallBackRef)
        return;
    let isValid = true;
    let message = null;
    const messageElement = debounceConfiguration.messageElement;
    const regexPattern = debounceConfiguration.regexPattern;
    const stateIconElemen = debounceConfiguration.stateIconElement;
    if (regexPattern !== null && regexPattern.trim().length > 0) {
        try {
            isValid = new RegExp(regexPattern).test(inputElement.value);
            messageElement.innerText = isValid ? "" : debounceConfiguration.validationMessage ?? "";
        }
        catch (ex) {
            if (messageElement)
                messageElement.innerText = debounceConfiguration.systemErrorMessage ?? "";
            message = ex.message;
            isValid = false;
        }
        inputElement.setAttribute("aria-invalid", (!isValid).toString().toLowerCase());
        stateIconElemen.setAttribute("data-br-invalid-state", (!isValid).toString().toLowerCase());
    }
    const debouncedFilterResult = { FilterValue: inputElement.value, IsValid: isValid, ExceptionMessage: message };
    debounceConfiguration.blazorCallBackRef.invokeMethodAsync(debounceConfiguration.callBackName, debouncedFilterResult);
};
const oninputHandler = (event) => {
    if (!(event.target instanceof HTMLInputElement))
        return;
    const inputElement = event.target;
    const mapEntry = _handlerMap.get(inputElement);
    if (!mapEntry)
        return;
    const { configuration } = mapEntry;
    if (mapEntry.timer)
        clearTimeout(mapEntry.timer);
    mapEntry.timer = setTimeout(raiseDebounceInterval, configuration.delayMs, inputElement, configuration);
};
const clearDebounceFilter = (inputElement) => {
    if (!inputElement)
        return;
    inputElement.value = "";
    inputElement.removeAttribute("aria-invalid");
};
const registerDebounceFilterHandler = (inputElement, debounceConfiguration) => {
    if (!inputElement || _handlerMap.has(inputElement) || !debounceConfiguration)
        return;
    const handler = (event) => oninputHandler(event);
    unregisterDebounceFilterHandler(inputElement);
    _handlerMap.set(inputElement, {
        configuration: debounceConfiguration,
        handler: handler
    });
    inputElement.addEventListener("input", handler);
};
const unregisterDebounceFilterHandler = (inputElement) => {
    if (!inputElement)
        return;
    const mapEntry = _handlerMap.get(inputElement);
    if (mapEntry) {
        inputElement.removeEventListener("input", mapEntry.handler);
        _handlerMap.delete(inputElement);
    }
};
export { registerDebounceFilterHandler, unregisterDebounceFilterHandler, clearDebounceFilter };
//# sourceMappingURL=debounce-filter.js.map