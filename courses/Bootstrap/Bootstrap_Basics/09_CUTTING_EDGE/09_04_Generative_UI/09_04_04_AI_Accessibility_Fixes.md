---
title: "AI Accessibility Fixes"
description: "AI-generated ARIA labels, automated contrast fixes, and AI alt text generation for Bootstrap components"
difficulty: 2
tags: [ai, accessibility, aria, contrast, alt-text, automation]
prerequisites:
  - 07_01_Accessibility_Overview
  - 09_04_02_AI_Component_Creation
---

## Overview

AI can automate tedious accessibility tasks: generating meaningful `alt` text from image content, suggesting `aria-label` values for icon-only buttons, calculating accessible color alternatives, and identifying missing ARIA attributes. These are tasks that are critical for WCAG compliance but often skipped due to time constraints.

The workflow integrates AI into the development pipeline: scan Bootstrap components for accessibility issues, generate fixes with AI, apply them automatically or with human review. This reduces the accessibility backlog from weeks to hours.

## Basic Implementation

```js
// AI-powered accessibility scanner
class AIAccessibilityFixer {
  constructor(aiClient) {
    this.ai = aiClient;
  }

  async scanAndFix(rootElement) {
    const issues = [];

    // Find images without alt text
    rootElement.querySelectorAll('img:not([alt])').forEach(async (img) => {
      const altText = await this.ai.complete(
        `Generate descriptive alt text for an image: ${img.src}. Context: Bootstrap card image.`
      );
      issues.push({ type: 'missing-alt', element: img, fix: altText });
    });

    // Find icon buttons without labels
    rootElement.querySelectorAll('button:not([aria-label])').forEach(btn => {
      const hasText = btn.textContent.trim().length > 0;
      const hasIcon = btn.querySelector('i, svg, .bi-');
      if (!hasText && hasIcon) {
        issues.push({
          type: 'missing-label',
          element: btn,
          fix: `Add aria-label="${this.suggestLabel(btn)}"`
        });
      }
    });

    // Find low contrast text
    rootElement.querySelectorAll('*').forEach(el => {
      const style = getComputedStyle(el);
      const contrast = this.calculateContrast(style.color, style.backgroundColor);
      if (contrast < 4.5 && el.textContent.trim()) {
        issues.push({
          type: 'low-contrast',
          element: el,
          contrast,
          fix: { color: this.suggestAccessibleColor(style.color, style.backgroundColor) }
        });
      }
    });

    return issues;
  }

  suggestLabel(button) {
    const icon = button.querySelector('i, svg');
    const iconClass = icon?.className || '';
    const labels = {
      'bi-trash': 'Delete',
      'bi-pencil': 'Edit',
      'bi-plus': 'Add',
      'bi-x': 'Close',
      'bi-search': 'Search',
    };
    for (const [cls, label] of Object.entries(labels)) {
      if (iconClass.includes(cls)) return label;
    }
    return 'Action';
  }
}
```

```html
<!-- Before: inaccessible Bootstrap components -->
<div class="card">
  <img src="product.jpg">
  <div class="card-body">
    <h5>Product</h5>
    <button class="btn btn-sm"><i class="bi bi-cart"></i></button>
  </div>
</div>

<!-- After: AI-fixed accessibility -->
<div class="card">
  <img src="product.jpg" alt="Blue wireless headphones on a white background">
  <div class="card-body">
    <h5>Product</h5>
    <button class="btn btn-sm" aria-label="Add to cart"><i class="bi bi-cart" aria-hidden="true"></i></button>
  </div>
</div>
```

```css
/* AI-suggested contrast fix */
.low-contrast-fix {
  /* Original: color: #9ca3af on #f9fafb — contrast 2.8:1 */
  /* AI fix: */
  color: #6b7280; /* contrast 5.1:1 */
}
```

## Advanced Variations

Batch alt text generation:

```js
async function generateAltTexts(images) {
  const prompts = images.map(img => ({
    src: img.src,
    context: img.closest('.card') ? 'product card' : 'general',
    prompt: `Describe this image for screen reader users in under 125 characters.`
  }));

  const results = await Promise.all(
    prompts.map(p => aiClient.complete(p.prompt))
  );

  images.forEach((img, i) => img.alt = results[i]);
}
```

## Best Practices

1. Generate alt text that describes content and function, not appearance.
2. Use AI to suggest ARIA labels; review them for accuracy.
3. Automate contrast checking with `getComputedStyle` and luminance calculation.
4. Run accessibility fixes in CI/CD pipelines, not just during development.
5. Provide human review for AI-generated accessibility fixes.
6. Use AI to generate `aria-describedby` content for form fields.
7. Automate `role` attribute suggestions for non-semantic elements.
8. Keep alt text under 125 characters for screen reader compatibility.
9. Use `aria-hidden="true"` on decorative icons.
10. Generate focus order documentation from AI analysis of DOM structure.
11. Use AI to suggest skip navigation link placement.
12. Document every automated fix for audit trails.

## Common Pitfalls

1. **Generic alt text** — AI may generate "image of a thing" instead of descriptive text.
2. **Context ignorance** — AI doesn't know the purpose of the image in the UI.
3. **Over-labeling** — AI may add redundant ARIA labels that duplicate visible text.
4. **False positives** — Contrast checkers may flag acceptable combinations.
5. **Cultural context** — AI-generated descriptions may miss cultural significance.
6. **Hallucinated content** — AI may describe elements not present in the image.

## Accessibility Considerations

AI is a tool to accelerate accessibility work, not replace it. Always validate AI suggestions with real users and screen reader testing. AI cannot understand the lived experience of disabled users.

## Responsive Behavior

Accessibility fixes are viewport-independent. Alt text, ARIA labels, and contrast ratios work at all screen sizes. Touch target sizes (44x44px minimum) should be verified on mobile.
