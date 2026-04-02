---
title: "Sanitize Options"
difficulty: 2
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - Bootstrap 5 Sanitize Config
  - DOMPurify Configuration
  - HTML Allowlist Patterns
---

## Overview

Bootstrap 5 includes a built-in sanitization system for components that accept HTML content (Tooltip, Popover, Dropdown). The `allowList` defines which HTML tags and attributes are permitted, stripping everything else. While Bootstrap's default allowlist is reasonable for most cases, production applications often need custom sanitization rules that match their specific security requirements.

The sanitization pipeline works by parsing HTML content, checking each element and attribute against the allowlist, and removing anything not explicitly permitted. Bootstrap uses `Sanitizer.sanitize()` internally when `html: true` is set on tooltips and popovers.

## Basic Implementation

```js
// Bootstrap's default allowList structure
const defaultAllowList = {
  '*': ['class', 'dir', 'id', 'lang', 'role', /^aria-[\w-]*$/i],
  a: ['target', 'href', 'title', 'rel'],
  area: [],
  b: [],
  br: [],
  col: [],
  code: [],
  div: [],
  em: [],
  hr: [],
  h1: [],
  h2: [],
  h3: [],
  h4: [],
  h5: [],
  h6: [],
  i: [],
  img: ['src', 'srcset', 'alt', 'title', 'width', 'height'],
  li: [],
  ol: [],
  p: [],
  pre: [],
  s: [],
  small: [],
  span: [],
  sub: [],
  sup: [],
  strong: [],
  u: [],
  ul: []
};

// Custom restrictive allowlist
const strictAllowList = {
  '*': ['class', 'id', 'role'],
  a: ['href', 'title'],
  b: [],
  em: [],
  i: [],
  p: [],
  strong: [],
  span: []
};

// Apply custom allowlist to all tooltips
bootstrap.Tooltip.Default.allowList = strictAllowList;

// Per-component allowlist override
const tooltip = new bootstrap.Tooltip(element, {
  title: '<strong>Safe</strong> <em>content</em>',
  html: true,
  allowList: strictAllowList
});
```

```js
// DOMPurify configuration for Bootstrap
import DOMPurify from 'dompurify';

const bootstrapPurifyConfig = {
  ALLOWED_TAGS: [
    'a', 'b', 'br', 'div', 'em', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
    'i', 'img', 'li', 'ol', 'p', 'pre', 'span', 'strong', 'table',
    'tbody', 'td', 'tfoot', 'th', 'thead', 'tr', 'u', 'ul'
  ],
  ALLOWED_ATTR: [
    'href', 'target', 'rel', 'title', 'class', 'id', 'role',
    'src', 'alt', 'width', 'height', 'style'
  ],
  ALLOW_DATA_ATTR: false,
  FORBID_TAGS: ['script', 'style', 'iframe', 'object', 'embed'],
  FORBID_ATTR: ['onerror', 'onload', 'onclick', 'onmouseover']
};

function sanitizeForBootstrap(html) {
  return DOMPurify.sanitize(html, bootstrapPurifyConfig);
}
```

```html
<!-- Usage with modal -->
<div class="modal fade" id="contentModal">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">User Content</h5>
      </div>
      <div class="modal-body" id="modalBody">
        <!-- Content sanitized before insertion -->
      </div>
    </div>
  </div>
</div>

<script>
  const userContent = getUserContent(); // Potentially unsafe
  document.getElementById('modalBody').innerHTML =
    DOMPurify.sanitize(userContent, bootstrapPurifyConfig);
</script>
```

## Best Practices

1. **Start with Bootstrap's default allowList** - It's a reasonable baseline; customize from there.
2. **Use DOMPurify for complex content** - Bootstrap's allowList is basic; DOMPurify handles edge cases better.
3. **Never allow script or style tags** - These should always be forbidden in user content.
4. **Block event handler attributes** - `onclick`, `onerror`, `onload` must always be stripped.
5. **Restrict href protocols** - Only allow `http:`, `https:`, and `mailto:` in links.
6. **Validate img src** - Block `javascript:` and `data:` URIs except for known-safe data images.
7. **Use per-component allowlists** - Different components may need different allowed HTML.
8. **Test with attack vectors** - Verify sanitization against known XSS payloads.
9. **Document your allowlist** - Explain why each tag and attribute is permitted.
10. **Review allowlist changes** - Treat allowlist modifications as security-sensitive changes.

## Common Pitfalls

1. **Allowing iframes** - Iframes can load arbitrary content and execute scripts in the parent context.
2. **Permitting style attributes** - `style="background: url('javascript:...')"` can execute code.
3. **Overly broad allowlist** - Allowing too many tags increases the attack surface.
4. **Not sanitizing on the server** - Client-side sanitization is defense-in-depth, not primary protection.
5. **Ignoring SVG content** - SVG can contain `<script>` elements and event handlers.

## Accessibility Considerations

Sanitization must preserve ARIA attributes. Add `aria-*` attributes to the allowlist pattern to ensure accessible content survives sanitization.

## Responsive Behavior

Sanitization rules are viewport-independent. The same rules apply at all screen sizes.
