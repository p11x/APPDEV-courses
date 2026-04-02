---
title: "Web Share API with Bootstrap"
slug: "share-api-bootstrap"
difficulty: 2
tags: ["bootstrap", "javascript", "share-api", "mobile", "native"]
prerequisites:
  - "06_02_13_Clipboard_API"
  - "03_02_Button_Types"
related:
  - "06_02_12_Broadcast_Channel"
  - "06_01_14_AI_Personalization_UI"
duration: "20 minutes"
---

# Web Share API with Bootstrap

## Overview

The Web Share API triggers native OS share sheets from web applications, allowing users to share content via messaging apps, email, social media, and more. Bootstrap buttons provide the share trigger UI, while the API handles platform-specific sharing dialogs. This replaces custom share widgets with native experiences that feel integrated with the user's device. The API supports sharing URLs, text, titles, and files on compatible platforms.

## Basic Implementation

A Bootstrap button that opens the native share sheet with page content.

```html
<div class="container mt-4">
  <div class="card">
    <div class="card-body text-center">
      <h5 class="card-title">Amazing Article Title</h5>
      <p class="card-text">This is a shareable article with interesting content that users want to share with their network.</p>
      <div class="d-flex justify-content-center gap-2">
        <button class="btn btn-primary" id="shareBtn">
          <i class="bi bi-share"></i> Share
        </button>
        <button class="btn btn-outline-secondary" id="copyLinkBtn">
          <i class="bi bi-link-45deg"></i> Copy Link
        </button>
      </div>
      <div id="shareStatus" class="mt-2 small text-muted"></div>
    </div>
  </div>
</div>

<script>
document.getElementById('shareBtn').addEventListener('click', async () => {
  const shareData = {
    title: 'Amazing Article Title',
    text: 'Check out this article about Bootstrap and the Web Share API!',
    url: window.location.href
  };

  if (navigator.share) {
    try {
      await navigator.share(shareData);
      document.getElementById('shareStatus').textContent = 'Shared successfully!';
    } catch (err) {
      if (err.name !== 'AbortError') {
        document.getElementById('shareStatus').textContent = 'Share failed: ' + err.message;
      }
    }
  } else {
    await navigator.clipboard.writeText(shareData.url);
    document.getElementById('shareStatus').textContent = 'Link copied to clipboard (share not supported)';
  }
});
</script>
```

## Advanced Variations

### Share with Files

Share images or documents directly from the page using the file-sharing capability.

```html
<div class="card">
  <img src="/images/chart.png" class="card-img-top" id="shareImage">
  <div class="card-body">
    <h5 class="card-title">Q4 Report Chart</h5>
    <button class="btn btn-primary" id="shareWithFile">
      <i class="bi bi-share"></i> Share Image
    </button>
    <button class="btn btn-outline-secondary ms-2" id="downloadBtn">
      <i class="bi bi-download"></i> Download
    </button>
    <div id="fileShareStatus" class="mt-2 small"></div>
  </div>
</div>

<script>
document.getElementById('shareWithFile').addEventListener('click', async () => {
  try {
    const response = await fetch('/images/chart.png');
    const blob = await response.blob();
    const file = new File([blob], 'q4-report-chart.png', { type: 'image/png' });

    if (navigator.canShare && navigator.canShare({ files: [file] })) {
      await navigator.share({
        title: 'Q4 Report Chart',
        text: 'Here is the Q4 performance chart',
        files: [file]
      });
      document.getElementById('fileShareStatus').textContent = 'Shared successfully!';
    } else {
      document.getElementById('fileShareStatus').textContent = 'File sharing not supported on this device';
    }
  } catch (err) {
    document.getElementById('fileShareStatus').textContent = 'Share cancelled or failed';
  }
});
</script>
```

### Share Menu with Fallbacks

A dropdown share menu that shows native share on supported devices and custom fallbacks elsewhere.

```html
<div class="dropdown">
  <button class="btn btn-outline-primary dropdown-toggle" data-bs-toggle="dropdown" id="shareDropdown">
    <i class="bi bi-share"></i> Share
  </button>
  <ul class="dropdown-menu" id="shareMenu">
    <li><h6 class="dropdown-header">Share via</h6></li>
    <li>
      <button class="dropdown-item" id="nativeShareBtn">
        <i class="bi bi-share-fill me-2"></i> System Share
      </button>
    </li>
    <li><hr class="dropdown-divider"></li>
    <li>
      <a class="dropdown-item" id="twitterShare" href="#" target="_blank" rel="noopener">
        <i class="bi bi-twitter-x me-2"></i> Twitter/X
      </a>
    </li>
    <li>
      <a class="dropdown-item" id="linkedinShare" href="#" target="_blank" rel="noopener">
        <i class="bi bi-linkedin me-2"></i> LinkedIn
      </a>
    </li>
    <li>
      <a class="dropdown-item" id="emailShare" href="#">
        <i class="bi bi-envelope me-2"></i> Email
      </a>
    </li>
    <li><hr class="dropdown-divider"></li>
    <li>
      <button class="dropdown-item" id="copyShareLink">
        <i class="bi bi-link-45deg me-2"></i> Copy Link
      </button>
    </li>
  </ul>
</div>

<script>
const shareUrl = encodeURIComponent(window.location.href);
const shareText = encodeURIComponent('Check this out!');

document.getElementById('twitterShare').href =
  `https://twitter.com/intent/tweet?url=${shareUrl}&text=${shareText}`;
