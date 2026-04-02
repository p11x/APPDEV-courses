---
title: "Size Guide Modal"
description: "Create accessible size guide modals with measurement charts, sizing tables, and recommendation UI using Bootstrap 5."
difficulty: 1
estimated_time: "25 minutes"
prerequisites:
  - "Bootstrap 5 Modals"
  - "Bootstrap 5 Tables"
  - "Bootstrap 5 Forms"
---

## Overview

Size guide modals reduce return rates by helping customers select the correct size before purchasing. Bootstrap 5's modal component provides the overlay container, while tables and form controls display measurement data and size recommendation tools. A well-designed size guide includes measurement instructions, size charts, unit conversion, and optionally a size recommendation quiz.

These modals should be lightweight, accessible, and easy to dismiss. They typically trigger from a link near the product size selector and present information without navigating away from the product page.

## Basic Implementation

### Basic Size Guide Modal Trigger

```html
<button type="button" class="btn btn-link p-0 text-decoration-underline" data-bs-toggle="modal" data-bs-target="#sizeGuideModal">
  <i class="bi bi-rulers me-1"></i> Size Guide
</button>
```

### Size Guide Modal Structure

```html
<div class="modal fade" id="sizeGuideModal" tabindex="-1" aria-labelledby="sizeGuideModalLabel" aria-hidden="true">
  <div class="modal-dialog modal-lg modal-dialog-scrollable">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="sizeGuideModalLabel">Size Guide</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <h6>How to Measure</h6>
        <p class="text-muted">Use a flexible measuring tape and measure over light clothing.</p>
        <div class="table-responsive">
          <table class="table table-bordered text-center">
            <thead class="table-light">
              <tr>
                <th>Size</th>
                <th>Chest (in)</th>
                <th>Waist (in)</th>
                <th>Hips (in)</th>
              </tr>
            </thead>
            <tbody>
              <tr><td>XS</td><td>32-34</td><td>24-26</td><td>34-36</td></tr>
              <tr><td>S</td><td>34-36</td><td>26-28</td><td>36-38</td></tr>
              <tr><td>M</td><td>36-38</td><td>28-30</td><td>38-40</td></tr>
              <tr><td>L</td><td>38-40</td><td>30-32</td><td>40-42</td></tr>
              <tr><td>XL</td><td>40-42</td><td>32-34</td><td>42-44</td></tr>
            </tbody>
          </table>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>
```

### Unit Toggle

```html
<div class="btn-group mb-3" role="group" aria-label="Unit system">
  <input type="radio" class="btn-check" name="unit" id="unitInches" checked>
  <label class="btn btn-outline-primary btn-sm" for="unitInches">Inches</label>
  <input type="radio" class="btn-check" name="unit" id="unitCm">
  <label class="btn btn-outline-primary btn-sm" for="unitCm">Centimeters</label>
</div>
```

## Advanced Variations

### Size Recommendation Quiz

```html
<div class="card bg-light mb-3">
  <div class="card-body">
    <h6 class="card-title"><i class="bi bi-magic me-1"></i> Find Your Perfect Size</h6>
    <form>
      <div class="row g-3">
        <div class="col-md-6">
          <label class="form-label">Height</label>
          <select class="form-select form-select-sm">
            <option selected>Select height range</option>
            <option>5'0" - 5'3"</option>
            <option>5'3" - 5'6"</option>
            <option>5'6" - 5'9"</option>
            <option>5'9" - 6'0"</option>
          </select>
        </div>
        <div class="col-md-6">
          <label class="form-label">Weight</label>
          <select class="form-select form-select-sm">
            <option selected>Select weight range</option>
            <option>100-130 lbs</option>
            <option>130-160 lbs</option>
            <option>160-190 lbs</option>
          </select>
        </div>
        <div class="col-md-6">
          <label class="form-label">Body Fit Preference</label>
          <select class="form-select form-select-sm">
            <option>Regular fit</option>
            <option>Slim fit</option>
            <option>Relaxed fit</option>
          </select>
        </div>
        <div class="col-12">
          <button type="button" class="btn btn-primary btn-sm">Get Recommendation</button>
        </div>
      </div>
    </form>
    <div class="alert alert-success mt-3 mb-0 d-none" id="sizeResult">
      <strong>Recommended Size: M</strong> - Based on your measurements, Medium will provide the best fit.
    </div>
  </div>
</div>
```

### Measurement Illustration with Tabs

