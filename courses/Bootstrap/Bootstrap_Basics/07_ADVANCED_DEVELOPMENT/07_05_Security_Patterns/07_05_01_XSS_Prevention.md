---
title: "XSS Prevention"
difficulty: 2
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - Cross-Site Scripting (XSS) Concepts
  - DOMPurify
  - Content Security Policy
---

## Overview

Cross-Site Scripting (XSS) prevention in Bootstrap applications addresses risks from dynamically generated HTML, user-supplied content rendered in components, and `innerHTML` usage that can execute malicious scripts. Bootstrap's JavaScript plugins (Tooltip, Popover, Modal) accept HTML content that, if not sanitized, creates XSS vectors.

Bootstrap 5 includes a built-in sanitization function for Tooltip and Popover content, but it uses a basic allowlist. For production applications, DOMPurify provides comprehensive HTML sanitization that strips dangerous elements and attributes while preserving safe markup.

The three XSS types - reflected, stored, and DOM-based - each require different prevention strategies. DOM-based XSS is most relevant for Bootstrap applications because client-side JavaScript renders dynamic content into the DOM.

## Basic Implementation

```js
// Bootstrap's built-in sanitize configuration
// Configure which HTML tags and attributes are allowed
const myDefaultAllowList = bootstrap.Tooltip.Default.allowList;

// Extend the allowlist
myDefaultAllowList.table = [];
myDefaultAllowList.tr = [];
myDefaultAllowList.td = [];
myDefaultAllowList.th = [];
myDefaultAllowList.thead = [];
myDefaultAllowList.tbody = [];

// Apply to all tooltips
bootstrap.Tooltip.Default.allowList = myDefaultAllowList;
```

```html
<!-- DANGEROUS: innerHTML with user input -->
<script>
  // NEVER do this
  const userName = getUrlParam('name');
  document.getElementById('greeting').innerHTML = 'Hello ' + userName;
  // If name=<script>alert('xss')</script>, the script executes
</script>

<!-- SAFE: textContent for plain text -->
<script>
  const userName = getUrlParam('name');
  document.getElementById('greeting').textContent = 'Hello ' + userName;
</script>
```

```js
// DOMPurify integration
import DOMPurify from 'dompurify';

// Safe HTML rendering
function renderSafeHTML(userContent, container) {
  const clean = DOMPurify.sanitize(userContent, {
    ALLOWED_TAGS: ['b', 'i', 'em', 'strong', 'a', 'p', 'br', 'ul', 'ol', 'li'],
    ALLOWED_ATTR: ['href', 'title', 'class'],
    ALLOW_DATA_ATTR: false
  });
  container.innerHTML = clean;
}

// Safe tooltip content
const tooltip = new bootstrap.Tooltip(trigger, {
  title: DOMPurify.sanitize(userData),
  html: true
});

// Safe modal body
const modal = new bootstrap.Modal(element);
element.querySelector('.modal-body').innerHTML =
  DOMPurify.sanitize(dynamicContent, { ALLOWED_TAGS: ['p', 'b', 'i', 'a', 'ul', 'li'] });
```

## Advanced Variations

```js
// Sanitization middleware for all dynamic content
class SafeRenderer {
  static render(template, data, container) {
    // Replace placeholders with sanitized data
    const sanitized = {};
    Object.entries(data).forEach(([key, value]) => {
      sanitized[key] = typeof value === 'string'
        ? DOMPurify.sanitize(value)
        : value;
    });

    let html = template;
    Object.entries(sanitized).forEach(([key, value]) => {
      html = html.replace(new RegExp(`{{${key}}}`, 'g'), value);
    });

    // Final sanitization of complete HTML
    container.innerHTML = DOMPurify.sanitize(html);
  }

  static createSafeElement(tag, attributes = {}, textContent = '') {
    const el = document.createElement(tag);

    Object.entries(attributes).forEach(([key, value]) => {
      if (key.startsWith('on')) return; // Skip event handler attributes
      el.setAttribute(key, DOMPurify.sanitize(value));
    });

    if (textContent) el.textContent = textContent;
    return el;
  }
}

// Usage
SafeRenderer.render(
  '<div class="card"><div class="card-body"><h5>{{title}}</h5><p>{{body}}</p></div></div>',
  { title: userTitle, body: userBody },
  container
);
```

## Best Practices

1. **Use textContent over innerHTML** - Plain text doesn't need sanitization; use textContent whenever possible.
2. **Sanitize all dynamic HTML** - Pass user content through DOMPurify before setting innerHTML.
3. **Configure Bootstrap's allowlist** - Extend or restrict allowed HTML in tooltips and popovers.
4. **Avoid eval() and Function()** - Never execute user-supplied strings as code.
5. **Use Content Security Policy** - CSP headers prevent inline script execution even if XSS is present.
6. **Validate on the server** - Client-side sanitization is defense-in-depth, not primary protection.
7. **Use template literals carefully** - Embedding variables in template literals for HTML is a common XSS vector.
8. **Sanitize URL attributes** - `href="javascript:..."` and `src` attributes can execute code.
9. **Test with XSS payloads** - Use known XSS vectors to verify sanitization effectiveness.
10. **Update DOMPurify regularly** - Sanitization libraries receive security patches.

## Common Pitfalls

1. **Using innerHTML with user input** - The most common XSS vector in Bootstrap applications.
2. **Incomplete allowlist** - Allowing `<iframe>` or `<object>` tags enables embedded content attacks.
3. **Trusting Bootstrap's default allowlist** - It's permissive by design; production apps need stricter rules.
4. **Sanitizing too late** - Sanitizing after concatenation instead of each individual value.
5. **Ignoring attribute XSS** - `onmouseover`, `onerror`, and `style` attributes can execute code.

## Accessibility Considerations

Sanitization must preserve ARIA attributes. DOMPurify's default config strips `aria-*` attributes unless explicitly allowed.

```js
DOMPurify.sanitize(content, {
  ADD_ATTR: ['aria-label', 'aria-describedby', 'aria-hidden', 'role']
});
```

## Responsive Behavior

XSS prevention is independent of responsive design. The same sanitization rules apply at all viewport sizes.
