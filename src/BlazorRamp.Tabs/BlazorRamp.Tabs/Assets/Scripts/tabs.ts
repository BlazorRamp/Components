const _handleKeyDown = (e: KeyboardEvent): void => {

    if (['ArrowLeft', 'ArrowRight', 'Home', 'End'].includes(e.key)) {
        e.preventDefault();
    }
};

const registerTabs = (tabListElementID: string): void => {

    const tabsListElement = document.getElementById(tabListElementID);

    if (!tabsListElement) return;

    tabsListElement.removeEventListener('keydown', _handleKeyDown);
    tabsListElement.addEventListener('keydown', _handleKeyDown);
};

const unregisterTabs = (tabListElementID: string): void => {

    const tabsListElement = document.getElementById(tabListElementID);

    if (!tabsListElement) return;

    tabsListElement.removeEventListener('keydown', _handleKeyDown);
};

export { registerTabs, unregisterTabs };