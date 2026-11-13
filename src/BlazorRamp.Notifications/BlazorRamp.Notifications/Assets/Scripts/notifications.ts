enum NotificationType {
    Info = "Info"
}

interface NotificationItem {
    id: string;
    title: string;
    message: string;
    category: NotificationType;
    actionUrl?: string | null;
    actionText?: string | null;
    createdAt: string;
}

interface StoredNotifications {
    userStoreKey: string;
    notificationItems: NotificationItem[];
}
interface RegisteredService {
    serviceId: string;
    dotNetObjectReference: any;
    callbackName: string;
}

const SESSION_STORAGE_KEY = "br-notifications";
const MAX_NOTIFICATIONS = 50;

let _userStoreKey = "";   // empty means memory only, nothing is saved to session storage
let _notificationItems: NotificationItem[]   = [];
let _registeredServices: RegisteredService[] = [];

const getAllNotifications = (): NotificationItem[] => {

    return [..._notificationItems].sort((firstItem, secondItem) => Date.parse(secondItem.createdAt) - Date.parse(firstItem.createdAt));
}

const saveNotificationsToSessionStorage = (): void => {

    if (!_userStoreKey) return;

    try {
        const storedNotifications: StoredNotifications = { userStoreKey: _userStoreKey, notificationItems: _notificationItems };

        sessionStorage.setItem(SESSION_STORAGE_KEY, JSON.stringify(storedNotifications));

    }
    catch {// Session storage is blocked or full, the in memory copy carries on.
    }
};

const loadNotificationsFromSessionStorage = (): NotificationItem[] => {

    if (!_userStoreKey) return [];

    try {
        const storedJson = sessionStorage.getItem(SESSION_STORAGE_KEY);
        if (!storedJson) return [];

        const storedNotifications: StoredNotifications = JSON.parse(storedJson);

        return storedNotifications.userStoreKey === _userStoreKey ? storedNotifications.notificationItems ?? [] : [];
    }
    catch {// Stored data is corrupt or unavailable, start empty.
        return [];
    }
};




const sendNotificationsToServices = (singleService?: RegisteredService): void => {

    const currentNotifications = getAllNotifications();
    const servicesToNotify     = singleService ? [singleService] : [..._registeredServices];

    for (const service of servicesToNotify) {
        service.dotNetObjectReference.invokeMethodAsync(service.callbackName, currentNotifications)
            .catch(() => {
                // The service is gone, for example the Server circuit ended, so stop calling it.
                _registeredServices = _registeredServices.filter(registeredService => registeredService !== service);
            });
    }
};

const saveAndNotifyServices = (): void => {
    saveNotificationsToSessionStorage();
    sendNotificationsToServices();
};


const addNotificationItemsToMemory = (notificationItems: NotificationItem[]): boolean => {

    let itemsAdded = false;

    for (const notificationItem of notificationItems) {
        if (_notificationItems.some(existingItem => existingItem.id === notificationItem.id)) continue;

        _notificationItems.push(notificationItem);
        itemsAdded = true;
    }

    if (itemsAdded) _notificationItems = getAllNotifications().slice(0, MAX_NOTIFICATIONS);

    return itemsAdded;
};

const addNotificationItems = (notificationItems: NotificationItem[]): void => {

    if (true === addNotificationItemsToMemory(notificationItems)) saveAndNotifyServices();
};

const addNotificationItem = (notificationItem: NotificationItem): void => addNotificationItems([notificationItem]);

const removeNotificationItem = (notificationId: string): void => {

    if (!_notificationItems.some(existingItem => existingItem.id === notificationId)) return;

    _notificationItems = _notificationItems.filter(existingItem => existingItem.id !== notificationId);

    saveAndNotifyServices();
}

const clearNotifications = (): void => {

    if (_notificationItems.length === 0) return;

    _notificationItems = [];

    saveAndNotifyServices();
};

const clearNotificationStore = (): void => {

    _notificationItems = [];
    _userStoreKey = "";

    try {
        sessionStorage.removeItem(SESSION_STORAGE_KEY);
    }
    catch { // Ignore storage errors.
       
    }

    sendNotificationsToServices();
};

const setUserStoreKey = (userStoreKey: string): boolean => {

    if (!userStoreKey || userStoreKey === _userStoreKey) return false;

    // Notifications sent before any key was known are kept and added to the new store.
    const notificationItemsAddedBeforeKeyWasSet = _userStoreKey === "" ? _notificationItems : [];

    _userStoreKey = userStoreKey;
    _notificationItems = loadNotificationsFromSessionStorage();
    addNotificationItemsToMemory(notificationItemsAddedBeforeKeyWasSet);

    return true;
};

const initialiseNotificationStore = (userStoreKey: string | null, notificationItems: NotificationItem[] | null): void => {
    const userStoreKeyChanged = setUserStoreKey(userStoreKey ?? "");
    const notificationItemsAdded = addNotificationItemsToMemory(notificationItems ?? []);

    if (userStoreKeyChanged || notificationItemsAdded) saveAndNotifyServices();
};


const registerNotificationService = (serviceId: string, dotNetObjectReference: any, callbackName: string): void => {


    console.log(serviceId);

    _registeredServices = _registeredServices.filter(registeredService => registeredService.serviceId !== serviceId);

    const newService: RegisteredService = { serviceId, dotNetObjectReference, callbackName };
    _registeredServices.push(newService);

    sendNotificationsToServices(newService);   // first list of notifications for this service only
};

const unregisterNotificationService = (serviceId: string): void => {
    _registeredServices = _registeredServices.filter(registeredService => registeredService.serviceId !== serviceId);
};

export {
    registerNotificationService, unregisterNotificationService, initialiseNotificationStore, addNotificationItem, removeNotificationItem,
    clearNotifications, addNotificationItems, getAllNotifications, clearNotificationStore
}