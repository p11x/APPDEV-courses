---
title: "Clipboard API with Bootstrap"
slug: "clipboard-api-bootstrap"
difficulty: 1
tags: ["bootstrap", "javascript", "clipboard", "copy-paste", "ux"]
prerequisites:
  - "03_02_Button_Types"
  - "06_02_01_Intersection_Observer"
related:
  - "06_02_14_Share_API"
  - "06_02_12_Broadcast_Channel"
duration: "15 minutes"
---

# Clipboard API with Bootstrap

## Overview

The Clipboard API enables programmatic copy and paste operations, replacing the legacy `document.execCommand('copy')` approach. Bootstrap buttons trigger clipboard actions with visual feedback through icons, tooltips, and badge states. Common patterns include copy-to-clipboard buttons for code snippets, API keys, shareable links, and table data export. The API also supports paste listeners for importing data directly into forms and tables.

## Basic Implementation

A copy button that copies adjacent text content to the clipboard with visual feedback.

```html
<div class="container mt-4">
  <div class="card">
    <div class="card-header d-flex justify-content-between align-items-center">
      <span>API Key</span>
      <button class="btn btn-sm btn-outline-primary copy-btn" data-copy-target="apiKey">
        <i class="bi bi-clipboard"></i> Copy
      </button>
    </div>
    <div class="card-body">
      <code id="apiKey" class="user-select-all">sk_test_YOUR_API_KEY_HERE</code>
    </div>
  </div>

  <div class="card mt-3">
    <div class="card-header d-flex justify-content-between align-items-center">
      <span>Install Command</span>
      <button class="btn btn-sm btn-outline-primary copy-btn" data-copy-target="installCmd">
        <i class="bi bi-clipboard"></i> Copy
      </button>
    </div>
    <div class="card-body">
      <pre class="mb-0"><code id="installCmd">npm install bootstrap@5</code></pre>
    </div>
  </div>
</div>

<script>
document.querySelectorAll('.copy-btn').forEach(btn => {
  btn.addEventListener('click', async () => {
    const targetId = btn.dataset.copyTarget;
    const text = document.getElementById(targetId).textContent;

    try {
      await navigator.clipboard.writeText(text);
      const originalHTML = btn.innerHTML;
      btn.innerHTML = '<i class="bi bi-check-lg text-success"></i> Copied!';
      btn.classList.replace('btn-outline-primary', 'btn-outline-success');
      setTimeout(() => {
        btn.innerHTML = originalHTML;
        btn.classList.replace('btn-outline-success', 'btn-outline-primary');
      }, 2000);
    } catch (err) {
      btn.innerHTML = '<i class="bi bi-x-lg text-danger"></i> Failed';
      setTimeout(() => {
        btn.innerHTML = '<i class="bi bi-clipboard"></i> Copy';
      }, 2000);
    }
  });
});
</script>
```

## Advanced Variations

### Copy Table Data as CSV

Export visible table rows to clipboard as tab-separated values for pasting into spreadsheets.

```html
<div class="card">
  <div class="card-header d-flex justify-content-between">
    <h5 class="mb-0">Sales Data</h5>
    <button class="btn btn-sm btn-outline-secondary" id="copyTableBtn">
      <i class="bi bi-clipboard"></i> Copy as CSV
    </button>
  </div>
  <div class="card-body">
    <div class="table-responsive">
      <table class="table table-hover" id="salesTable">
        <thead class="table-light">
          <tr>
            <th><input type="checkbox" class="form-check-input" id="selectAll"></th>
            <th>Product</th>
            <th>Revenue</th>
            <th>Units</th>
            <th>Region</th>
          </tr>
        </thead>
        <tbody>
          <tr><td><input type="checkbox" class="form-check-input row-check"></td><td>Widget Pro</td><td>$12,450</td><td>234</td><td>North</td></tr>
          <tr><td><input type="checkbox" class="form-check-input row-check"></td><td>Gadget Plus</td><td>$8,920</td><td>156</td><td>South</td></tr>
          <tr><td><input type="checkbox" class="form-check-input row-check"></td><td>Tool Max</td><td>$15,780</td><td>89</td><td>East</td></tr>
        </tbody>
      </table>
    </div>
  </div>
</div>

<script>
document.getElementById('copyTableBtn').addEventListener('click', async () => {
  const rows = document.querySelectorAll('#salesTable tbody tr');
  const selected = Array.from(rows).filter(row =>
    row.querySelector('.row-check').checked
  );
  const dataRows = selected.length > 0 ? selected : Array.from(rows);

  const headers = Array.from(document.querySelectorAll('#salesTable thead th'))
    .slice(1).map(th => th.textContent);

  const csv = [
    headers.join('\t'),
    ...dataRows.map(row =>
      Array.from(row.querySelectorAll('td')).slice(1).map(td => td.textContent).join('\t')
    )
  ].join('\n');

  await navigator.clipboard.writeText(csv);
  const btn = document.getElementById('copyTableBtn');
  btn.innerHTML = '<i class="bi bi-check-lg"></i> Copied!';
  setTimeout(() => {
    btn.innerHTML = '<i class="bi bi-clipboard"></i> Copy as CSV';
  }, 2000);
});

document.getElementById('selectAll').addEventListener('change', (e) => {
  document.querySelectorAll('.row-check').forEach(cb => cb.checked = e.target.checked);
});
</script>
```

### Paste Handler for Form Input

Listen for paste events and populate form fields from clipboard data.

