# Bootstrap Utilities

## Definition

Bootstrap utility classes are single-purpose CSS classes that perform specific styling tasks quickly without writing custom CSS. They handle spacing (margin/padding), display properties, text formatting, borders, colors, and positioning. Utilities are the building blocks that make Bootstrap flexible and customizable.

## Key Bullet Points

- **Margin/Padding**: m-*, p-* with t, b, l, r, x, y, auto
- **Display**: d-none, d-block, d-flex, d-inline, d-grid
- **Flexbox**: justify-content-*, align-items-*, flex-*, flex-column
- **Text**: text-center, text-uppercase, fw-bold, fst-italic
- **Border**: border, border-*, rounded, rounded-*
- **Colors**: bg-*, text-*, border-*
- **Sizing**: w-*, h-*,mw-*, mh-*
- **Spacing Scale**: 0-5 (0rem), auto

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Utilities</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Utility Classes</h2>

    <!-- SPACING - MARGIN -->
    <h5>Margin Utilities (m-*, me-*, ms-*, mt-*, mb-*, mx-*, my-*)</h5>
    <div class="d-flex gap-2 flex-wrap mb-4">
        <div class="bg-primary text-white p-2">m-0</div>
        <div class="bg-primary text-white p-2">m-1</div>
        <div class="bg-primary text-white p-2">m-2</div>
        <div class="bg-primary text-white p-2">m-3</div>
        <div class="bg-primary text-white p-2">m-4</div>
        <div class="bg-primary text-white p-2">m-5</div>
    </div>
    <div class="bg-light p-2 mb-4" style="max-width: 300px;">
        <div class="bg-success text-white p-2">mt-4 (margin-top)</div>
    </div>
    <div class="bg-light p-2 mb-4" style="max-width: 300px; margin-left: auto;">
        <div class="bg-success text-white p-2" style="margin-left: auto;">ms-auto (margin-start)</div>
    </div>

    <!-- SPACING - PADDING -->
    <h5>Padding Utilities (p-*, pe-*, ps-*, pt-*, pb-*, px-*, py-*)</h5>
    <div class="d-flex gap-2 flex-wrap mb-4">
        <div class="bg-warning p-0">p-0</div>
        <div class="bg-warning p-1">p-1</div>
        <div class="bg-warning p-2">p-2</div>
        <div class="bg-warning p-3">p-3</div>
        <div class="bg-warning p-4">p-4</div>
        <div class="bg-warning p-5">p-5</div>
    </div>

    <!-- DISPLAY -->
    <h5>Display Utilities (d-*)</h5>
    <div class="d-inline bg-primary text-white p-2 me-2">d-inline</div>
    <div class="d-inline bg-success text-white p-2 me-2">d-inline</div>
    <div class="d-block bg-info text-white p-2 mb-2">d-block</div>
    <div class="d-block bg-warning text-white p-2 mb-4">d-block</div>
    <div class="d-none bg-danger text-white p-2 mb-4">d-none (hidden)</div>

    <!-- FLEXBOX -->
    <h5>Flexbox Utilities</h5>
    <div class="border p-2 mb-2">
        <strong>justify-content-start:</strong>
        <div class="d-flex justify-content-start bg-light p-2">
            <div class="bg-primary text-white p-2 m-1">1</div>
            <div class="bg-primary text-white p-2 m-1">2</div>
            <div class="bg-primary text-white p-2 m-1">3</div>
        </div>
    </div>
    <div class="border p-2 mb-2">
        <strong>justify-content-center:</strong>
        <div class="d-flex justify-content-center bg-light p-2">
            <div class="bg-primary text-white p-2 m-1">1</div>
            <div class="bg-primary text-white p-2 m-1">2</div>
            <div class="bg-primary text-white p-2 m-1">3</div>
        </div>
    </div>
    <div class="border p-2 mb-4">
        <strong>justify-content-between:</strong>
        <div class="d-flex justify-content-between bg-light p-2">
            <div class="bg-primary text-white p-2 m-1">1</div>
            <div class="bg-primary text-white p-2 m-1">2</div>
            <div class="bg-primary text-white p-2 m-1">3</div>
        </div>
    </div>

    <!-- TEXT UTILITIES -->
    <h5>Text Utilities</h5>
    <div class="mb-2"><span class="text-start">text-start</span></div>
    <div class="mb-2"><span class="text-center">text-center</span></div>
    <div class="mb-2"><span class="text-end">text-end</span></div>
    <div class="mb-2"><span class="text-uppercase">text-uppercase</span></div>
    <div class="mb-2"><span class="text-lowercase">TEXT-LOWERCASE</span></div>
    <div class="mb-2"><span class="text-capitalize">text capitalize each word</span></div>
    <div class="mb-2"><span class="fw-bold">fw-bold</span></div>
    <div class="mb-2"><span class="fst-italic">fst-italic</span></div>
    <div class="mb-4"><span class="text-primary">text-primary</span> | <span class="text-success">text-success</span> | <span class="text-danger">text-danger</span></div>

    <!-- BORDER UTILITIES -->
    <h5>Border Utilities</h5>
    <div class="d-flex gap-3 mb-2">
        <div class="p-3 border">border</div>
        <div class="p-3 border-top">border-top</div>
        <div class="p-3 border-end">border-end</div>
        <div class="p-3 border-bottom">border-bottom</div>
        <div class="p-3 border-start">border-start</div>
    </div>
    <div class="d-flex gap-3 mb-2">
        <div class="p-3 border border-primary">border-primary</div>
        <div class="p-3 border border-success">border-success</div>
        <div class="p-3 border border-danger">border-danger</div>
    </div>
    <div class="d-flex gap-3 mb-4">
        <div class="p-3 border rounded">rounded</div>
        <div class="p-3 border rounded-top">rounded-top</div>
        <div class="p-3 border rounded-end">rounded-end</div>
        <div class="p-3 border rounded-bottom">rounded-bottom</div>
        <div class="p-3 border rounded-start">rounded-start</div>
        <div class="p-3 border rounded-circle" style="width: 60px;">circle</div>
        <div class="p-3 border rounded-0">rounded-0</div>
    </div>

    <!-- SIZING -->
    <h5>Sizing Utilities (w-*, h-*)</h5>
    <div class="bg-light p-2 mb-2">
        <div class="bg-primary text-white p-2 w-25">w-25</div>
    </div>
    <div class="bg-light p-2 mb-2">
        <div class="bg-primary text-white p-2 w-50">w-50</div>
    </div>
    <div class="bg-light p-2 mb-2">
        <div class="bg-primary text-white p-2 w-75">w-75</div>
    </div>
    <div class="bg-light p-2 mb-4">
        <div class="bg-primary text-white p-2 w-100">w-100</div>
    </div>

    <!-- SHADOW -->
    <h5>Shadow Utilities</h5>
    <div class="d-flex gap-4 p-4">
        <div class="p-3 bg-light shadow-none">shadow-none</div>
        <div class="p-3 bg-light shadow-sm">shadow-sm</div>
        <div class="p-3 bg-light shadow">shadow</div>
        <div class="p-3 bg-light shadow-lg">shadow-lg</div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.m-*`**: Margin utility (0-5, auto) - m-0 removes margin, m-5 is largest
- **`.mt-*`, `.mb-*`, `.ml-*`, `.mr-*`**: Specific margin directions
- **`.mx-*`, `.my-*`**: Horizontal/vertical margins
- **`.p-*`**: Padding utility (0-5)
- **`.d-flex`**: Display flexbox
- **`.d-inline`, `.d-block`, `.d-none`**: Display properties
- **`.justify-content-*`**: Horizontal alignment in flexbox
- **`.align-items-*`**: Vertical alignment in flexbox
- **`.text-center`, `.text-start`, `.text-end`**: Text alignment
- **`.text-uppercase`**: Transform text to uppercase
- **`.fw-bold`**: Font weight bold
- **`.border`, `.border-*`**: Border utilities
- **`.rounded`, `.rounded-*`**: Border radius utilities
- **`.w-*`**: Width percentage utilities
- **`.shadow-*`**: Box shadow utilities

## Expected Visual Result

The page displays multiple utility class examples:

1. **Margin**: Boxes showing different margin sizes (m-0 through m-5), directional margins

2. **Padding**: Boxes showing padding sizes (p-0 through p-5)

3. **Display**: Inline and block elements showing display behavior, hidden element

4. **Flexbox**: Rows showing different justify-content values (start, center, between)

5. **Text**: Various text transformations and alignments

6. **Borders**: Elements with different border styles, colors, and rounded corners

7. **Sizing**: Bars showing different width percentages

8. **Shadows**: Cards with different shadow levels

These utilities can be combined to create any layout without writing custom CSS.