```html
<ul class="nav nav-tabs" id="measurementTabs" role="tablist">
  <li class="nav-item" role="presentation">
    <button class="nav-link active" id="chest-tab" data-bs-toggle="tab" data-bs-target="#chest" type="button" role="tab">Chest</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="waist-tab" data-bs-toggle="tab" data-bs-target="#waist" type="button" role="tab">Waist</button>
  </li>
  <li class="nav-item" role="presentation">
    <button class="nav-link" id="hips-tab" data-bs-toggle="tab" data-bs-target="#hips" type="button" role="tab">Hips</button>
  </li>
</ul>
<div class="tab-content border border-top-0 p-3" id="measurementTabContent">
  <div class="tab-pane fade show active" id="chest" role="tabpanel">
    <div class="row align-items-center">
      <div class="col-md-5">
        <img src="chest-measure.svg" class="img-fluid" alt="How to measure chest">
      </div>
      <div class="col-md-7">
        <p>Measure around the fullest part of your chest, keeping the tape horizontal.</p>
        <ol class="small text-muted">
          <li>Stand straight with arms at your sides</li>
          <li>Wrap tape around the fullest part</li>
          <li>Keep tape parallel to the floor</li>
          <li>Breathe normally and read the measurement</li>
        </ol>
      </div>
    </div>
  </div>
  <div class="tab-pane fade" id="waist" role="tabpanel">
    <p>Measure around your natural waistline, keeping the tape comfortably loose.</p>
  </div>
  <div class="tab-pane fade" id="hips" role="tabpanel">
    <p>Measure around the fullest part of your hips while standing with feet together.</p>
  </div>
</div>
```

### International Size Conversion Table

```html
<div class="table-responsive">
  <table class="table table-sm table-bordered text-center">
    <thead class="table-light">
      <tr>
        <th>US</th>
        <th>UK</th>
        <th>EU</th>
        <th>AU</th>
        <th>Japan</th>
      </tr>
    </thead>
    <tbody>
      <tr><td>XS (0-2)</td><td>4-6</td><td>32-34</td><td>4-6</td><td>3-5</td></tr>
      <tr><td>S (4-6)</td><td>8-10</td><td>36-38</td><td>8-10</td><td>7-9</td></tr>
      <tr><td>M (8-10)</td><td>12-14</td><td>40-42</td><td>12-14</td><td>11-13</td></tr>
      <tr class="table-primary"><td>L (12-14)</td><td>16-18</td><td>44-46</td><td>16-18</td><td>15-17</td></tr>
    </tbody>
  </table>
</div>
```

## Best Practices

1. Use `modal-dialog-scrollable` for size charts that exceed viewport height
2. Always include unit toggle between inches and centimeters for international customers
3. Provide visual measurement guides alongside text instructions
4. Place the size guide trigger close to the size selector on the product page
5. Use `table-bordered` and `table-light` for clear size chart readability
6. Highlight the currently selected size row in the chart using `table-primary`
7. Include fit notes (slim, regular, relaxed) next to measurements
8. Test modal behavior on mobile devices for proper scroll behavior
9. Use `aria-labelledby` to connect the modal title for accessibility
10. Keep the modal close button always visible with `modal-dialog-scrollable`
11. Pre-load size guide data to avoid modal opening delays
12. Use consistent sizing terminology across all product categories
13. Add measurement illustration images with descriptive alt text

## Common Pitfalls

1. **No scrollable body on long charts**: Without `modal-dialog-scrollable`, users cannot access content below the fold on smaller screens.
2. **Missing unit conversion**: Displaying only inches excludes international customers. Always provide centimeter equivalents.
3. **Static size data**: Hardcoding size tables means updates require code changes. Load size data from a CMS or API.
4. **Blocking body scroll**: Bootstrap modals handle this by default, but custom CSS can break this behavior causing double scrollbars.
5. **No keyboard dismissal**: Missing `data-bs-dismiss="modal"` on the close button or `Escape` key support traps users in the modal.
6. **Generic alt text on illustrations**: Using "size guide image" as alt text provides no value to screen reader users. Describe the measurement technique.
7. **Oversized modal on mobile**: Using `modal-xl` on mobile devices creates usability issues. Use `modal-lg` at most or responsive sizing.

## Accessibility Considerations

- Set `aria-labelledby` on the modal to reference the title element
- Use `aria-describedby` to reference body content when the modal opens
- Ensure focus is trapped inside the modal when open (Bootstrap handles this automatically)
- Provide `aria-label` on the unit toggle group
- Use proper `th` and `scope` attributes in size tables for screen reader navigation
- Announce size recommendations using `aria-live` regions
- Ensure the close button has `aria-label="Close"` even when using the icon-only variant

## Responsive Behavior

On screens below 768px, the modal should use `modal-fullscreen-sm-down` to maximize screen real estate. Size tables should use `table-responsive` wrapping to allow horizontal scrolling on narrow screens. The measurement illustration should stack above the instructions using `col-12` on small breakpoints. Unit toggle buttons should remain full-width accessible tap targets. The size recommendation form should stack its selects vertically using the existing `col-md-6` grid behavior.
