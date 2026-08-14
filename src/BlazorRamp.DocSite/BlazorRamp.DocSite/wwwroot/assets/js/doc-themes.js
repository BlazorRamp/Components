const setElementVariable = (elementId, variableName, variableValue, reset) => {
    const element = document.getElementById(elementId);
    if (!element)
        return;
    console.log(element);
    if (reset == true) {
        element.style.removeProperty(variableName);
        return;
    }
    element.style.setProperty(variableName, variableValue);
};
const setRootVariable = (variableName, variableValue) => {
    document.documentElement.style.setProperty(variableName, variableValue);
};
export { setRootVariable, setElementVariable };
//# sourceMappingURL=doc-themes.js.map