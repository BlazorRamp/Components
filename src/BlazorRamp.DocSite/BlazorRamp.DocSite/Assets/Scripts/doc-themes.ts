

const setElementVariable = (elementId: string, variableName: string, variableValue: string, reset: boolean): void => {

    const element = document.getElementById(elementId);

    if (!element) return;

    console.log(element);

    if (reset == true) {
        element.style.removeProperty(variableName);
        return;
    }
    
    element.style.setProperty(variableName, variableValue);

};


const setRootVariable = (variableName: string, variableValue: string): void => {

    document.documentElement.style.setProperty(variableName, variableValue);
};


export { setRootVariable, setElementVariable };