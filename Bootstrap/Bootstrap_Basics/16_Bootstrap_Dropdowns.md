# Bootstrap Dropdowns

## Definition

Bootstrap dropdowns are toggleable menus that display a list of links or actions when clicked. They allow you to compactly present multiple options without taking up much space. Dropdowns require JavaScript to function and use the Popper library for positioning the menu correctly.

## Key Bullet Points

- **`.dropdown`**: Container for dropdown button and menu
- **`.dropdown-toggle`**: Button that triggers the dropdown menu
- **`.dropdown-menu`**: The actual dropdown list container
- **`.dropdown-item`**: Individual items within the dropdown
- **`.dropdown-divider`**: Horizontal line to separate groups of items
- **`.dropdown-header`**: Heading for dropdown item groups
- **Split Dropdown**: Two buttons - one for action, one for menu toggle

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Dropdowns</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Dropdown Examples</h2>

    <!-- BASIC DROPDOWN -->
    <h5>Basic Dropdown</h5>
    <div class="dropdown mb-4">
        <button class="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown">
            Dropdown Button
        </button>
        <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
            <li><a class="dropdown-item" href="#">Action</a></li>
            <li><a class="dropdown-item" href="#">Another action</a></li>
            <li><a class="dropdown-item" href="#">Something else</a></li>
        </ul>
    </div>

    <!-- SPLIT BUTTON DROPDOWN -->
    <h5>Split Button Dropdown</h5>
    <div class="btn-group mb-4">
        <button type="button" class="btn btn-success">Split Button</button>
        <button type="button" class="btn btn-success dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown">
            <span class="visually-hidden">Toggle Dropdown</span>
        </button>
        <ul class="dropdown-menu">
            <li><a class="dropdown-item" href="#">Action</a></li>
            <li><a class="dropdown-item" href="#">Another action</a></li>
            <li><hr class="dropdown-divider"></li>
            <li><a class="dropdown-item" href="#">Separated link</a></li>
        </ul>
    </div>

    <hr>

    <!-- DROPDOWN SIZES -->
    <h5>Dropdown Sizes</h5>
    <div class="mb-2">
        <div class="btn-group">
            <button class="btn btn-secondary btn-sm dropdown-toggle" type="button" data-bs-toggle="dropdown">Small</button>
            <ul class="dropdown-menu">
                <li><a class="dropdown-item" href="#">Small dropdown</a></li>
            </ul>
        </div>
    </div>
    <div class="mb-2">
        <div class="btn-group">
            <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">Default</button>
            <ul class="dropdown-menu">
                <li><a class="dropdown-item" href="#">Default dropdown</a></li>
            </ul>
        </div>
    </div>
    <div class="mb-4">
        <div class="btn-group">
            <button class="btn btn-secondary btn-lg dropdown-toggle" type="button" data-bs-toggle="dropdown">Large</button>
            <ul class="dropdown-menu">
                <li><a class="dropdown-item" href="#">Large dropdown</a></li>
            </ul>
        </div>
    </div>

    <!-- DROPDOWN DARK -->
    <h5>Dark Dropdown</h5>
    <div class="dropdown mb-4">
        <button class="btn btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
            Dark Dropdown Menu
        </button>
        <ul class="dropdown-menu dropdown-menu-dark">
            <li><a class="dropdown-item active" href="#">Active Item</a></li>
            <li><a class="dropdown-item" href="#">Action</a></li>
            <li><a class="dropdown-item" href="#">Another action</a></li>
            <li><hr class="dropdown-divider"></li>
            <li><a class="dropdown-item" href="#">Separated link</a></li>
        </ul>
    </div>

    <!-- DROPDOWN WITH HEADERS AND DIVIDERS -->
    <h5>With Headers and Dividers</h5>
    <div class="dropdown mb-4">
        <button class="btn btn-info dropdown-toggle" type="button" data-bs-toggle="dropdown">
            Menu with Structure
        </button>
        <ul class="dropdown-menu">
            <li><h6 class="dropdown-header">Group 1</h6></li>
            <li><a class="dropdown-item" href="#">First option</a></li>
            <li><a class="dropdown-item" href="#">Second option</a></li>
            <li><hr class="dropdown-divider"></li>
            <li><h6 class="dropdown-header">Group 2</h6></li>
            <li><a class="dropdown-item" href="#">Third option</a></li>
            <li><a class="dropdown-item" href="#">Fourth option</a></li>
        </ul>
    </div>

    <!-- DROPDOWN DIRECTION -->
    <h5>Dropend and Dropup</h5>
    <div class="row mb-4">
        <div class="col-6">
            <div class="btn-group dropend">
                <button type="button" class="btn btn-warning dropdown-toggle" data-bs-toggle="dropdown">
                    Dropend (Right)
                </button>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Menu item</a></li>
                    <li><a class="dropdown-item" href="#">Menu item</a></li>
                </ul>
            </div>
        </div>
        <div class="col-6">
            <div class="btn-group dropup">
                <button type="button" class="btn btn-danger dropdown-toggle" data-bs-toggle="dropdown">
                    Dropup (Up)
                </button>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#">Menu item</a></li>
                    <li><a class="dropdown-item" href="#">Menu item</a></li>
                </ul>
            </div>
        </div>
    </div>

    <!-- NAVBAR DROPDOWN -->
    <h5>Navbar Dropdown</h5>
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">Navbar</a>
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                <li class="nav-item">
                    <a class="nav-link active" href="#">Home</a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown">
                        Services
                    </a>
                    <ul class="dropdown-menu">
                        <li><a class="dropdown-item" href="#">Web Design</a></li>
                        <li><a class="dropdown-item" href="#">Development</a></li>
                        <li><hr class="dropdown-divider"></li>
                        <li><a class="dropdown-item" href="#">Consulting</a></li>
                    </ul>
                </li>
            </ul>
        </div>
    </nav>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.dropdown`**: Wrapper that positions the dropdown
- **`.dropdown-toggle`**: Class applied to button that shows/hides menu - adds caret arrow
- **`.dropdown-menu`**: The actual list of items - positioned absolutely
- **`.dropdown-item`**: Individual clickable items in the menu
- **`.dropdown-divider`**: Horizontal line separating item groups
- **`.dropdown-header`**: Non-clickable heading for item groups
- **`.dropdown-menu-dark`**: Dark themed dropdown
- **`.dropend`**: Opens dropdown to the right
- **`.dropup`**: Opens dropdown upward instead of down
- **`.btn-group`**: Groups split button with toggle
- **`.dropdown-toggle-split`**: Special toggle for split buttons with less padding
- **`data-bs-toggle="dropdown"`**: Bootstrap attribute that enables dropdown functionality
- **`aria-labelledby`**: Accessibility attribute linking button to menu

## Expected Visual Result

The page displays multiple dropdown examples:

1. **Basic Dropdown**: Blue button with downward arrow that opens a menu with three options

2. **Split Button**: Two buttons - left for action (clicking does something), right for toggling menu

3. **Size Variations**: Small, default, and large dropdown buttons

4. **Dark Dropdown**: Dark-themed dropdown menu with light text

5. **Structured Menu**: Dropdown with section headers and divider lines

6. **Directional Dropdowns**:
   - Dropend: Opens to the right of button
   - Dropup: Opens above the button

7. **Navbar Dropdown**: In-context navbar showing dropdown works within navigation

Clicking any dropdown button reveals a menu with options. Menus close when clicking outside or selecting an item.
