# Bootstrap Images

## Definition

Bootstrap provides several image styling classes that make images responsive and add visual effects. The `.img-fluid` class ensures images scale properly with their container, preventing large images from breaking layouts. Additional classes like `.rounded`, `.rounded-circle`, and `.img-thumbnail` add different corner and border styles to images.

## Key Bullet Points

- **`.img-fluid`**: Makes image responsive - scales to fit container width, never exceeds original size
- **`.rounded`**: Adds rounded corners to all four corners of an image
- **`.rounded-circle`**: Makes the image a perfect circle (requires square image)
- **`.img-thumbnail`**: Adds rounded corners plus a thin gray border
- **Alignment**: Use `.float-start` and `.float-end` or text alignment classes
- **Picture Element**: For art direction, use `<picture>` with `<source>`

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Images</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .demo-img {
            background-color: #f8f9fa;
            padding: 10px;
            margin-bottom: 20px;
        }
    </style>
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Image Classes</h2>

    <!-- RESPONSIVE IMAGE -->
    <h5>Responsive Image (.img-fluid)</h5>
    <div class="container mb-4" style="max-width: 600px; border: 2px dashed #ccc;">
        <p>This container is 600px wide. The image below will scale to fit:</p>
        <img src="https://picsum.photos/1200/400" alt="Responsive image" class="img-fluid">
    </div>

    <!-- ROUNDED CORNERS -->
    <h5>Rounded Corners (.rounded)</h5>
    <img src="https://picsum.photos/300/200" alt="Rounded image" class="rounded mb-4">

    <!-- CIRCULAR IMAGE -->
    <h5>Circle Image (.rounded-circle)</h5>
    <div class="demo-img d-inline-block">
        <img src="https://picsum.photos/200" alt="Circle image" class="rounded-circle" width="200" height="200">
    </div>
    <div class="demo-img d-inline-block">
        <img src="https://picsum.photos/200?grayscale" alt="Circle grayscale" class="rounded-circle" width="200" height="200">
    </div>

    <!-- THUMBNAIL -->
    <h5>Thumbnail (.img-thumbnail)</h5>
    <img src="https://picsum.photos/250" alt="Thumbnail" class="img-thumbnail mb-4" width="250">

    <hr>

    <!-- IMAGE GALLERY GRID -->
    <h5>Image Gallery with Grid</h5>
    <div class="row">
        <div class="col-md-4 mb-3">
            <div class="card">
                <img src="https://picsum.photos/400/300?random=1" class="card-img-top" alt="Gallery image 1">
                <div class="card-body">
                    <p class="card-text text-center">Image 1</p>
                </div>
            </div>
        </div>
        <div class="col-md-4 mb-3">
            <div class="card">
                <img src="https://picsum.photos/400/300?random=2" class="card-img-top rounded" alt="Gallery image 2">
                <div class="card-body">
                    <p class="card-text text-center">Rounded corners</p>
                </div>
            </div>
        </div>
        <div class="col-md-4 mb-3">
            <div class="card">
                <img src="https://picsum.photos/400/300?random=3" class="card-img-top img-thumbnail" alt="Gallery image 3">
                <div class="card-body">
                    <p class="card-text text-center">Thumbnail style</p>
                </div>
            </div>
        </div>
    </div>

    <!-- ALIGNED IMAGES -->
    <h5 class="mt-4">Aligned Images</h5>
    <div class="clearfix mb-4">
        <img src="https://picsum.photos/150" class="rounded float-start me-3" alt="Float left">
        <p>This paragraph has an image floated to the left. The text wraps around the image on the right side. Use .float-end to float right, or use text alignment classes.</p>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.img-fluid`**: Sets max-width: 100% and height: auto, making the image scale down when container is smaller but never scale up beyond original size
- **`.rounded`**: Applies border-radius: 0.375rem (about 6px) rounding to all corners
- **`.rounded-circle`**: Applies border-radius: 50% making the image perfectly circular (works best with square images)
- **`.img-thumbnail`**: Applies rounded corners plus a 1px gray border (padding: 0.25rem)
- **`.float-start`**: Floats element to the left, allowing text to wrap around it
- **`.float-end`**: Floats element to the right
- **`.me-3`**: Adds margin-end (right margin) spacing of 1rem
- **`.card`**: Container for image with caption below
- **`.card-img-top`**: Places image at top of card component
- **`.card-body`**: Content area within card

## Expected Visual Result

The page displays various image examples:

1. **Responsive Image**: A large image displayed inside a 600px container - the image scales to fit without overflowing

2. **Rounded Corners**: Image with slightly rounded corners

3. **Circular Images**: Two perfectly circular images (one color, one grayscale) - works best when source image is square

4. **Thumbnail**: Image with rounded corners and gray border frame

5. **Image Gallery**: Three-column grid of image cards showing different styles (card-img-top, rounded, thumbnail)

6. **Aligned Images**: An image floated left with text wrapping around it on the right side

All images maintain their aspect ratios and respond properly when the browser window is resized.
