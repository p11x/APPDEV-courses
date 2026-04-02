---
title: "Secure Modal Content"
difficulty: 3
category: "Advanced Development"
subcategory: "Security Patterns"
prerequisites:
  - iframe Sandboxing
  - DOMPurify
  - Bootstrap 5 Modal API
---

## Overview

Loading external or user-generated content into Bootstrap modals creates security risks if the content contains scripts, iframes, or malicious markup. Secure modal content loading uses iframe sandboxing for external URLs, DOMPurify sanitization for HTML content, and Content Security Policy to restrict what the modal can execute.

The primary attack vectors are: loading untrusted URLs directly in modals (phishing, script injection), rendering user-supplied HTML without sanitization (XSS), and modal content that escapes the modal's intended boundaries (clickjacking).

## Basic Implementation

```js
// Secure external content loading in modals
class SecureModal {
  static openExternal(url, options = {}) {
    const modalEl = document.getElementById('externalModal');
    const iframe = modalEl.querySelector('iframe');

    // Validate URL protocol
    const parsed = new URL(url);
    if (!['https:', 'http:'].includes(parsed.protocol)) {
      throw new Error('Only HTTP(S) URLs are allowed');
    }

    // Configure iframe sandbox
    iframe.sandbox = 'allow-scripts allow-same-origin allow-forms';
    iframe.src = url;
    iframe.title = options.title || 'External content';

    const modal = new bootstrap.Modal(modalEl);
    modal.show();

    // Clear iframe on close
    modalEl.addEventListener('hidden.bs.modal', () => {
      iframe.src = '';
    }, { once: true });
  }

  static openSafeHTML(html, options = {}) {
    const modalEl = document.getElementById('contentModal');
    const body = modalEl.querySelector('.modal-body');

    // Sanitize before insertion
    body.innerHTML = DOMPurify.sanitize(html, {
      ALLOWED_TAGS: ['p', 'b', 'i', 'em', 'strong', 'a', 'ul', 'ol', 'li', 'br'],
      ALLOWED_ATTR: ['href', 'title', 'target', 'rel']
    });

    const modal = new bootstrap.Modal(modalEl);
    modal.show();

    // Clear on close
    modalEl.addEventListener('hidden.bs.modal', () => {
      body.innerHTML = '';
    }, { once: true });
  }
}
```

```html
<!-- Secure iframe modal template -->
<div class="modal fade" id="externalModal" tabindex="-1" aria-label="External content viewer">
  <div class="modal-dialog modal-lg modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">External Content</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body p-0">
        <iframe
          class="w-100"
          style="height: 500px; border: none;"
          sandbox="allow-scripts allow-same-origin allow-forms"
          referrerpolicy="strict-origin-when-cross-origin"
          loading="lazy"
          title="External content">
        </iframe>
      </div>
      <div class="modal-footer">
        <a href="#" class="btn btn-outline-primary" target="_blank" rel="noopener noreferrer">
          Open in New Tab
        </a>
        <button class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

<!-- Secure content modal template -->
<div class="modal fade" id="contentModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Details</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <!-- Sanitized content inserted here -->
      </div>
    </div>
  </div>
</div>
```

```js
// Usage
// Load external page securely
SecureModal.openExternal('https://example.com/terms', { title: 'Terms of Service' });

// Load user-generated content safely
const userContent = getUserContent();
SecureModal.openSafeHTML(userContent);

// Dynamic modal with sanitized content
function showItemDetails(item) {
  const modal = new bootstrap.Modal('#detailModal');
  const body = document.querySelector('#detailModal .modal-body');

  body.innerHTML = DOMPurify.sanitize(`
    <h5>${item.name}</h5>
    <p>${item.description}</p>
    <small class="text-muted">Added: ${item.date}</small>
  `);

  modal.show();
}
```

## Best Practices

1. **Always sanitize modal content** - Use DOMPurify before inserting any dynamic HTML into modals.
2. **Sandbox iframes** - Use `sandbox` attribute to restrict iframe capabilities.
3. **Validate URLs** - Check protocol and domain before loading external content.
4. **Use referrerpolicy** - `strict-origin-when-cross-origin` prevents URL leakage.
5. **Clear content on close** - Remove iframe src and body content when modal closes.
6. **Block top navigation** - Iframe `sandbox` without `allow-top-navigation` prevents escape.
7. **Prefer opening in new tab** - For untrusted external content, link to new tab instead of iframe.
8. **Set CSP on modal content** - Apply restrictive CSP to modal content areas.
9. **Use `noopener` on links** - Prevent opened pages from accessing `window.opener`.
10. **Log external URL loads** - Audit trail for security monitoring.

## Common Pitfalls

1. **Unsandboxed iframes** - Iframes without `sandbox` can execute scripts, navigate parent, etc.
2. **Loading HTTP in HTTPS** - Mixed content blocked by browsers, but worth validating explicitly.
3. **Missing DOMPurify** - Inserting user HTML without sanitization enables XSS.
4. **Allowing top-navigation** - Iframe can redirect the parent page if `allow-top-navigation` is set.
5. **Not clearing on close** - iframe continues loading in background, wasting bandwidth and potential data leakage.

## Accessibility Considerations

Modal content must maintain proper heading hierarchy and focus management. Iframes need descriptive `title` attributes.

```html
<iframe sandbox="..."
        title="Terms of Service document from example.com"
        aria-label="External terms of service content">
</iframe>
```

## Responsive Behavior

Modal size should adapt to viewport. Use `modal-fullscreen-*` classes for mobile to provide adequate viewing area for iframe content.

```html
<div class="modal-dialog modal-lg modal-fullscreen-sm-down">
  <!-- Content fills screen on mobile, modal-lg on desktop -->
</div>
```
