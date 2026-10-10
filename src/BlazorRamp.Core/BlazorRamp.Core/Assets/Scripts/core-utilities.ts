interface AutoTabindexRegistration {
    resizeObserver: ResizeObserver;          // watches the container and its direct children
    mutationObserver: MutationObserver;      // watches for content being added/removed anywhere inside the container
    observedChildren: Set<Element>;          // direct children currently observed by the resize observer
    pendingFrame: number | null;             // requestAnimationFrame id while a check is scheduled
    labelName: string;
    addRoleAndLabel: boolean;
    originalTabindex: string | null;         // captured at registration so it can be restored (e.g. "-1" panels)
    applied: boolean;                        // true while we have added the tab stop
    addedRole: boolean;                      // true if we added role="group" (none was present) and so must remove it
    addedAriaLabel: boolean;                 // true if we added aria-label (no name was present) and so must remove it
}
const _tabFocusElements =
    'a[href], area[href], input:not([disabled]):not([type="hidden"]), select:not([disabled]), textarea:not([disabled]), ' +
    'button:not([disabled]), iframe, object, embed, [contenteditable]:not([contenteditable="false"]), ' +
    '[tabindex]:not([tabindex="-1"]), audio[controls], video[controls], summary';

const _genericTagNames = new Set(["DIV", "SPAN"]);

const _scrollableOverflowValues = ["auto", "scroll"];

const _autoTabindexRegistrations = new Map<HTMLElement, AutoTabindexRegistration>();

const getScreenInnerWidth = (): number => window.innerWidth;


/* Can the user actually scroll this element (overflow auto/scroll AND content bigger than the box)? 1px tolerance for rounding. */
const canScroll = (element: HTMLElement): boolean => {
    const computedStyle = getComputedStyle(element);

    const scrollsVertically = _scrollableOverflowValues.includes(computedStyle.overflowY) && element.scrollHeight - element.clientHeight > 1;
    const scrollsHorizontally = _scrollableOverflowValues.includes(computedStyle.overflowX) && element.scrollWidth - element.clientWidth > 1;

    return scrollsVertically || scrollsHorizontally;
};

const hasFocusableContent = (element: HTMLElement): boolean => {
    return Array.from(element.querySelectorAll<HTMLElement>(_tabFocusElements)).some(descendant => !descendant.closest("[inert]") && descendant.checkVisibility?.({ visibilityProperty: true }) !== false);
}


const addTabStopToContainer = (element: HTMLElement, registration: AutoTabindexRegistration): void => {

    element.setAttribute("tabindex", "0");

    if (!registration.addRoleAndLabel) {
        registration.applied = true;
        return;
    }

    // Only add what is actually missing - a role, a name, or both the consumer (or component) may already have set stay untouched.
    // "group" rather than "region" on purpose: region is a landmark, and auto-labelling every scrollable panel on a
    // page as a landmark would clutter landmark navigation. A consumer who wants the landmark can set role="region"
    // themselves, and since we only add a role when none exists, that choice is left alone.
    // Restricted to div/span: hasAttribute("role") only sees an *explicit* role attribute, not an element's *implicit*
    // one (e.g. <nav>, <table>, <ul>, <dialog> all have a real implicit role with no role attribute present). An
    // explicit role always overrides implicit semantics in the accessibility tree, so setting one on those tags
    // would silently strip their native role. div/span are the only tags with no meaningful implicit role of their
    // own (both map to role=generic), so they're the only tags it's safe to assign role="group" to here.



    if (_genericTagNames.has(element.tagName) && !element.hasAttribute("role")) {
        element.setAttribute("role", "group");
        registration.addedRole = true;
    }

    // WCAG 4.1.2 (Name, Role, Value): a role needs an accessible name, and that need is even stronger once it is
    // also a tab stop, since a screen reader user can now land directly on it via Tab with nothing read out.
    // This covers both a role we just added above and a pre-existing role (implicit or explicit) that was never
    // given a name - either way we only ever add the missing name, we never overwrite one that's already there
    // (aria-label or aria-labelledby). Left ungated by tag name on purpose: naming a <nav> or <table> doesn't
    // change its role, it only supplies what's missing, so it's safe even on elements with strong native semantics.



    if (!element.hasAttribute("aria-label") && !element.hasAttribute("aria-labelledby")) {
        element.setAttribute("aria-label", registration.labelName);
        registration.addedAriaLabel = true;
    }

    registration.applied = true;
};


