;
;
const _dataAttribute = "data-br-goto";
const _currentAttribute = "data-br-goto-current";
const _handledViewers = new WeakSet();
let _shortcut = null;
let _dotNetRef = null;
let _callBackName = "";
let _viewerId = "";
let _keydownListenerAdded = false;
const matchesShortcut = (e, s) => {
    // Exact modifier match, which also rules out AltGr (reported as Ctrl + Alt) when Ctrl isn't wanted
    if (e.altKey !== s.alt || e.ctrlKey !== s.ctrl || e.shiftKey !== s.shift || e.metaKey !== s.meta)
        return false;
    const key = s.key.toLowerCase();
    const code = /^[a-z]$/.test(key) ? `Key${key.toUpperCase()}` : `Digit${key}`;
    // event.code is the physical key, so it still matches when Option/Alt types a different character on a Mac
    return e.code === code || e.key.toLowerCase() === key;
};
const setGoToShortcut = (shortcut) => {
    _shortcut = shortcut;
};
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
const pageIsNotUserScrollable = () => {
    const htmlOverflow = getComputedStyle(document.documentElement).overflowY;
    const effective = htmlOverflow !== "visible" ? htmlOverflow : getComputedStyle(document.body).overflowY;
    return effective === "hidden" || effective === "clip";
};
const scrollMarkerTo = (marker) => {
    const root = document.documentElement;
    const before = root.scrollTop;
    marker.scrollIntoView({ block: "start" });
    if (pageIsNotUserScrollable())
        requestAnimationFrame(() => { root.scrollTop = before; });
};
const showGoTo = (markerId, popoverId) => {
    const markerElement = document.getElementById(markerId);
    const popoverLabelElement = document.getElementById(popoverId);
    if (!markerElement || !popoverLabelElement)
        return;
    if (popoverLabelElement.matches(':popover-open'))
        return;
    scrollMarkerTo(markerElement);
    popoverLabelElement.showPopover({ source: markerElement });
    markerElement.addEventListener("focusout", hidePopoverOnFocusout, { once: true });
};
const scrollMarkerIntoView = (markerId) => {
    clearCurrentMarker();
    const markerElement = document.getElementById(markerId);
    if (!markerElement)
        return;
    markerElement.setAttribute(_currentAttribute, "");
    scrollMarkerTo(markerElement);
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
    if (!viewerPopover || _handledViewers.has(viewerPopover))
        return;
    _handledViewers.add(viewerPopover);
    const handler = (e) => {
        if (viewerPopover.contains(e.relatedTarget))
            return;
        if (viewerPopover.matches(":popover-open"))
            viewerPopover.hidePopover();
    };
    viewerPopover.addEventListener("keydown", handleViewerKeyDown);
    viewerPopover.addEventListener("focusout", handler);
};
const onDocumentKeyDown = (event) => {
    if (!_dotNetRef || !_shortcut || event.repeat || !matchesShortcut(event, _shortcut))
        return;
    const gotoViewerElement = document.getElementById(_viewerId);
    // No viewer, a modal dialog or fullscreen element is open, or the page is inert:
    if (!gotoViewerElement || document.querySelector(":modal") || gotoViewerElement.closest("[inert]"))
        return;
    event.preventDefault();
    callBlazor(_dotNetRef, _callBackName);
};
const registerGoToComponent = (dotNetRef, callBackName, gotoViewerId, shortcut) => {
    if (!dotNetRef)
        return;
    _dotNetRef = dotNetRef;
    _callBackName = callBackName;
    _viewerId = gotoViewerId;
    _shortcut = shortcut;
    registerGoToVieweHandlers(gotoViewerId);
    if (!_keydownListenerAdded) {
        document.addEventListener("keydown", onDocumentKeyDown);
        _keydownListenerAdded = true;
    }
};
export { showGoTo, getGoToData, registerGoToComponent, setGoToShortcut, scrollMarkerIntoView, showGoToViewer, closeGoToViewer };
//# sourceMappingURL=goto.js.map