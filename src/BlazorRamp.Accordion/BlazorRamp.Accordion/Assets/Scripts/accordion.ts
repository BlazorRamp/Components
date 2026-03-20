const _handleKeyDown = (e: KeyboardEvent): void => {

    const keys = ["Home", "End", "ArrowUp", "ArrowDown"];

    if (!keys.includes(e.key)) return;

    const target = e.target as HTMLElement;

    if (target.closest("[data-br-accordion-trigger]")) e.preventDefault();

};

const registerKeyHandler = (headerElementID: string): void => {

    const headerElement = document.getElementById(headerElementID);

    if (!headerElement) return;

    headerElement.removeEventListener("keydown", _handleKeyDown);
    headerElement.addEventListener("keydown", _handleKeyDown);
};

const unregisterKeyHandler = (headerElementID: string): void => {

    const headerElement = document.getElementById(headerElementID);

    if (!headerElement) return;

    headerElement.removeEventListener("keydown", _handleKeyDown);
};

export { registerKeyHandler, unregisterKeyHandler };