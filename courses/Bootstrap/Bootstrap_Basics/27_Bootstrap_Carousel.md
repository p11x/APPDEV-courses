# Bootstrap Carousel

## Definition

Bootstrap carousel is a slideshow component for cycling through elements (images or slides) like a rotating gallery. It displays one item at a time and automatically or manually transitions between slides. Carousels are commonly used for hero sections, image galleries, or featured content sliders on websites.

## Key Bullet Points

- **`.carousel`**: Main container for carousel
- **`.carousel-inner`**: Wrapper for slides
- **`.carousel-item`**: Individual slide
- **`.carousel-control-prev` / `.carousel-control-next`**: Navigation arrows
- **`.carousel-indicators`**: Dot indicators at bottom
- **`.carousel-caption`**: Text overlay on slides
- **Auto-play**: Add data-bs-ride="carousel" for automatic sliding
- **Intervals**: data-bs-interval for timing between slides

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Carousel</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Carousel Examples</h2>

    <!-- BASIC CAROUSEL -->
    <h5>Basic Carousel</h5>
    <div id="basicCarousel" class="carousel slide mb-4" data-bs-ride="carousel" style="max-width: 600px;">
        <!-- Indicators -->
        <div class="carousel-indicators">
            <button type="button" data-bs-target="#basicCarousel" data-bs-slide-to="0" class="active"></button>
            <button type="button" data-bs-target="#basicCarousel" data-bs-slide-to="1"></button>
            <button type="button" data-bs-target="#basicCarousel" data-bs-slide-to="2"></button>
        </div>
        <!-- Slides -->
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="https://picsum.photos/800/400?random=1" class="d-block w-100" alt="First slide">
                <div class="carousel-caption d-none d-md-block">
                    <h5>First Slide</h5>
                    <p>Description for the first slide.</p>
                </div>
            </div>
            <div class="carousel-item">
                <img src="https://picsum.photos/800/400?random=2" class="d-block w-100" alt="Second slide">
                <div class="carousel-caption d-none d-md-block">
                    <h5>Second Slide</h5>
                    <p>Description for the second slide.</p>
                </div>
            </div>
            <div class="carousel-item">
                <img src="https://picsum.photos/800/400?random=3" class="d-block w-100" alt="Third slide">
                <div class="carousel-caption d-none d-md-block">
                    <h5>Third Slide</h5>
                    <p>Description for the third slide.</p>
                </div>
            </div>
        </div>
        <!-- Controls -->
        <button class="carousel-control-prev" type="button" data-bs-target="#basicCarousel" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Previous</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#basicCarousel" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Next</span>
        </button>
    </div>

    <!-- CAROUSEL WITH CROSSFADE -->
    <h5>Carousel with Crossfade</h5>
    <div id="crossfadeCarousel" class="carousel slide carousel-fade mb-4" data-bs-ride="carousel" style="max-width: 600px;">
        <div class="carousel-indicators">
            <button type="button" data-bs-target="#crossfadeCarousel" data-bs-slide-to="0" class="active"></button>
            <button type="button" data-bs-target="#crossfadeCarousel" data-bs-slide-to="1"></button>
            <button type="button" data-bs-target="#crossfadeCarousel" data-bs-slide-to="2"></button>
        </div>
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="https://picsum.photos/800/400?random=4" class="d-block w-100" alt="Slide 1">
            </div>
            <div class="carousel-item">
                <img src="https://picsum.photos/800/400?random=5" class="d-block w-100" alt="Slide 2">
            </div>
            <div class="carousel-item">
                <img src="https://picsum.photos/800/400?random=6" class="d-block w-100" alt="Slide 3">
            </div>
        </div>
        <button class="carousel-control-prev" type="button" data-bs-target="#crossfadeCarousel" data-bs-slide="prev">
            <span class="carousel-control-prev-icon"></span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#crossfadeCarousel" data-bs-slide="next">
            <span class="carousel-control-next-icon"></span>
        </button>
    </div>

    <!-- CAROUSEL WITH BUTTONS -->
    <h5>Carousel with Custom Controls</h5>
    <div id="customCarousel" class="carousel slide mb-4" data-bs-ride="carousel" style="max-width: 600px;">
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img src="https://picsum.photos/800/400?random=7" class="d-block w-100" alt="Slide">
                <div class="carousel-caption">
                    <button class="btn btn-primary btn-sm">Learn More</button>
                    <button class="btn btn-outline-light btn-sm">Contact Us</button>
                </div>
            </div>
            <div class="carousel-item">
                <img src="https://picsum.photos/800/400?random=8" class="d-block w-100" alt="Slide">
            </div>
            <div class="carousel-item">
                <img src="https://picsum.photos/800/400?random=9" class="d-block w-100" alt="Slide">
            </div>
        </div>
        <button class="carousel-control-prev" type="button" data-bs-target="#customCarousel" data-bs-slide="prev">
            <span class="carousel-control-prev-icon bg-dark rounded-circle p-4"></span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#customCarousel" data-bs-slide="next">
            <span class="carousel-control-next-icon bg-dark rounded-circle p-4"></span>
        </button>
    </div>

    <style>
        .bg-dark { background-color: rgba(0,0,0,0.5) !important; }
    </style>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
