# Setup Bootstrap

## Definition

Setting up Bootstrap means adding the Bootstrap library to your web project so you can use its pre-built CSS classes and components. There are three main ways to include Bootstrap: using a CDN (Content Delivery Network), downloading files locally, or installing via npm for Angular projects. The CDN method is the quickest and easiest for beginners to get started.

## Key Bullet Points

- **CDN Method**: The fastest way to start - just add a link tag in your HTML head section
- **Local Files**: Download Bootstrap files and reference them locally for offline use
- **npm Installation**: For Angular projects, install Bootstrap as a package and import in angular.json
- **Bootstrap 5.3.2**: The current stable version with latest features and improvements
- **JavaScript Required**: Some Bootstrap components need JavaScript - include the Bootstrap bundle script

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Setting Up Bootstrap</title>
    
    <!-- METHOD 1: CDN (Recommended for beginners) -->
    <!-- Add this single line to include Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <!-- Navbar to show Bootstrap is working -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
        <div class="container">
            <a class="navbar-brand" href="#">My Bootstrap Site</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link active" href="#">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">About</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Contact</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <div class="container mt-5">
        <h1>Welcome to Bootstrap!</h1>
        <p class="lead">Bootstrap is now set up and working.</p>
        <button class="btn btn-success">Click Me</button>
    </div>

    <!-- METHOD 1 CONTINUED: Include Bootstrap JavaScript bundle -->
    <!-- This is required for interactive components like navbar, modals, dropdowns -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css">`**: The main Bootstrap CSS file from CDN - this contains all the styling classes
- **CDN (Content Delivery Network)**: A network of servers that host files globally, ensuring fast loading from a location near the user
- **`navbar`**: Bootstrap class for creating navigation bars
- **`navbar-expand-lg`**: Makes navbar horizontal on large screens, collapses on smaller ones
- **`navbar-dark bg-primary`**: Applies dark text color on light backgrounds with primary (blue) background
- **`container`**: Centers content with appropriate margins
- **`<script src="bootstrap.bundle.min.js">`**: Includes both Bootstrap JavaScript and Popper (required for tooltips, popovers, dropdowns)

## Expected Visual Result

The page will display:
- A dark blue navigation bar at the top with "My Bootstrap Site" brand text
- The navbar has Home, About, and Contact links
- On mobile screens, links collapse into a hamburger menu icon
- Below the navbar, a centered content area with a welcome heading
- A large, prominent green success-colored button
- All elements will have consistent, professional Bootstrap styling

**For Angular npm installation, the commands would be:**
```
npm install bootstrap@5.3.2
```

Then add to angular.json styles array: `"node_modules/bootstrap/dist/css/bootstrap.min.css"`
