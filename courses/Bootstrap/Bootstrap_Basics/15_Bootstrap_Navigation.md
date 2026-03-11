# Bootstrap Navigation

## Definition

Bootstrap provides navigation components for creating tabbed or pill-style navigation interfaces. The `.nav` base class creates reusable navigation elements that can be styled as tabs (with borders like folder tabs) or pills (rounded button-like navigation). These components are commonly used for organizing content into separate views without page reloads.

## Key Bullet Points

- **`.nav`**: Base class for navigation
- **`.nav-tabs`**: Renders navigation as tab style with borders
- **`.nav-pills`**: Renders navigation as pill buttons
- **`.nav-fill`**: Fills available space, distributing nav items proportionally
- **`.nav-justified`**: Makes all nav items equal width
- **`.active`**: Indicates the currently selected/active item
- **`.disabled`**: Disables a navigation item
- **`.nav-item`**: Wrapper for each navigation item

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Navigation</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Navigation Examples</h2>

    <!-- BASE NAV -->
    <h5>Base Navigation (.nav)</h5>
    <nav class="nav mb-4">
        <a class="nav-link active" href="#">Active</a>
        <a class="nav-link" href="#">Link</a>
        <a class="nav-link" href="#">Link</a>
        <a class="nav-link disabled" href="#">Disabled</a>
    </nav>

    <hr>

    <!-- NAV TABS -->
    <h5>Navigation Tabs (.nav-tabs)</h5>
    <nav class="nav nav-tabs mb-2">
        <a class="nav-link active" href="#">Home</a>
        <a class="nav-link" href="#">Profile</a>
        <a class="nav-link" href="#">Messages</a>
        <a class="nav-link disabled" href="#">Settings</a>
    </nav>

    <!-- TABS WITH DROPDOWN -->
    <nav class="nav nav-tabs mb-4">
        <a class="nav-link active" href="#">Home</a>
        <a class="nav-link" href="#">Profile</a>
        <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown" href="#" role="button">Dropdown</a>
            <ul class="dropdown-menu">
                <li><a class="dropdown-item" href="#">Action</a></li>
                <li><a class="dropdown-item" href="#">Another action</a></li>
                <li><a class="dropdown-item" href="#">Something else</a></li>
            </ul>
        </li>
        <a class="nav-link" href="#">Contact</a>
    </nav>

    <hr>

    <!-- NAV PILLS -->
    <h5>Navigation Pills (.nav-pills)</h5>
    <nav class="nav nav-pills mb-2">
        <a class="nav-link active" href="#">Active</a>
        <a class="nav-link" href="#">Link</a>
        <a class="nav-link" href="#">Link</a>
        <a class="nav-link disabled" href="#">Disabled</a>
    </nav>

    <!-- PILLS WITH STACKED -->
    <h5>Stacked Pills (.flex-column)</h5>
    <div class="row">
        <div class="col-4">
            <nav class="nav nav-pills flex-column">
                <a class="nav-link active" href="#">Dashboard</a>
                <a class="nav-link" href="#">Analytics</a>
                <a class="nav-link" href="#">Reports</a>
                <a class="nav-link" href="#">Settings</a>
            </nav>
        </div>
        <div class="col-8">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Content Area</h5>
                    <p>This is the content that corresponds to the selected navigation item on the left.</p>
                </div>
            </div>
        </div>
    </div>

    <hr>

    <!-- NAV FILL AND JUSTIFIED -->
    <h5>Nav Fill (.nav-fill)</h5>
    <nav class="nav nav-pills nav-fill mb-3">
        <a class="nav-link active" href="#">Active</a>
        <a class="nav-link" href="#">Much longer nav link</a>
        <a class="nav-link" href="#">Link</a>
    </nav>

    <h5>Nav Justified (.nav-justified)</h5>
    <nav class="nav nav-pills nav-justified mb-4">
        <a class="nav-link active" href="#">Active</a>
        <a class="nav-link" href="#">Much longer nav link</a>
        <a class="nav-link" href="#">Link</a>
    </nav>

    <!-- TABS WITH TAB CONTENT -->
    <h5>Tabs with Content Panels</h5>
    <ul class="nav nav-tabs mb-3" id="myTab" role="tablist">
        <li class="nav-item" role="presentation">
            <button class="nav-link active" id="home-tab" data-bs-toggle="tab" data-bs-target="#home" type="button">Home</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="profile-tab" data-bs-toggle="tab" data-bs-target="#profile" type="button">Profile</button>
        </li>
        <li class="nav-item" role="presentation">
            <button class="nav-link" id="contact-tab" data-bs-toggle="tab" data-bs-target="#contact" type="button">Contact</button>
        </li>
    </ul>
    <div class="tab-content" id="myTabContent">
        <div class="tab-pane fade show active" id="home" role="tabpanel">
            <h5>Home Tab Content</h5>
            <p>This is the home panel content. It shows when Home is clicked.</p>
        </div>
        <div class="tab-pane fade" id="profile" role="tabpanel">
            <h5>Profile Tab Content</h5>
            <p>This is the profile panel content. It shows when Profile is clicked.</p>
        </div>
        <div class="tab-pane fade" id="contact" role="tabpanel">
            <h5>Contact Tab Content</h5>
            <p>This is the contact panel content. It shows when Contact is clicked.</p>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.nav`**: Base class - provides basic navigation styling (horizontal layout, spacing)
- **`.nav-tabs`**: Transforms nav into tab-style with borders and backgrounds
- **`.nav-pills`**: Transforms nav into pill/button style with rounded backgrounds
- **`.nav-link`**: Styles individual links within nav
- **`.active`**: Highlights the currently selected navigation item
- **`.disabled`**: Makes item unclickable with reduced opacity
- **`.nav-fill`**: Fills horizontal space - items use proportional width
- **`.nav-justified`**: Makes all items exactly equal width
- **`.flex-column`**: Changes nav to vertical/stacked layout
- **`.nav-item`**: Required wrapper for each item in nav-based components
- **`.dropdown`**: Creates dropdown menu within navigation
- **`.tab-pane`**: Container for tab panel content
- **`.fade`**: Adds fade animation when switching tabs

## Expected Visual Result

The page displays multiple navigation examples:

1. **Base Nav**: Simple horizontal links with one marked as active

2. **Nav Tabs**: Tab-style navigation with folder-like appearance, one active

3. **Tabs with Dropdown**: Tabs with a dropdown menu item that expands

4. **Nav Pills**: Button-style navigation, one active with filled background

5. **Stacked Pills**: Vertical navigation on left side with content area on right

6. **Nav Fill**: Pills that proportionally fill available space

7. **Nav Justified**: Pills that are all equal width

8. **Tabs with Content**: Working tabs that show/hide different content panels when clicked

All navigation items show visual feedback (highlighting) when active or hovered.
