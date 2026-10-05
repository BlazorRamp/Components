namespace BlazorRamp.DocSite.Common.Constants;

public class DisclosureSnippets
{
    public const string Add_Disclosure_Style_Sheet = """
        <head>
            <link rel="stylesheet" href="_content/BlazorRamp.Core/assets/css/core.min.css" />
            <link rel="stylesheet" href="_content/BlazorRamp.Disclosure/assets/css/disclosure.min.css" />
        </head>
        """;


    public const string Disclosure_With_Scrollbar_Example = """
         <Disclosure TriggerText="Lorem ipsum text in a disclosure" ContentMaxHeight="40vh">
        <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut pretium pharetra ullamcorper. Quisque lectus enim, laoreet eu nisi id, 
            hendrerit convallis est.Nam et gravida purus, eget tincidunt erat. Ut mattis diam at est ullamcorper, et ultrices est ultricies. Nam 
            vel ultricies metus. Cras at aliquam sem. Curabitur consectetur velit vulputate vestibulum sagittis. In et scelerisque 
            libero. Vestibulum condimentum venenatis metus, sit amet commodo tortor rutrum vel.
        </p>
        <p>
            Suspendisse felis nibh, molestie id sapien at, venenatis condimentum erat. Nam id dui mi. Vestibulum accumsan lacus nec turpis pharetra, 
            eget scelerisque quam gravida.Sed eget libero condimentum, sodales lorem non, bibendum dui. Nunc iaculis lacinia turpis non mattis. 
            Pellentesque ut lectus nec quam rutrum sagittis eget non mi. Aliquam at euismod purus. Curabitur maximus, risus eget mattis fermentum, 
            lorem lorem vehicula sapien, id consequat nisi est at tellus. In a volutpat enim. Aliquam vitae nisl in nisl vulputate mollis. Donec 
            sapien quam, sagittis at est eget, egestas placerat quam. Donec ipsum orci, facilisis sit amet viverra a, ultrices auctor nisi. Duis volutpat
            rutrum dui ac scelerisque. Vivamus dapibus faucibus massa sit amet efficitur.

        </p>
        <p>
            Donec eu turpis leo. Morbi vehicula sem feugiat, aliquet erat malesuada, pellentesque metus. Mauris vitae dolor sodales, imperdiet magna consectetur,
            tristique felis. Nunc aliquam elit vitae orci tristique, ut porta nisi interdum. Donec tellus tortor, lobortis vel augue sed, imperdiet cursus massa. 
            Duis quis magna porttitor, molestie tortor et, rutrum ipsum. Nunc et nibh porta, dignissim purus et, rutrum sapien. Vestibulum a orci libero. 
            Suspendisse quis scelerisque quam. Quisque purus lorem, accumsan vel mauris ut, pharetra volutpat mauris. Aliquam venenatis lacus at magna aliquet, nec 
            semper tortor dapibus. Donec eget consectetur metus. Duis pulvinar aliquet augue quis accumsan. Quisque risus odio, dapibus sit amet consequat vel, 
            elementum id libero.
        </p>
        <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut pretium pharetra ullamcorper. Quisque lectus enim, laoreet eu nisi id, hendrerit convallis est.
            Nam et gravida purus, eget tincidunt erat. Ut mattis diam at est ullamcorper, et ultrices est ultricies. Nam vel ultricies metus. Cras at aliquam sem. 
            Curabitur consectetur velit vulputate vestibulum sagittis. In et scelerisque libero. Vestibulum condimentum venenatis metus, sit amet commodo tortor 
            rutrum vel.
        </p>

        </Disclosure>
        """;

    public const string Disclosure_Content_Border_Removed_Example = """
        <Disclosure TriggerText="More lorem ipsum text" SvgIcon="--svg-article-icon">
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut pretium pharetra ullamcorper. Quisque lectus enim, laoreet eu nisi id, hendrerit convallis est.
            Nam et gravida purus, eget tincidunt erat. Ut mattis diam at est ullamcorper, et ultrices est ultricies. Nam vel ultricies metus. Cras at aliquam sem. Curabitur
            consectetur velit vulputate vestibulum sagittis. In et scelerisque libero. Vestibulum condimentum venenatis metus, sit amet commodo tortor rutrum vel.
          </p>
        </Disclosure>
        """;


    public const string Disclosre_Binding_Example = """
        <Disclosure TriggerText="Even more lorem ipsum text" SvgIcon="--svg-article-icon" @bind-Expanded="@_isExpanded">
            <div class="@FlexContent.Base @LayoutAlignment.AlignItems(UnitAlignItems.Centre) @LayoutAlignment.JustifyContent(UnitJustifyContent.SpaceBetween) @FlexContent.Wrap(UnitFlexWrap.Wrap)">
                <span>
                    Lorem Ipsum Text
                </span>
                <div class="@FlexContent.Base @Gap.SetGaps(UnitGapSize.Four) @Padding.Block(UnitSpace.Three) @FlexContent.Wrap(UnitFlexWrap.NoWrap)">
                    <Tooltip TooltipText="Opens a dialog for editing the content." TooltipID="edit-tooltip" TooltipPosition="TooltipPosition.TopRight" InvertColours="true">
                        <button aria-label="Edit content." aria-describedby="edit-tooltip" type="button" class="@Button.Base @Button.Scheme(ButtonSolidScheme.Warning)  @Button.Squared @BoxShadow.SetShadow(UnitBoxShadow.One)">
                            <span class="@SvgIcon.Base" style="--_svg-icon-source:var(--svg-pencil-icon);"></span>
                        </button>
                    </Tooltip>
                    <Tooltip TooltipText="Opens a new page with the full article." TooltipID="view-tooltip" TooltipPosition="TooltipPosition.TopRight" InvertColours="true">
                        <button aria-label="View more." aria-describedby="view-tooltip" type="button" class="@Button.Base @Button.Scheme(ButtonSolidScheme.Info) @Button.Squared @BoxShadow.SetShadow(UnitBoxShadow.One)">
                            <span class="@SvgIcon.Base" style="--_svg-icon-source:var(--svg-glasses-icon);"></span>
                        </button>
                    </Tooltip>
                </div>
            </div>

            <p style="margin:0;">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut pretium pharetra ullamcorper. Quisque lectus enim, laoreet eu nisi id, hendrerit convallis est.
                Nam et gravida purus, eget tincidunt erat. Ut mattis diam at est ullamcorper, et ultrices est ultricies. Nam vel ultricies metus. Cras at aliquam sem. Curabitur
                consectetur velit vulputate vestibulum sagittis. In et scelerisque libero. Vestibulum condimentum venenatis metus, sit amet commodo tortor rutrum ve [. . . ]
            </p>

        </Disclosure>
        """;
}
