---
title: "Image Crop UI"
description: "Build image cropping interfaces with crop previews, aspect ratio selectors, and crop modals using Bootstrap 5 and Cropper.js."
difficulty: 3
estimated_time: "45 minutes"
prerequisites:
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Forms"
  - "Bootstrap 5 Button Group"
  - "Bootstrap 5 Cards"
---

## Overview

Image crop UIs allow users to select and crop portions of an uploaded image for profile photos, product images, or content thumbnails. Bootstrap 5 provides the modal container, aspect ratio controls, and preview cards, while a library like Cropper.js handles the actual cropping interaction.

The component includes an image upload trigger, a crop area within a modal, aspect ratio presets, zoom controls, a live preview of the cropped result, and a confirm/cancel workflow. This is essential for any CMS or profile management system.

## Basic Implementation

### Image Upload Trigger

```html
<div class="text-center">
  <div class="position-relative d-inline-block">
    <img src="placeholder-avatar.jpg" class="rounded-circle" width="120" height="120" style="object-fit: cover;" id="profilePreview" alt="Profile photo">
    <label for="imageUpload" class="position-absolute bottom-0 end-0 btn btn-sm btn-primary rounded-circle" style="width: 36px; height: 36px; line-height: 28px;" title="Change photo">
      <i class="bi bi-camera"></i>
    </label>
    <input type="file" class="d-none" id="imageUpload" accept="image/*">
  </div>
  <div class="form-text mt-2">Click the camera icon to upload a new photo</div>
</div>
```

### Crop Modal Structure

```html
<div class="modal fade" id="cropModal" tabindex="-1" aria-labelledby="cropModalLabel">
  <div class="modal-dialog modal-lg">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="cropModalLabel">Crop Image</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="row">
          <div class="col-md-8">
            <div class="bg-dark rounded text-center p-3" style="min-height: 300px;">
              <img src="uploaded-image.jpg" id="cropImage" class="img-fluid" alt="Image to crop" style="max-width: 100%;">
            </div>
          </div>
          <div class="col-md-4">
            <h6>Preview</h6>
            <div class="border rounded overflow-hidden mb-3" style="width: 150px; height: 150px;">
              <div id="cropPreview" class="w-100 h-100" style="overflow: hidden;">
                <img src="uploaded-image.jpg" class="img-fluid" alt="Crop preview">
              </div>
            </div>
            <div class="border rounded overflow-hidden mb-3" style="width: 150px; height: 84px;">
              <div class="w-100 h-100" style="overflow: hidden;">
                <img src="uploaded-image.jpg" class="img-fluid" alt="Wide crop preview">
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
        <button type="button" class="btn btn-primary" id="cropConfirm">Apply Crop</button>
      </div>
    </div>
  </div>
</div>
```

### Aspect Ratio Selector

```html
<div class="mb-3">
  <label class="form-label">Aspect Ratio</label>
  <div class="btn-group w-100" role="group" aria-label="Aspect ratio">
    <input type="radio" class="btn-check" name="aspectRatio" id="ratioFree" checked>
    <label class="btn btn-outline-primary btn-sm" for="ratioFree">Free</label>
    <input type="radio" class="btn-check" name="aspectRatio" id="ratio1x1">
    <label class="btn btn-outline-primary btn-sm" for="ratio1x1">1:1</label>
    <input type="radio" class="btn-check" name="aspectRatio" id="ratio16x9">
    <label class="btn btn-outline-primary btn-sm" for="ratio16x9">16:9</label>
    <input type="radio" class="btn-check" name="aspectRatio" id="ratio4x3">
    <label class="btn btn-outline-primary btn-sm" for="ratio4x3">4:3</label>
    <input type="radio" class="btn-check" name="aspectRatio" id="ratio3x2">
    <label class="btn btn-outline-primary btn-sm" for="ratio3x2">3:2</label>
  </div>
</div>
```

## Advanced Variations

### Zoom Controls

```html
<div class="mb-3">
  <label class="form-label">Zoom</label>
  <div class="d-flex align-items-center gap-2">
    <button class="btn btn-outline-secondary btn-sm" id="zoomOut"><i class="bi bi-zoom-out"></i></button>
    <input type="range" class="form-range flex-grow-1" min="0.1" max="3" step="0.1" value="1" id="zoomSlider">
    <button class="btn btn-outline-secondary btn-sm" id="zoomIn"><i class="bi bi-zoom-in"></i></button>
    <button class="btn btn-outline-secondary btn-sm" id="zoomReset" title="Reset zoom"><i class="bi bi-arrow-counterclockwise"></i></button>
  </div>
</div>
```

### Rotation Controls

