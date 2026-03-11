# Bootstrap Button Groups

## Definition

Button groups allow you to group multiple buttons together on a single line, creating cohesive toolbar-style interfaces. The `.btn-group` wrapper makes buttons appear connected as a single unit with shared borders. This is useful for grouping related actions like text formatting (bold, italic, underline) or navigation controls.

## Key Bullet Points

- **`.btn-group`**: Container that groups buttons together horizontally
- **`.btn-group-vertical`**: Stacks buttons vertically instead of horizontally
- **Button Toolbar**: Combine multiple button groups with `.btn-toolbar`
- **Size Classes**: btn-group-sm, btn-group-lg for sizing entire groups
- **Nesting**: Button groups can be nested within each other for dropdown menus
- **Justified**: Use btn-group-justified for equal-width buttons (requires `<a>` tags)

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Button Groups</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Button Groups</h2>

    <!-- BASIC BUTTON GROUP -->
    <h5>Basic Button Group (.btn-group)</h5>
    <div class="btn-group mb-4" role="group" aria-label="Basic example">
        <button type="button" class="btn btn-primary">Left</button>
        <button type="button" class="btn btn-primary">Middle</button>
        <button type="button" class="btn btn-primary">Right</button>
    </div>

    <!-- OUTLINE BUTTONS IN GROUP -->
    <h5>Outline Button Group</h5>
    <div class="btn-group mb-4" role="group" aria-label="Outline example">
        <button type="button" class="btn btn-outline-primary">One</button>
        <button type="button" class="btn btn-outline-primary">Two</button>
        <button type="button" class="btn btn-outline-primary">Three</button>
    </div>

    <!-- MIXED COLORS -->
    <h5>Mixed Colors in Group</h5>
    <div class="btn-group mb-4" role="group" aria-label="Mixed colors">
        <button type="button" class="btn btn-success">Approve</button>
        <button type="button" class="btn btn-warning">Review</button>
        <button type="button" class="btn btn-danger">Reject</button>
    </div>

    <hr>

    <!-- BUTTON GROUP SIZES -->
    <h5>Button Group Sizes</h5>
    <div class="mb-2">
        <strong>Small:</strong>
        <div class="btn-group btn-group-sm" role="group">
            <button class="btn btn-secondary">S</button>
            <button class="btn btn-secondary">M</button>
            <button class="btn btn-secondary">L</button>
        </div>
    </div>
    <div class="mb-2">
        <strong>Default:</strong>
        <div class="btn-group" role="group">
            <button class="btn btn-secondary">S</button>
            <button class="btn btn-secondary">M</button>
            <button class="btn btn-secondary">L</button>
        </div>
    </div>
    <div class="mb-4">
        <strong>Large:</strong>
        <div class="btn-group btn-group-lg" role="group">
            <button class="btn btn-secondary">S</button>
            <button class="btn btn-secondary">M</button>
            <button class="btn btn-secondary">L</button>
        </div>
    </div>

    <hr>

    <!-- VERTICAL BUTTON GROUP -->
    <h5>Vertical Button Group (.btn-group-vertical)</h5>
    <div class="btn-group-vertical mb-4" role="group" aria-label="Vertical button group">
        <button type="button" class="btn btn-primary">Top</button>
        <button type="button" class="btn btn-primary">Middle</button>
        <button type="button" class="btn btn-primary">Bottom</button>
    </div>

    <hr>

    <!-- BUTTON TOOLBAR -->
    <h5>Button Toolbar (.btn-toolbar)</h5>
    <div class="btn-toolbar mb-4" role="toolbar" aria-label="Toolbar with button groups">
        <div class="btn-group me-2" role="group" aria-label="First group">
            <button type="button" class="btn btn-secondary">1</button>
            <button type="button" class="btn btn-secondary">2</button>
            <button type="button" class="btn btn-secondary">3</button>
            <button type="button" class="btn btn-secondary">4</button>
        </div>
        <div class="btn-group me-2" role="group" aria-label="Second group">
            <button type="button" class="btn btn-secondary">5</button>
            <button type="button" class="btn btn-secondary">6</button>
            <button type="button" class="btn btn-secondary">7</button>
        </div>
        <div class="btn-group" role="group" aria-label="Third group">
            <button type="button" class="btn btn-secondary">8</button>
        </div>
    </div>

    <!-- CHECKBOX BUTTON GROUP -->
    <h5>Checkbox Button Group</h5>
    <div class="btn-group mb-4" role="group" aria-label="Checkbox button group">
        <input type="checkbox" class="btn-check" id="btncheck1" autocomplete="off">
        <label class="btn btn-outline-primary" for="btncheck1">Checkbox 1</label>
        
        <input type="checkbox" class="btn-check" id="btncheck2" autocomplete="off">
        <label class="btn btn-outline-primary" for="btncheck2">Checkbox 2</label>
        
        <input type="checkbox" class="btn-check" id="btncheck3" autocomplete="off">
        <label class="btn btn-outline-primary" for="btncheck3">Checkbox 3</label>
    </div>

    <!-- RADIO BUTTON GROUP -->
    <h5>Radio Button Group</h5>
    <div class="btn-group mb-4" role="group" aria-label="Radio button group">
        <input type="radio" class="btn-check" name="btnradio" id="btnradio1" autocomplete="off" checked>
        <label class="btn btn-outline-danger" for="btnradio1">Radio 1</label>
        
        <input type="radio" class="btn-check" name="btnradio" id="btnradio2" autocomplete="off">
        <label class="btn btn-outline-danger" for="btnradio2">Radio 2</label>
        
        <input type="radio" class="btn-check" name="btnradio" id="btnradio3" autocomplete="off">
        <label class="btn btn-outline-danger" for="btnradio3">Radio 3</label>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.btn-group`**: Container that groups buttons horizontally, removing individual button borders between them for a unified look
- **`.btn-group-vertical`**: Stacks buttons vertically, useful for sidebar toolbars
- **`.btn-toolbar`**: Container for multiple button groups, useful for complex toolbars
- **`role="group"`**: ARIA attribute for accessibility - tells screen readers this is a group of buttons
- **`aria-label`**: ARIA attribute providing a label for the group
- **`.btn-group-sm`**: Makes the entire button group smaller
- **`.btn-group-lg`**: Makes the entire button group larger
- **`.me-2`**: Adds margin-end (right margin) spacing between groups
- **`.btn-check`**: Hidden checkbox/radio input for checkbox/radio button groups
- **`.btn-check` + `<label>`**: Pattern for making buttons work as checkboxes or radio buttons

## Expected Visual Result

The page displays multiple button group examples:

1. **Basic Button Group**: Three blue buttons connected as one unit with shared borders

2. **Outline Button Group**: Three outline buttons grouped together

3. **Mixed Colors**: Three buttons in success/warning/danger colors grouped together

4. **Size Variations**: Three sets of buttons showing small, default, and large sizes

5. **Vertical Group**: Three buttons stacked vertically instead of horizontally

6. **Button Toolbar**: Multiple button groups combined in a toolbar layout with number buttons

7. **Checkbox Group**: Three toggle buttons that can be checked individually

8. **Radio Group**: Three toggle buttons where only one can be selected at a time
