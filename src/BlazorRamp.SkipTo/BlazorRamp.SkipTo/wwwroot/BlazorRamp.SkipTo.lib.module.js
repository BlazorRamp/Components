const scrollToView = (elementID) => {
    const element = document.getElementById(elementID);
    if (!element)
        return;
    const prefersReducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    element.scrollIntoView({
        behavior: prefersReducedMotion ? "auto" : "smooth",
        block: "start",
        inline: "nearest"
    });
    element.focus();
};
const initializeSkipLink = () => {
    const skipLink = document.querySelector(".br-skip-to");
    if (!skipLink)
        return;
    skipLink.addEventListener("click", (event) => {
        event.preventDefault();
        const targetID = skipLink.hash.substring(1);
        if (targetID) {
            scrollToView(targetID);
            history.pushState(null, '', skipLink.hash);
        }
    });
};
const afterWebStarted = () => initializeSkipLink();
const afterServerStarted = () => initializeSkipLink();
const afterWebAssemblyStarted = () => initializeSkipLink();
export { afterWebStarted, afterServerStarted, afterWebAssemblyStarted, scrollToView };
//# sourceMappingURL=BlazorRamp.SkipTo.lib.module.js.map