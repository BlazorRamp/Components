namespace BlazorRamp.DataTable.Common.Constants;

public enum TitleAlignment : int
{
    Start = 0,
    Centre = 1,
    End = 2
}
public enum ColumnAlignment : int 
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

public enum FilterAlignment : int
{
    None   = 0,
    Start  = 1,
    Centre = 2,
    End    = 3
}