---
tags: [bootstrap, tooltip, popover, components, javascript]
category: Interactive Elements
difficulty: 2
time: 30 minutes
---

# Tooltip Component

## Overview

Tooltips are small, floating UI elements that display contextual information when a user hovers over, focuses on, or taps an element. Bootstrap 5's tooltip component is built on Popper.js (bundled with Bootstrap), which handles positioning relative to the trigger element, automatic placement adjustment when space is limited, and scroll-aware repositioning. Tooltips are designed for supplementary information that does not require interaction—they are dismissed automatically when the pointer moves away.

Unlike CSS-only tooltips that use `::after` pseudo-elements and `data-*` attributes, Bootstrap tooltips render as dynamically injected DOM elements. This approach enables rich HTML content, custom templates, programmatic show/hide control, and event-driven lifecycle hooks. However, it also requires explicit JavaScript initialization because Bootstrap does not auto-initialize tooltips to avoid performance issues on pages with many potential tooltip triggers.

The tooltip system supports 12 placement positions, HTML content injection, custom CSS classes, delay configuration, and fallback placements when the preferred position lacks sufficient viewport space. Understanding these options is essential for creating tooltips that enhance usability without obstructing content or breaking layout.

## Basic Implementation

Tooltips require two things: a `data-bs-toggle="tooltip"` attribute on the trigger element and JavaScript initialization via `new bootstrap.Tooltip()`:

```html
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="top" title="Tooltip on top">
  Tooltip on top
</button>
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="right" title="Tooltip on right">
  Tooltip on right
</button>
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Tooltip on bottom">
  Tooltip on bottom
</button>
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="left" title="Tooltip on left">
  Tooltip on left
</button>

<script>
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
</script>
```

The initialization script selects all elements with the tooltip attribute and creates a Tooltip instance for each. Without this initialization, the `data-bs-*` attributes have no effect.

For a single tooltip, initialize directly:

```html
<button type="button" class="btn btn-primary" id="myTooltip" data-bs-toggle="tooltip" title="Custom tooltip">
  Hover me
</button>

<script>
const tooltipEl = document.getElementById('myTooltip');
new bootstrap.Tooltip(tooltipEl);
</script>
```

## Advanced Variations

Placement options include 12 positions: `top`, `bottom`, `left`, `right` and their variations with `-start` and `-suffix`:

```html
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="top-start" title="Top Start">
  Top Start
</button>
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="top-end" title="Top End">
  Top End
</button>
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="bottom-start" title="Bottom Start">
  Bottom Start
</button>
<button type="button" class="btn btn-secondary" data-bs-toggle="tooltip" data-bs-placement="bottom-end" title="Bottom End">
  Bottom End
</button>
```

HTML content in tooltips requires enabling the `html` option. This allows formatted text, links, images, and other HTML elements:

```html
<button type="button" class="btn btn-primary" data-bs-toggle="tooltip" data-bs-html="true" title="<em>Tooltip</em> <u>with</u> <b>HTML</b>">
  Tooltip with HTML
</button>

<script>
const tooltipEl = document.querySelector('[data-bs-html="true"]');
new bootstrap.Tooltip(tooltipEl, { html: true });
</script>
```

Custom templates allow complete control over tooltip structure:

```html
<button type="button" class="btn btn-primary" id="customTemplateTooltip">
  Custom Template
</button>

<script>
new bootstrap.Tooltip(document.getElementById('customTemplateTooltip'), {
  title: 'Custom styled tooltip',
  template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner bg-danger"></div></div>',
  placement: 'right'
});
</script>
```

Delay configuration controls the show and hide timing:

```html
<button type="button" class="btn btn-primary" data-bs-toggle="tooltip" data-bs-delay='{"show": 500, "hide": 100}' title="Delayed tooltip">
  Delayed Tooltip
</button>
```

Fallback placements define alternative positions when the preferred placement lacks viewport space:

```html
<script>
new bootstrap.Tooltip(document.getElementById('fallbackTooltip'), {
  placement: 'top',
  fallbackPlacements: ['right', 'bottom', 'left']
});
</script>
```

Custom CSS classes are applied through the `customClass` option:

```html
<style>
.tooltip-custom .tooltip-inner {
  background-color: #6f42c1;
  font-size: 1rem;
}
.tooltip-custom .tooltip-arrow::before {
  border-top-color: #6f42c1;
}
</style>

<button type="button" class="btn btn-primary" id="styledTooltip" title="Purple tooltip">
  Styled Tooltip
</button>

<script>
new bootstrap.Tooltip(document.getElementById('styledTooltip'), {
  customClass: 'tooltip-custom',
  placement: 'top'
});
</script>
```

## Best Practices

1. **Always initialize tooltips with JavaScript.** Bootstrap does not auto-initialize tooltips. Without the `new bootstrap.Tooltip()` call, tooltip attributes are ignored and no tooltip appears.

2. **Use tooltips for supplementary information only.** Tooltips are hidden by default and require hover or focus to reveal. Critical information must be visible in the page content, not hidden in tooltips.

3. **Provide `data-bs-placement` to control tooltip direction.** Without a placement attribute, Bootstrap uses `top` by default. Explicitly setting placement ensures the tooltip appears in the most appropriate position relative to surrounding content.

