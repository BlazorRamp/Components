using BlazorRamp.DataTable.Components;

namespace BlazorRamp.DataTable.Common.Models;

/// <summary>
/// Holds the paging state shared between a <see cref="DataTable{TData}"/> and its pager UI.
/// </summary>
/// <param name="itemsPerPage">The number of rows to display per page. Defaults to 10</param>
public class PagerBinding(int itemsPerPage = 10)
{
    /// <summary>
    /// The page currently being displayed, 1-based.
    /// </summary>
    public int CurrentPage { get; set; } = 0;

    /// <summary>
    /// The number of rows in the current filtered/unfiltered data set.
    /// </summary>
    public int CurrentItemCount { get; set; } = 0;

    /// <summary>
    /// The total number of rows available before filtering.
    /// </summary>
    public int TotalItemCount { get; set; } = 0;

    /// <summary>
    /// The number of rows to display per page.
    /// </summary>
    public int ItemsPerPage { get; set; } = itemsPerPage;
}