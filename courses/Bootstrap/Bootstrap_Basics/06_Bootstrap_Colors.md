# Bootstrap Colors

## Definition

Bootstrap provides a comprehensive color system with predefined contextual colors that convey meaning (like success for positive actions or danger for warnings). These colors can be applied to text, backgrounds, borders, and many other elements. The color palette includes primary (blue), secondary (gray), success (green), danger (red), warning (yellow), info (cyan), light (white), and dark (black).

## Key Bullet Points

- **Primary Colors**: Primary (blue), secondary (gray), success (green), danger (red), warning (yellow), info (cyan), light, dark
- **Text Colors**: Use text-{color} class to color text
- **Background Colors**: Use bg-{color} class for background colors
- **Contextual Meaning**: Each color has a specific semantic meaning (success = good, danger = bad/warning)
- **Combination**: Text and background colors can be combined for different effects

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Colors</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Color Palette</h2>

    <!-- TEXT COLORS -->
    <h5>Text Colors (text-{color})</h5>
    <div class="container mb-4">
        <p class="text-primary">.text-primary - Primary color (usually blue)</p>
        <p class="text-secondary">.text-secondary - Secondary color (usually gray)</p>
        <p class="text-success">.text-success - Success color (usually green)</p>
        <p class="text-danger">.text-danger - Danger color (usually red)</p>
        <p class="text-warning">.text-warning - Warning color (usually yellow)</p>
        <p class="text-info">.text-info - Info color (usually cyan)</p>
        <p class="text-light bg-dark">.text-light - Light color (usually white)</p>
        <p class="text-dark">.text-dark - Dark color (usually black)</p>
    </div>

    <hr>

    <!-- BACKGROUND COLORS -->
    <h5>Background Colors (bg-{color})</h5>
    <div class="container mb-4">
        <div class="p-2 mb-2 bg-primary text-white">.bg-primary</div>
        <div class="p-2 mb-2 bg-secondary text-white">.bg-secondary</div>
        <div class="p-2 mb-2 bg-success text-white">.bg-success</div>
        <div class="p-2 mb-2 bg-danger text-white">.bg-danger</div>
        <div class="p-2 mb-2 bg-warning text-dark">.bg-warning</div>
        <div class="p-2 mb-2 bg-info text-dark">.bg-info</div>
        <div class="p-2 mb-2 bg-light text-dark">.bg-light</div>
        <div class="p-2 mb-2 bg-dark text-white">.bg-dark</div>
        <div class="p-2 mb-2 bg-white text-dark border">.bg-white</div>
    </div>

    <hr>

    <!-- COMBINED EXAMPLES -->
    <h5>Combined Text & Background</h5>
    <div class="container">
        <div class="row">
            <div class="col-md-4 mb-2">
                <div class="p-3 bg-primary text-white rounded">
                    <h5>Primary Card</h5>
                    <p>Primary background with white text</p>
                </div>
            </div>
            <div class="col-md-4 mb-2">
                <div class="p-3 bg-success text-white rounded">
                    <h5>Success Card</h5>
                    <p>Success background with white text</p>
                </div>
            </div>
            <div class="col-md-4 mb-2">
                <div class="p-3 bg-danger text-white rounded">
                    <h5>Danger Card</h5>
                    <p>Danger background with white text</p>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.text-primary`**: Colors text with the primary brand color (typically blue)
- **`.text-secondary`**: Colors text with secondary color (gray)
- **`.text-success`**: Colors text green, indicating success or positive action
- **`.text-danger`**: Colors text red, indicating error or danger
- **`.text-warning`**: Colors text yellow/orange, indicating warnings
- **`.text-info`**: Colors text cyan, for informational text
- **`.text-light`**: Colors text light (white) - use on dark backgrounds
- **`.text-dark`**: Colors text dark (near black)
- **`.bg-*`**: Same color palette applied to background instead of text
- **`.text-white`**: Ensures text is white (for use on dark backgrounds)
- **`.rounded`**: Adds rounded corners to the element

## Expected Visual Result

The page displays three sections:

1. **Text Colors**: A list showing all 8 contextual text colors - primary blue, secondary gray, success green, danger red, warning yellow, info cyan, light white, and dark black

2. **Background Colors**: Colorful boxes showing each background color with appropriate text color (white text on dark backgrounds, dark text on light backgrounds)

3. **Combined Examples**: Three colored cards side-by-side:
   - Blue card with white text (Primary)
   - Green card with white text (Success)
   - Red card with white text (Danger)

All elements use Bootstrap's carefully designed color palette for consistent visual design across the website.
