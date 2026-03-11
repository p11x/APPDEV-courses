# Bootstrap Cards

## Definition

Bootstrap cards are flexible, extensible content containers that provide a standardized way to display content. Cards can contain various elements including headers, footers, images, body text, and links. They feature a bordered box with padding and optional shadows, making them perfect for displaying blog posts, product items, or user profiles.

## Key Bullet Points

- **`.card`**: Base class for card container
- **`.card-body`**: Main content area inside card
- **`.card-header`**: Optional header section at top
- **`.card-footer`**: Optional footer section at bottom
- **`.card-img-top`**: Image at top of card (above body)
- **`.card-img-overlay`**: Image as card background with text overlay
- **Card Colors**: Use bg-primary, bg-success, etc. for colored cards

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Cards</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Card Examples</h2>

    <!-- BASIC CARD -->
    <h5>Basic Card</h5>
    <div class="card mb-4" style="max-width: 350px;">
        <div class="card-body">
            <h5 class="card-title">Card Title</h5>
            <h6 class="card-subtitle mb-2 text-muted">Card Subtitle</h6>
            <p class="card-text">This is the card body text. Cards are flexible containers that can hold various content.</p>
            <a href="#" class="card-link">Card link</a>
            <a href="#" class="card-link">Another link</a>
        </div>
    </div>

    <!-- CARD WITH IMAGE -->
    <h5>Card with Image</h5>
    <div class="row mb-4">
        <div class="col-md-4">
            <div class="card">
                <img src="https://picsum.photos/400/200" class="card-img-top" alt="Card image">
                <div class="card-body">
                    <h5 class="card-title">Card with Image</h5>
                    <p class="card-text">This card has an image at the top using .card-img-top class.</p>
                    <a href="#" class="btn btn-primary">Go somewhere</a>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Card without Image</h5>
                    <p class="card-text">This is a simple card with just text content in the body.</p>
                    <a href="#" class="btn btn-outline-primary">Click Me</a>
                </div>
            </div>
        </div>
    </div>

    <!-- CARD WITH HEADER AND FOOTER -->
    <h5>Card with Header and Footer</h5>
    <div class="card mb-4" style="max-width: 450px;">
        <div class="card-header bg-primary text-white">
            Featured
        </div>
        <div class="card-body">
            <h5 class="card-title">Special Title Treatment</h5>
            <p class="card-text">With supporting text below as a natural lead-in to additional content.</p>
            <a href="#" class="btn btn-primary">Learn More</a>
        </div>
        <div class="card-footer text-muted">
            2 days ago
        </div>
    </div>

    <!-- CARD GRID LAYOUT -->
    <h5>Card Grid (Product Cards)</h5>
    <div class="row">
        <div class="col-md-4 mb-3">
            <div class="card h-100">
                <img src="https://picsum.photos/400/250?random=1" class="card-img-top" alt="Product 1">
                <div class="card-body">
                    <h5 class="card-title">Product One</h5>
                    <p class="card-text">Description for the first product item.</p>
                    <h6 class="text-success">$99.99</h6>
                </div>
                <div class="card-footer">
                    <button class="btn btn-sm btn-primary">Add to Cart</button>
                </div>
            </div>
        </div>
        <div class="col-md-4 mb-3">
            <div class="card h-100">
                <img src="https://picsum.photos/400/250?random=2" class="card-img-top" alt="Product 2">
                <div class="card-body">
                    <h5 class="card-title">Product Two</h5>
                    <p class="card-text">Description for the second product item.</p>
                    <h6 class="text-success">$149.99</h6>
                </div>
                <div class="card-footer">
                    <button class="btn btn-sm btn-primary">Add to Cart</button>
                </div>
            </div>
        </div>
        <div class="col-md-4 mb-3">
            <div class="card h-100">
                <img src="https://picsum.photos/400/250?random=3" class="card-img-top" alt="Product 3">
                <div class="card-body">
                    <h5 class="card-title">Product Three</h5>
                    <p class="card-text">Description for the third product item.</p>
                    <h6 class="text-success">$79.99</h6>
                </div>
                <div class="card-footer">
                    <button class="btn btn-sm btn-primary">Add to Cart</button>
                </div>
            </div>
        </div>
    </div>

    <!-- COLORED CARDS -->
    <h5>Colored Cards</h5>
    <div class="row mb-4">
        <div class="col-md-3">
            <div class="card bg-primary text-white mb-3">
                <div class="card-header">Header</div>
                <div class="card-body">
                    <h5 class="card-title">Primary Card</h5>
                    <p class="card-text">Card with primary background color.</p>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card bg-success text-white mb-3">
                <div class="card-header">Header