```html
<div class="card">
  <div class="card-header">
    <h5 class="mb-0">Bulk Import</h5>
    <small class="text-muted">Paste CSV data directly into the table</small>
  </div>
  <div class="card-body">
    <div class="border border-2 border-dashed rounded p-4 text-center mb-3"
         id="pasteZone" tabindex="0" role="textbox" aria-label="Paste data here">
      <i class="bi bi-clipboard-plus fs-3 text-muted"></i>
      <p class="text-muted mb-0">Click here and press Ctrl+V to paste</p>
    </div>
    <div class="table-responsive d-none" id="previewSection">
      <table class="table table-sm" id="previewTable">
        <thead class="table-light" id="previewHead"></thead>
        <tbody id="previewBody"></tbody>
      </table>
      <button class="btn btn-primary btn-sm" id="importBtn">Import</button>
      <button class="btn btn-outline-secondary btn-sm" id="clearBtn">Clear</button>
    </div>
  </div>
</div>

<script>
const pasteZone = document.getElementById('pasteZone');

pasteZone.addEventListener('paste', (e) => {
  e.preventDefault();
  const text = e.clipboardData.getData('text');
  const rows = text.trim().split('\n').map(r => r.split('\t'));

  if (rows.length === 0) return;

  document.getElementById('previewHead').innerHTML =
    `<tr>${rows[0].map(h => `<th>${h}</th>`).join('')}</tr>`;
  document.getElementById('previewBody').innerHTML =
    rows.slice(1).map(r => `<tr>${r.map(c => `<td>${c}</td>`).join('')}</tr>`).join('');

  document.getElementById('previewSection').classList.remove('d-none');
  pasteZone.classList.add('d-none');
});

document.getElementById('clearBtn').addEventListener('click', () => {
  document.getElementById('previewSection').classList.add('d-none');
  pasteZone.classList.remove('d-none');
});
</script>
```

### Code Block with Copy Button

Syntax-highlighted code blocks with integrated copy functionality.

```html
<div class="position-relative">
  <pre class="bg-dark text-light p-3 rounded"><code id="codeBlock">const bootstrap = require('bootstrap');
const tooltip = new bootstrap.Tooltip(element);
tooltip.show();</code></pre>
  <button class="btn btn-sm btn-outline-light position-absolute top-0 end-0 m-2 copy-code-btn">
    <i class="bi bi-clipboard"></i>
  </button>
</div>

<script>
document.querySelectorAll('.copy-code-btn').forEach(btn => {
  btn.addEventListener('click', async () => {
    const code = btn.closest('.position-relative').querySelector('code').textContent;
    await navigator.clipboard.writeText(code);
    btn.innerHTML = '<i class="bi bi-check-lg"></i>';
    setTimeout(() => { btn.innerHTML = '<i class="bi bi-clipboard"></i>'; }, 2000);
  });
});
</script>
```

## Best Practices

1. Always provide visual feedback after a copy action (icon change, toast, or badge)
2. Use `navigator.clipboard.writeText()` instead of the deprecated `execCommand`
3. Handle clipboard permission denials gracefully with a fallback message
4. Provide a keyboard shortcut hint (Ctrl+C) alongside copy buttons
5. Use `user-select: all` on code blocks to enable manual selection as fallback
6. Limit copied content length to avoid overwhelming the clipboard
7. Show "Copied!" state for at least 2 seconds before reverting
8. Use consistent copy button placement across the application (top-right of content blocks)
9. Support copy on click without requiring additional confirmation dialogs
10. Test paste handlers with various data formats (CSV, TSV, plain text)
11. Sanitize pasted content before displaying in the DOM
12. Use `navigator.clipboard.readText()` only in response to user gestures
13. Provide tooltip hints explaining what will be copied
14. Respect clipboard API permissions and prompt users if access is denied

## Common Pitfalls

1. **No HTTPS**: Clipboard API requires a secure context (HTTPS or localhost)
2. **Missing error handling**: Clipboard write can fail due to permissions or browser restrictions
3. **No visual feedback**: Users click copy and see no indication it worked
4. **Copying wrong element**: Selecting child elements that include UI artifacts
5. **Blocking paste**: Preventing default paste behavior without providing alternative
6. **Permission issues**: Not handling `NotAllowedError` when clipboard access is denied
7. **Stale content**: Copying content that has been dynamically updated but not re-rendered

## Accessibility Considerations

Use `aria-label` on copy buttons describing what will be copied (e.g., "Copy API key"). Mark paste zones with `role="textbox"` and `aria-label` for screen reader discovery. Announce copy success with `aria-live="polite"` regions. Ensure copy buttons are keyboard-focusable and operable with Enter/Space. Provide alternative manual selection with `user-select: all` for users who cannot use the copy button. Include visible text labels alongside clipboard icons.

```html
<button class="btn btn-outline-primary copy-btn" aria-label="Copy installation command to clipboard">
  <i class="bi bi-clipboard" aria-hidden="true"></i> Copy
</button>
<div aria-live="polite" class="visually-hidden" id="copyAnnounce"></div>
```

## Responsive Behavior

Copy buttons should remain visible and tappable on mobile with adequate touch targets (minimum 44px). Use `btn-sm` on desktop and regular `btn` on mobile for comfortable tapping. Paste zones should be full-width on mobile. Code blocks should use `overflow-x: auto` for horizontal scrolling on small screens. Toast notifications for copy confirmation should appear at the bottom-center on mobile. Use `d-grid` for multiple copy action buttons on mobile layouts.
