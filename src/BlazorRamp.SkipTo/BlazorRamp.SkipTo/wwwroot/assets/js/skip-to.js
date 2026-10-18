const pageIsNotUserScrollable = () => {
    const htmlOverflow = getComputedStyle(document.documentElement).overflowY;
    const effective = htmlOverflow !== "visible" ? htmlOverflow : getComputedStyle(document.body).overflowY;
    return effective === "hidden" || effective === "clip";
};
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
    element.focus({ preventScroll: true });
    if (pageIsNotUserScrollable()) {
        requestAnimationFrame(() => {
            document.documentElement.scrollTop = 0;
        });
    }
};
export { scrollToView };
//# sourceMappingURL=skip-to.js.map