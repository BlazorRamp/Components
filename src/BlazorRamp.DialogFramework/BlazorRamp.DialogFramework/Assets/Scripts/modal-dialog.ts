const _cancelHandlerMap = new WeakMap();

const getModalDialog = (elementID: string): HTMLDialogElement | null => document.getElementById(elementID) as HTMLDialogElement | null;

function openModalDialog(elementID: string): void {

    const modalDialog = getModalDialog(elementID);

    if (!modalDialog) return

    if (!_cancelHandlerMap.has(modalDialog)) addCancelEscapeHandler(modalDialog);

    if (!modalDialog.open) modalDialog.showModal();
}
function closeModalDialog(elementID: string): void {

    const modalDialog = getModalDialog(elementID);

    if (!modalDialog) return

    removeCancelEscapeHandler(modalDialog);

    if (modalDialog.open) modalDialog.close();
}

function addCancelEscapeHandler(modalDialog: HTMLDialogElement) {

    if (!modalDialog) return;

    const handler = (event: KeyboardEvent) => {
        if (event.key === "Escape") {
            event.preventDefault();
        }
    };

    modalDialog.addEventListener('keydown', handler);

    _cancelHandlerMap.set(modalDialog, handler);
}
function removeCancelEscapeHandler(modalDialog: HTMLDialogElement) {

    if (!modalDialog) return;

    const handler = _cancelHandlerMap.get(modalDialog);

    if (handler) modalDialog.removeEventListener('keydown', handler);

    _cancelHandlerMap.delete(modalDialog);

}

export { openModalDialog, closeModalDialog };