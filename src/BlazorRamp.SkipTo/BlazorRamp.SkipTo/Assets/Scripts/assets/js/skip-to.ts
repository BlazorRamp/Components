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

export { scrollToView };