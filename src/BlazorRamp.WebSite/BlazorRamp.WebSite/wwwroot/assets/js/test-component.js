let _initialised = false;
const handleBreakpointChange = (event, menuButton) => {
    if (event.matches) {
        if (menuButton)
            menuButton.setAttribute("aria-expanded", "false");
    }
};
const registerBreakpointHandler = (menuButton, query) => {
    const mediaQuery = window.matchMedia(query);
    mediaQuery.addEventListener("change", (e) => {
        handleBreakpointChange(e, menuButton);
    });
};
const registerDocumenEscapeKeyHandler = (menuButton, sideNavigation) => {
    if (!menuButton || !sideNavigation)
        return;
    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && menuButton.getAttribute("aria-expanded") === "true" && (menuButton === document.activeElement || sideNavigation.contains(document.activeElement))) {
            menuButton.setAttribute("aria-expanded", "false");
            menuButton.focus();
        }
    });
};
const registerMenuButtonClickHandler = (menuButton) => {
    if (!menuButton)
        return;
    menuButton.addEventListener("click", (event) => {
        const isExpanded = menuButton.getAttribute("aria-expanded") === "true";
        menuButton.setAttribute("aria-expanded", (!isExpanded).toString());
    });
};
const setStartExapndedState = (menuButton, startMinWidth) => {
    if (!menuButton)
        return;
    const expanded = window.innerWidth <= startMinWidth ? false : true;
    menuButton.setAttribute("aria-expanded", expanded.toString());
};
const checkCloseSideNavigation = (menuButton, minWidth = 576) => {
    if (window.innerWidth <= minWidth && menuButton)
        menuButton.setAttribute("aria-expanded", "false");
};
const showModalDialog = (dialogElement) => {
    if (dialogElement) {
        dialogElement.showModal();
        const heading = document.getElementById("Screen-Dialog-Title-ID");
        if (heading)
            heading.focus();
    }
};
const closeModalDialog = (dialogElement) => {
    if (dialogElement && dialogElement.open)
        dialogElement.close();
};
const initialise = (menuButton, sideNavigation, mainContent, startMinWidth = 576) => {
    if (!menuButton || !sideNavigation || !mainContent)
        return;
    if (_initialised)
        return;
    setStartExapndedState(menuButton, startMinWidth);
    registerDocumenEscapeKeyHandler(menuButton, sideNavigation);
    registerMenuButtonClickHandler(menuButton);
    registerBreakpointHandler(menuButton, `(max-width: ${startMinWidth}px)`);
    _initialised = true;
};
export { initialise, checkCloseSideNavigation, showModalDialog, closeModalDialog };
//# sourceMappingURL=test-component.js.map