```html
<div class="mb-3">
  <label class="form-label">Rotate</label>
  <div class="btn-group" role="group">
    <button class="btn btn-outline-secondary btn-sm" id="rotateLeft" title="Rotate left 90°">
      <i class="bi bi-arrow-counterclockwise"></i> 90°
    </button>
    <button class="btn btn-outline-secondary btn-sm" id="rotateRight" title="Rotate right 90°">
      <i class="bi bi-arrow-clockwise"></i> 90°
    </button>
    <button class="btn btn-outline-secondary btn-sm" id="flipH" title="Flip horizontal">
      <i class="bi bi-symmetry-horizontal"></i>
    </button>
    <button class="btn btn-outline-secondary btn-sm" id="flipV" title="Flip vertical">
      <i class="bi bi-symmetry-vertical"></i>
    </button>
  </div>
</div>
```

### Crop Dimension Display

```html>
<div class="bg-light rounded p-2 mb-3">
  <div class="row text-center small">
    <div class="col-4">
      <div class="text-muted">Width</div>
      <strong id="cropWidth">320px</strong>
    </div>
    <div class="col-4">
      <div class="text-muted">Height</div>
      <strong id="cropHeight">320px</strong>
    </div>
    <div class="col-4">
      <div class="text-muted">Ratio</div>
      <strong id="cropRatio">1:1</strong>
    </div>
  </div>
</div>
```

### Multiple Output Sizes Preview

```html
<h6>Output Preview</h6>
<div class="d-flex gap-3 flex-wrap">
  <div class="text-center">
    <div class="border rounded-circle overflow-hidden mx-auto mb-1" style="width: 80px; height: 80px;">
      <img src="uploaded-image.jpg" class="w-100 h-100" style="object-fit: cover;" alt="Circle preview">
    </div>
    <small class="text-muted">Profile (80px)</small>
  </div>
  <div class="text-center">
    <div class="border rounded overflow-hidden mx-auto mb-1" style="width: 120px; height: 67px;">
      <img src="uploaded-image.jpg" class="w-100 h-100" style="object-fit: cover;" alt="Thumbnail preview">
    </div>
    <small class="text-muted">Thumbnail (120x67)</small>
  </div>
  <div class="text-center">
    <div class="border rounded overflow-hidden mx-auto mb-1" style="width: 200px; height: 112px;">
      <img src="uploaded-image.jpg" class="w-100 h-100" style="object-fit: cover;" alt="Banner preview">
    </div>
    <small class="text-muted">Banner (200x112)</small>
  </div>
</div>
```

## Best Practices

1. Use a modal to contain the crop interface and prevent background interaction
2. Provide aspect ratio presets relevant to your use case (1:1 for avatars, 16:9 for banners)
3. Show a live preview of the cropped area as the user adjusts
4. Display the crop dimensions (width x height) in real-time
5. Include zoom controls with both slider and +/- buttons
6. Provide rotation and flip controls for image orientation correction
7. Use `accept="image/*"` on the file input to restrict to image files
8. Show the original filename and file size in the modal
9. Validate image dimensions and file size before allowing crop
10. Use `object-fit: cover` on preview containers for consistent display
11. Provide multiple output size previews to show how the crop will appear
12. Include a reset button to restore the original crop area
13. Support keyboard shortcuts: arrow keys for nudging, +/- for zoom

## Common Pitfalls

1. **No aspect ratio constraints**: Free-form cropping produces images that do not fit predefined layouts. Always offer relevant presets.
2. **Missing file size validation**: Accepting 50MB images causes performance issues. Validate file size before opening the crop modal.
3. **No preview of cropped result**: Without a preview, users cannot verify the crop before applying it.
4. **Forgetting mobile touch support**: Desktop-only crop tools do not work on mobile. Use a library that supports touch gestures.
5. **No minimum crop size**: Allowing crops that are 10x10 pixels produces unusable results. Set minimum dimensions.
6. **Missing loading state during upload**: Large image processing needs a loading indicator so users know the image is being processed.
7. **No image format validation**: Accepting SVG or animated GIF files without handling them causes unexpected behavior.

## Accessibility Considerations

- Use `aria-label` on all crop control buttons (zoom, rotate, aspect ratio)
- Provide keyboard navigation for aspect ratio selection using `btn-check` pattern
- Announce crop dimensions using `aria-live="polite"` regions
- Ensure the modal traps focus and returns focus on close
- Include descriptive `alt` text on all preview images
- Use `role="group"` with `aria-label` on button groups for zoom and rotate controls
- Provide text alternatives for visual previews describing the expected output

## Responsive Behavior

On mobile, the crop modal should use `modal-fullscreen-sm-down` for maximum viewport usage. The aspect ratio selector should wrap buttons on narrow screens. Preview images should stack vertically below the crop area. Zoom controls should remain full-width accessible. The output preview section should use flex-wrap for multiple size previews. Crop dimension display should remain compact and readable.
