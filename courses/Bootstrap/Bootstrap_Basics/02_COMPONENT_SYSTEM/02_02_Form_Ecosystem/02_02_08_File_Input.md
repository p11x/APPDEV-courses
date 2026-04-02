---
title: File Input
category: Form Ecosystem
difficulty: 1
time: 25 minutes
tags:
  - bootstrap
  - forms
  - file-input
  - upload
  - accept
---

## Overview

The file input component in Bootstrap provides a styled wrapper around the native `<input type="file">` element. The `.form-control` class applied to a file input produces a consistent appearance across browsers, replacing the default file browser button with a clean, bordered input area that displays the selected filename(s).

The file input supports the `multiple` attribute for selecting more than one file at a time, the `accept` attribute for restricting file types, and the `disabled` attribute for preventing interaction. Bootstrap's styling normalizes the visual differences between Chrome, Firefox, Safari, and Edge, ensuring a uniform experience.

Unlike earlier versions of Bootstrap, version 5 does not require a custom file input class (`.form-file` was removed during the beta). Instead, the standard `.form-control` class is used on the file input, making it consistent with all other form controls. The browser's native file picker dialog is still used for file selection; Bootstrap only styles the visible input area.

File inputs are commonly used in profile forms (avatar uploads), document submission forms, media upload panels, and any interface where users need to provide files. The component integrates with Bootstrap's validation system, form layout utilities, and responsive grid.

## Basic Implementation

The simplest file input uses the `.form-control` class on an `<input type="file">` element with an associated label.

```html
<div class="mb-3">
  <label for="basicFile" class="form-label">Upload File</label>
  <input class="form-control" type="file" id="basicFile">
</div>
```

To allow multiple file selection, add the `multiple` attribute. The input will display the count of selected files when more than one is chosen.

```html
<div class="mb-3">
  <label for="multiFile" class="form-label">Upload Multiple Files</label>
  <input class="form-control" type="file" id="multiFile" multiple>
</div>
```

The `accept` attribute restricts the file types shown in the browser's file picker dialog. This does not enforce server-side validation but guides the user toward the correct file types.

```html
<div class="mb-3">
  <label for="imageFile" class="form-label">Upload Image</label>
  <input class="form-control" type="file" id="imageFile" accept="image/*">
</div>
```

Specific MIME types or file extensions can be used with the `accept` attribute for precise filtering.

```html
<div class="mb-3">
  <label for="pdfFile" class="form-label">Upload PDF Document</label>
  <input class="form-control" type="file" id="pdfFile" accept=".pdf,application/pdf">
</div>
```

A disabled file input prevents file selection and renders with reduced opacity.

```html
<div class="mb-3">
  <label for="disabledFile" class="form-label text-muted">Upload (Disabled)</label>
  <input class="form-control" type="file" id="disabledFile" disabled>
</div>
```

## Advanced Variations

File inputs integrate with Bootstrap's validation system. Apply `.is-valid` or `.is-invalid` classes to show validation states after the user selects a file or submits the form.

```html
<form class="was-validated" novalidate>
  <div class="mb-3">
    <label for="validFile" class="form-label">Required Upload</label>
    <input class="form-control" type="file" id="validFile" required>
    <div class="invalid-feedback">Please select a file to upload.</div>
  </div>

  <button type="submit" class="btn btn-primary">Upload</button>
</form>
```

Combining the `accept` attribute with `multiple` allows selecting multiple files of specific types.

```html
<div class="mb-3">
  <label for="multiImage" class="form-label">Upload Image Gallery</label>
  <input class="form-control" type="file" id="multiImage" accept="image/png, image/jpeg, image/webp" multiple>
  <div class="form-text">Accepted formats: PNG, JPEG, WebP. Maximum 10 files.</div>
</div>
```

JavaScript can be used to display the selected file names, sizes, or preview images. The `change` event on the file input provides access to the `FileList` object.

