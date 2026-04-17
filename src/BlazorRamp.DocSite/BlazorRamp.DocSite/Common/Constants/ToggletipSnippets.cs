namespace BlazorRamp.DocSite.Common.Constants;

public class ToggletipSnippets
{
    public const string Add_ToggleTip_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.ToggleTip/assets/css/toggle-tip.min.css" />
        </head>
        """;


    public const string Usage_Example = """
        <ToggleTip CloseText="Close Keyboard Info." Label="Keyboard info:" ShowClose="true" ShowLabel="true" 
        ToggleTipLabelOrder="ToggleTipLabelOrder.LabelFirst" ToggleTipSize="ToggleTipSize.Small">
        <h3 id="toggletip-demo" class="@GlobalValues.Info_Box_Heading_Class">Keyboard interaction</h3>
        <ul aria-labelledby="toggletip-demo">
            <li><kbd>Space</kbd> - when focus is on the toggletip icon, expands or collapses the content.</li>
            <li><kbd>Enter</kbd> - when focus is on the toggletip icon, expands or collapses the content.</li>
            <li><kbd>Escape</kbd> - closes the toggletip</li>
        </ul>

        <p>
            Lorem ipsum text below to force a scroll bar. The toggletip is responsive so if you do not see a scrollbar, 
            just make your window narrower.
        </p>

        <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque ipsum massa, feugiat id sapien
            non, egestas vestibulum dolor. Nulla luctus est ac urna imperdiet tristique. Vestibulum ante
            ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Praesent egestas massa
            nec sem sagittis, at efficitur urna rutrum. Nunc et.
        </p>
        <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque ipsum massa, feugiat id sapien
            non, egestas vestibulum dolor. Nulla luctus est ac urna imperdiet tristique. Vestibulum ante
            ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Praesent egestas massa
            nec sem sagittis, at efficitur urna rutrum. Nunc et.
        </p>
        """;
}