interface GoToData { markerId: string, popoverId: string, gotoText: string, gotoNumber: string};

const _dataAttribute    = "data-br-goto";
const _currentAttribute = "data-br-goto-current";

let _shortCutCallBackRegistered = false;


const clearCurrentMarker = (): void => {
    for (const marker of document.querySelectorAll(`[${_currentAttribute}]`)) {
        marker.removeAttribute(_currentAttribute);
    }
};

const hidePopoverOnBlur = (e: FocusEvent): void => {

    const marker  = e.currentTarget as HTMLElement;
    const popover = marker.querySelector<HTMLElement>("[popover]");

    if (popover?.matches(":popover-open")) popover.hidePopover();
};


const showGoTo = (markerId: string, popoverId:string): void => {

    const markerElement       = document.getElementById(markerId);
    const popoverLabelElement = document.getElementById(popoverId);

    if (!markerElement || !popoverLabelElement) return;

    markerElement.focus();

    if (popoverLabelElement.matches(':popover-open')) return;

    popoverLabelElement.showPopover({ source: markerElement });

    markerElement.addEventListener("blur", hidePopoverOnBlur, { once: true });

};

const scrollMarkerIntoView = (markerId: string): void => {

    clearCurrentMarker()

    const markerElement = document.getElementById(markerId);

    if (!markerElement) return;

    markerElement.setAttribute(_currentAttribute, "");
    markerElement.scrollIntoView({ block: "start" });
   
}; 

const getGoToData = (): GoToData[] => {

    clearCurrentMarker();

    const gotoData: GoToData[] = [];

    const markers = document.querySelectorAll<HTMLElement>(`[${_dataAttribute}]`);

    for (const marker of markers) {

        const label = marker.querySelector<HTMLElement>("[popover]");

        if (!marker.id || !label?.id) continue;

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


const callBlazor = (dotNetRef:any, callBackName:string): void => {

    if (!dotNetRef) return;

    const gotoData: GoToData[] = getGoToData();

    dotNetRef.invokeMethodAsync(callBackName, gotoData);
};

const showGoToViewer = (gotoViewerID: string,): void => {

    const gotoViewerElement = document.getElementById(gotoViewerID);

    if (!gotoViewerElement) return;

    gotoViewerElement.showPopover();
    gotoViewerElement.focus();
};


const registerShortCutCallBack = ( dotNetRef:any, callBackName:string): void => {

    if (!dotNetRef || true === _shortCutCallBackRegistered) return;

    document.addEventListener("keydown", (event) => {

        if (event.ctrlKey && event.shiftKey && event.key.toUpperCase() === "Y") {
            event.preventDefault();
            callBlazor(dotNetRef,callBackName);
        }
    });

    _shortCutCallBackRegistered = true;
};

export { showGoTo, getGoToData, registerShortCutCallBack, scrollMarkerIntoView, showGoToViewer };