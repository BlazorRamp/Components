
interface IDebounceConfiguration {
    blazorCallBackRef: any, callBackName: string, messageElement: HTMLElement, stateIconElement: HTMLElement,
    delayMs: number, systemErrorMessage: string | null, regexPattern: string | null, validationMessage: string | null
};

interface IDebouncedFilterResult { FilterValue: string, IsValid: boolean, ExceptionMessage: string | null };

const _handlerMap = new WeakMap<HTMLInputElement, { configuration: IDebounceConfiguration, handler: EventListener; timer?: number }>();


const hasValue = (value: string | null): boolean => (value !== null && value.trim().length > 0);

const raiseDebounceInterval = (inputElement: HTMLInputElement, debounceConfiguration: IDebounceConfiguration): void => {

    if (!debounceConfiguration || !debounceConfiguration.blazorCallBackRef) return;


    let isValid = true;
    let message = null;

    const messageElement:  HTMLElement   = debounceConfiguration.messageElement;
    const regexPattern:    string | null = debounceConfiguration.regexPattern;
    const stateIconElemen: HTMLElement   = debounceConfiguration.stateIconElement;

    if (regexPattern !== null && regexPattern.trim().length > 0) {

        try {

            isValid = new RegExp(regexPattern).test(inputElement.value);

            messageElement.innerText = isValid ? "" : debounceConfiguration.validationMessage ?? "";
        }
        catch (ex: any) {
            if (messageElement) messageElement.innerText = debounceConfiguration.systemErrorMessage ?? "";
            message = ex.message;
            isValid = false;
        }
      

        inputElement.setAttribute("aria-invalid", (!isValid).toString().toLowerCase());
        stateIconElemen.setAttribute("data-br-invalid-state", (!isValid).toString().toLowerCase());
    }

    const debouncedFilterResult: IDebouncedFilterResult = { FilterValue: inputElement.value, IsValid: isValid, ExceptionMessage: message };

    debounceConfiguration.blazorCallBackRef.invokeMethodAsync(debounceConfiguration.callBackName, debouncedFilterResult);

};


const oninputHandler = (event: Event): void => {

    if (!(event.target instanceof HTMLInputElement)) return;

    const inputElement = event.target;
    const mapEntry = _handlerMap.get(inputElement);

    if (!mapEntry) return;

    const { configuration } = mapEntry;

    if (mapEntry.timer) clearTimeout(mapEntry.timer);

    mapEntry.timer = setTimeout(raiseDebounceInterval, configuration.delayMs, inputElement, configuration);
};

const clearDebounceFilter = (inputElement: HTMLInputElement):void => {

    if (!inputElement) return;
    inputElement.value = "";
    inputElement.removeAttribute("aria-invalid");
};

const registerDebounceFilterHandler = (inputElement: HTMLInputElement, debounceConfiguration: IDebounceConfiguration): void => {

    if (!inputElement || _handlerMap.has(inputElement) || !debounceConfiguration) return;

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
