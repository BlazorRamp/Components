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
const getResolvedHexColourValue = (variableName) => {
    const value = getComputedStyle(document.documentElement).getPropertyValue(variableName).trim();
    const context = document.createElement('canvas').getContext('2d');
    if (!context)
        return "";
    context.fillStyle = value;
    const resolvedValue = context.fillStyle;
    if (resolvedValue.startsWith('#'))
        return resolvedValue.length === 9 ? resolvedValue.slice(0, 7) : resolvedValue; // strip alpha if present
    // color(srgb r g b) - what you'll get from hsl(from ...) chains etc.
    let match = resolvedValue.match(/^color\(srgb\s+([\d.]+)\s+([\d.]+)\s+([\d.]+)/);
    if (match) {
        const [r, g, b] = match.slice(1, 4).map(n => Math.round(parseFloat(n) * 255));
        return toHex(r, g, b);
    }
    // rgb(r, g, b) / rgba(r, g, b, a) - fallback, shouldn't normally hit this via canvas
    match = resolvedValue.match(/^rgba?\(([\d.]+)[,\s]+([\d.]+)[,\s]+([\d.]+)/);
    if (match) {
        const [r, g, b] = match.slice(1, 4).map(Number);
        return toHex(r, g, b);
    }
    return "";
};
const toHex = (r, g, b) => '#' + [r, g, b].map(x => Math.round(Math.max(0, Math.min(255, x))).toString(16).padStart(2, '0')).join('');
const setStyleProperty = (variableName, variableValue) => {
    document.documentElement.style.setProperty(variableName, variableValue);
};
const getComputedStyleProperty = (variableName) => {
    return getComputedStyle(document.documentElement).getPropertyValue(variableName)?.trim();
};
const removeStyleProperty = (variableName) => {
    document.documentElement.style.removeProperty(variableName);
};
export { setStyleProperty, setElementVariable, getComputedStyleProperty, removeStyleProperty, getResolvedHexColourValue };
//# sourceMappingURL=doc-themes.js.map