# Bootstrap Responsive Design

## Definition

Bootstrap responsive design allows websites to adapt their layout based on the screen size of the device viewing them. Bootstrap uses a mobile-first approach with responsive breakpoints (xs, sm, md, lg, xl, xxl). By using responsive classes, the same HTML can display differently on phones, tablets, and desktop computers without writing separate code.

## Key Bullet Points

- **Breakpoints**: xs (<576px), sm (≥576px), md (≥768px), lg (≥992px), xl (≥1200px), xxl (≥1400px)
- **Responsive Grid**: col-*, col-sm-*, col-md-*, col-lg-*, col-xl-*, col-xxl-*
- **Responsive Display**: d-none, d-sm-block, d-md-flex, etc.
- **Responsive Text**: text-sm-center, text-md-start, etc.
- **Responsive Margin/Padding**: m-sm-*, p-md-*, etc.
- **Responsive Visibility**: Show/hide elements at specific breakpoints
- **Mobile First**: Default is mobile, add larger breakpoints for bigger screens

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Responsive Design</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .demo-box {
            background-color: #e7f1ff;
            border: 1px solid #0d6efd;
            padding: 20px;
            text-align: center;
        }
        .demo-box-2 {
            background-color: #d1e7dd;
            border: 1px solid #0f5132;
            padding: 20px;
            text-align: center;
        }
    </style>
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Responsive Design</h2>

    <!-- RESPONSIVE BREAKPOINTS EXPLAINED -->
    <h5>Responsive Grid with Breakpoints</h5>
    <p>Resize your browser to see how columns stack differently:</p>
    <div class="row mb-4">
        <div class="col-12 col-md-6 col-lg-4 demo-box">
            col-12 col-md-6 col-lg-4
            <br><small>Mobile: 12 | Tablet: 6 | Desktop: 4</small>
        </div>
        <div class="col-12 col-md-6 col-lg-4 demo-box-2">
            col-12 col-md-6 col-lg-4
            <br><small>Mobile: 12 | Tablet: 6 | Desktop: 4</small>
        </div>
        <div class="col-12 col-md-12 col-lg-4 demo-box">
            col-12 col-md-12 col-lg-4
            <br><small>Mobile: 12 | Tablet: 12 | Desktop: 4</small>
        </div>
    </div>

    <hr>

    <!-- RESPONSIVE VISIBILITY -->
    <h5>Responsive Visibility (d-*)</h5>
    <div class="d-block d-sm-none bg-warning p-2 mb-2">Only visible on XS (mobile)</div>
    <div class="d-none d-sm-block d-md-none bg-info p-2 mb-2">Only visible on SM (landscape phone)</div>
    <div class="d-none d-md-block d-lg-none bg-success text-white p-2 mb-2">Only visible on MD (tablet)</div>
    <div class="d-none d-lg-block d-xl-none bg-primary text-white p-2 mb-2">Only visible on LG (desktop)</div>
    <div class="d-none d-xl-block bg-dark text-white p-2 mb-4">Visible on XL and above</div>

    <hr>

    <!-- RESPONSIVE TEXT -->
    <h5>Responsive Text Alignment</h5>
    <div class="row mb-4">
        <div class="col-md-4 mb-2">
            <p class="text-sm-center text-md-start">text-sm-center text-md-start</p>
        </div>
        <div class="col-md-4 mb-2">
            <p class="text-md-center">text-md-center</p>
        </div>
        <div class="col-md-4 mb-2">
            <p class="text-lg-end">text-lg-end</p>
        </div>
    </div>

    <hr>

    <!-- RESPONSIVE FLEX -->
    <h5>Responsive Flex Direction</h5>
    <div class="border p-2 mb-2">
        <strong>Mobile (column):</strong> flex-column
        <div class="d-flex flex-column bg-light p-2">
            <div class="bg-primary text-white p-2 m-1">Item 1</div>
            <div class="bg-primary text-white p-2 m-1">Item 2</div>
            <div class="bg-primary text-white p-2 m-1">Item 3</div>
        </div>
    </div>
    <div class="border p-2 mb-4">
        <strong>Desktop (row):</strong> flex-md-row
        <div class="d-flex flex-md-row bg-light p-2">
            <div class="bg-success text-white p-2 m-1">Item 1</div>
            <div class="bg-success text-white p-2 m-1">Item 2</div>
            <div class="bg-success text-white p-2 m-1">Item 3</div>
        </div>
    </div>

    <hr>

    <!-- RESPONSIVE OFFSET -->
    <h5>Responsive Offset</h5>
    <div class="row mb-4">
        <div class="col-4 demo-box">col-4</div>
        <div class="col-4 offset-4 demo-box-2">col-4 offset-4</div>
    </div>
    <div class="row mb-4">
        <div class="col-md-4 demo-box">col-md-4</div>
        <div class="col-md-4 offset-md-4 demo-box-2">col-md-4 offset-md-4</div>
    </div>

    <hr>

    <!-- RESPONSIVE HIDDEN/SHOW -->
    <h5>Hiding Elements on Specific Sizes</h5>
    <div class="row mb-4">
        <div class="col-12">
            <div class="alert alert-info">
                <strong>Try resizing your browser!</strong> The message below changes based on screen size.
            </div>
        </div>
    </div>
    <div class="d-lg-none bg-danger text-white p-3 rounded mb-2">
        ⚠️ You're viewing on a mobile device or small screen!
    </div>
    <div class="d-none d-lg-block bg-success text-white p-3 rounded">
        ✅ You're viewing on a desktop/large screen!
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.col-*`**: Equal width columns on all sizes (mobile-first default)
- **`.col-md-6`**: 6 columns on medium screens and larger, full width on smaller
- **`.d-none`**: Hide element on all sizes
- **`.d-sm-block`**: Show as block on small and larger
- **`.d-md-none`**: Hide on medium and larger
- **`.text-sm-center`**: Center text on small+, left-align on xs
- **`.flex-column`**: Stack items vertically
- **`.flex-md-row`**: Row on medium+, column on smaller
- **`.offset-*`**: Left margin to push column over
- **Breakpoint hierarchy**: When you use col-md-6, it applies to md, lg, xl, xxl but not to sm, xs

## Expected Visual Result

The page displays multiple responsive design examples:

1. **Responsive Grid**: Three columns that change from stacked (mobile) to 2+1 (tablet) to 3 side-by-side (desktop)

2. **Responsive Visibility**: Messages that show only at specific screen sizes - different messages appear at different breakpoints

3. **Responsive Text**: Text alignment that changes from centered on small screens to left or right aligned on larger screens

4. **Responsive Flex**: Flexbox that shows items stacked vertically on mobile but horizontally on desktop

5. **Responsive Offset**: Columns that are centered using offset classes

6. **Show/Hide Elements**: A message that changes between mobile warning (red) and desktop welcome (green) based on screen size

Try resizing the browser window to see elements appear, disappear, and rearrange!
