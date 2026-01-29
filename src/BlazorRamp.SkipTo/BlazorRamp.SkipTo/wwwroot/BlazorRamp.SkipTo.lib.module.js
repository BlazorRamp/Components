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
let startUpCompleted = false;
const initialiseSkipLink = () => {
    const skipLink = document.querySelector('.br-skip-to[data-br-skip-to="site"]');
    if (!skipLink || startUpCompleted === true)
        return;
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
const afterWebStarted = () => initialiseSkipLink();
const afterServerStarted = () => initialiseSkipLink();
const afterWebAssemblyStarted = () => initialiseSkipLink();
export { afterWebStarted, afterServerStarted, afterWebAssemblyStarted };
//# sourceMappingURL=BlazorRamp.SkipTo.lib.module.js.map