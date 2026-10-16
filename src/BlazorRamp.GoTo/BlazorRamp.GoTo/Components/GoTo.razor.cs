
using BlazorRamp.GoTo.Common.Constants;
using Microsoft.AspNetCore.Components;

namespace BlazorRamp.GoTo.Components;


/// <summary>
/// A marker that gives keyboard users a place to jump to. The marker takes up no space on the page and
/// is listed, in document order, by the <see cref="GoToViewer"/> while it is visible. Choosing it in the
/// viewer moves keyboard focus to the marker and briefly shows its label. Place it between blocks of
/// content, such as before a section heading, and not inside lists or tables.
/// </summary>
public partial class GoTo
{
    /// <summary>
    /// Gets or sets the text shown in the viewer's list and in the label displayed when the marker is reached.
    /// Leading and trailing whitespace is trimmed. Required: an <see cref="ArgumentNullException"/>
    /// is thrown if it is <see langword="null"/>, empty or whitespace.
    /// </summary>
    [Parameter] public string LabelText { get; set; } = default!;

    private string _markerID  = Guid.NewGuid().ToString();
    private string _popoverID = Guid.NewGuid().ToString();
    private string _labelID   = Guid.NewGuid().ToString();
    private string _labelText = String.Empty;

    private string _labelPosition = "centre-right";

    /// <summary>
    /// Validates <see cref="LabelText"/> and copies it into its working field.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <see cref="LabelText"/> is <see langword="null"/>, empty or whitespace.
    /// </exception>
    protected override void OnParametersSet()
    {
        _labelText = String.IsNullOrWhiteSpace(LabelText) ? throw new ArgumentNullException(nameof(LabelText), GlobalValues.GoTo_Label_Text_Exception_Message) : LabelText.Trim();
    }

}
