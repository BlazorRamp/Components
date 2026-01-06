enum AnnouncementType {
    Info = "Info", OperationStarted = "OperationStarted", OperationCompleted = "OperationCompleted", OperationFailed = "OperationFailed",
    OperationCancelled = "OperationCancelled", SystemWarning = "SystemWarning", SystemError = "SystemError"
}
enum LiveRegionType {Polite = "Polite", Assertive = "Assertive"}
interface Announcement { message: string; announcementType: AnnouncementType; announcementTrigger?: string; liveRegionType: LiveRegionType; }
interface AnnouncementRecord extends Announcement { announcementRecordID: number; timestamp: string; page: string; } 
interface QueueItem { element: HTMLElement; message: string };


const QUEUE_LENGTH = 20;

let _liveRegionPoliteOne     = document.getElementById("blazor-ramp-live-region-polite_one");
let _liveRegionPoliteTwo     = document.getElementById("blazor-ramp-live-region-polite_two");
let _liveRegionAssertiveOne  = document.getElementById("blazor-ramp-live-region-assertive_one");
let _liveRegionAssertiveTwo  = document.getElementById("blazor-ramp-live-region-assertive_two");
let _liveRegionPoliteToggle     = false;
let _liveRegionsAssertiveToggle = false

const _messageQueue: QueueItem[]          = [];
const _historyQueue: AnnouncementRecord[] = [];

let _isAnnouncing       = false;
let _announceRegistered = false
let _messageCounter     = 0;
let focusBeforePopover: HTMLElement | null = null;

let elementsForLocationChanged: { containerElement: HTMLElement | null, componentsElement: HTMLElement | null, popoverElement: HTMLElement | null, triggerElement: HTMLButtonElement | null };

/*
    * Bug in api spent see https://github.com/whatwg/html/issues/10890a 
    * Spnet a full day trying all combinations of things to have ligtht dismiss but with the button being clicked to both
    * open and close the popover. Its this toggling behavious that is the issue as the repoprted state is not correct.
    * The state flag is the best workaround I found.
*/
let _isPopoverOpen: boolean = false;

const getLocalIsoTimestamp = () => {

    const now = new Date();
    const pad = (num) => String(num).padStart(2, "0");

    return (now.getFullYear() + "-" + pad(now.getMonth() + 1) + "-" + pad(now.getDate()) + "T" + pad(now.getHours()) + ":" + pad(now.getMinutes()) + ":" + pad(now.getSeconds()));
}


const delay = (ms: number) => new Promise(resolve => setTimeout(resolve, ms));

const getNextID = () => _messageCounter++; 

const addToHistoryQueue = (announcementRecord: AnnouncementRecord): void => {

    if (_historyQueue.length >= QUEUE_LENGTH) _historyQueue.shift();

    _historyQueue.push(announcementRecord);
};


const announcement = (announcement: Announcement, replayable: boolean = true) => {

    if (!announcement || !_liveRegionPoliteOne || !_liveRegionAssertiveOne || !_liveRegionPoliteTwo || !_liveRegionAssertiveTwo || announcement.message?.trim().length === 0) return;

    let liveRegion: HTMLElement;

    if (announcement.liveRegionType === LiveRegionType.Polite) {
        _liveRegionPoliteToggle = !_liveRegionPoliteToggle;
        liveRegion = _liveRegionPoliteToggle ? _liveRegionPoliteOne : _liveRegionPoliteTwo;
    }
    else {
        _liveRegionsAssertiveToggle = !_liveRegionsAssertiveToggle;
        liveRegion = _liveRegionsAssertiveToggle ? _liveRegionAssertiveOne : _liveRegionAssertiveTwo;
    }

    const queueItem: QueueItem = { element: liveRegion, message: announcement.message };

    if (replayable) {

        const trigger = (announcement.announcementTrigger ?? "").trim();

        const announcementRecord: AnnouncementRecord = {announcementRecordID: getNextID(), timestamp: getLocalIsoTimestamp(), page: document.title,
            message: announcement.message, announcementType: announcement.announcementType, announcementTrigger: trigger, liveRegionType: announcement.liveRegionType}

        addToHistoryQueue(announcementRecord);
    }

    _messageQueue.push(queueItem);

    if (!_isAnnouncing) processQueue();

};

const processQueue = async () => {
    _isAnnouncing = true;

    while (_messageQueue.length > 0) {

        const queueItem = _messageQueue.shift();

        if (queueItem) await makeAnnouncement(queueItem);
    }

    _isAnnouncing = false;
};


