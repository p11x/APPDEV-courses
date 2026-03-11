# Bootstrap Pagination

## Definition

Bootstrap pagination provides a way to navigate through large sets of content by dividing it into pages. The pagination component displays page numbers with previous/next navigation. It uses the `.pagination` class as the container and `.page-item` for each page number. Pagination helps users browse through multi-page content like search results or blog archives.

## Key Bullet Points

- **`.pagination`**: Container for pagination links
- **`.page-item`**: Individual page number wrapper
- **`.page-link`**: The actual clickable link/button
- **`.active`**: Current/highlighted page
- **`.disabled`**: Disabled state for prev/next on first/last page
- **Sizing**: pagination-lg (large), pagination-sm (small)
- **Alignment**: Use justify-content-center, justify-content-end

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Pagination</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Pagination Examples</h2>

    <!-- BASIC PAGINATION -->
    <h5>Basic Pagination</h5>
    <nav aria-label="Page navigation">
        <ul class="pagination mb-4">
            <li class="page-item"><a class="page-link" href="#">Previous</a></li>
            <li class="page-item"><a class="page-link" href="#">1</a></li>
            <li class="page-item"><a class="page-link" href="#">2</a></li>
            <li class="page-item"><a class="page-link" href="#">3</a></li>
            <li class="page-item"><a class="page-link" href="#">Next</a></li>
        </ul>
    </nav>

    <!-- ACTIVE AND DISABLED STATES -->
    <h5>Active and Disabled States</h5>
    <nav aria-label="Page navigation">
        <ul class="pagination mb-4">
            <li class="page-item disabled">
                <a class="page-link" href="#" tabindex="-1" aria-disabled="true">Previous</a>
            </li>
            <li class="page-item"><a class="page-link" href="#">1</a></li>
            <li class="page-item active" aria-current="page">
                <a class="page-link" href="#">2</a>
            </li>
            <li class="page-item"><a class="page-link" href="#">3</a></li>
            <li class="page-item">
                <a class="page-link" href="#">Next</a>
            </li>
        </ul>
    </nav>

    <!-- PAGINATION WITH ICONS -->
    <h5>Pagination with Icons</h5>
    <nav aria-label="Page navigation">
        <ul class="pagination mb-4">
            <li class="page-item">
                <a class="page-link" href="#" aria-label="Previous">
                    <span aria-hidden="true">&laquo;</span>
                </a>
            </li>
            <li class="page-item"><a class="page-link" href="#">1</a></li>
            <li class="page-item"><a class="page-link" href="#">2</a></li>
            <li class="page-item"><a class="page-link" href="#">3</a></li>
            <li class="page-item">
                <a class="page-link" href="#" aria-label="Next">
                    <span aria-hidden="true">&raquo;</span>
                </a>
            </li>
        </ul>
    </nav>

    <!-- PAGINATION SIZES -->
    <h5>Pagination Sizes</h5>
    <div class="mb-2">
        <strong>Small:</strong>
        <nav aria-label="Page navigation">
            <ul class="pagination pagination-sm">
                <li class="page-item"><a class="page-link" href="#">1</a></li>
                <li class="page-item"><a class="page-link" href="#">2</a></li>
                <li class="page-item"><a class="page-link" href="#">3</a></li>
            </ul>
        </nav>
    </div>
    <div class="mb-2">
        <strong>Default:</strong>
        <nav aria-label="Page navigation">
            <ul class="pagination">
                <li class="page-item"><a class="page-link" href="#">1</a></li>
                <li class="page-item"><a class="page-link" href="#">2</a></li>
                <li class="page-item"><a class="page-link" href="#">3</a></li>
            </ul>
        </nav>
    </div>
    <div class="mb-4">
        <strong>Large:</strong>
        <nav aria-label="Page navigation">
            <ul class="pagination pagination-lg">
                <li class="page-item"><a class="page-link" href="#">1</a></li>
                <li class="page-item"><a class="page-link" href="#">2</a></li>
                <li class="page-item"><a class="page-link" href="#">3</a></li>
            </ul>
        </nav>
    </div>

    <!-- ALIGNMENT -->
    <h5>Alignment</h5>
    <nav aria-label="Page navigation">
        <ul class="pagination justify-content-start mb-2">
            <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
            <li class="page-item"><a class="page-link" href="#">1</a></li>
            <li class="page-item"><a class="page-link" href="#">2</a></li>
            <li class="page-item"><a class="page-link" href="#">3</a></li>
        </ul>
    </nav>
    <nav aria-label="Page navigation">
        <ul class="pagination justify-content-center mb-2">
            <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
            <li class="page-item"><a class="page-link" href="#">1</a></li>
            <li class="page-item active"><a class="page-link" href="#">2</a></li>
            <li class="page-item"><a class="page-link" href="#">3</a></li>
            <li class="page-item"><a class="page-link" href="#">Next</a></li>
        </ul>
    </nav>
    <nav aria-label="Page navigation">
        <ul class="pagination justify-content-end mb-4">
            <li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>
            <li class="page-item"><a class="page-link" href="#">1</a></li>
            <li class="page-item"><a class="page-link" href="#">2</a></li>
            <li class="page-item"><a class="page-link" href="#">3</a></li>
            <li class="page-item"><a class="page-link" href="#">Next</a></li>
        </ul>
    </nav>

    <!-- BREADCRUMB (similar style) -->
    <h5>Breadcrumb (Navigation Path)</h5>
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb mb-4">
            <li class="breadcrumb-item"><a href="#">Home</a></li>
            <li class="breadcrumb-item"><a href="#">Library</a></li>
            <li class="breadcrumb-item active" aria-current="page">Data</li>
        </ol>
    </nav>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.pagination`**: Container for pagination links - uses flexbox for layout
- **`.page-item`**: Individual page number wrapper
- **`.page-link`**: Clickable link for each page
- **`.active`**: Highlights current page with different background
- **`.disabled`**: Grayed out, non-clickable state for prev on first page, next on last page
- **`.pagination-sm`**: Smaller pagination links
- **`.pagination-lg`**: Larger pagination links
- **`.justify-content-start`**: Left-align pagination
- **`.justify-content-center`**: Center-align pagination
- **`.justify-content-end`**: Right-align pagination
- **`.breadcrumb`**: Similar component for showing navigation path
- **`aria-label`**: Accessibility labels for screen readers
- **`aria-current`**: Indicates current page to screen readers

## Expected Visual Result

The page displays multiple pagination examples:

1. **Basic Pagination**: Previous, 1, 2, 3, Next links in a row

2. **Active/Disabled States**: Current page (2) highlighted, Previous button disabled (grayed out)

3. **Icon Pagination**: Using « and » arrows instead of Previous/Next text

4. **Sizes**: Three pagination bars showing small, default, and large sizes

5. **Alignment**: Left-aligned, centered, and right-aligned pagination examples

6. **Breadcrumb**: Navigation path showing Home > Library > Data

All pagination items have hover effects and the active page is visually distinct.