const removeTabStopFromContainer = (element: HTMLElement, registration: AutoTabindexRegistration): void => {

    if (registration.originalTabindex === null) {
        element.removeAttribute("tabindex");
    }
    else {
        element.setAttribute("tabindex", registration.originalTabindex);
    }

    // Mirror of addTabStopToContainer: only remove the piece we ourselves added, leave anything pre-existing exactly as it was
    if (registration.addedRole) {
        element.removeAttribute("role");
        registration.addedRole = false;
    }

    if (registration.addedAriaLabel) {
        element.removeAttribute("aria-label");
        registration.addedAriaLabel = false;
    }

    registration.applied = false;
};


const checkAutoTabindex = (element: HTMLElement): void => {

    const registration = _autoTabindexRegistrations.get(element);

    if (!registration) return;

    if (!element.isConnected) {
        unregisterContainerForAutoTabindex(element);
        return;
    }

    const needsTabStop = canScroll(element) && !hasFocusableContent(element);

    if (needsTabStop === registration.applied) return; // already in the right state, nothing to do

    if (needsTabStop) {
        addTabStopToContainer(element, registration);
        return;
    }

    // Never pull the tabindex out from under a focused element, wait until focus leaves
    if (document.activeElement === element) {
        element.addEventListener("blur", () => checkAutoTabindex(element), { once: true });
        return;
    }

    removeTabStopFromContainer(element, registration);
};

const scheduleAutoTabindexCheck = (element: HTMLElement): void => {

    const registration = _autoTabindexRegistrations.get(element);

    if (!registration || registration.pendingFrame !== null) return;

    registration.pendingFrame = requestAnimationFrame(() => {
        registration.pendingFrame = null;
        checkAutoTabindex(element);
    });
};

const syncObservedChildren = (element: HTMLElement, registration: AutoTabindexRegistration): void => {

    const currentChildren = new Set<Element>(Array.from(element.children));

    registration.observedChildren.forEach(child => {
        if (currentChildren.has(child)) return;

        registration.resizeObserver.unobserve(child);
        registration.observedChildren.delete(child);
    });

    currentChildren.forEach(child => {
        if (registration.observedChildren.has(child)) return;

        registration.resizeObserver.observe(child);
        registration.observedChildren.add(child);
    });
};

const registerContainerForAutoTabindex = (containerElement: HTMLElement, labelName: string = "Scroll region", addRoleAndLabel: boolean = true): void => {

    if (!containerElement) return;

    unregisterContainerForAutoTabindex(containerElement); // idempotent, safe if a component re-registers after a re-render

    const registration: AutoTabindexRegistration = {
        resizeObserver: new ResizeObserver(() => scheduleAutoTabindexCheck(containerElement)),
        mutationObserver: new MutationObserver(() => {
            syncObservedChildren(containerElement, registration);
            scheduleAutoTabindexCheck(containerElement);
        }),

        observedChildren: new Set<Element>(),
        pendingFrame: null,
        labelName, addRoleAndLabel,
        originalTabindex: containerElement.getAttribute("tabindex"),
        applied: false,
        addedRole: false,
        addedAriaLabel: false
    };

    _autoTabindexRegistrations.set(containerElement, registration);

    registration.resizeObserver.observe(containerElement);
    registration.mutationObserver.observe(containerElement, { childList: true, subtree: true }); // no attributes: we change our own attributes
    syncObservedChildren(containerElement, registration);

    checkAutoTabindex(containerElement); // initial check, don't rely on the resize observer's first callback
};


const unregisterContainerForAutoTabindex = (containerElement: HTMLElement): void => {

    const registration = _autoTabindexRegistrations.get(containerElement);

    if (!registration) return;

    registration.resizeObserver.disconnect();
    registration.mutationObserver.disconnect();
    registration.observedChildren.clear();

    if (registration.pendingFrame !== null) cancelAnimationFrame(registration.pendingFrame);

    if (registration.applied) removeTabStopFromContainer(containerElement, registration);

    _autoTabindexRegistrations.delete(containerElement);

};

export { getScreenInnerWidth, registerContainerForAutoTabindex, unregisterContainerForAutoTabindex };