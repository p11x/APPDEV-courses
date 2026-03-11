# Bootstrap Progress Bars

## Definition

Bootstrap progress bars are visual indicators showing the completion percentage of a task or process. They display how much of something is complete using a horizontal bar. Progress bars can show different states through colors and can include labels showing the percentage. They support striped patterns and animations for visual enhancement.

## Key Bullet Points

- **`.progress`**: Container for progress bar
- **`.progress-bar`**: The actual fill bar indicating progress
- **`.progress-bar-striped`**: Adds striped pattern to bar
- **`.progress-bar-animated`**: Animates the stripes (requires animation)
- **Width**: Set width using inline style (e.g., style="width: 75%")
- **Colors**: Use bg-primary, bg-success, etc. for colored bars
- **Labels**: Add text inside bar to show percentage

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Progress Bars</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Progress Bar Examples</h2>

    <!-- BASIC PROGRESS BAR -->
    <h5>Basic Progress Bar</h5>
    <div class="progress mb-4" style="height: 20px;">
        <div class="progress-bar" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
    </div>
    <div class="progress mb-4" style="height: 20px;">
        <div class="progress-bar" role="progressbar" style="width: 25%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">25%</div>
    </div>
    <div class="progress mb-4" style="height: 20px;">
        <div class="progress-bar" role="progressbar" style="width: 50%" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100">50%</div>
    </div>
    <div class="progress mb-4" style="height: 20px;">
        <div class="progress-bar" role="progressbar" style="width: 75%" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100">75%</div>
    </div>
    <div class="progress mb-4" style="height: 20px;">
        <div class="progress-bar" role="progressbar" style="width: 100%" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100">100%</div>
    </div>

    <hr>

    <!-- PROGRESS BAR COLORS -->
    <h5>Progress Bar Colors</h5>
    <div class="progress mb-2">
        <div class="progress-bar bg-primary" role="progressbar" style="width: 25%">Primary</div>
    </div>
    <div class="progress mb-2">
        <div class="progress-bar bg-secondary" role="progressbar" style="width: 50%">Secondary</div>
    </div>
    <div class="progress mb-2">
        <div class="progress-bar bg-success" role="progressbar" style="width: 75%">Success</div>
    </div>
    <div class="progress mb-2">
        <div class="progress-bar bg-danger" role="progressbar" style="width: 100%">Danger</div>
    </div>
    <div class="progress mb-2">
        <div class="progress-bar bg-warning text-dark" role="progressbar" style="width: 60%">Warning</div>
    </div>
    <div class="progress mb-4">
        <div class="progress-bar bg-info text-dark" role="progressbar" style="width: 45%">Info</div>
    </div>

    <hr>

    <!-- STRIPED PROGRESS BARS -->
    <h5>Striped Progress Bars</h5>
    <div class="progress mb-2">
        <div class="progress-bar progress-bar-striped" role="progressbar" style="width: 25%">25%</div>
    </div>
    <div class="progress mb-2">
        <div class="progress-bar progress-bar-striped bg-success" role="progressbar" style="width: 50%">50%</div>
    </div>
    <div class="progress mb-4">
        <div class="progress-bar progress-bar-striped bg-danger" role="progressbar" style="width: 75%">75%</div>
    </div>

    <!-- ANIMATED STRIPES -->
    <h5>Animated Stripes</h5>
    <div class="progress mb-4">
        <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 75%">Animated</div>
    </div>

    <hr>

    <!-- MULTIPLE PROGRESS BARS -->
    <h5>Multiple Progress Bars in One</h5>
    <div class="progress mb-4">
        <div class="progress-bar" role="progressbar" style="width: 15%" aria-valuenow="15" aria-valuemin="0" aria-valuemax="100">15%</div>
        <div class="progress-bar bg-success" role="progressbar" style="width: 30%" aria-valuenow="30" aria-valuemin="0" aria-valuemax="100">30%</div>
        <div class="progress-bar bg-info" role="progressbar" style="width: 20%" aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">20%</div>
    </div>

    <!-- CUSTOM HEIGHT -->
    <h5>Custom Heights</h5>
    <div class="progress mb-2" style="height: 5px;">
        <div class="progress-bar" role="progressbar" style="width: 50%"></div>
    </div>
    <div class="progress mb-2" style="height: 20px;">
        <div class="progress-bar" role="progressbar" style="width: 50%"></div>
    </div>
    <div class="progress mb-4" style="height: 35px;">
        <div class="progress-bar" role="progressbar" style="width: 50%">Tall Bar</div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.progress`**: Outer container - provides background and sets height
- **`.progress-bar`**: Inner bar that fills based on percentage
- **`.bg-primary`**: Primary blue background color
- **`.bg-success`**: Green background color
- **`.bg-danger`**: Red background color
- **`.bg-warning`**: Yellow background color
- **`.bg-info`**: Light blue background color
- **`.progress-bar-striped`**: Adds diagonal stripes to bar
- **`.progress-bar-animated`**: Animates stripes moving left to right
- **`style="width: 50%"`**: Sets the fill percentage of the bar
- **`aria-*`**: Accessibility attributes for screen readers
- **Multiple bars**: Multiple `.progress-bar` elements in one `.progress` container create stacked bars

## Expected Visual Result

The page displays multiple progress bar examples:

1. **Basic Progress Bars**: Horizontal bars showing 0%, 25%, 50%, 75%, 100% completion with percentage labels

2. **Colored Progress Bars**: Same bars in different colors (primary blue, secondary gray, success green, danger red, warning yellow, info cyan)

3. **Striped Bars**: Bars with diagonal stripe pattern

4. **Animated Stripes**: Bar with moving stripes animation (animated left-to-right)

5. **Multiple Stacked Bars**: Single progress bar with three colored segments that add up to show multiple data points

6. **Custom Heights**: Very thin (5px), normal (20px), and tall (35px) progress bars

All bars show percentage text when there's room, and colors indicate different states or categories.
