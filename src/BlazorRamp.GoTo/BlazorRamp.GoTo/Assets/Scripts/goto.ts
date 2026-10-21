interface GoToData { markerId: string, popoverId: string, gotoText: string, gotoNumber: string};
interface GoToShortcut { key: string, alt: boolean, ctrl: boolean, shift: boolean, meta: boolean };

const _dataAttribute    = "data-br-goto";
const _currentAttribute = "data-br-goto-current";
const _handledViewers = new WeakSet<HTMLElement>();

let _shortcut: GoToShortcut | null = null;

let _dotNetRef: any = null;
let _callBackName = "";
let _viewerId = "";
let _keydownListenerAdded = false;


const matchesShortcut = (e: KeyboardEvent, s: GoToShortcut): boolean => {

    // Exact modifier match, which also rules out AltGr (reported as Ctrl + Alt) when Ctrl isn't wanted
    if (e.altKey !== s.alt || e.ctrlKey !== s.ctrl || e.shiftKey !== s.shift || e.metaKey !== s.meta) return false;

    const key = s.key.toLowerCase();
    const code = /^[a-z]$/.test(key) ? `Key${key.toUpperCase()}` : `Digit${key}`;

    // event.code is the physical key, so it still matches when Option/Alt types a different character on a Mac
    return e.code === code || e.key.toLowerCase() === key;
};

const setGoToShortcut = (shortcut: GoToShortcut): void => {
    _shortcut = shortcut;
};

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

const pageIsNotUserScrollable = (): boolean => {
    const htmlOverflow = getComputedStyle(document.documentElement).overflowY;
    const effective = htmlOverflow !== "visible" ? htmlOverflow : getComputedStyle(document.body).overflowY;
    return effective === "hidden" || effective === "clip";
};

const scrollMarkerTo = (marker: HTMLElement): void => {

    const root = document.documentElement;
    const before = root.scrollTop;

    marker.scrollIntoView({ block: "start" });

    if (pageIsNotUserScrollable()) requestAnimationFrame(() => { root.scrollTop = before; });
};

const showGoTo = (markerId: string, popoverId: string): void => {

    const markerElement = document.getElementById(markerId);
    const popoverLabelElement = document.getElementById(popoverId);

    if (!markerElement || !popoverLabelElement) return;

    if (popoverLabelElement.matches(':popover-open')) return;

    scrollMarkerTo(markerElement);

    popoverLabelElement.showPopover({ source: markerElement });

    markerElement.addEventListener("focusout", hidePopoverOnFocusout, { once: true });
};
const scrollMarkerIntoView = (markerId: string): void => {

    clearCurrentMarker();

    const markerElement = document.getElementById(markerId);

    if (!markerElement) return;

    markerElement.setAttribute(_currentAttribute, "");
    scrollMarkerTo(markerElement);
   
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


const registerGoToVieweHandlers = (gotoViewerId: string): void => {

    const viewerPopover = document.getElementById(gotoViewerId);

    if (!viewerPopover || _handledViewers.has(viewerPopover)) return;

    _handledViewers.add(viewerPopover);

    const handler = (e: FocusEvent): void => {

        if (viewerPopover.contains(e.relatedTarget as Node)) return;

        if (viewerPopover.matches(":popover-open")) viewerPopover.hidePopover();
    };

    viewerPopover.addEventListener("keydown", handleViewerKeyDown);
    viewerPopover.addEventListener("focusout", handler);
};

const onDocumentKeyDown = (event: KeyboardEvent): void => {

    if (!_dotNetRef || !_shortcut || event.repeat || !matchesShortcut(event, _shortcut)) return;

    const gotoViewerElement = document.getElementById(_viewerId);

    // No viewer, a modal dialog or fullscreen element is open, or the page is inert:
    if (!gotoViewerElement || document.querySelector(":modal") || gotoViewerElement.closest("[inert]")) return;

    event.preventDefault();
    callBlazor(_dotNetRef, _callBackName);
};

const registerGoToComponent = (dotNetRef: any, callBackName: string, gotoViewerId: string, shortcut: GoToShortcut): void => {

    if (!dotNetRef) return;

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
