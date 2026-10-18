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


const showGoTo = (markerId: string, popoverId: string): void => {

    const markerElement = document.getElementById(markerId);
    const popoverLabelElement = document.getElementById(popoverId);

    if (!markerElement || !popoverLabelElement) return;

    if (popoverLabelElement.matches(':popover-open')) return;

    // Already in view for keyboard users, so a no-op there; brings it into view otherwise
    markerElement.scrollIntoView({ block: "start" });

    requestAnimationFrame(() => {
        document.documentElement.scrollTop = 0;
    });

    popoverLabelElement.showPopover({ source: markerElement });

    markerElement.addEventListener("focusout", hidePopoverOnFocusout, { once: true });
};
const scrollMarkerIntoView = (markerId: string): void => {

    clearCurrentMarker();

    const markerElement = document.getElementById(markerId);

    if (!markerElement) return;

    markerElement.setAttribute(_currentAttribute, "");
    markerElement.scrollIntoView({ block: "start" });


    requestAnimationFrame(() => {
        document.documentElement.scrollTop = 0;
    });
   
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

    dotNetRef.invokeMethodAsync(callBackName, gotoData).catch(() => { });
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

    // Tabbing out of the viewer: close it now, so the browser carries on from where focus returns to
    if (e.key === "Tab" && !e.altKey && !e.ctrlKey && !e.metaKey) {

        const viewer = e.currentTarget as HTMLElement;
        const active = e.target as HTMLElement;
        const buttons = Array.from(viewer.querySelectorAll<HTMLButtonElement>("button"));

        const leaving = e.shiftKey
            ? active === viewer || active === buttons[0]
            : active === buttons[buttons.length - 1];

        if (leaving && viewer.matches(":popover-open")) viewer.hidePopover();

        return;
    }

    if (e.altKey || e.ctrlKey || e.metaKey || e.shiftKey) return;

    const viewer = e.currentTarget as HTMLElement;
    const active = e.target as HTMLElement;
    const list = viewer.querySelector("ul");

    if (!list || (active !== viewer && !list.contains(active))) return;

    const buttons = Array.from(list.querySelectorAll<HTMLButtonElement>("button"));

    if (buttons.length === 0) return;

    const index = buttons.indexOf(active as HTMLButtonElement);

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
const registerGoToVieweHandlers = (gotoViewerId:string): void => {

    const viewerPopover = document.getElementById(gotoViewerId);

    if (!viewerPopover) return;

    const handler = (e: FocusEvent): void => {

        if (viewerPopover.contains(e.relatedTarget as Node)) return;

        if (viewerPopover.matches(":popover-open")) viewerPopover.hidePopover();
    }


    viewerPopover.addEventListener("keydown", handleViewerKeyDown);
    viewerPopover.addEventListener("focusout", handler);
};

const registerGoToComponent = (dotNetRef: any, callBackName: string, gotoViewerId: string): void => {

    if (!dotNetRef || true === _gotoComponentRegistered) return;

    registerGoToVieweHandlers(gotoViewerId);

    document.addEventListener("keydown", (event) => {

        if (!(event.ctrlKey && event.shiftKey && event.key.toUpperCase() === "Y")) return;

        const gotoViewerElement = document.getElementById(gotoViewerId);

        // No viewer, a modal dialog or fullscreen element is open, or the page is inert:
        if (!gotoViewerElement || document.querySelector(":modal") || gotoViewerElement.closest("[inert]")) return;

        event.preventDefault();
        callBlazor(dotNetRef, callBackName);
    });

    _gotoComponentRegistered = true;
};

export { showGoTo, getGoToData, registerGoToComponent, scrollMarkerIntoView, showGoToViewer, closeGoToViewer };