---
title: "AI Image Alt Text"
slug: "ai-image-alt-text"
difficulty: 2
tags: ["bootstrap", "ai", "accessibility", "images", "alt-text"]
prerequisites:
  - "03_01_Card_Components"
  - "05_01_Accessibility_Overview"
related:
  - "06_01_11_AI_Content_Summary"
  - "06_01_14_AI_Personalization_UI"
duration: "25 minutes"
---

# AI Image Alt Text

## Overview

Automated alt text generation uses computer vision AI to describe images, dramatically improving accessibility compliance. Instead of relying on manual alt text entry, AI models analyze image content and generate descriptive text. Bootstrap image components display the visuals, while the AI service provides contextual descriptions. This pattern is critical for content-heavy sites, CMS platforms, and any application where users upload images without providing descriptions.

## Basic Implementation

An image component with AI-generated alt text fetched on page load.

```html
<div class="container mt-4">
  <div class="card" style="max-width: 500px;">
    <img src="/images/hero-photo.jpg" class="card-img-top ai-image"
         data-ai-alt="true" loading="lazy">
    <div class="card-body">
      <div class="d-flex justify-content-between align-items-center">
        <small class="text-muted" id="altStatus-1">
          <span class="spinner-border spinner-border-sm"></span> Generating alt text...
        </small>
        <button class="btn btn-sm btn-outline-secondary" data-edit-alt="1">
          <i class="bi bi-pencil"></i> Edit
        </button>
      </div>
    </div>
  </div>
</div>

<script>
document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('[data-ai-alt]').forEach(async (img) => {
    try {
      const res = await fetch('/api/ai/describe-image', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ src: img.src })
      });
      const data = await res.json();
      img.alt = data.description;
      const statusEl = img.closest('.card').querySelector('[id^="altStatus"]');
      statusEl.innerHTML = `<i class="bi bi-check-circle text-success"></i> AI-generated alt text`;
    } catch (err) {
      img.alt = 'Image';
      const statusEl = img.closest('.card').querySelector('[id^="altStatus"]');
      statusEl.innerHTML = `<i class="bi bi-exclamation-triangle text-warning"></i> Could not generate alt text`;
    }
  });
});
</script>
```

## Advanced Variations

### Alt Text Editor with AI Suggestions

Display AI-generated alt text in an editable field, allowing users to refine descriptions.

```html
<div class="card mb-3">
  <img src="/uploads/user-photo.jpg" class="card-img-top" id="previewImg">
  <div class="card-body">
    <label class="form-label fw-semibold">Alt Text</label>
    <div class="input-group">
      <input type="text" class="form-control" id="altTextInput"
             placeholder="Describe this image...">
      <button class="btn btn-outline-primary" type="button" id="generateAlt">
        <i class="bi bi-stars"></i> AI Suggest
      </button>
    </div>
    <div class="form-text">
      <span id="charCount">0</span>/125 characters recommended
    </div>
    <div id="aiSuggestion" class="alert alert-info mt-2 d-none">
      <strong>AI Suggestion:</strong> <span id="suggestedText"></span>
      <div class="mt-2">
        <button class="btn btn-sm btn-success" id="acceptSuggestion">Accept</button>
        <button class="btn btn-sm btn-outline-secondary" id="dismissSuggestion">Dismiss</button>
      </div>
    </div>
  </div>
</div>

<script>
document.getElementById('generateAlt').addEventListener('click', async () => {
  const imgSrc = document.getElementById('previewImg').src;
  const res = await fetch('/api/ai/describe-image', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ src: imgSrc, style: 'detailed' })
  });
  const data = await res.json();

  document.getElementById('suggestedText').textContent = data.description;
  document.getElementById('aiSuggestion').classList.remove('d-none');
});

document.getElementById('acceptSuggestion').addEventListener('click', () => {
  document.getElementById('altTextInput').value =
    document.getElementById('suggestedText').textContent;
  document.getElementById('aiSuggestion').classList.add('d-none');
  updateCharCount();
});

document.getElementById('altTextInput').addEventListener('input', updateCharCount);

function updateCharCount() {
  const count = document.getElementById('altTextInput').value.length;
  document.getElementById('charCount').textContent = count;
  document.getElementById('charCount').className =
    count > 125 ? 'text-danger' : '';
}
</script>
```

### Bulk Alt Text Audit Dashboard

Scan all images on a page and report accessibility status.

