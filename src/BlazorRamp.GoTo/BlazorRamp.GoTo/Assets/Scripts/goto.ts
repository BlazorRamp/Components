interface GoToData { markerId: string, popoverId: string, gotoText: string, inactive: boolean };

const _dataAttribute = "data-br-goto-active";

let _shortCutCallBackRegistered = false;

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
    const markerElement = document.getElementById(markerId);

    if(markerElement) markerElement.scrollIntoView({ block: "start" });
   
}; 

const getGoToData = (): GoToData[] => {

    const gotoData: GoToData[] = [];

    const markers = document.querySelectorAll<HTMLElement>(`[${_dataAttribute}]`);

    markers.forEach(marker => {

        const label = marker.querySelector<HTMLElement>("[popover]");

        if (!marker.id || !label?.id) return;

        gotoData.push({
            markerId: marker.id,
            popoverId: label.id,
            gotoText: label.textContent?.trim() ?? "",
            inactive: marker.getAttribute(_dataAttribute)?.toLowerCase() !== "true"
        });
    });

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