4. **Enable `html: true` only for trusted content.** HTML tooltips are vulnerable to XSS if the content comes from user input. Never render unescaped user-provided HTML in tooltips.

5. **Use `container: 'body'` for tooltips inside overflow-hidden containers.** Tooltips positioned inside containers with `overflow: hidden` may be clipped. Setting `container: 'body'` appends the tooltip to the document body, avoiding clipping.

6. **Configure appropriate delays for dense UIs.** In interfaces with many tooltips, a show delay of 200-500ms prevents tooltips from flashing rapidly as the user moves the cursor across the interface.

7. **Do not put interactive content inside tooltips.** Tooltips are dismissed when the pointer leaves the trigger element. Users cannot interact with links or buttons inside tooltips because the tooltip disappears before they can click.

8. **Use `trigger: 'focus'` for form inputs.** Hover tooltips on form inputs can conflict with autocomplete dropdowns. Focus-triggered tooltips appear when the input receives keyboard focus, which is more reliable.

9. **Dispose of tooltips when removing trigger elements from the DOM.** Memory leaks occur when tooltip instances are not cleaned up. Call `tooltip.dispose()` when the trigger element is removed.

10. **Test tooltips on touch devices.** Hover-based tooltips do not work on touchscreens. Use `trigger: 'hover focus'` or add tap-to-toggle behavior for mobile support.

11. **Ensure tooltip text has sufficient contrast.** The default dark tooltip background with white text meets WCAG AA contrast requirements. Custom tooltip colors must maintain at least 4.5:1 contrast ratio.

## Common Pitfalls

1. **Forgetting JavaScript initialization.** This is the most common issue. Developers add `data-bs-toggle="tooltip"` and `title` attributes but skip the initialization script. No tooltip renders without the JavaScript call.

2. **Using tooltips for critical information.** Information hidden in tooltips is inaccessible to keyboard users who do not focus the trigger, touch users who cannot hover, and users with cognitive disabilities who may not discover the tooltip.

3. **Not specifying `container: 'body'` in modal dialogs.** Tooltips inside modals can be clipped by the modal's overflow settings or stacked incorrectly. Appending to the body ensures proper layering.

4. **Placing tooltips on disabled elements.** Disabled buttons do not fire pointer events, so tooltips on disabled triggers never appear. Wrap the disabled element in a `<span>` with the tooltip:

```html
<span data-bs-toggle="tooltip" title="Disabled tooltip">
  <button type="button" class="btn btn-primary" disabled>Disabled</button>
</span>
```

5. **Relying on `title` attribute for dynamic content.** Bootstrap reads the `title` attribute on initialization. Changing the attribute afterward requires calling `tooltip.setContent({'.tooltip-inner': 'new content'})` to update the displayed text.

6. **Not handling tooltip cleanup in single-page applications.** When navigating between views in SPAs, old tooltip instances remain in memory. Implement proper disposal in component lifecycle hooks.

7. **Enabling `html: true` with user-generated content.** This opens XSS vulnerabilities. Always sanitize user input before passing it to HTML-enabled tooltips.

8. **Using `data-bs-placement` values without checking available space.** The `top` placement fails if the trigger is at the top of the viewport. Use `fallbackPlacements` to handle edge cases.

## Accessibility Considerations

Tooltips are associated with their trigger elements through `aria-describedby`. Bootstrap manages this automatically: when a tooltip is shown, the trigger element receives `aria-describedby` pointing to the tooltip's `id`. Screen readers then announce the tooltip content when the trigger receives focus.

However, tooltips on focus-only can be problematic. Some screen readers do not announce `aria-describedby` content for all element types. Test with NVDA, JAWS, and VoiceOver to verify behavior.

For keyboard users, tooltips must appear on focus, not just hover. The default trigger is `hover focus`, which handles this. If customizing the trigger, ensure `focus` is always included.

Tooltips should not be the sole mechanism for conveying essential information. WCAG 2.1 Success Criterion 1.3.3 (Sensory Characteristics) requires that instructions do not rely solely on hover-revealed content.

Consider using Popovers instead of tooltips when the content needs to be interactive or when users need time to read longer content. Popovers support click-based dismissal and interactive content.

## Responsive Behavior

Tooltips are positioned dynamically by Popper.js and automatically adjust to viewport boundaries. On small screens, tooltips may flip to the opposite side of the trigger if the preferred placement lacks space.

The `fallbackPlacements` option controls which alternative positions Popper considers:

```javascript
new bootstrap.Tooltip(triggerElement, {
  placement: 'right',
  fallbackPlacements: ['left', 'top', 'bottom']
});
```

On mobile devices, hover-based tooltips are unreliable. For responsive applications, consider using `trigger: 'click'` on mobile and `trigger: 'hover focus'` on desktop, detected through touch capability:

```javascript
const isTouchDevice = 'ontouchstart' in window;
const trigger = isTouchDevice ? 'click' : 'hover focus';
new bootstrap.Tooltip(element, { trigger });
```

Tooltips inside scrollable containers with `overflow: auto` or `overflow: scroll` may behave unexpectedly. Use `container: 'body'` to ensure the tooltip scrolls with the page rather than the container.

For responsive forms with inline tooltips, consider switching to persistent helper text below the input on mobile instead of relying on hover tooltips, which are difficult to use on small touch screens.
