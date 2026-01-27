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
export { scrollToView };
//# sourceMappingURL=skip-to.js.map