# Bootstrap Spinners

## Definition

Bootstrap spinners (also called loading indicators or loaders) are animated graphics that show users content is loading. They provide visual feedback during async operations. Bootstrap offers two spinner styles: border spinners (rotating circles) and growing spinners (fading dots). Spinners can be customized with sizes and colors.

## Key Bullet Points

- **`.spinner-border`**: Rotating border spinner
- **`.spinner-grow`**: Growing/fading dot spinner
- **Sizes**: spinner-border-sm, spinner-grow-sm (small), or use inline styles for custom size
- **Colors**: Use text-color classes (text-primary, text-success, etc.)
- **Button Spinners**: Include spinner inside buttons to show loading state
- **Accessibility**: Use sr-only class or aria-label for screen readers

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Spinners</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Spinner Examples</h2>

    <!-- BORDER SPINNERS -->
    <h5>Border Spinners</h5>
    <div class="spinner-border mb-4" role="status">
        <span class="visually-hidden">Loading...</span>
    </div>

    <!-- GROW SPINNERS -->
    <h5>Grow Spinners</h5>
    <div class="spinner-grow mb-4" role="status">
        <span class="visually-hidden">Loading...</span>
    </div>

    <hr>

    <!-- COLORS -->
    <h5>Colored Spinners</h5>
    <div class="d-flex align-items-center gap-3 mb-4">
        <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-border text-secondary" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-border text-success" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-border text-danger" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-border text-warning" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-border text-info" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>

    <!-- GROW COLORS -->
    <h5>Grow Spinners - Colors</h5>
    <div class="d-flex align-items-center gap-3 mb-4">
        <div class="spinner-grow text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-grow text-success" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-grow text-warning" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-grow text-danger" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>

    <hr>

    <!-- SIZES -->
    <h5>Spinner Sizes</h5>
    <div class="d-flex align-items-center gap-3 mb-4">
        <div class="spinner-border spinner-border-sm" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-border" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-border" style="width: 3rem; height: 3rem;" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>

    <!-- GROW SIZES -->
    <h5>Grow Spinner Sizes</h5>
    <div class="d-flex align-items-center gap-3 mb-4">
        <div class="spinner-grow spinner-grow-sm" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-grow" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        <div class="spinner-grow" style="width: 3rem; height: 3rem;" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>

    <hr>

    <!-- SPINNERS IN BUTTONS -->
    <h5>Spinners in Buttons</h5>
    <div class="d-flex flex-wrap gap-2 mb-4">
        <button class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
            <span class="visually-hidden">Loading...</span>
        </button>
        <button class="btn btn-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
            Loading...
        </button>
        <button class="btn btn-secondary" type="button" disabled>
            <span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>
            Loading...
        </button>
        <button class="btn btn-outline-primary" type="button" disabled>
            <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
        </button>
    </div>

    <!-- BUTTON WITH SPINNER TOGGLE -->
    <h5>Loading Button State</h5>
    <button class="btn btn-primary mb-4" type="button" id="loadingBtn">
        <span class="spinner-border spinner-border-sm" role="status" hidden></span>
        <span>Click to Load</span>
    </button>

    <!-- CENTERED SPINNER -->
    <h5>Centered Spinner</h5>
    <div class="d-flex justify-content-center mb-4">
        <div class="spinner-border" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        document.getElementById('loadingBtn').addEventListener('click', function() {
            const btn = this;
            const spinner = btn.querySelector('.spinner-border');
            const text = btn.querySelector('span:last-child');
            
            btn.disabled = true;
            spinner.hidden = false;
            text.textContent = 'Loading...';
            
            setTimeout(function() {
                btn.disabled = false;
                spinner.hidden = true;
                text.textContent = 'Click to Load';
            }, 2000);
        });
    </script>
</body>
</html>
```

## Component Explanation

- **`.spinner-border`**: Creates rotating circle animation (border spinner)
- **`.spinner-grow`**: Creates growing/fading dot animation (grow spinner)
- **`.spinner-border-sm`**: Small size version
- **`.spinner-grow-sm`**: Small grow spinner
- **`.text-primary`**: Colors spinner using contextual colors
- **`.text-secondary`**: Secondary gray color
- **`.text-success`**: Success green color
- **`.text-danger`**: Danger red color
- **`.text-warning`**: Warning yellow color
- **`.text-info`**: Info cyan color
- **`.visually-hidden`**: Hides element visually but keeps for screen readers
- **`.d-flex`**: Flexbox display
- **`.justify-content-center`**: Centers content horizontally
- **`role="status"`**: ARIA role for status updates
- **`aria-hidden="true"`**: Hides decorative spinner from screen readers

## Expected Visual Result

The page displays multiple spinner examples:

1. **Border Spinner**: Single rotating circle animation
2. **Grow Spinner**: Single growing/fading dot animation

3. **Colored Spinners**: Row of border spinners in different colors (blue, gray, green, red, yellow, cyan)

4. **Grow Colors**: Row of grow spinners in various colors

5. **Sizes**: Small, default, and large border spinners side by side

6. **Grow Sizes**: Small, default, and large grow spinners

7. **Spinners in Buttons**: Various button styles showing loading state - with text, without text, outline buttons

8. **Centered Spinner**: A spinner centered on the page using flexbox

9. **Interactive Button**: A button that changes to show spinner when clicked (simulates loading)
