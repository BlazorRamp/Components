const _cancelHandlerMap = new WeakMap();

const ANNOUNCEMENT_DIALOG_ID = "blazor-ramp-announcement-history-dialog";


const getModalDialog = (elementID: string): HTMLDialogElement | null => document.getElementById(elementID) as HTMLDialogElement | null;

const openModalDialog = (elementID: string): void => {

    const modalDialog = getModalDialog(elementID);

    if (!modalDialog) return

    if (!_cancelHandlerMap.has(modalDialog)) addCancelEscapeHandler(modalDialog);

    if (!modalDialog.open) modalDialog.showModal();
}
const closeModalDialog = (elementID: string): void => {

    const modalDialog = getModalDialog(elementID);

    if (!modalDialog) return

    removeCancelEscapeHandler(modalDialog);

    if (modalDialog.open) modalDialog.close();
}

const addCancelEscapeHandler = (modalDialog: HTMLDialogElement):void => {

    if (!modalDialog) return;

    const historyDialog: HTMLDialogElement | null = document.getElementById(ANNOUNCEMENT_DIALOG_ID) as HTMLDialogElement;

    const handler = (event: KeyboardEvent) => {
        if (event.key === "Escape") {

            if (historyDialog?.contains(event.target as Node) && historyDialog.matches(':popover-open')) {
                event.stopPropagation();//stop blazor getting the event which it uses to notify dialog component

                return; // Allow escape to work normally for announcement history
            }

            event.preventDefault();
        }
    };

    modalDialog.addEventListener('keydown', handler);

    _cancelHandlerMap.set(modalDialog, handler);
}

const removeCancelEscapeHandler = (modalDialog: HTMLDialogElement):void => {

    if (!modalDialog) return;

    const handler = _cancelHandlerMap.get(modalDialog);

    if (handler) modalDialog.removeEventListener('keydown', handler);

    _cancelHandlerMap.delete(modalDialog);

}

export { openModalDialog, closeModalDialog };