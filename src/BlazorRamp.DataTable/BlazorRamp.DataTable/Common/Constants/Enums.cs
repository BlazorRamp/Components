namespace BlazorRamp.DataTable.Common.Constants;


/// <summary>
/// Horizontal alignment of the table's title text.
/// </summary>
public enum TitleAlignment : int
{
    /// <summary>
    /// Align the title to the start (left in LTR layouts).
    /// </summary>
    Start = 0,

    /// <summary>
    /// Centre the title.
    /// </summary>
    Centre = 1,

    /// <summary>
    /// Align the title to the end (right in LTR layouts).
    /// </summary>
    End = 2
}

/// <summary>
/// Horizontal alignment applied to a column's header and cell content.
/// </summary>
public enum ColumnAlignment : int
{
    /// <summary>
    /// Align content to the start (left in LTR layouts).
    /// </summary>
    Start = 0,

    /// <summary>Centre the content.</summary>
    Centre = 1,

    /// <summary>A
    /// lign content to the end (right in LTR layouts).
    /// </summary>
    End = 2
}

/// <summary>
/// The current sort state of a column.
/// </summary>
public enum ColumnSortDirection : int
{
    /// <summary>
    /// The column is not currently used to sort the data.
    /// </summary>
    NotSorted = 0,

    /// <summary>
    /// The data is sorted by this column in ascending order.
    /// </summary>
    Ascending = 1,

    /// <summary>
    /// The data is sorted by this column in descending order.
    /// </summary>
    Descending = 2
}

/// <summary>
/// Determines whether rows can be selected, and if so, whether one or many rows may be selected at a time.
/// </summary>
public enum RowSelectionMode : int
{
    /// <summary>
    /// Rows cannot be selected; no selection column is rendered.
    /// </summary>
    None = 0,

    /// <summary
    /// >Only one row may be selected at a time; selecting a new row clears any previous selection.
    /// </summary>
    Single = 1,

    /// <summary>
    /// Any number of rows may be selected at once.
    /// </summary>
    Multiple = 2
}

/// <summary>
/// Horizontal alignment of the filter area within the table header region.
/// </summary>
public enum FilterAlignment : int
{
    /// <summary>
    /// No filter area is rendered.
    /// </summary>
    None = 0,

    /// <summary>
    /// Align the filter area to the start (left in LTR layouts).
    /// </summary>
    Start = 1,

    /// <summary>
    /// Centre the filter area.
    /// </summary>
    Centre = 2,

    /// <summary>
    /// Align the filter area to the end (right in LTR layouts).
    /// </summary>
    End = 3
}