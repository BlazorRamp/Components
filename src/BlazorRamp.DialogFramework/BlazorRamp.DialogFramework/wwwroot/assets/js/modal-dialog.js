const _cancelHandlerMap = new WeakMap();
const getModalDialog = (elementID) => document.getElementById(elementID);
function openModalDialog(elementID) {
    const modalDialog = getModalDialog(elementID);
    if (!modalDialog)
        return;
    if (!_cancelHandlerMap.has(modalDialog))
        addCancelEscapeHandler(modalDialog);
    if (!modalDialog.open)
        modalDialog.showModal();
}
function closeModalDialog(elementID) {
    const modalDialog = getModalDialog(elementID);
    if (!modalDialog)
        return;
    removeCancelEscapeHandler(modalDialog);
    if (modalDialog.open)
        modalDialog.close();
}
function addCancelEscapeHandler(modalDialog) {
    if (!modalDialog)
        return;
    const handler = (event) => {
        if (event.key === "Escape") {
            event.preventDefault();
        }
    };
    modalDialog.addEventListener('keydown', handler);
    _cancelHandlerMap.set(modalDialog, handler);
}
function removeCancelEscapeHandler(modalDialog) {
    if (!modalDialog)
        return;
    const handler = _cancelHandlerMap.get(modalDialog);
    if (handler)
        modalDialog.removeEventListener('keydown', handler);
    _cancelHandlerMap.delete(modalDialog);
}
export { openModalDialog, closeModalDialog };
//# sourceMappingURL=modal-dialog.js.map