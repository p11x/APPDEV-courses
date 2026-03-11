# Bootstrap Badges

## Definition

Bootstrap badges are small labeling components used to add counts, labels, or status indicators to your interface. They're commonly used for notification counts, status labels, and category tags. Badges are simple, lightweight components that can be placed inside headings, buttons, or next to text elements.

## Key Bullet Points

- **`.badge`**: Base class for badge styling
- **Color Variants**: badge-primary, badge-secondary, badge-success, badge-danger, badge-warning, badge-info
- **Pill Badges**: Use `.rounded-pill` for fully rounded (pill-shaped) badges
- **Badges in Headings**: Commonly used with h1-h6 tags
- **Badges in Buttons**: Show notification counts or status
- **Contextual Colors**: Same color system as alerts and buttons

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Badges</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Badge Examples</h2>

    <!-- BASIC BADGES -->
    <h5>Basic Badges</h5>
    <p>
        This is a <span class="badge bg-primary">primary</span> badge.
    </p>
    <p>
        This is a <span class="badge bg-secondary">secondary</span> badge.
    </p>
    <p>
        This is a <span class="badge bg-success">success</span> badge.
    </p>
    <p>
        This is a <span class="badge bg-danger">danger</span> badge.
    </p>
    <p>
        This is a <span class="badge bg-warning text-dark">warning</span> badge.
    </p>
    <p>
        This is a <span class="badge bg-info text-dark">info</span> badge.
    </p>

    <hr>

    <!-- PILL BADGES -->
    <h5>Pill Badges (.rounded-pill)</h5>
    <p>
        <span class="badge rounded-pill bg-primary">Primary</span>
        <span class="badge rounded-pill bg-secondary">Secondary</span>
        <span class="badge rounded-pill bg-success">Success</span>
        <span class="badge rounded-pill bg-danger">Danger</span>
        <span class="badge rounded-pill bg-warning text-dark">Warning</span>
        <span class="badge rounded-pill bg-info text-dark">Info</span>
    </p>

    <hr>

    <!-- BADGES IN HEADINGS -->
    <h5>Badges in Headings</h5>
    <h1>Example heading <span class="badge bg-secondary">New</span></h1>
    <h2>Example heading <span class="badge bg-secondary">New</span></h2>
    <h3>Example heading <span class="badge bg-secondary">New</span></h3>
    <h4>Example heading <span class="badge bg-secondary">New</span></h4>
    <h5>Example heading <span class="badge bg-secondary">New</span></h5>
    <h6>Example heading <span class="badge bg-secondary">New</span></h6>

    <hr>

    <!-- BADGES IN BUTTONS -->
    <h5>Badges in Buttons</h5>
    <div class="mb-4">
        <button type="button" class="btn btn-primary">
            Notifications <span class="badge bg-light text-dark">3</span>
        </button>
        <button type="button" class="btn btn-success">
            Messages <span class="badge bg-light text-dark">5</span>
        </button>
        <button type="button" class="btn btn-danger">
            Alerts <span class="badge bg-light text-dark">!</span>
        </button>
    </div>

    <hr>

    <!-- STATUS BADGES -->
    <h5>Status Badges</h5>
    <div class="mb-4">
        <span class="badge bg-success">Online</span>
        <span class="badge bg-secondary">Offline</span>
        <span class="badge bg-warning text-dark">Away</span>
        <span class="badge bg-danger">Busy</span>
    </div>

    <!-- CATEGORY TAGS -->
    <h5>Category Tags</h5>
    <div class="mb-4">
        <span class="badge bg-primary">Technology</span>
        <span class="badge bg-success">Science</span>
        <span class="badge bg-info text-dark">Education</span>
        <span class="badge bg-warning text-dark">Business</span>
        <span class="badge bg-danger">Health</span>
        <span class="badge bg-secondary">Sports</span>
    </div>

    <!-- CONTEXTUAL BADGES -->
    <h5>List Group with Badges</h5>
    <ul class="list-group mb-4">
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Inbox messages
            <span class="badge bg-primary rounded-pill">12</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Unread emails
            <span class="badge bg-warning rounded-pill">5</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Pending tasks
            <span class="badge bg-danger rounded-pill">3</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Completed items
            <span class="badge bg-success rounded-pill">25</span>
        </li>
    </ul>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.badge`**: Base class providing padding, font-size, font-weight, and background color
- **`.bg-primary`**: Sets background color to primary blue
- **`.bg-secondary`**: Sets background color to gray
- **`.bg-success`**: Sets background color to green
- **`.bg-danger`**: Sets background color to red
- **`.bg-warning`**: Sets background color to yellow - use `.text-dark` for readable text
- **`.bg-info`**: Sets background color to cyan - use `.text-dark` for readable text
- **`.rounded-pill`**: Makes badge fully rounded (pill shape) with larger border-radius
- **`.text-dark`**: Adds dark text color for better contrast on light backgrounds
- **`.d-flex`**: Display flex - enables flexible layout
- **`.justify-content-between`**: Spaces items evenly with space between them
- **`.align-items-center`**: Vertically centers items

## Expected Visual Result

The page displays multiple badge examples:

1. **Basic Badges**: Small colored labels inline with text - blue, gray, green, red, yellow, cyan

2. **Pill Badges**: Fully rounded badges that look like pills/capsules - same colors but longer and rounded

3. **Badges in Headings**: H1 through H6 headings each with a "New" badge next to them showing the badge scales with heading size

4. **Badges in Buttons**: Buttons that contain badge counts showing notification numbers

5. **Status Badges**: Small status indicators showing Online/Offline/Away/Busy states

6. **Category Tags**: Multiple colored tags representing categories like Technology, Science, etc.

7. **List with Badges**: A list where each item has a badge on the right showing counts (like email inbox)
