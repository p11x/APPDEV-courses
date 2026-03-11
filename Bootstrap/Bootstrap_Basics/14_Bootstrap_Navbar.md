# Bootstrap Navbar

## Definition

The Bootstrap navbar is a responsive navigation header that can collapse into a hamburger menu on mobile devices. It supports branding (logo/text), navigation links, forms, and buttons. Navbars are essential for website navigation and can be customized with different color schemes and responsive behaviors.

## Key Bullet Points

- **`.navbar`**: Base class for navbar container
- **`.navbar-expand-*`**: Controls when navbar expands (sm, md, lg, xl, xxl)
- **`.navbar-brand`**: Logo/brand name on the left
- **`.navbar-nav`**: Navigation links container
- **`.navbar-toggler`**: Hamburger menu button for mobile
- **Color Schemes**: navbar-dark (light text), navbar-light (dark text), bg-* (background colors)
- **Responsive**: Automatically collapses on smaller screens

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Navbar</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    
    <!-- BASIC NAVBAR -->
    <h5>Basic Navbar</h5>
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
        <div class="container">
            <a class="navbar-brand" href="#">My Website</a>
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
                        <a class="nav-link" href="#">Services</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Contact</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <!-- NAVBAR WITH DROPDOWN -->
    <h5>Navbar with Dropdown</h5>
    <nav class="navbar navbar-expand-lg navbar-light bg-light mb-4">
        <div class="container">
            <a class="navbar-brand" href="#">
                <img src="https://picsum.photos/30" width="30" height="30" alt="Logo">
            </a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavDropdown">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavDropdown">
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link active" href="#">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Features</a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
                            Dropdown Menu
                        </a>
                        <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="#">Action</a></li>
                            <li><a class="dropdown-item" href="#">Another action</a></li>
                            <li><hr class="dropdown-divider"></li>
                            <li><a class="dropdown-item" href="#">Something else</a></li>
                        </ul>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Pricing</a>
                    </li>
                </ul>
                <form class="d-flex ms-auto">
                    <input class="form-control me-2" type="search" placeholder="Search" aria-label="Search">
                    <button class="btn btn-outline-primary" type="submit">Search</button>
                </form>
            </div>
        </div>
    </nav>

    <!-- COLOR VARIANTS -->
    <h5>Color Variants</h5>
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary mb-2">
        <div class="container">
            <a class="navbar-brand" href="#">Primary Navbar</a>
        </div>
    </nav>
    <nav class="navbar navbar-expand-lg navbar-dark bg-success mb-2">
        <div class="container">
            <a class="navbar-brand" href="#">Success Navbar</a>
        </div>
    </nav>
    <nav class="navbar navbar-expand-lg navbar-light bg-warning mb-2">
        <div class="container">
            <a class="navbar-brand" href="#">Warning Navbar</a>
        </div>
    </nav>

    <div class="container mt-4">
        <h3>Navbar Examples Above</h3>
        <p>Scroll up to see the responsive navigation bars.</p>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.navbar`**: Base class for navbar - sets up positioning and basic styles
- **`.navbar-expand-lg`**: Navbar is horizontal on large screens (≥992px), collapses on smaller
- **`.navbar-dark`**: Sets text color to light (for dark backgrounds)
- **`.navbar-light`**: Sets text color to dark (for light backgrounds)
- **`.bg-dark`, `.bg-primary`**: Background color classes
- **`.navbar-brand`**: Brand/logo area - typically positioned on the left
- **`.navbar-toggler`**: Hamburger button visible on mobile - toggles the collapse
- **`.navbar-collapse`**: Collapsible content area
- **`.navbar-nav`**: Container for navigation links
- **`.nav-item`**: Individual navigation item
- **`.nav-link`**: Navigation link styling
- **`.dropdown`**: Enables dropdown menu functionality
- **`.d-flex`**: Display flex for layout
- **`.ms-auto`**: Margin start auto - pushes element to the right
- **`.me-2`**: Margin end spacing
- **`data-bs-toggle="collapse"`**: Bootstrap attribute for toggle functionality

## Expected Visual Result

The page displays multiple navbar examples:

1. **Basic Navbar**: Dark navbar with brand "My Website" on left and navigation links. On mobile, links collapse into hamburger menu.

2. **Navbar with Dropdown**: Light navbar with logo image, navigation links including a dropdown, and a search form on the right. All elements collapse on mobile.

3. **Color Variants**: Three colored navbars stacked - primary blue, success green, and warning yellow - showing different background color options.

On desktop, all navbars display horizontally with brand on left and links on right. On mobile (resize browser), they collapse into hamburger menus that expand when clicked.