document.getElementById('linkedinShare').href =
  `https://www.linkedin.com/sharing/share-offsite/?url=${shareUrl}`;
document.getElementById('emailShare').href =
  `mailto:?subject=Check this out&body=${shareText}%20${shareUrl}`;

document.getElementById('nativeShareBtn').addEventListener('click', async () => {
  if (navigator.share) {
    await navigator.share({
      title: document.title,
      text: 'Check this out!',
      url: window.location.href
    });
  }
});

document.getElementById('copyShareLink').addEventListener('click', async () => {
  await navigator.clipboard.writeText(window.location.href);
  document.getElementById('copyShareLink').innerHTML =
    '<i class="bi bi-check-lg me-2 text-success"></i> Copied!';
  setTimeout(() => {
    document.getElementById('copyShareLink').innerHTML =
      '<i class="bi bi-link-45deg me-2"></i> Copy Link';
  }, 2000);
});

// Hide native share option if not supported
if (!navigator.share) {
  document.getElementById('nativeShareBtn').closest('li').classList.add('d-none');
}
</script>
```

### Product Card with Share

E-commerce product cards with built-in sharing capability.

```html
<div class="card" style="max-width: 300px;">
  <img src="/images/product.jpg" class="card-img-top" alt="Product">
  <div class="card-body">
    <h5 class="card-title">Premium Widget</h5>
    <p class="card-text text-muted">$99.99</p>
    <div class="d-flex gap-2">
      <button class="btn btn-primary flex-grow-1">Add to Cart</button>
      <button class="btn btn-outline-secondary share-product" data-product-id="123"
              data-product-name="Premium Widget" data-product-url="/products/123">
        <i class="bi bi-share"></i>
      </button>
      <button class="btn btn-outline-secondary">
        <i class="bi bi-heart"></i>
      </button>
    </div>
  </div>
</div>

<script>
document.querySelectorAll('.share-product').forEach(btn => {
  btn.addEventListener('click', async () => {
    const data = {
      title: btn.dataset.productName,
      text: `Check out ${btn.dataset.productName}!`,
      url: window.location.origin + btn.dataset.productUrl
    };

    if (navigator.share) {
      try { await navigator.share(data); } catch {}
    } else {
      await navigator.clipboard.writeText(data.url);
      btn.innerHTML = '<i class="bi bi-check-lg text-success"></i>';
      setTimeout(() => { btn.innerHTML = '<i class="bi bi-share"></i>'; }, 2000);
    }
  });
});
</script>
```

## Best Practices

1. Always check `navigator.share` availability before showing share UI
2. Use `navigator.canShare()` to verify the data can be shared before calling share
3. Provide fallback sharing options (clipboard, social links) for unsupported browsers
4. Handle `AbortError` silently when users cancel the share sheet
5. Include meaningful title and description text to improve shared content appearance
6. Use absolute URLs for the share target to ensure links work when shared
7. Test share functionality on actual mobile devices, not just desktop browsers
8. Do not call `navigator.share()` outside of a user gesture (click, tap)
9. Hide the share button entirely on browsers that do not support the API if preferred
10. Use descriptive button text that matches the sharing context
11. Compress images before sharing files to reduce transfer size
12. Track share events for analytics without capturing share content
13. Support multiple file types when sharing documents
14. Use Bootstrap's `dropdown` component for share menus with multiple options

## Common Pitfalls

1. **Desktop incompatibility**: Web Share API has limited support on desktop browsers
2. **Calling outside user gesture**: Share must be triggered by a direct user action
3. **Missing error handling**: Not catching `AbortError` when users dismiss the share sheet
4. **Relative URLs**: Sharing relative URLs results in broken links for recipients
5. **Large files**: Sharing oversized files that fail or timeout
6. **No fallback**: Application becomes unshareable on unsupported platforms
7. **Permission issues**: Sharing files without checking `canShare()` first

## Accessibility Considerations

Use `aria-label` on share buttons describing what will be shared. Provide visible text labels alongside share icons. Announce share success or failure with `aria-live="polite"` regions. Ensure share dropdown menus are keyboard navigable. Mark share menus with `role="menu"` and items with `role="menuitem"`. Include a descriptive label on the share dropdown trigger. Ensure focus returns to the share button after the share sheet closes.

```html
<button class="btn btn-primary" id="shareBtn" aria-label="Share this article">
  <i class="bi bi-share" aria-hidden="true"></i> Share
</button>
<div aria-live="polite" class="visually-hidden" id="shareAnnounce"></div>
```

## Responsive Behavior

On mobile, use full-width share buttons with `d-grid` for easier touch targets. Display share options as a bottom sheet pattern on mobile using Bootstrap's `offcanvas` component. On desktop, show share as a dropdown menu with social link options. Use `btn-lg` for primary share buttons on mobile. Hide text labels and show only icons on very small screens with `d-none d-sm-inline`. Position share dropdowns with `dropdown-menu-end` to prevent overflow on right-aligned cards.
