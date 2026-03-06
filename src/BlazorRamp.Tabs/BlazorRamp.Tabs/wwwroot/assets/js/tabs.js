const _handleKeyDown = (e) => {
    if (['ArrowLeft', 'ArrowRight', 'Home', 'End'].includes(e.key)) {
        e.preventDefault();
    }
};
const registerTabs = (tabListElementID) => {
    const tabsListElement = document.getElementById(tabListElementID);
    if (!tabsListElement)
        return;
    tabsListElement.removeEventListener('keydown', _handleKeyDown);
    tabsListElement.addEventListener('keydown', _handleKeyDown);
};
const unregisterTabs = (tabListElementID) => {
    const tabsListElement = document.getElementById(tabListElementID);
    if (!tabsListElement)
        return;
    tabsListElement.removeEventListener('keydown', _handleKeyDown);
};
export { registerTabs, unregisterTabs };
//# sourceMappingURL=tabs.js.map