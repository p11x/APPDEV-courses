# Bootstrap Tooltips

## Definition

Bootstrap tooltips are small pop-up boxes that appear when users hover over an element, providing additional information. They are useful for explaining abbreviations, providing hints, or showing extra details. Tooltips must be initialized with JavaScript and use the Popper library for positioning.

## Key Bullet Points

- **`.tooltip`**: Base class (applied automatically)
- **Data Attributes**: data-bs-toggle="tooltip" and title attribute
- **JavaScript Required**: Must initialize with JavaScript
- **Placement**: Use data-bs-placement for position (top, bottom, left, right)
- **Custom HTML**: Use data-bs-html="true" for HTML content in tooltip
- **Trigger Options**: Can trigger on hover, click, or focus
- **Positioning**: Tooltips use Popper for smart positioning

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Tooltips</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Tooltip Examples</h2>

    <h5>Basic Tooltips on Buttons</h5>
    <div class="d-flex gap-2 mb-4 flex-wrap">
        <button type="button" class="btn btn-primary" data-bs-toggle="tooltip" title="This is a tooltip!">
            Hover me (Top)
        </button>
        <button type="button" class="btn btn-success" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Tooltip on bottom!">
            Bottom
        </button>
        <button type="button" class="btn btn-danger" data-bs-toggle="tooltip" data-bs-placement="left" title="Tooltip on left!">
            Left
        </button>
        <button type="button" class="btn btn-warning" data-bs-toggle="tooltip" data-bs-placement="right" title="Tooltip on right!">
            Right
        </button>
    </div>

    <hr>

    <h5>Tooltips on Links</h5>
    <p>
        Check out this <a href="#" data-bs-toggle="tooltip" data-bs-placement="top" title="Go to documentation">link</a> 
        for more information about 
        <a href="#" data-bs-toggle="tooltip" data-bs-placement="bottom" title="Learn more about features">features</a>.
    </p>

    <hr>

    <h5>Tooltips with Custom HTML</h5>
    <button type="button" class="btn btn-info mb-4" data-bs-toggle="tooltip" data-bs-html="true" title="<em>Tooltip</em> <u>with</u> <b>HTML</b>">
        Tooltip with HTML
    </button>

    <hr>

    <h5>Disabled Tooltip on Disabled Button</h5>
    <button type="button" class="btn btn-secondary" disabled data-bs-toggle="tooltip" title="Disabled button tooltip">
        Disabled Button
    </button>

    <hr>

    <h5>Tooltips on Icons</h5>
    <div class="d-flex gap-3 mb-4">
        <span data-bs-toggle="tooltip" title="Search">
            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-search text-primary" viewBox="0 0 16 16">
                <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
            </svg>
        </span>
        <span data-bs-toggle="tooltip" title="Settings">
            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-gear text-secondary" viewBox="0 0 16 16">
                <path d="M8 4.754a3.246 3.246 0 1 0 0 6.492 3.246 3.246 0 0 0 0-6.492zM5.754 8a2.246 2.246 0 1 1 4.492 0 2.246 2.246 0 0 1-4.492 0z"/>
                <path d="M9.796 1.343c-.527-1.79-3.065-1.79-3.592 0l-.094.319a.873.873 0 0 1-1.255.52l-.292-.16c-1.64-.892-3.433.902-2.54 2.541l.159.292a.873.873 0 0 1-.52 1.255l-.319.094c-1.79.527-1.79 3.065 0 3.592l.319.094a.873.873 0 0 1 .52 1.255l-.16.292c-.892 1.64.901 3.434 2.541 2.54l.292-.159a.873.873 0 0 1 1.255.52l.094.319c.527 1.79 3.065 1.79 3.592 0l.094-.319a.873.873 0 0 1 1.255-.52l.292.16c1.64.893 3.434-.902 2.54-2.541l-.159-.292a.873.873 0 0 1 .52-1.255l.319-.094c1.79-.527 1.79-3.065 0-3.592l-.319-.094a.873.873 0 0 1-.52-1.255l.16-.292c.893-1.64-.902-3.433-2.541-2.54l-.292.159a.873.873 0 0 1-1.255-.52l-.094-.319zm-2.633.283c.246-.835 1.428-.835 1.674 0l.094.319a1.873 1.873 0 0 0 2.693 1.115l.291-.16c.764-.415 1.6.42 1.184 1.185l-.159.292a1.873 1.873 0 0 0 1.116 2.692l.318.094c.835.246.835 1.428 0 1.674l-.319.094a1.873 1.873 0 0 0-1.115 2.693l.16.291c.415.764-.42 1.6-1.185 1.184l-.291-.159a1.873 1.873 0 0 0-2.693 1.116l-.094.318c-.246.835-1.428.835-1.674 0l-.094-.319a1.873 1.873 0 0 0-2.692-1.115l-.292.16c-.764.415-1.6-.42-1.184-1.185l.159-.291A1.873 1.873 0 0 0 1.945 8.93l-.319-.094c-.835-.246-.835-1.428 0-1.674l.319-.094A1.873 1.873 0 0 0 3.06 4.377l-.16-.292c-.415-.764.42-1.6 1.185-1.184l.292.159a1.873 1.873 0 0 0 2.692-1.115l.094-.319z"/>
            </svg>
        </span>
        <span data-bs-toggle="tooltip" title="Notifications">
            <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="currentColor" class="bi bi-bell text-danger" viewBox="0 0 16 16">
                <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2zM8 1.918l-.797.161A4.002 4.002 0 0 0 4 6c0 .628-.134 2.197-.459 3.742-.16.767-.376 1.566-.663 2.258h10.244c-.287-.692-.502-1.49-.663-2.258C12.134 8.197 12 6.628 12 6a4.002 4.002 0 0 0-3.203-3.92L8 1.917z"/>
            </svg>
        </span>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // Initialize all tooltips on the page
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
        var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl)
        })
    </script>
</body>
</html>
```

## Component Explanation

- **`.tooltip`**: Base class applied by JavaScript for styling
- **`data-bs-toggle="tooltip"`**: Bootstrap attribute that identifies element as tooltip trigger
- **`title="..."`**: The actual tooltip text content
- **`data-bs-placement="top"`**: Position of tooltip (top, bottom, left, right)
- **`data-bs-html="true"`**: Allows HTML content in tooltip (use carefully for security)
- **JavaScript initialization**: Creates tooltip instances on elements
- **`new bootstrap.Tooltip()`**: Creates new tooltip instance
- **Hover behavior**: Default triggers on hover

## Expected Visual Result

The page displays multiple tooltip examples:

1. **Buttons with Tooltips**: Four buttons showing tooltips on different sides (top, bottom, left, right)

2. **Links with Tooltips**: Links with tooltips appearing when hovering

3. **HTML Tooltip**: Button showing tooltip with formatted HTML content (bold, underline, italic)

4. **Disabled Button**: Disabled button that still shows tooltip

5. **Icons with Tooltips**: SVG icons (search, gear, bell) that show tooltips when hovering

When hovering over any of these elements, a small white box with dark text appears near the element, positioned according to the placement setting.