```html
<div class="mb-3">
  <label for="previewFile" class="form-label">Upload with Preview</label>
  <input class="form-control" type="file" id="previewFile" accept="image/*">
  <div id="filePreview" class="mt-2"></div>
</div>

<script>
document.getElementById('previewFile').addEventListener('change', function(e) {
  const preview = document.getElementById('filePreview');
  preview.innerHTML = '';

  const file = e.target.files[0];
  if (file && file.type.startsWith('image/')) {
    const img = document.createElement('img');
    img.src = URL.createObjectURL(file);
    img.className = 'img-thumbnail mt-2';
    img.style.maxHeight = '200px';
    preview.appendChild(img);

    const info = document.createElement('p');
    info.className = 'text-muted small mb-0 mt-1';
    info.textContent = `${file.name} (${(file.size / 1024).toFixed(1)} KB)`;
    preview.appendChild(info);
  }
});
</script>
```

File inputs inside horizontal forms use the grid system to align the label and input. Use `col-form-label` for proper vertical alignment.

```html
<form>
  <div class="row mb-3">
    <label for="horizFile" class="col-sm-3 col-form-label">Resume</label>
    <div class="col-sm-9">
      <input class="form-control" type="file" id="horizFile" accept=".pdf,.doc,.docx">
      <div class="form-text">Accepted formats: PDF, DOC, DOCX</div>
    </div>
  </div>

  <div class="row mb-3">
    <label for="horizPhoto" class="col-sm-3 col-form-label">Photo</label>
    <div class="col-sm-9">
      <input class="form-control" type="file" id="horizPhoto" accept="image/*">
    </div>
  </div>
</form>
```

A file input with a small or large size variant uses the `.form-control-sm` or `.form-control-lg` classes respectively.

```html
<div class="mb-3">
  <label for="smallFile" class="form-label">Small File Input</label>
  <input class="form-control form-control-sm" type="file" id="smallFile">
</div>

<div class="mb-3">
  <label for="largeFile" class="form-label">Large File Input</label>
  <input class="form-control form-control-lg" type="file" id="largeFile">
</div>
```

## Best Practices

- **Always include a `<label>` for the file input.** The label describes what the user should upload. Without it, the input displays only "Choose file" with no context about acceptable content.
- **Use the `accept` attribute** to guide users toward the correct file types. While it does not enforce server-side validation, it filters the file picker to show only matching files, reducing user errors.
- **Combine `accept` with server-side validation.** The `accept` attribute can be bypassed by the user. Always verify file types on the server after upload.
- **Use `multiple` when users need to upload several files.** Without it, each file requires a separate input or interaction. The `multiple` attribute streamlines batch uploads.
- **Provide a `form-text` hint** below the file input to specify size limits, accepted formats, and any other constraints. Users need to know these restrictions before selecting files.
- **Show file names or previews after selection.** The native file input only shows a truncated filename. Use JavaScript to display the full name, file size, or a thumbnail preview for better UX.
- **Use validation states appropriately.** Apply `.is-invalid` when a required file is missing or an invalid file type is selected. Apply `.is-valid` when a file is accepted.
- **Test across browsers.** Firefox and Chrome display the file input slightly differently even with Bootstrap's normalization. Verify the appearance on all target browsers.
- **Consider file size limits in the UI.** Display the maximum allowed file size near the input to prevent users from attempting large uploads that will fail.
- **Do not disable the file input as a substitute for validation.** A disabled input cannot receive focus, which creates accessibility issues. Use validation feedback to communicate errors instead.
- **Use the `capture` attribute on mobile devices** when you want to prompt the camera directly. Adding `capture="environment"` or `capture="user"` to a file input with `accept="image/*"` opens the camera instead of the file picker on supported mobile browsers.
- **Preserve the native file picker behavior.** Avoid building custom drag-and-drop interfaces that replace the file input entirely, as they often lack keyboard accessibility. Instead, enhance the native input with additional drop zone behavior.

## Common Pitfalls

- **Forgetting that `accept` is not enforced server-side.** A user can bypass the `accept` filter by selecting "All Files" in the file picker or by dragging a file onto the input. Always validate file types on the server.
- **Not providing feedback after file selection.** The native file input shows a brief filename, but users may not notice it, especially on mobile. Always display a confirmation of the selected file(s) using JavaScript.
- **Using `accept="image/*"` when only specific formats are needed.** This accepts all image types, including SVG (which can contain scripts) and BMP (which are often very large). Be specific with accepted formats.
- **Omitting the `enctype="multipart/form-data"` attribute on the form.** Without this, the form sends data as `application/x-www-form-urlencoded`, and file data is not transmitted to the server.
- **Expecting the filename to be fully visible.** Long filenames are truncated in the file input display. Use JavaScript to show the full filename elsewhere on the page after selection.
- **Not handling the case where the user cancels the file picker.** When the user opens the file dialog and clicks Cancel, the `change` event does not fire. If your code relies on this event for state management, it may not update correctly.
- **Using file inputs inside input groups.** While technically possible, file inputs inside `.input-group` containers may have inconsistent styling across browsers because of how browsers render the file input's internal button element.
- **Forgetting accessibility on the file preview area.** If you display previews or file information with JavaScript, ensure the container is announced by screen readers using `aria-live="polite"` or by moving focus to the preview.

