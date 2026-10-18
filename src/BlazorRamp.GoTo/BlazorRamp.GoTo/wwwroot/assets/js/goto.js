;
const _dataAttribute = "data-br-goto";
const _currentAttribute = "data-br-goto-current";
let _gotoComponentRegistered = false;
const clearCurrentMarker = () => {
    for (const marker of document.querySelectorAll(`[${_currentAttribute}]`)) {
        marker.removeAttribute(_currentAttribute);
    }
};
const hidePopoverOnFocusout = (e) => {
    const marker = e.currentTarget;
    const popover = marker.querySelector("[popover]");
    if (popover?.matches(":popover-open"))
        popover.hidePopover();
};
const showGoTo = (markerId, popoverId) => {
    const markerElement = document.getElementById(markerId);
    const popoverLabelElement = document.getElementById(popoverId);
    if (!markerElement || !popoverLabelElement)
        return;
    if (popoverLabelElement.matches(':popover-open'))
        return;
    // Already in view for keyboard users, so a no-op there; brings it into view otherwise
    markerElement.scrollIntoView({ block: "start" });
    requestAnimationFrame(() => {
        document.documentElement.scrollTop = 0;
    });
    popoverLabelElement.showPopover({ source: markerElement });
    markerElement.addEventListener("focusout", hidePopoverOnFocusout, { once: true });
};
const scrollMarkerIntoView = (markerId) => {
    clearCurrentMarker();
    const markerElement = document.getElementById(markerId);
    if (!markerElement)
        return;
    markerElement.setAttribute(_currentAttribute, "");
    markerElement.scrollIntoView({ block: "start" });
    requestAnimationFrame(() => {
        document.documentElement.scrollTop = 0;
    });
};
const getGoToData = () => {
    clearCurrentMarker();
    const gotoData = [];
    const markers = document.querySelectorAll(`[${_dataAttribute}]`);
    for (const marker of markers) {
        if (!marker.checkVisibility({ visibilityProperty: true }) || marker.closest("[inert]"))
            continue;
        const label = marker.querySelector("[popover]");
        if (!marker.id || !label?.id)
            continue;
        const gotoNumber = String(gotoData.length + 1).padStart(2, "0");
        marker.setAttribute("data-br-goto-number", gotoNumber);
        label.setAttribute("data-br-goto-number", gotoNumber);
        gotoData.push({
            markerId: marker.id,
            popoverId: label.id,
            gotoText: label.textContent?.trim() ?? "",
            gotoNumber
        });
    }
    return gotoData;
};
const callBlazor = (dotNetRef, callBackName) => {
    if (!dotNetRef)
        return;
    const gotoData = getGoToData();
    dotNetRef.invokeMethodAsync(callBackName, gotoData).catch(() => { });
};
const showGoToViewer = (gotoViewerId) => {
    const gotoViewerElement = document.getElementById(gotoViewerId);
    if (!gotoViewerElement)
        return;
    gotoViewerElement.showPopover();
};
const closeGoToViewer = (gotoViewerId) => {
    const gotoViewerElement = document.getElementById(gotoViewerId);
    if (!gotoViewerElement)
        return;
    if (gotoViewerElement.matches(":popover-open"))
        gotoViewerElement.hidePopover();
};
const handleViewerKeyDown = (e) => {
    // Tabbing out of the viewer: close it now, so the browser carries on from where focus returns to
    if (e.key === "Tab" && !e.altKey && !e.ctrlKey && !e.metaKey) {
        const viewer = e.currentTarget;
        const active = e.target;
        const buttons = Array.from(viewer.querySelectorAll("button"));
        const leaving = e.shiftKey
            ? active === viewer || active === buttons[0]
            : active === buttons[buttons.length - 1];
        if (leaving && viewer.matches(":popover-open"))
            viewer.hidePopover();
        return;
    }
    if (e.altKey || e.ctrlKey || e.metaKey || e.shiftKey)
        return;
    const viewer = e.currentTarget;
    const active = e.target;
    const list = viewer.querySelector("ul");
    if (!list || (active !== viewer && !list.contains(active)))
        return;
    const buttons = Array.from(list.querySelectorAll("button"));
    if (buttons.length === 0)
        return;
    const index = buttons.indexOf(active);
    let target;
    switch (e.key) {
        case "ArrowDown":
            target = buttons[Math.min(index + 1, buttons.length - 1)];
            break;
        case "ArrowUp":
            target = index > 0 ? buttons[index - 1] : undefined;
            break;
        case "Home":
            target = buttons[0];
            break;
        case "End":
            target = buttons[buttons.length - 1];
            break;
        default: return;
    }
    e.preventDefault();
    target?.focus();
};
const registerGoToVieweHandlers = (gotoViewerId) => {
    const viewerPopover = document.getElementById(gotoViewerId);
    if (!viewerPopover)
        return;
    const handler = (e) => {
        if (viewerPopover.contains(e.relatedTarget))
            return;
        if (viewerPopover.matches(":popover-open"))
            viewerPopover.hidePopover();
    };
    viewerPopover.addEventListener("keydown", handleViewerKeyDown);
    viewerPopover.addEventListener("focusout", handler);
};
const registerGoToComponent = (dotNetRef, callBackName, gotoViewerId) => {
    if (!dotNetRef || true === _gotoComponentRegistered)
        return;
    registerGoToVieweHandlers(gotoViewerId);
    document.addEventListener("keydown", (event) => {
        if (!(event.ctrlKey && event.shiftKey && event.key.toUpperCase() === "Y"))
            return;
        const gotoViewerElement = document.getElementById(gotoViewerId);
        // No viewer, a modal dialog or fullscreen element is open, or the page is inert:
        if (!gotoViewerElement || document.querySelector(":modal") || gotoViewerElement.closest("[inert]"))
            return;
        event.preventDefault();
        callBlazor(dotNetRef, callBackName);
    });
    _gotoComponentRegistered = true;
};
export { showGoTo, getGoToData, registerGoToComponent, scrollMarkerIntoView, showGoToViewer, closeGoToViewer };
//# sourceMappingURL=goto.js.map