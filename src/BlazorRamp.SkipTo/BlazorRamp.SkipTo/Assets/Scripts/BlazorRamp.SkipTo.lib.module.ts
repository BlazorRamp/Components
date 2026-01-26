
const scrollToView = (elementID: string) => {

    const element: HTMLElement = document.getElementById(elementID) as HTMLElement;

    if (!element) return;

    const prefersReducedMotion: boolean = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

    element.scrollIntoView({
        behavior: prefersReducedMotion ? "auto" : "smooth",
        block: "start",
        inline: "nearest"
    });

    element.focus();
}

const initializeSkipLink = ():void => {

    const skipLink = document.querySelector(".br-skip-to") as HTMLAnchorElement;

    if (!skipLink) return;

    skipLink.addEventListener("click", (event) => {

        event.preventDefault();

        const targetID = skipLink.hash.substring(1);

        if (targetID) {
            scrollToView(targetID);
            history.pushState(null, '', skipLink.hash);
        }
 
    });

};
const afterWebStarted = (): void => initializeSkipLink();

const afterServerStarted = (): void => initializeSkipLink();

const afterWebAssemblyStarted = (): void => initializeSkipLink();

export { afterWebStarted, afterServerStarted, afterWebAssemblyStarted, scrollToView };