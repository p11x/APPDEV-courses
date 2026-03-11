# Bootstrap Grid System

## Definition

The Bootstrap grid system is a powerful layout mechanism that divides the page into 12 equal columns, allowing you to create flexible multi-column layouts. You can combine columns in various ways (like 6+6, 4+4+4, 3+3+3+3) to create responsive layouts that adapt to different screen sizes. The grid uses flexbox under the hood and automatically stacks columns vertically on mobile devices.

## Key Bullet Points

- **12-Column System**: Total of 12 available columns to distribute content across
- **`.row`**: Creates a horizontal container that holds columns
- **`.col`**: Automatically divides space equally among columns in a row
- **Responsive Prefixes**: col-xs (always), col-sm (576px+), col-md (768px+), col-lg (992px+), col-xl (1200px+), col-xxl (1400px+)
- **Column Stacking**: Columns stack vertically on smaller screens automatically
- **Gutter**: Built-in horizontal padding between columns for spacing

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Grid System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .demo-col {
            background-color: #e7f1ff;
            border: 1px solid #0d6efd;
            padding: 15px;
            text-align: center;
        }
    </style>
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap 12-Column Grid System</h2>

    <!-- BASIC: Auto-dividing columns -->
    <h5>Auto-Dividing Columns (.col)</h5>
    <div class="row mb-4">
        <div class="col demo-col">Column 1</div>
        <div class="col demo-col">Column 2</div>
        <div class="col demo-col">Column 3</div>
    </div>

    <!-- SPECIFIC WIDTH: 6+6 = 12 columns -->
    <h5>Two Equal Columns (6 + 6)</h5>
    <div class="row mb-4">
        <div class="col-6 demo-col">col-6</div>
        <div class="col-6 demo-col">col-6</div>
    </div>

    <!-- THREE EQUAL COLUMNS: 4 + 4 + 4 = 12 -->
    <h5>Three Equal Columns (4 + 4 + 4)</h5>
    <div class="row mb-4">
        <div class="col-4 demo-col">col-4</div>
        <div class="col-4 demo-col">col-4</div>
        <div class="col-4 demo-col">col-4</div>
    </div>

    <!-- IRREGULAR: 3 + 9 = 12 -->
    <h5>Irregular Columns (3 + 9)</h5>
    <div class="row mb-4">
        <div class="col-3 demo-col">col-3</div>
        <div class="col-9 demo-col">col-9</div>
    </div>

    <!-- RESPONSIVE: col-md adjusts on medium+ screens -->
    <h5>Responsive Columns (col-md)</h5>
    <p>On mobile: stacked vertically. On md+ screens: side by side.</p>
    <div class="row mb-4">
        <div class="col-md-4 demo-col">col-md-4</div>
        <div class="col-md-4 demo-col">col-md-4</div>
        <div class="col-md-4 demo-col">col-md-4</div>
    </div>

    <!-- DIFFERENT BREAKPOINTS -->
    <h5>Multiple Responsive Breakpoints</h5>
    <div class="row mb-4">
        <!-- col-12 on mobile, col-md-6 on tablet, col-lg-4 on desktop -->
        <div class="col-12 col-md-6 col-lg-4 demo-col">col-12 col-md-6 col-lg-4</div>
        <div class="col-12 col-md-6 col-lg-4 demo-col">col-12 col-md-6 col-lg-4</div>
        <div class="col-12 col-md-12 col-lg-4 demo-col">col-12 col-md-12 col-lg-4</div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.row`**: Creates a flex container that holds columns; includes negative margins to offset container padding
- **`.col`**: Without a number, automatically calculates column width equally for all columns
- **`.col-6`**: Sets a column to take 6 of 12 columns (50% width)
- **`.col-4`**: Sets a column to take 4 of 12 columns (33% width)
- **`.col-md-4`**: Responsive class - takes 4 columns on medium screens and larger, stacks on smaller
- **`.col-12`**: Full width column (12 of 12)
- **Breakpoints**: xs (<576px), sm (≥576px), md (≥768px), lg (≥992px), xl (≥1200px), xxl (≥1400px)

## Expected Visual Result

The page will display multiple grid examples:

1. **Auto-dividing**: Three equal-width blue boxes side by side
2. **Two columns**: Two equal 50% width boxes
3. **Three equal**: Three equal 33% width boxes
4. **Irregular**: One small (25%) box and one large (75%) box
5. **Responsive behavior**: On desktop, three boxes side by side. On mobile, they stack vertically, each taking full width
6. **Multiple breakpoints**: Shows how columns change at different screen sizes

On mobile phones, all columns will stack vertically (each taking 100% width), demonstrating the mobile-first responsive design.
