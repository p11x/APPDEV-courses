# Bootstrap Buttons

## Definition

Bootstrap buttons are styled HTML button elements that provide clickable actions with consistent, attractive styling. Buttons come in multiple color variants (primary, secondary, success, etc.) that convey different meanings, plus size variations (small, large) and outline styles. The `.btn` class is the base class that enables all button styling.

## Key Bullet Points

- **`.btn`**: Base class that enables Bootstrap button styling
- **Color Variants**: btn-primary, btn-secondary, btn-success, btn-danger, btn-warning, btn-info, btn-light, btn-dark, btn-link
- **Button Sizes**: btn-sm (small), btn-lg (large), default size (medium)
- **Outline Buttons**: btn-outline-primary, btn-outline-secondary, etc. (border only, no fill)
- **Block Buttons**: btn-block spans full width of parent container
- **States**: Buttons can show active (pressed) and disabled states

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Buttons</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Button Showcase</h2>

    <!-- BUTTON COLOR VARIANTS -->
    <h5>Solid Color Buttons</h5>
    <div class="mb-4">
        <button type="button" class="btn btn-primary">Primary</button>
        <button type="button" class="btn btn-secondary">Secondary</button>
        <button type="button" class="btn btn-success">Success</button>
        <button type="button" class="btn btn-danger">Danger</button>
        <button type="button" class="btn btn-warning">Warning</button>
        <button type="button" class="btn btn-info">Info</button>
        <button type="button" class="btn btn-light">Light</button>
        <button type="button" class="btn btn-dark">Dark</button>
        <button type="button" class="btn btn-link">Link</button>
    </div>

    <!-- OUTLINE BUTTONS -->
    <h5>Outline Buttons</h5>
    <div class="mb-4">
        <button type="button" class="btn btn-outline-primary">Primary</button>
        <button type="button" class="btn btn-outline-secondary">Secondary</button>
        <button type="button" class="btn btn-outline-success">Success</button>
        <button type="button" class="btn btn-outline-danger">Danger</button>
        <button type="button" class="btn btn-outline-warning">Warning</button>
        <button type="button" class="btn btn-outline-info">Info</button>
        <button type="button" class="btn btn-outline-dark">Dark</button>
    </div>

    <hr>

    <!-- BUTTON SIZES -->
    <h5>Button Sizes</h5>
    <div class="mb-4">
        <button type="button" class="btn btn-primary btn-sm">Small Button</button>
        <button type="button" class="btn btn-primary">Default Button</button>
        <button type="button" class="btn btn-primary btn-lg">Large Button</button>
    </div>

    <hr>

    <!-- BUTTON WITH <a> AND <input> TAGS -->
    <h5>Buttons with Different Tags</h5>
    <div class="mb-4">
        <a class="btn btn-primary" href="#" role="button">Link Button</a>
        <button class="btn btn-success" type="submit">Submit Button</button>
        <input class="btn btn-info" type="button" value="Input Button">
        <input class="btn btn-warning" type="reset" value="Reset Button">
    </div>

    <hr>

    <!-- DISABLED BUTTONS -->
    <h5>Disabled Buttons</h5>
    <div class="mb-4">
        <button type="button" class="btn btn-primary" disabled>Disabled Primary</button>
        <button type="button" class="btn btn-secondary" disabled>Disabled Secondary</button>
        <a class="btn btn-danger disabled" role="button" aria-disabled="true">Disabled Link</a>
    </div>

    <hr>

    <!-- ACTIVE STATE -->
    <h5>Active (Pressed) State</h5>
    <div class="mb-4">
        <button type="button" class="btn btn-primary">Normal</button>
        <button type="button" class="btn btn-primary active">Active</button>
    </div>

    <hr>

    <!-- BUTTON GROUPS WITH SIZES -->
    <h5>Full Width Block Button</h5>
    <div class="d-grid gap-2 mb-4">
        <button class="btn btn-primary" type="button">Block Button Full Width</button>
        <button class="btn btn-outline-secondary" type="button">Another Block Button</button>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.btn`**: Base class that applies padding, font-size, border-radius, and transitions
- **`.btn-primary`**: Blue button - main action button
- **`.btn-secondary`**: Gray button - secondary actions
- **`.btn-success`**: Green button - positive/successful actions
- **`.btn-danger`**: Red button - destructive/dangerous actions
- **`.btn-warning`**: Yellow/orange button - warnings
- **`.btn-info`**: Light blue button - informational actions
- **`.btn-outline-*`**: Buttons with colored border and text, transparent background
- **`.btn-sm`**: Smaller padding and font-size
- **`.btn-lg`**: Larger padding and font-size
- **`.disabled`**: Disables button, prevents clicking (adds opacity and removes pointer events)
- **`.active`**: Shows button in pressed state
- **`.d-grid`**: Display grid - makes buttons block-level
- **`.gap-2`**: Gap between grid items

## Expected Visual Result

The page displays multiple button examples:

1. **Solid Color Buttons**: Row of 9 buttons in different colors - Primary (blue), Secondary (gray), Success (green), Danger (red), Warning (yellow), Info (cyan), Light (white), Dark (black), and Link (looks like text link)

2. **Outline Buttons**: Same colors but with only border colored, transparent background - modern flat look

3. **Button Sizes**: Three buttons showing small, default, and large sizes side by side

4. **Different Tags**: Same button styling works on `<a>`, `<button>`, and `<input>` elements

5. **Disabled Buttons**: Grayed out buttons that cannot be clicked

6. **Active Button**: One button appears pressed/activated compared to normal

7. **Block Button**: Full-width button that spans the entire container width