let _toggleChar:boolean = false;

const makeAnnouncement = async (queueItem: QueueItem): Promise<void> => {

    const { element, message } = queueItem;

    let messageText = (message === null || message === undefined) ? "" : message.trim();

    if (!element) return;

    const nbsp = "\u00A0";

    _toggleChar = !_toggleChar;

    element.textContent = "\u200B";

    await delay(400)

    element.textContent = _toggleChar ? messageText : messageText + nbsp; 

    await delay(400); 

    element.setAttribute('aria-hidden', 'true');

    await delay(800); 

    element.textContent = "";
    element.removeAttribute('aria-hidden');

   

}

type RelativeTimeUnit = 'year' | 'month' | 'day' | 'hour' | 'minute' | 'second';

const isNullOrWhitespace = (input: string): boolean => !input || input.trim().length === 0;

const formatTimestampLocalized = (isoTimestamp: string, locale: string = 'en-GB'): string => {

    const timeStamp = new Date(isoTimestamp);
    const timeNow = new Date();

    const diffMs = timeNow.getTime() - timeStamp.getTime();
    const diffSeconds = Math.round(diffMs / 1000); // Use round for better precision

    // Initialize the RelativeTimeFormat object
    const rtf = new Intl.RelativeTimeFormat(locale, {numeric: 'auto', style: 'long'});

    const diffMinutes = Math.floor(diffSeconds / 60);
    const diffHours   = Math.floor(diffMinutes / 60);
    const diffDays    = Math.floor(diffHours / 24);
    const diffMonths  = Math.floor(diffDays / 30);
    const diffYears   = Math.floor(diffDays / 365);

    // Note: We pass a NEGATIVE value to RTF to indicate time in the PAST ("ago")

    if (diffSeconds < 60)  return rtf.format(-diffSeconds, 'second');

    if (diffMinutes < 60) return rtf.format(-diffMinutes, 'minute');

    if (diffHours < 24) return rtf.format(-diffHours, 'hour');

    if (diffDays < 30) return rtf.format(-diffDays, 'day');
    
    if (diffMonths < 12) return rtf.format(-diffMonths, 'month');
        
    return rtf.format(-diffYears, 'year');
};

const buildAnnouncementList = (ahContentElement: HTMLElement, locale: string) => {

    if (!ahContentElement || !ahContentElement.firstElementChild) return;

    const olElement   = ahContentElement.firstElementChild as HTMLElement;
    const paraElement = ahContentElement.lastElementChild as HTMLElement;

    locale = isNullOrWhitespace(locale) ? "en-GB" : locale.trim();

    paraElement.style.display = "none";
    olElement.innerHTML   = "";

    for (const record of [..._historyQueue].reverse()) {

        const li        = document.createElement("li");
        const time      = formatTimestampLocalized(record.timestamp, locale) + ";";
        const page      = record.page === "" ? "" : record.page.trim() + ";";
        const trigger   = (!record.announcementTrigger || record.announcementTrigger.trim().length === 0) ? "" : record.announcementTrigger.trim() + ";";
        li.textContent  = `${time} ${page} ${trigger} ${record.message}`;

        olElement.appendChild(li);
    }

    if (_historyQueue.length === 0) paraElement.style.display = "block";
};


const setPopoverState = (isOpen: boolean, triggerButton: HTMLButtonElement, popoverElement: HTMLElement):void => {

    if (isOpen) {
        popoverElement.showPopover();
    }
    else { popoverElement.hidePopover();}

    _isPopoverOpen = isOpen;

    if (triggerButton) triggerButton.setAttribute("aria-expanded", isOpen.toString());
}

const clearHistoryAndClosePopover = (popoverElement: HTMLElement, triggerButton:HTMLButtonElement) => {

    _historyQueue.length = 0;
    setPopoverState(false, triggerButton, popoverElement);
};

const checkMoveComponentsElement = (componentsElement: HTMLElement, originalParentElement: HTMLElement) => {

    if (!componentsElement || !originalParentElement) return;

    const openDialogs = Array.from(document.querySelectorAll("dialog")).filter(dialog => dialog.open);

    if (openDialogs.length > 0) {
        const topDialog = openDialogs[openDialogs.length - 1];
        topDialog.appendChild(componentsElement);
        return;
    }

    if (componentsElement.parentElement !== originalParentElement) originalParentElement.appendChild(componentsElement);
}

