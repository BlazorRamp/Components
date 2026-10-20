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
        if (e.key === "Escape" && menuButton.getAttribute("aria-expanded") === "true" && (document.activeElement?.tagName !== "DIALOG") && (menuButton === document.activeElement || sideNavigation.contains(document.activeElement))) {
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
const initialise = (menuButton, sideNavigation, startMinWidth = 576) => {
    if (!menuButton || !sideNavigation)
        return;
    if (_initialised)
        return;
    setStartExapndedState(menuButton, startMinWidth);
    registerDocumenEscapeKeyHandler(menuButton, sideNavigation);
    registerMenuButtonClickHandler(menuButton);
    registerBreakpointHandler(menuButton, `(max-width: ${startMinWidth}px)`);
    _initialised = true;
};
const setFocus = (elementId) => {
    const element = document.getElementById(elementId);
    if (!element)
        return;
    element.focus();
};
const scrollMainToTop = () => {
    const main = document.getElementById("app__main");
    if (main)
        main.scrollTop = 0;
};
export { initialise, checkCloseSideNavigation, setFocus, scrollMainToTop };
//# sourceMappingURL=doc-site.js.map