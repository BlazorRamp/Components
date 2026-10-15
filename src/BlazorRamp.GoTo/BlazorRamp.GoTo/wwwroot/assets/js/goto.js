;
const _dataAttribute = "data-br-goto-active";
let _shortCutCallBackRegistered = false;
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
    const markerElement = document.getElementById(markerId);
    if (markerElement)
        markerElement.scrollIntoView({ block: "start" });
};
const getGoToData = () => {
    const gotoData = [];
    const markers = document.querySelectorAll(`[${_dataAttribute}]`);
    markers.forEach(marker => {
        const label = marker.querySelector("[popover]");
        if (!marker.id || !label?.id)
            return;
        gotoData.push({
            markerId: marker.id,
            popoverId: label.id,
            gotoText: label.textContent?.trim() ?? "",
            inactive: marker.getAttribute(_dataAttribute)?.toLowerCase() !== "true"
        });
    });
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
export { showGoTo, getGoToData, registerShortCutCallBack, scrollMarkerIntoView, showGoToViewer };
//# sourceMappingURL=goto.js.map