interface GoToData { markerId: string, popoverId: string, gotoText: string, gotoNumber: string};

const _dataAttribute    = "data-br-goto";
const _currentAttribute = "data-br-goto-current";

let _gotoComponentRegistered = false;



const clearCurrentMarker = (): void => {
    for (const marker of document.querySelectorAll(`[${_currentAttribute}]`)) {
        marker.removeAttribute(_currentAttribute);
    }
};

const hidePopoverOnFocusout = (e: FocusEvent): void => {

    const marker  = e.currentTarget as HTMLElement;
    const popover = marker.querySelector<HTMLDialogElement>("[popover]");
    if (popover?.matches(":popover-open")) popover.hidePopover();


};


const showGoTo = (markerId: string, popoverId:string): void => {

    const markerElement       = document.getElementById(markerId);
    const popoverLabelElement = document.getElementById(popoverId);

    if (!markerElement || !popoverLabelElement) return;

    markerElement.focus();

    if (popoverLabelElement.matches(':popover-open')) return;

    popoverLabelElement.showPopover({ source: markerElement });

    markerElement.addEventListener("focusout", hidePopoverOnFocusout, { once: true });

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

        if (!marker.checkVisibility({ visibilityProperty: true }) || marker.closest("[inert]")) continue;

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

const showGoToViewer = (gotoViewerId: string,): void => {

    const gotoViewerElement = document.getElementById(gotoViewerId);

    if (!gotoViewerElement) return;

    gotoViewerElement.showPopover();
   
};


const closeGoToViewer = (gotoViewerId: string,): void => {

    const gotoViewerElement = document.getElementById(gotoViewerId);

    if (!gotoViewerElement) return;

   if (gotoViewerElement.matches(":popover-open")) gotoViewerElement.hidePopover();

};

const handleViewerKeyDown = (e: KeyboardEvent): void => {


    if (e.altKey || e.ctrlKey || e.metaKey || e.shiftKey) return;

    const viewer = e.currentTarget as HTMLElement;
    const active = e.target as HTMLElement;
    const list = viewer.querySelector("ul");

    if (!list || (active !== viewer && !list.contains(active))) return;

    const buttons = Array.from(list.querySelectorAll<HTMLButtonElement>("button"));

    if (buttons.length === 0) return;

    const index = buttons.indexOf(active as HTMLButtonElement);   // -1 when focus is on the dialog

    let target: HTMLButtonElement | undefined;

    switch (e.key) {
        case "ArrowDown": target = buttons[Math.min(index + 1, buttons.length - 1)]; break;
        case "ArrowUp": target = index > 0 ? buttons[index - 1] : undefined; break;
        case "Home": target = buttons[0]; break;
        case "End": target = buttons[buttons.length - 1]; break;
        default: return;
    }

    e.preventDefault();
    target?.focus();
};


const registerGotoComponent = (dotNetRef: any, callBackName: string, gotoViewerId: string): void => {

    if (!dotNetRef || true === _gotoComponentRegistered) return;

    document.getElementById(gotoViewerId)?.addEventListener("keydown", handleViewerKeyDown);

    document.addEventListener("keydown", (event) => {

        if (event.ctrlKey && event.shiftKey && event.key.toUpperCase() === "Y") {
            event.preventDefault();
            callBlazor(dotNetRef, callBackName);
        }
    });

    _gotoComponentRegistered = true;
};


export { showGoTo, getGoToData, registerGotoComponent, scrollMarkerIntoView, showGoToViewer, closeGoToViewer };