## Accessibility Considerations

File inputs are natively accessible when implemented with proper labeling. The `<label>` element associated with the file input via the `for` attribute provides the accessible name that screen readers announce when the input receives focus.

The `accept` attribute does not affect the accessible name or description, so its constraints should be communicated through visible text (such as `.form-text`) rather than relying on the file picker dialog's filtering behavior. Screen reader users may not see the filtered file types in the picker.

```html
<div class="mb-3">
  <label for="accFile" class="form-label">Upload Profile Picture</label>
  <input
    class="form-control"
    type="file"
    id="accFile"
    accept="image/png, image/jpeg"
    aria-describedby="accFileHelp"
  >
  <div id="accFileHelp" class="form-text">
    Accepted formats: PNG, JPEG. Maximum file size: 5MB.
  </div>
</div>
```

When displaying file previews or selected file information via JavaScript, use `aria-live="polite"` on the container so that screen readers announce the change when files are selected.

```html
<div class="mb-3">
  <label for="liveFile" class="form-label">Upload Document</label>
  <input class="form-control" type="file" id="liveFile">
  <div id="liveFileInfo" class="form-text" aria-live="polite"></div>
</div>

<script>
document.getElementById('liveFile').addEventListener('change', function(e) {
  const info = document.getElementById('liveFileInfo');
  const files = e.target.files;
  if (files.length === 1) {
    info.textContent = `Selected: ${files[0].name} (${(files[0].size / 1024).toFixed(1)} KB)`;
  } else if (files.length > 1) {
    info.textContent = `${files.length} files selected`;
  }
});
</script>
```

For validation states, the `.invalid-feedback` element should be linked to the input with `aria-describedby` so that the error message is announced when the user focuses the invalid file input.

## Responsive Behavior

File inputs are fully responsive by default. The `.form-control` class on a file input stretches to fill the full width of its container at all breakpoints. No additional responsive utilities are needed.

When placed inside Bootstrap grid columns, file inputs adapt to their column width. A file input inside a `col-md-6` column will be half-width on medium screens and full-width on small screens when the column stacks.

```html
<div class="row">
  <div class="col-md-6 mb-3">
    <label for="respFront" class="form-label">Front Photo</label>
    <input class="form-control" type="file" id="respFront" accept="image/*">
  </div>

  <div class="col-md-6 mb-3">
    <label for="respBack" class="form-label">Back Photo</label>
    <input class="form-control" type="file" id="respBack" accept="image/*">
  </div>
</div>
```

On very narrow screens (under 350px), the file input's text ("Choose file" and the selected filename) may overflow. Bootstrap's `.text-truncate` class cannot be directly applied to the internal elements of a file input because browsers render them as shadow DOM components. If overflow is a concern on extremely small viewports, consider using a custom file upload component built with JavaScript that provides better control over the display.

The size variants (`.form-control-sm` and `.form-control-lg`) scale the file input consistently across all breakpoints. Use them to match the file input's visual weight with other form controls in the same form.

```html
<div class="row">
  <div class="col-md-4 mb-3">
    <label for="smResp" class="form-label">Small</label>
    <input class="form-control form-control-sm" type="file" id="smResp">
  </div>

  <div class="col-md-4 mb-3">
    <label for="defResp" class="form-label">Default</label>
    <input class="form-control" type="file" id="defResp">
  </div>

  <div class="col-md-4 mb-3">
    <label for="lgResp" class="form-label">Large</label>
    <input class="form-control form-control-lg" type="file" id="lgResp">
  </div>
</div>
```

For file inputs in horizontal forms that collapse to stacked layouts on mobile, the label and input will automatically stack vertically on small screens when using Bootstrap's column classes with `col-12` on the mobile breakpoint.
