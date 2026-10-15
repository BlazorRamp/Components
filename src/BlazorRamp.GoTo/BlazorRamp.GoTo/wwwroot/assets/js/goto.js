;
const _dataAttribute = "data-br-goto";
const _currentAttribute = "data-br-goto-current";
let _shortCutCallBackRegistered = false;
const clearCurrentMarker = () => {
    for (const marker of document.querySelectorAll(`[${_currentAttribute}]`)) {
        marker.removeAttribute(_currentAttribute);
    }
};
const hidePopoverOnBlur = (e) => {
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
    markerElement.focus();
    if (popoverLabelElement.matches(':popover-open'))
        return;
    popoverLabelElement.showPopover({ source: markerElement });
    markerElement.addEventListener("blur", hidePopoverOnBlur, { once: true });
};
const scrollMarkerIntoView = (markerId) => {
    clearCurrentMarker();
    const markerElement = document.getElementById(markerId);
    if (!markerElement)
        return;
    markerElement.setAttribute(_currentAttribute, "");
    markerElement.scrollIntoView({ block: "start" });
};
const getGoToData = () => {
    clearCurrentMarker();
    const gotoData = [];
    const markers = document.querySelectorAll(`[${_dataAttribute}]`);
    for (const marker of markers) {
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
    dotNetRef.invokeMethodAsync(callBackName, gotoData);
};
const showGoToViewer = (gotoViewerID) => {
    const gotoViewerElement = document.getElementById(gotoViewerID);
    if (!gotoViewerElement)
        return;
    gotoViewerElement.showPopover();
    gotoViewerElement.focus();
};
const closeGoToViewer = (gotoViewerID) => {
    const gotoViewerElement = document.getElementById(gotoViewerID);
    if (!gotoViewerElement)
        return;
    if (gotoViewerElement.matches(":popover-open"))
        gotoViewerElement.hidePopover();
};
const registerShortCutCallBack = (dotNetRef, callBackName) => {
    if (!dotNetRef || true === _shortCutCallBackRegistered)
        return;
    document.addEventListener("keydown", (event) => {
        if (event.ctrlKey && event.shiftKey && event.key.toUpperCase() === "Y") {
            event.preventDefault();
            callBlazor(dotNetRef, callBackName);
        }
    });
    _shortCutCallBackRegistered = true;
};
export { showGoTo, getGoToData, registerShortCutCallBack, scrollMarkerIntoView, showGoToViewer, closeGoToViewer };
//# sourceMappingURL=goto.js.map