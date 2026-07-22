namespace BlazorRamp.DataTable.Common.Models;

public class PagerBinding(int currentPage, int currentItemCount, int totalItemCount, int itemsPerPage)
{
    public int CurrentPage      { get; set; } = currentPage;
    public int CurrentItemCount { get; set; } = currentItemCount;
    public int TotalItemCount   { get; set; } = totalItemCount;
    public int ItemsPerPage     { get; set; } = itemsPerPage;
}