const registerCloseButtonHandler = (closeButton: HTMLButtonElement, popoverElement: HTMLElement, triggerButton: HTMLButtonElement) => {

    if (!closeButton || !popoverElement || !triggerButton) return;

    closeButton.addEventListener("click", (_) =>  setPopoverState(false, triggerButton, popoverElement));
};

const registerClearButtonHandler = (clearButton: HTMLButtonElement, popoverElement:HTMLElement, triggerButton: HTMLButtonElement ) => {

    if (!clearButton || !popoverElement || !triggerButton) return;

    clearButton.addEventListener("click", (_) => clearHistoryAndClosePopover(popoverElement, triggerButton));
};

const registerTriggerButtonHandler = (triggerButton:HTMLButtonElement, popoverElement:HTMLElement, contentElement:HTMLElement, locale:string = "en-GB") => {

    if (!triggerButton || !popoverElement || !contentElement) return;

    triggerButton.addEventListener("click", (event) => {

        if (_isPopoverOpen === true) {
            setPopoverState(false, triggerButton, popoverElement);
            return;
        }
        buildAnnouncementList(contentElement, locale);
        setPopoverState(true, triggerButton, popoverElement);
    });
}

const registerPopoverToggleHandler = (popoverElement: HTMLElement, triggerButton: HTMLButtonElement): void => {

    if (!popoverElement) return;

    popoverElement.addEventListener('toggle', (event: ToggleEvent) => {
        const isOpen = event.newState === 'open';

        triggerButton.setAttribute("aria-expanded", isOpen.toString());
        _isPopoverOpen = isOpen;
    });
};

const registerDocumentKeyDownHandler = (componentsElement: HTMLElement, originalParentElement: HTMLElement, contentElement: HTMLElement, popoverElement: HTMLElement,
                                        triggerButton: HTMLButtonElement, locale: string = "en-GB"): void => {

    document.addEventListener("keydown", (event) => {

        if (event.ctrlKey && event.shiftKey && event.key.toUpperCase() === "H") {
            event.preventDefault();
            if (_isPopoverOpen) return;
            checkMoveComponentsElement(componentsElement, originalParentElement);
            buildAnnouncementList(contentElement, locale);
            setPopoverState(true, triggerButton, popoverElement);
        }
    });
};

const registerGlobalDialogWatcher = (containerElement: HTMLElement, componentsElement: HTMLElement, popoverElement: HTMLElement, triggerButton: HTMLButtonElement) => {

    document.addEventListener("keydown", (e) => {

        if (e.key !== "Escape") return;

        if (popoverElement && popoverElement.matches(":popover-open")) {
            popoverElement.hidePopover();
            setPopoverState(false, triggerButton, popoverElement);
            e.preventDefault();
            return;
        }
   
        const openDialogs = Array.from(document.querySelectorAll("dialog")).filter(dialog => dialog.open);
        const topmostDialog = openDialogs.length > 0 ? openDialogs[openDialogs.length - 1] : null;

        if (topmostDialog && topmostDialog.contains(componentsElement)) {

            if (componentsElement.parentElement !== containerElement) containerElement.appendChild(componentsElement);
        }
    });

    document.addEventListener("close", (e) => {
        const closedDialog = e.target as HTMLElement;

        if (closedDialog.tagName === "DIALOG" && closedDialog.contains(componentsElement)) {

            if (componentsElement.parentElement !== containerElement) containerElement.appendChild(componentsElement);
        }
    });
}

const registerMutationObserver = (containerElement: HTMLElement, componentsElement: HTMLElement): void => {

    const handleDialogOpened: MutationCallback = (mutationsList: MutationRecord[], observer: MutationObserver) => {

        const dialogOpened = mutationsList.some((mutation: MutationRecord) => {

            const targetElement = mutation.target as HTMLElement;

            return (mutation.type === 'attributes' && mutation.attributeName === 'open' && targetElement.tagName === 'DIALOG' && targetElement.hasAttribute('open'));
        });

        if (dialogOpened) checkMoveComponentsElement(componentsElement, containerElement)
    };

    const observer: MutationObserver = new MutationObserver(handleDialogOpened);

    const config: MutationObserverInit = {attributes: true,subtree: true, attributeFilter: ['open']};

    observer.observe(document.body, config);
};

