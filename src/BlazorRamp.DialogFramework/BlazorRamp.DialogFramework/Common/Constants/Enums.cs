using BlazorRamp.DialogFramework.Framework;

namespace BlazorRamp.DialogFramework.Common.Constants;

/// <summary>
/// Specifies the horizontal alignment of the dialog window within the dialog.
/// </summary>
public enum HorizontalAlignment : int 
{
    /// <summary>
    /// Aligns the dialog window to the left.
    /// </summary>
    Left = 0,
    /// <summary>
    /// Centres the dialog window horizontally.
    /// </summary>
    Centre = 1,
   
    /// <summary>
    /// Aligns the dialog window to the right.
    /// </summary>
    Right = 2 
}

/// <summary>
/// Specifies the vertical alignment of the dialog window within the dialog.
/// </summary>
public enum VerticalAlignment : int 
{
    /// <summary>
    /// Aligns the dialog window to the top.
    /// </summary>
    Top = 0,
    /// <summary>
    /// Centres the dialog window vertically.
    /// </summary>
    Centre = 1,
    /// <summary>
    /// Aligns the dialog window to the bottom.
    /// </summary>
    Bottom = 2 
}
/// <summary>
/// Identifies which button was clicked to close a modal dialog.
/// </summary>
public enum DialogResultButtons : int 
{
    /// <summary>
    /// The OK or confirm button was clicked.
    /// </summary>
    Ok = 0,
    /// <summary>
    /// The Cancel button was clicked, or the dialog was dismissed via the escape key.
    /// </summary>
    Cancel = 1,
    /// <summary>
    /// A custom button was clicked. Check <see cref="ModalDialogResult.ButtonText"/> for the label.
    /// </summary>
    Other = 2 
}
