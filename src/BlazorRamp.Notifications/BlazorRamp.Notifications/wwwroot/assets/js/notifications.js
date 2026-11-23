var NotificationType;
(function (NotificationType) {
    NotificationType["Info"] = "Info";
})(NotificationType || (NotificationType = {}));
const SESSION_STORAGE_KEY = "br-notifications";
const MAX_NOTIFICATIONS = 50;
const LOCALE = navigator.language || "en-GB";
let _userStoreKey = ""; // empty means memory only, nothing is saved to session storage
let _notificationItems = [];
let _registeredServices = [];
const formatDisplayTime = (createdAt, locale = LOCALE) => {
    const timeStamp = new Date(createdAt);
    const timeNow = new Date();
    const diffMs = timeNow.getTime() - timeStamp.getTime();
    const diffSeconds = Math.round(diffMs / 1000); // Use round for better precision
    // Initialize the RelativeTimeFormat object
    const rtf = new Intl.RelativeTimeFormat(locale, { numeric: 'auto', style: 'long' });
    const diffMinutes = Math.floor(diffSeconds / 60);
    const diffHours = Math.floor(diffMinutes / 60);
    const diffDays = Math.floor(diffHours / 24);
    const diffMonths = Math.floor(diffDays / 30);
    const diffYears = Math.floor(diffDays / 365);
    // Note: We pass a NEGATIVE value to RTF to indicate time in the PAST ("ago")
    if (diffSeconds < 60)
        return rtf.format(-diffSeconds, 'second');
    if (diffMinutes < 60)
        return rtf.format(-diffMinutes, 'minute');
    if (diffHours < 24)
        return rtf.format(-diffHours, 'hour');
    if (diffDays < 30)
        return rtf.format(-diffDays, 'day');
    if (diffMonths < 12)
        return rtf.format(-diffMonths, 'month');
    return rtf.format(-diffYears, 'year');
};
const refreshActiveDisplayTimes = () => {
    for (const item of _notificationItems.filter(item => !item.isDeleted)) {
        item.displayTime = formatDisplayTime(item.createdAt);
    }
};
const getAllNotifications = (filter = "Active") => {
    const filtered = _notificationItems.filter(item => filter === "All" ? true : filter === "Deleted" ? item.isDeleted : !item.isDeleted);
    return filtered.sort((firstItem, secondItem) => Date.parse(secondItem.createdAt) - Date.parse(firstItem.createdAt));
};
const saveNotificationsToSessionStorage = () => {
    if (!_userStoreKey)
        return;
    try {
        const storedNotifications = { userStoreKey: _userStoreKey, notificationItems: _notificationItems };
        sessionStorage.setItem(SESSION_STORAGE_KEY, JSON.stringify(storedNotifications));
    }
    catch { // Session storage is blocked or full, the in memory copy carries on.
    }
};
const loadNotificationsFromSessionStorage = () => {
    if (!_userStoreKey)
        return [];
    try {
        const storedJson = sessionStorage.getItem(SESSION_STORAGE_KEY);
        if (!storedJson)
            return [];
        const storedNotifications = JSON.parse(storedJson);
        return storedNotifications.userStoreKey === _userStoreKey ? storedNotifications.notificationItems ?? [] : [];
    }
    catch { // Stored data is corrupt or unavailable, start empty.
        return [];
    }
};
const sendNotificationsToServices = (singleService, focusId = null) => {
    const currentNotifications = getAllNotifications();
    const servicesToNotify = singleService ? [singleService] : [..._registeredServices];
    for (const service of servicesToNotify) {
        service.dotNetObjectReference.invokeMethodAsync(service.callbackName, currentNotifications, focusId)
            .catch(() => {
            // The service is gone, for example the Server circuit ended, so stop calling it.
            _registeredServices = _registeredServices.filter(registeredService => registeredService !== service);
        });
    }
};
const saveAndNotifyServices = (focusId = null) => {
    refreshActiveDisplayTimes();
    saveNotificationsToSessionStorage();
    sendNotificationsToServices(undefined, focusId);
};
const addNotificationItemsToMemory = (notificationItems) => {
    let itemsAdded = false;
    for (const notificationItem of notificationItems) {
        if (_notificationItems.some(existingItem => existingItem.id === notificationItem.id))
            continue;
        _notificationItems.push(notificationItem);
        itemsAdded = true;
    }
    if (itemsAdded)
        _notificationItems = getAllNotifications().slice(0, MAX_NOTIFICATIONS);
    return itemsAdded;
};
const addNotificationItems = (notificationItems) => {
    if (true === addNotificationItemsToMemory(notificationItems))
        saveAndNotifyServices();
};
const addNotificationItem = (notificationItem) => addNotificationItems([notificationItem]);
const removeNotificationItem = (notificationItemId) => {
    const existingItem = _notificationItems.find(item => item.id === notificationItemId);
    if (!existingItem)
        return;
    existingItem.isDeleted = true;
    existingItem.title = "";
    existingItem.message = "";
    existingItem.actionUrl = null;
    existingItem.actionText = null;
    existingItem.actionFocusId = null;
    existingItem.displayTime = "";
    const itemIndex = _notificationItems.indexOf(existingItem);
    const itemToFocus = _notificationItems.slice(0, itemIndex).reverse().find(item => !item.isDeleted) ?? // nearest earlier item
        _notificationItems.slice(itemIndex + 1).find(item => !item.isDeleted); // otherwise nearest later item
    saveAndNotifyServices(itemToFocus?.id ?? null);
};
const clearNotifications = () => {
    if (_notificationItems.length === 0)
        return;
    _notificationItems = [];
    saveAndNotifyServices();
};
const clearNotificationStore = () => {
    _notificationItems = [];
    _userStoreKey = "";
    try {
        sessionStorage.removeItem(SESSION_STORAGE_KEY);
    }
    catch { // Ignore storage errors.
    }
    sendNotificationsToServices();
};
const setUserStoreKey = (userStoreKey) => {
    if (!userStoreKey || userStoreKey === _userStoreKey)
        return false;
    // Notifications sent before any key was known are kept and added to the new store.
    const notificationItemsAddedBeforeKeyWasSet = _userStoreKey === "" ? _notificationItems : [];
    _userStoreKey = userStoreKey;
    _notificationItems = loadNotificationsFromSessionStorage();
    addNotificationItemsToMemory(notificationItemsAddedBeforeKeyWasSet);
    return true;
};
const initialiseNotificationStore = (userStoreKey, notificationItems) => {
    const userStoreKeyChanged = setUserStoreKey(userStoreKey ?? "");
    const notificationItemsAdded = addNotificationItemsToMemory(notificationItems ?? []);
    if (userStoreKeyChanged || notificationItemsAdded)
        saveAndNotifyServices();
};
const closeNotificationCentre = (dialogElementId, blurActiveElementFirst = false) => {
    const dialogElement = document.getElementById(dialogElementId);
    if (!dialogElement)
        return;
    const activeElement = document.activeElement;
    if (blurActiveElementFirst && activeElement && dialogElement.contains(activeElement))
        activeElement.blur();
    if (dialogElement.open)
        dialogElement.close();
};
const setFocus = (elementId, timeoutMs = 3000) => {
    const tryFocus = () => {
        const elementToFocus = document.getElementById(elementId);
        if (!elementToFocus)
            return false;
        elementToFocus.focus();
        return true;
    };
    if (tryFocus())
        return;
    const observer = new MutationObserver(() => { if (tryFocus())
        observer.disconnect(); });
    observer.observe(document.body, { childList: true, subtree: true });
    window.setTimeout(() => observer.disconnect(), timeoutMs);
};
const showNotificationCentre = (dialogElementId) => {
    const dialogElement = document.getElementById(dialogElementId);
    if (dialogElement)
        dialogElement.showModal();
    refreshActiveDisplayTimes();
    sendNotificationsToServices();
};
const registerNotificationService = (serviceId, dotNetObjectReference, callbackName) => {
    _registeredServices = _registeredServices.filter(registeredService => registeredService.serviceId !== serviceId);
    const newService = { serviceId, dotNetObjectReference, callbackName };
    _registeredServices.push(newService);
    sendNotificationsToServices(newService); // first list of notifications for this service only
};
const unregisterNotificationService = (serviceId) => {
    _registeredServices = _registeredServices.filter(registeredService => registeredService.serviceId !== serviceId);
};
export { registerNotificationService, unregisterNotificationService, showNotificationCentre, closeNotificationCentre, initialiseNotificationStore, addNotificationItem, removeNotificationItem, clearNotifications, addNotificationItems, getAllNotifications, clearNotificationStore, setFocus };
//# sourceMappingURL=notifications.js.map