
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

let startUpCompleted: boolean = false;

const initialiseSkipLink = ():void => {

    const skipLink = document.querySelector(".br-skip-to") as HTMLAnchorElement;

    if (!skipLink || startUpCompleted === true) return;

    skipLink.addEventListener("click", (event) => {

        event.preventDefault();

        const targetID = skipLink.hash.substring(1);

        if (targetID) {
            scrollToView(targetID);
            history.pushState(null, '', skipLink.hash);
        }
 
    });

    startUpCompleted = true;

};
const afterWebStarted = (): void => initialiseSkipLink();

const afterServerStarted = (): void => initialiseSkipLink();

const afterWebAssemblyStarted = (): void => initialiseSkipLink();

export { afterWebStarted, afterServerStarted, afterWebAssemblyStarted};