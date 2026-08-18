namespace BlazorRamp.DocSite.Common.Constants;

public class ThemingSnippets
{
    public const string Dark_Theme_Setting = """
        :root:has(#theme-toggler[aria-checked="true"]) {
            --br-kbd-background-colour: var(--br-unit-colour-info-darker);
            --br-info-box-alternate-heading-text: var(--br-unit-colour-accent-light);
            --br-code-block-background-colour: var(--br-unit-colour-secondary-darker);
            --br-code-block-text: var(--br-unit-colour-info-lighter);
            //Below are the component variables I changed for the dark theme.
            --br-comp-switch-thumb-Hover-colour: var(--br-unit-colour-primary-20);
            --br-comp-switch-thumb-text: var(--br-unit-colour-primary-30);
            --br-comp-all-thumb-colour: var(--br-unit-colour-neutral-80);
            --br-comp-switch-on-track-colour: var(--br-unit-colour-primary-30);
            --br-comp-all-track-colour: var(--br-unit-colour-neutral-70);
            --br-comp-history-trigger-pane-surface-background: var(--br-unit-colour-neutral-5);
            --br-comp-history-trigger-pane-surface-text: var(--br-unit-colour-neutral-90);
            --br-comp-all-pane-base-background: var(--br-unit-colour-neutral-80);
            --br-comp-all-focus-indicator-colour: var(--br-unit-colour-primary-30);
            --br-comp-all-pane-surface-background: var(--br-unit-colour-neutral-70);
            --br-comp-all-area-header-background: var(--br-unit-colour-neutral-70);
            --br-comp-all-area-header-text: var(--br-unit-colour-primary-text-light);
            --br-comp-all-area-content-text: var(--br-unit-colour-neutral-5);
            --br-unit-colour-canvas: var(--br-unit-colour-neutral-90);
            --br-unit-colour-canvas-text: var(--br-unit-colour-neutral-5);
            --br-comp-all-button-text: var(--br-unit-colour-primary-text-light);
            --br-comp-all-button-state-hover: hsl(from var(--br-unit-colour-primary) h s l / var(--br-unit-opacity-val-30));
            --br-comp-all-link-current-page-background-colour: var(--br-unit-colour-secondary-darker);
            --br-comp-all-link-hover-background-colour: hsl(from var(--br-unit-colour-secondary) h s l / 0.15);
            --br-comp-all-link-active-background-colour: hsl(from var(--br-unit-colour-secondary) h s l / 0.3);
            --br-comp-all-link-focused-background-colour: var(--br-unit-colour-secondary-darker);
            --br-unit-colour-canvas-inverted: var(--br-unit-colour-neutral-5);
            --br-unit-colour-canvas-text-inverted: var(--br-unit-colour-neutral-90);
            --br-comp-all-button-text: var(--br-unit-colour-primary-text-light);
            --br-comp-input-es-error-colour: var(--br-unit-colour-danger-light);
            --br-comp-input-error-colour: var(--br-unit-colour-danger-light);
            --br-comp-input-success-colour: var(--br-unit-colour-success-light);
            --br-comp-debounce-filter-error-colour: var(--br-unit-colour-danger-light);
            --br-comp-all-pane-panel-background: var(--br-unit-colour-neutral-90);
            --br-comp-data-table-area-header-background: var(--br-unit-colour-neutral-70);
            --br-comp-data-table-alternate-row: var(--br-unit-colour-neutral-70);
            --br-comp-all-button-border-colour: var(--br-unit-colour-primary-20);
            --br-comp-all-border-interactive-colour: var(--br-unit-colour-primary-20);
        }
        """;

}
