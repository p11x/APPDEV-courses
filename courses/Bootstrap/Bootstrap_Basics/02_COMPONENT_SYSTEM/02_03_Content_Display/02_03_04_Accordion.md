---
title: Accordion
category: Component System
difficulty: 2
time: 30 min
tags: bootstrap5, accordion, collapse, content-display, expandable
---

## Overview

The Bootstrap accordion component builds on the Collapse plugin to provide vertically stacked, expandable and collapsible content panels. Each accordion item consists of a header button and a collapsible body region. By default, opening one panel closes the others, creating an exclusive toggle behavior. Accordions are ideal for FAQs, settings panels, multi-step forms, and any interface where screen real estate must be conserved.

## Basic Implementation

A standard accordion uses the `.accordion` wrapper with multiple `.accordion-item` children. Each item contains an `.accordion-header` with an `.accordion-button` and an `.accordion-collapse` body.

```html
<div class="accordion" id="accordionExample">
  <div class="accordion-item">
    <h2 class="accordion-header" id="headingOne">
      <button class="accordion-button" type="button" data-bs-toggle="collapse"
        data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
        Accordion Item #1
      </button>
    </h2>
    <div id="collapseOne" class="accordion-collapse collapse show"
      aria-labelledby="headingOne" data-bs-parent="#accordionExample">
      <div class="accordion-body">
        <strong>This is the first item's accordion body.</strong> It is shown by default until the collapse plugin adds the appropriate classes for toggling.
      </div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header" id="headingTwo">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
        data-bs-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
        Accordion Item #2
      </button>
    </h2>
    <div id="collapseTwo" class="accordion-collapse collapse"
      aria-labelledby="headingTwo" data-bs-parent="#accordionExample">
      <div class="accordion-body">
        Content for the second accordion panel goes here.
      </div>
    </div>
  </div>
</div>
```

## Advanced Variations

Remove the `data-bs-parent` attribute to allow multiple panels open simultaneously.

```html
<div class="accordion" id="accordionAlwaysOpen">
  <div class="accordion-item">
    <h2 class="accordion-header" id="openHeadingOne">
      <button class="accordion-button" type="button" data-bs-toggle="collapse"
        data-bs-target="#openCollapseOne" aria-expanded="true" aria-controls="openCollapseOne">
        Always Open Item #1
      </button>
    </h2>
    <div id="openCollapseOne" class="accordion-collapse collapse show"
      aria-labelledby="openHeadingOne">
      <div class="accordion-body">
        This panel stays open even when others are expanded.
      </div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header" id="openHeadingTwo">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
        data-bs-target="#openCollapseTwo" aria-expanded="false" aria-controls="openCollapseTwo">
        Always Open Item #2
      </button>
    </h2>
    <div id="openCollapseTwo" class="accordion-collapse collapse"
      aria-labelledby="openHeadingTwo">
      <div class="accordion-body">
        Second panel that can coexist with the first.
      </div>
    </div>
  </div>
</div>
```

Flush accordion removes outer borders for placement inside containers like cards.

```html
<div class="accordion accordion-flush" id="flushAccordion">
  <div class="accordion-item">
    <h2 class="accordion-header" id="flushHeadingOne">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
        data-bs-target="#flushCollapseOne" aria-expanded="false" aria-controls="flushCollapseOne">
        Flush Item #1
      </button>
    </h2>
    <div id="flushCollapseOne" class="accordion-collapse collapse"
      aria-labelledby="flushHeadingOne">
      <div class="accordion-body">Flush content one.</div>
    </div>
  </div>
  <div class="accordion-item">
    <h2 class="accordion-header" id="flushHeadingTwo">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
        data-bs-target="#flushCollapseTwo" aria-expanded="false" aria-controls="flushCollapseTwo">
        Flush Item #2
      </button>
    </h2>
    <div id="flushCollapseTwo" class="accordion-collapse collapse"
      aria-labelledby="flushHeadingTwo">
      <div class="accordion-body">Flush content two.</div>
    </div>
  </div>
</div>
```

Apply contextual background classes for color customization.

```html
<div class="accordion" id="coloredAccordion">
  <div class="accordion-item">
    <h2 class="accordion-header">
      <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
        data-bs-target="#coloredCollapse" aria-expanded="false">
        Styled Accordion
      </button>
    </h2>
    <div id="coloredCollapse" class="accordion-collapse collapse">
      <div class="accordion-body bg-light">
        Customized body with a light background.
      </div>
    </div>
  </div>
</div>
```

## Best Practices

1. Always include unique `id` attributes for each accordion item, header, and collapse region.
2. Pair every `data-bs-target` with a matching `id` on the collapse element.
3. Set `aria-expanded` correctly on buttons to reflect open/closed state.
4. Use `aria-controls` on each button to reference its collapse region.
5. Keep `data-bs-parent` on the collapse divs when only one panel should be open at a time.
6. Remove `data-bs-parent` to allow simultaneous open panels (always-open behavior).
7. Use `.accordion-flush` inside cards or bordered containers to avoid double borders.
8. Start at least one item with `.show` on its collapse div if a default open state is needed.
9. Keep accordion header text concise; move detailed explanations to the body.
10. Avoid nesting accordions deeper than two levels to prevent UX confusion.
11. Ensure the accordion toggle arrow indicator is visible on all color backgrounds.
12. Test keyboard navigation to confirm Tab and Enter/Space activate the correct panels.

## Common Pitfalls

- Omitting `data-bs-parent` when exclusive behavior is expected, causing multiple panels to stay open simultaneously.
- Mismatching `data-bs-target` values with element `id` attributes breaks toggle functionality entirely.
- Using `<a>` tags instead of `<button>` for accordion headers creates conflicting navigation behavior.
- Forgetting `aria-expanded` means screen readers cannot convey the open/closed state.
- Nesting interactive elements like forms directly inside accordion headers causes event propagation issues.
- Overriding the `.accordion-button` CSS without preserving the focus ring breaks keyboard accessibility.
- Placing `.accordion-flush` on the wrong element (not the outer `.accordion` wrapper) has no effect.

## Accessibility Considerations

Each accordion header uses a `<button>` element by default, which provides native keyboard support. The `aria-expanded` attribute must be toggled between `true` and `false` to announce state changes. Use `aria-controls` to associate each button with its collapsible region. Screen readers rely on these attributes to present the accordion as an accessible widget with clear expand/collapse semantics.

## Responsive Behavior

Accordions are fully responsive out of the box. They stack vertically and expand/collapse within any container width. On narrow screens, the full-width header buttons remain easy to tap. Avoid setting fixed heights on accordion bodies, as content length varies. For very long bodies, consider combining accordions with scroll containers or pagination within the body content.
