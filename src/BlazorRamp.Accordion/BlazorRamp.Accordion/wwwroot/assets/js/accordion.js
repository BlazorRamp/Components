const _handleKeyDown = (e) => {
    const keys = ['Home', 'End', 'ArrowUp', 'ArrowDown'];
    if (!keys.includes(e.key))
        return;
    const target = e.target;
    if (target.closest('[data-br-accordion-trigger]'))
        e.preventDefault();
};
const registerKeyHandler = (headerElementID) => {
    const headerElement = document.getElementById(headerElementID);
    if (!headerElement)
        return;
    headerElement.removeEventListener('keydown', _handleKeyDown);
    headerElement.addEventListener('keydown', _handleKeyDown);
};
const unregisterKeyHandler = (headerElementID) => {
    const headerElement = document.getElementById(headerElementID);
    if (!headerElement)
        return;
    headerElement.removeEventListener('keydown', _handleKeyDown);
};
export { registerKeyHandler, unregisterKeyHandler };
//# sourceMappingURL=accordion.js.map