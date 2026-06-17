;
;
const _handlerMap = new WeakMap();
const raiseDebounceFilterResult = (inputElement, debounceConfiguration, clearCalled = false) => {
    if (!debounceConfiguration || !debounceConfiguration.blazorCallBackRef || !inputElement || !debounceConfiguration.messageElement)
        return;
    let isValid = true;
    let message = null;
    const messageElement = debounceConfiguration.messageElement;
    const regexPattern = debounceConfiguration.regexPattern;
    const stateIconElement = debounceConfiguration.stateIconElement;
    const inputValue = inputElement.value.trimStart();
    if (regexPattern !== null && regexPattern.trim().length > 0) {
        try {
            isValid = new RegExp(regexPattern).test(inputValue);
            messageElement.innerText = isValid ? "" : debounceConfiguration.validationMessage ?? "";
        }
        catch (ex) {
            messageElement.innerText = debounceConfiguration.systemErrorMessage ?? "";
            message = ex.message;
            isValid = false;
        }
    }
    if (inputValue.length === 0) {
        isValid = true;
        messageElement.innerText = "";
        message = null;
    }
    if (isValid) {
        inputElement.removeAttribute("aria-invalid");
    }
    else {
        inputElement.setAttribute("aria-invalid", "true");
    }
    stateIconElement.setAttribute("data-br-invalid-state", (!isValid).toString().toLowerCase());
    if (clearCalled)
        stateIconElement.removeAttribute("data-br-invalid-state");
    const debouncedFilterResult = { FilterValue: inputValue, IsValid: isValid, ClearCalled: clearCalled, ExceptionMessage: message };
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
    mapEntry.timer = setTimeout(raiseDebounceFilterResult, configuration.delayMs, inputElement, configuration, false);
};
const clearDebounceFilter = (inputElement) => {
    if (!inputElement)
        return;
    const mapEntry = _handlerMap.get(inputElement);
    if (mapEntry?.timer)
        clearTimeout(mapEntry.timer);
    inputElement.value = "";
    if (!mapEntry)
        return;
    raiseDebounceFilterResult(inputElement, mapEntry.configuration, true);
};
const registerDebounceFilterHandler = (inputElement, debounceConfiguration) => {
    if (!inputElement || !debounceConfiguration)
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