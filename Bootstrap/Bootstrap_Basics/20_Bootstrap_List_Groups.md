# Bootstrap List Groups

## Definition

Bootstrap list groups are flexible components for displaying sequences of content. They can be used for simple lists with basic styling, or enhanced with badges, links, or buttons. List groups are useful for sidebars, navigation menus, or displaying collections of related items like notifications or activity feeds.

## Key Bullet Points

- **`.list-group`**: Base container for list group
- **`.list-group-item`**: Individual item in the list
- **`.list-group-item-action`**: Makes item clickable with hover effects
- **`.active`**: Highlights the active/selected item
- **`.disabled`**: Visually disables the item
- **`.list-group-item-*`**: Contextual color variants
- **Badges/Badges in List Groups**: Add badges to show counts

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap List Groups</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap List Group Examples</h2>

    <!-- BASIC LIST GROUP -->
    <h5>Basic List Group</h5>
    <ul class="list-group mb-4" style="max-width: 400px;">
        <li class="list-group-item">First item</li>
        <li class="list-group-item">Second item</li>
        <li class="list-group-item">Third item</li>
        <li class="list-group-item">Fourth item</li>
        <li class="list-group-item">Fifth item</li>
    </ul>

    <!-- ACTIVE AND DISABLED -->
    <h5>Active and Disabled Items</h5>
    <ul class="list-group mb-4" style="max-width: 400px;">
        <li class="list-group-item active">Active item</li>
        <li class="list-group-item">Second item</li>
        <li class="list-group-item">Third item</li>
        <li class="list-group-item disabled">Disabled item</li>
        <li class="list-group-item">Fifth item</li>
    </ul>

    <!-- LINK ITEMS -->
    <h5>List Group with Links</h5>
    <div class="list-group mb-4" style="max-width: 400px;">
        <a href="#" class="list-group-item list-group-item-action active">Active link</a>
        <a href="#" class="list-group-item list-group-item-action">Second link</a>
        <a href="#" class="list-group-item list-group-item-action">Third link</a>
        <a href="#" class="list-group-item list-group-item-action disabled">Disabled link</a>
    </div>

    <!-- CONTEXTUAL COLORS -->
    <h5>Contextual Colors</h5>
    <ul class="list-group mb-4" style="max-width: 400px;">
        <li class="list-group-item">Default item</li>
        <li class="list-group-item list-group-item-primary">Primary item</li>
        <li class="list-group-item list-group-item-secondary">Secondary item</li>
        <li class="list-group-item list-group-item-success">Success item</li>
        <li class="list-group-item list-group-item-danger">Danger item</li>
        <li class="list-group-item list-group-item-warning">Warning item</li>
        <li class="list-group-item list-group-item-info">Info item</li>
    </ul>

    <!-- WITH BADGES -->
    <h5>List Group with Badges</h5>
    <ul class="list-group mb-4" style="max-width: 400px;">
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Inbox messages
            <span class="badge bg-primary rounded-pill">12</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Sent messages
            <span class="badge bg-secondary rounded-pill">5</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Drafts
            <span class="badge bg-warning text-dark rounded-pill">8</span>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Spam
            <span class="badge bg-danger rounded-pill">3</span>
        </li>
    </ul>

    <!-- CUSTOM CONTENT -->
    <h5>Custom Content</h5>
    <div class="list-group mb-4" style="max-width: 500px;">
        <a href="#" class="list-group-item list-group-item-action active">
            <div class="d-flex w-100 justify-content-between">
                <h5 class="mb-1">List group item heading</h5>
                <small>3 days ago</small>
            </div>
            <p class="mb-1">Some placeholder content in a paragraph.</p>
            <small>And some small print.</small>
        </a>
        <a href="#" class="list-group-item list-group-item-action">
            <div class="d-flex w-100 justify-content-between">
                <h5 class="mb-1">List group item heading</h5>
                <small class="text-muted">3 days ago</small>
            </div>
            <p class="mb-1">Some placeholder content in a paragraph.</p>
            <small class="text-muted">And some small print.</small>
        </a>
    </div>

    <!-- NUMBERED LIST -->
    <h5>Numbered List</h5>
    <ol class="list-group list-group-numbered mb-4" style="max-width: 400px;">
        <li class="list-group-item">First item</li>
        <li class="list-group-item">Second item</li>
        <li class="list-group-item">Third item</li>
    </ol>

    <!-- HORIZONTAL LIST -->
    <h5>Horizontal List (all breakpoints)</h5>
    <ul class="list-group list-group-horizontal mb-4">
        <li class="list-group-item">First</li>
        <li class="list-group-item">Second</li>
        <li class="list-group-item">Third</li>
    </ul>
    <ul class="list-group list-group-horizontal-sm mb-4">
        <li class="list-group-item">First</li>
        <li class="list-group-item">Second</li>
        <li class="list-group-item">Third</li>
    </ul>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.list-group`**: Container for list group items
- **`.list-group-item`**: Individual item in the list
- **`.list-group-item-action`**: Makes items clickable with hover/active states
- **`.active`**: Highlights selected item with primary color
- **`.disabled`**: Grayed out and non-clickable
- **`.list-group-item-primary`**: Primary colored item
- **`.list-group-item-success`**: Success colored item
- **`.list-group-item-danger`**: Danger colored item
- **`.d-flex`**: Flexbox for layout
- **`.justify-content-between`**: Space between content and badge
- **`.align-items-center`**: Vertically center content
- **`.list-group-numbered`**: Adds numbers to items
- **`.list-group-horizontal`**: Displays items horizontally instead of vertically
- **`.rounded-pill`**: Makes badge pill-shaped

## Expected Visual Result

The page displays multiple list group examples:

1. **Basic List**: Simple vertical list with bordered items

2. **Active/Disabled**: One active (highlighted) item and one disabled (grayed out) item

3. **Link Items**: Clickable items with hover effects - one active

4. **Contextual Colors**: List items in different colors (primary, secondary, success, danger, warning, info)

5. **With Badges**: List items showing counts with badges on the right side

6. **Custom Content**: Rich content with heading, paragraph, and small text

7. **Numbered List**: Ordered list with numbers

8. **Horizontal Lists**: Items displayed horizontally instead of vertically

All list groups have consistent styling with proper padding, borders, and hover effects.
