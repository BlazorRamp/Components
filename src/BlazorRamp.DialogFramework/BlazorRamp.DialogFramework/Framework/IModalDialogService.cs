
namespace BlazorRamp.DialogFramework.Framework
{
    public interface IModalDialogService
    {
        IReadOnlyList<ModalDialogWindow> DialogWindows { get; }

        Task CloseDialog(ModalDialogResult dialogResult);
        string GetAriaLabelledByID();
        Task<ModalDialogResult> ShowDialog<TDialog>();
        Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogOptions dialogOptions);
        Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogParameters<TDialog> dialogParameters);
        Task<ModalDialogResult> ShowDialog<TDialog>(ModalDialogParameters<TDialog> dialogParameters, ModalDialogOptions dialogOptions);
    }
}