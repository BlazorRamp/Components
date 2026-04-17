namespace BlazorRamp.DocSite.Common.Constants;

public class BusySnippets
{
    public const string Add_Busy_Indicator_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.BusyIndicator/assets/css/busy-indicator.min.css" />
        </head>
        """;




    public const string Busy_Indicator_Example = """
        <BusyIndicator AriaStartText="Saving, please wait" AriaEndText=@_endMessage ShowIndicator="@_showIndicator" 
                OverlayPosition="OverlayPosition.Screen" BusyText=". . . Saving . . . ."  ContentPosition="ContentPosition.Top" 
                EndStatus="@_announcementType" DisplayTimeoutMS="15_000" OnBusyCompleted="HandleOnBusyCompleted" />

        public async Task SaveCustomer(CustomerData customerData)
        {
            _showIndicator = true;

            try
            {
                await _customerService.UpdateCustomer(customerData);
                _announcementType = AnnouncementType.OperationCompleted;
                _endMessage = "Saved Successfully";

            }
            catch(Exception ex)
            {
                /*
                    * Perhaps show some dialog or set a summary panel with the details etc
                 */

                _announcementType = AnnouncementType.SystemWarning;
                _endMessage = "Your data was not saved, please review the details in the summary panel.";
            }
            finally
            {
                _showIndicator = false;
            }
        }
        """;


    public const string Busy_Indicator_Content = """
        <BusyIndicator>

        <!-- Your content in here -->

        </BusyIndicator>

        """;
}
