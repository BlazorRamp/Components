namespace BlazorRamp.DataTable.Common.Constants;

public enum ContentAlignment : int 
{ 
    Start  = 0, 
    Centre = 1, 
    End    = 2 
}
public enum ColumnSortDirection : int 
{ 
    NotSorted  = 0, 
    Ascending  = 1, 
    Descending = 2 
}

public enum RowSelectionMode : int 
{ 
    None     = 0, 
    Single   = 1,
    Multiple = 2
}