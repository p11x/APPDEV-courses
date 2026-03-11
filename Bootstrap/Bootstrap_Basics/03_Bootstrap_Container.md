# Bootstrap Container

## Definition

A container is the most basic Bootstrap layout element that wraps your website content and provides appropriate left and right padding. Bootstrap offers two types of containers: `.container` which has a maximum width and centers content, and `.container-fluid` which spans the full width of the viewport. Containers help create a proper layout structure for your Bootstrap website.

## Key Bullet Points

- **`.container`**: Fixed-width container that adjusts max-width at each responsive breakpoint
- **`.container-fluid`**: Full-width container spanning 100% of the viewport at all times
- **Responsive Breakpoints**: Containers have different max-widths at sm, md, lg, xl, and xxl breakpoints
- **Built-in Padding**: Containers include horizontal padding to prevent content from touching edges
- **Foundation Element**: Almost every Bootstrap page starts with a container

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Container Example</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        /* Add background colors to visualize containers */
        .demo-section {
            background-color: #e3f2fd; /* Light blue */
            border: 2px solid #0d6efd;
            padding: 20px;
            margin-bottom: 20px;
        }
    </style>
</head>
<body class="p-4">
    
    <h2 class="mb-4">Container vs Container-Fluid Comparison</h2>

    <!-- FIXED CONTAINER: .container -->
    <h5>Fixed Container (.container)</h5>
    <div class="container demo-section">
        <p>This container has a maximum width and is centered on the page.</p>
        <p>It adapts to different screen sizes:</p>
        <ul>
            <li>Small screens (xs): 100% width</li>
            <li>Medium screens (md): 720px max</li>
            <li>Large screens (lg): 960px max</li>
            <li>Extra large (xl): 1140px max</li>
            <li>XXL screens: 1320px max</li>
        </ul>
    </div>

    <!-- FULL-WIDTH CONTAINER: .container-fluid -->
    <h5>Full-Width Container (.container-fluid)</h5>
    <div class="container-fluid demo-section">
        <p>This container spans the full width of the viewport (100%).</p>
        <p>No matter the screen size, it always stretches edge-to-edge.</p>
        <p>Perfect for: Hero sections, full-width headers, footers</p>
    </div>

    <!-- SIDE BY SIDE COMPARISON -->
    <h5 class="mt-4">Side by Side in Same Row</h5>
    <div class="row">
        <div class="col-md-6">
            <div class="container demo-section">
                <strong>.container</strong><br>
                Centered with max-width<br>
                Has side margins on large screens
            </div>
        </div>
        <div class="col-md-6">
            <div class="container-fluid demo-section">
                <strong>.container-fluid</strong><br>
                Full viewport width<br>
                Edge-to-edge on all sizes
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.container`**: Creates a responsive fixed-width container that centers content and has maximum width limits
- **`.container-fluid`**: Creates a full-width container that spans 100% of the viewport width at all times
- **`.row`**: Creates a horizontal group for columns, with negative margins to offset container padding
- **`.col-md-6`**: Creates two equal columns on medium and larger screens
- **`.p-4`**: Adds padding (4 = large) all around the body element
- **`.mb-4`**: Adds margin-bottom spacing
- **`demo-section`**: Custom CSS class to add visible borders and backgrounds for learning

## Expected Visual Result

The page will display three sections:

1. **Fixed Container Section**: A blue-bordered box that appears centered with visible max-width. On large screens, it has clear left and right margins from the viewport edge.

2. **Full-Width Container Section**: A blue-bordered box that stretches completely from left to right edge of the browser window, with no side margins.

3. **Side by Side Comparison**: Two boxes side-by-side on medium+ screens showing:
   - Left box (.container): Centered, has margins on sides
   - Right box (.container-fluid): Full width edge-to-edge

On mobile phones, all containers will display at full width (100%).