```html
<div class="card">
  <div class="card-header d-flex justify-content-between align-items-center">
    <h5 class="mb-0">Image Accessibility Audit</h5>
    <button class="btn btn-primary btn-sm" id="runAudit">
      <i class="bi bi-search"></i> Scan Page
    </button>
  </div>
  <div class="card-body">
    <div class="row mb-3 text-center">
      <div class="col-4">
        <div class="fs-3 fw-bold text-danger" id="missingCount">-</div>
        <small class="text-muted">Missing Alt</small>
      </div>
      <div class="col-4">
        <div class="fs-3 fw-bold text-warning" id="poorCount">-</div>
        <small class="text-muted">Needs Improvement</small>
      </div>
      <div class="col-4">
        <div class="fs-3 fw-bold text-success" id="goodCount">-</div>
        <small class="text-muted">Good</small>
      </div>
    </div>
    <div id="auditResults" class="list-group"></div>
  </div>
</div>

<script>
document.getElementById('runAudit').addEventListener('click', async () => {
  const images = Array.from(document.querySelectorAll('img')).map(img => ({
    src: img.src,
    alt: img.alt,
    width: img.naturalWidth,
    height: img.naturalHeight
  }));

  const res = await fetch('/api/ai/audit-alt-text', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ images })
  });
  const audit = await res.json();

  document.getElementById('missingCount').textContent = audit.missing;
  document.getElementById('poorCount').textContent = audit.poor;
  document.getElementById('goodCount').textContent = audit.good;

  document.getElementById('auditResults').innerHTML = audit.items.map(item => `
    <div class="list-group-item">
      <div class="d-flex align-items-center">
        <img src="${item.src}" class="rounded me-3" style="width:60px;height:60px;object-fit:cover;">
        <div class="flex-grow-1">
          <div class="badge bg-${item.status === 'good' ? 'success' : item.status === 'poor' ? 'warning' : 'danger'} mb-1">
            ${item.status}
          </div>
          <p class="mb-0 small">${item.currentAlt || '<em>No alt text</em>'}</p>
          ${item.suggestion ? `<p class="mb-0 small text-primary">Suggested: ${item.suggestion}</p>` : ''}
        </div>
        ${item.suggestion ? `<button class="btn btn-sm btn-outline-primary apply-suggestion" data-alt="${item.suggestion}">Apply</button>` : ''}
      </div>
    </div>
  `).join('');
});
</script>
```

### Upload with Auto Alt Text

Automatically generate alt text when users upload images.

```html
<div class="mb-3">
  <label for="imageUpload" class="form-label">Upload Image</label>
  <input type="file" class="form-control" id="imageUpload" accept="image/*">
  <div id="uploadPreview" class="mt-3 d-none">
    <img id="previewThumb" class="img-thumbnail mb-2" style="max-height: 200px;">
    <div class="input-group">
      <span class="input-group-text"><i class="bi bi-stars"></i></span>
      <input type="text" class="form-control" id="autoAltText" readonly>
      <button class="btn btn-outline-secondary" type="button" id="editAutoAlt">
        <i class="bi bi-pencil"></i>
      </button>
    </div>
  </div>
</div>

<script>
document.getElementById('imageUpload').addEventListener('change', async (e) => {
  const file = e.target.files[0];
  if (!file) return;

  const reader = new FileReader();
  reader.onload = async (ev) => {
    document.getElementById('previewThumb').src = ev.target.result;
    document.getElementById('uploadPreview').classList.remove('d-none');

    const res = await fetch('/api/ai/describe-image-base64', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ image: ev.target.result })
    });
    const data = await res.json();
    document.getElementById('autoAltText').value = data.description;
  };
  reader.readAsDataURL(file);
});
</script>
```

## Best Practices

1. Always allow users to edit or override AI-generated alt text
2. Keep alt text under 125 characters for optimal screen reader compatibility
3. Use specific, descriptive language rather than generic labels like "image" or "photo"
4. Generate alt text asynchronously to avoid blocking page rendering
5. Cache generated alt text to avoid redundant API calls for the same image
6. Provide a visual indicator showing which alt text was AI-generated
7. Audit regularly for images missing alt text using automated scanning
8. Use `loading="lazy"` on images to defer both loading and alt text generation
9. Handle API failures gracefully with meaningful fallback descriptions
10. Store alt text in the CMS or database after user approval
11. Avoid including "image of" or "picture of" in alt text, screen readers already announce it as an image
12. Differentiate decorative images (empty alt) from informational images
13. Log alt text generation quality for continuous model improvement
14. Respect image privacy by not sending sensitive images to external APIs

## Common Pitfalls

1. **No edit capability**: Forcing users to accept AI descriptions without modification
2. **Generic descriptions**: AI returning "a photo" instead of meaningful content descriptions
3. **Blocking rendering**: Waiting for AI alt text before displaying the image
4. **Ignoring decorative images**: Generating alt text for spacer or decorative images that should have `alt=""`
5. **No error state**: Blank alt attributes when the AI service fails
6. **Exceeding length limits**: Generating overly long alt text that overwhelms screen reader users
7. **Privacy violations**: Sending user-uploaded images to external AI without consent

## Accessibility Considerations

Ensure all images have alt attributes even while AI generation is pending. Use `aria-describedby` to link images to extended descriptions when needed. Provide a visible "Alt text quality" indicator for content editors. Announce alt text generation completion with `aria-live` regions. Support manual alt text entry for users who prefer to write their own descriptions. Ensure the alt text editor itself is keyboard accessible and screen reader friendly.

```html
<img src="photo.jpg" alt="" aria-describedby="extendedDesc" role="img">
<div id="extendedDesc" class="visually-hidden">
  Extended description will be populated by AI
</div>
<div aria-live="polite" class="visually-hidden" id="altTextAnnounce"></div>
```

## Responsive Behavior

On mobile, display the alt text editor below the image preview in a stacked layout. Use `img-fluid` on all images to ensure they scale within their containers. Collapse the audit dashboard into a compact list on small screens with expandable details. Use `d-grid` for action buttons on mobile. On desktop, show image thumbnails and alt text fields side by side in `row`/`col` layouts. Ensure the audit results list scrolls independently with `overflow-y: auto` and a max height.