const registerRefreshButtonHandler = (refreshButton: HTMLButtonElement, contentElement: HTMLElement, locale: string = "en-GB") => {

    if (!refreshButton || !contentElement) return;

    refreshButton.addEventListener("click", _ => {

        buildAnnouncementList(contentElement, locale);
        contentElement.focus();
    });
};

const getElementsForLocationChanged = () => {

    if (!elementsForLocationChanged) {
        elementsForLocationChanged = {
            containerElement: document.getElementById("blazor-ramp-announcement-history"),
            componentsElement: document.getElementById("blazor-ramp-announcement-history-components"),
            popoverElement: document.getElementById("blazor-ramp-announcement-history-dialog"),
            triggerElement: document.getElementById("blazor-ramp-announcement-history-trigger") as HTMLButtonElement,
        };
    }
    return elementsForLocationChanged;
};

function closePopoverOnLocationChanged():void  {

    const { containerElement, componentsElement, popoverElement, triggerElement } = getElementsForLocationChanged();

    if (popoverElement && triggerElement && _isPopoverOpen) setPopoverState(false, triggerElement, popoverElement);

    if (componentsElement && containerElement) checkResetComponentsLocation(componentsElement, containerElement);
    
};

const checkResetComponentsLocation = (componentsElement:HTMLElement, originalParentElement: HTMLElement): void => {

    if (!componentsElement || !originalParentElement) return;

    if (componentsElement.parentElement !== originalParentElement) originalParentElement.appendChild(componentsElement);

};

const checkMoveElementToBody = (containerElement:HTMLElement) => {
    /*
        * To simplify things the live regionss are also in the container.
    */
    if (!containerElement) return;

    if (containerElement.parentElement !== document.body) document.body.appendChild(containerElement);
};

const registerLiveRegionAndHistory = () => {

    if (_announceRegistered) return;

    _liveRegionPoliteOne    = document.getElementById("blazor-ramp-live-region-polite_one");
    _liveRegionAssertiveOne = document.getElementById("blazor-ramp-live-region-assertive_one");
    _liveRegionPoliteTwo = document.getElementById("blazor-ramp-live-region-polite_two");
    _liveRegionAssertiveTwo = document.getElementById("blazor-ramp-live-region-assertive_two");

    const containerElement = document.getElementById("blazor-ramp-announcement-history") as HTMLElement;

    if (!containerElement) return;
    
    const componentsElement = document.getElementById("blazor-ramp-announcement-history-components") as HTMLElement;
    const popoverElement    = document.getElementById("blazor-ramp-announcement-history-dialog") as HTMLDialogElement;
    const ahContentElement  = document.getElementById("blazor-ramp-announcement-history-content") as HTMLElement;
    const closeButton       = document.getElementById("blazor-ramp-announcement-history-close") as HTMLButtonElement;
    const clearButton       = document.getElementById("blazor-ramp-announcement-history-clear") as HTMLButtonElement;
    const refreshButton     = document.getElementById("blazor-ramp-announcement-history-refresh") as HTMLButtonElement;
    const triggerButton     = document.getElementById("blazor-ramp-announcement-history-trigger") as HTMLButtonElement;

    const originalParent = componentsElement?.parentElement as HTMLElement;

    const locale = popoverElement.getAttribute("data-br-locale") ?? "en-GB";

    //registerLocationChangedHandler(containerElement, componentsElement, popoverElement, triggerButton);

    registerTriggerButtonHandler(triggerButton, popoverElement, ahContentElement, locale);

    registerCloseButtonHandler(closeButton, popoverElement, triggerButton);
    
    registerClearButtonHandler(clearButton, popoverElement, triggerButton);

    registerRefreshButtonHandler(refreshButton, ahContentElement, locale);

    registerPopoverToggleHandler(popoverElement, triggerButton);

    registerDocumentKeyDownHandler(componentsElement, originalParent, ahContentElement, popoverElement,triggerButton, locale);

    registerGlobalDialogWatcher(containerElement, componentsElement, popoverElement, triggerButton);

    registerMutationObserver(containerElement, componentsElement);

    checkMoveElementToBody(containerElement);

    _announceRegistered = true;
}

export { announcement, closePopoverOnLocationChanged };

const tryRegister = (attemptsLeft = 600) => {

    if (_announceRegistered) return;

    registerLiveRegionAndHistory();

    if (!_announceRegistered && attemptsLeft > 0) setTimeout(() => tryRegister(attemptsLeft - 1), 1000); 
};

const start = () => {

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => tryRegister());
        return;
    }

    tryRegister();
}

start();