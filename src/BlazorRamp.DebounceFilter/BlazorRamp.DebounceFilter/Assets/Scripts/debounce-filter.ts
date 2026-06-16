
interface IDebounceConfiguration {
    blazorCallBackRef: any, callBackName: string, messageElement: HTMLElement, stateIconElement: HTMLElement,
    delayMs: number, systemErrorMessage: string | null, regexPattern: string | null, validationMessage: string | null
};

interface IDebouncedFilterResult { FilterValue: string, IsValid: boolean, ClearCalled: boolean, ExceptionMessage: string | null };

const _handlerMap = new WeakMap<HTMLInputElement, { configuration: IDebounceConfiguration, handler: EventListener; timer?: number }>();


const raiseDebounceFilterResult = (inputElement: HTMLInputElement, debounceConfiguration: IDebounceConfiguration, clearCalled: boolean = false): void => {

    if (!debounceConfiguration || !debounceConfiguration.blazorCallBackRef || !inputElement || !debounceConfiguration.messageElement) return;

    let isValid = true;
    let message = null;

    const messageElement:  HTMLElement    = debounceConfiguration.messageElement;
    const regexPattern:    string | null  = debounceConfiguration.regexPattern;
    const stateIconElement: HTMLElement   = debounceConfiguration.stateIconElement;

    const inputValue = inputElement.value.trimStart();

    if (regexPattern !== null && regexPattern.trim().length > 0) {

        try {

            isValid = new RegExp(regexPattern).test(inputElement.value);

            messageElement.innerText = isValid ? "" : debounceConfiguration.validationMessage ?? "";
        }
        catch (ex: any) {
            messageElement.innerText = debounceConfiguration.systemErrorMessage ?? "";
            message = ex.message;
            isValid = false;
        }
    }

    if (inputValue.length === 0) {
        isValid = true;
        messageElement.innerText = "";
    }

    inputElement.setAttribute("aria-invalid", (!isValid).toString().toLowerCase());
    stateIconElement.setAttribute("data-br-invalid-state", (!isValid).toString().toLowerCase());

    if (clearCalled) stateIconElement.removeAttribute("data-br-invalid-state");

    const debouncedFilterResult: IDebouncedFilterResult = { FilterValue: inputElement.value, IsValid: isValid, ClearCalled: clearCalled, ExceptionMessage: message };

    debounceConfiguration.blazorCallBackRef.invokeMethodAsync(debounceConfiguration.callBackName, debouncedFilterResult);

};


const oninputHandler = (event: Event): void => {

    if (!(event.target instanceof HTMLInputElement)) return;

    const inputElement = event.target;
    const mapEntry = _handlerMap.get(inputElement);

    if (!mapEntry) return;

    const { configuration } = mapEntry;

    if (mapEntry.timer) clearTimeout(mapEntry.timer);

    mapEntry.timer = setTimeout(raiseDebounceFilterResult, configuration.delayMs, inputElement, configuration, false);
};

const clearDebounceFilter = (inputElement: HTMLInputElement):void => {

    if (!inputElement) return;

    const mapEntry = _handlerMap.get(inputElement);

    if (mapEntry?.timer) clearTimeout(mapEntry.timer);

    inputElement.value = "";

    if (!mapEntry) return;

    raiseDebounceFilterResult(inputElement, mapEntry.configuration, true);
};

const registerDebounceFilterHandler = (inputElement: HTMLInputElement, debounceConfiguration: IDebounceConfiguration): void => {

    if (!inputElement || !debounceConfiguration) return;

    const handler: EventListener = (event) => oninputHandler(event);

    unregisterDebounceFilterHandler(inputElement);

    _handlerMap.set(inputElement, {
        configuration: debounceConfiguration,
        handler: handler
    });

    inputElement.addEventListener("input", handler);

};

const unregisterDebounceFilterHandler = (inputElement: HTMLInputElement): void => {

    if (!inputElement) return;

    const mapEntry = _handlerMap.get(inputElement);

    if (mapEntry) {
        inputElement.removeEventListener("input", mapEntry.handler);
        _handlerMap.delete(inputElement);
    }
};

export { registerDebounceFilterHandler, unregisterDebounceFilterHandler, clearDebounceFilter };
