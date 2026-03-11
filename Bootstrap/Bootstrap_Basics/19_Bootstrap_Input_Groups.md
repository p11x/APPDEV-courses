# Bootstrap Input Groups

## Definition

Bootstrap input groups allow you to extend form controls by adding text, buttons, or button groups before or after the input. This is useful for creating components like search bars with search icons, input fields with currency symbols, or form inputs with add-on buttons. The `.input-group` class wraps the input and add-on elements.

## Key Bullet Points

- **`.input-group`**: Container for input group
- **`.input-group-text`**: Text addon (icon, symbol, label)
- **Prefix/Suffix**: Place addon before (prefix) or after (suffix) the input
- **Input Sizes**: input-group-sm, input-group-lg
- **Multiple Addons**: Can have multiple addons on each side
- **Buttons in Input Groups**: Add buttons before or after inputs
- **Checkboxes/Radios**: Can include checkboxes or radios in addon

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Input Groups</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Input Group Examples</h2>

    <!-- BASIC INPUT GROUP -->
    <h5>Basic Input Group</h5>
    <div class="input-group mb-3" style="max-width: 400px;">
        <span class="input-group-text" id="basic-addon1">@</span>
        <input type="text" class="form-control" placeholder="Username" aria-label="Username" aria-describedby="basic-addon1">
    </div>
    <div class="input-group mb-4" style="max-width: 400px;">
        <input type="text" class="form-control" placeholder="Recipient's username" aria-label="Recipient's username" aria-describedby="basic-addon2">
        <span class="input-group-text" id="basic-addon2">@example.com</span>
    </div>

    <!-- WITH LABELS -->
    <h5>Input Group with Labels</h5>
    <label for="basic-url" class="form-label">Your vanity URL</label>
    <div class="input-group mb-3" style="max-width: 400px;">
        <span class="input-group-text" id="basic-addon3">https://example.com/users/</span>
        <input type="text" class="form-control" id="basic-url" aria-describedby="basic-addon3">
    </div>

    <!-- CURRENCY EXAMPLE -->
    <h5>Currency Input</h5>
    <div class="input-group mb-4" style="max-width: 300px;">
        <span class="input-group-text">$</span>
        <input type="number" class="form-control" aria-label="Amount (to the nearest dollar)">
        <span class="input-group-text">.00</span>
    </div>

    <!-- TEXTAREA INPUT GROUP -->
    <h5>Textarea with Input Group</h5>
    <div class="input-group mb-4" style="max-width: 500px;">
        <span class="input-group-text">With textarea</span>
        <textarea class="form-control" aria-label="With textarea"></textarea>
    </div>

    <!-- CHECKBOX INPUT GROUP -->
    <h5>Checkbox Input Group</h5>
    <div class="input-group mb-3" style="max-width: 400px;">
        <div class="input-group-text">
            <input class="form-check-input mt-0" type="checkbox" aria-label="Checkbox for following text input">
        </div>
        <input type="text" class="form-control" aria-label="Text input with checkbox">
    </div>

    <!-- BUTTON INPUT GROUP -->
    <h5>Button Input Groups</h5>
    <div class="input-group mb-3" style="max-width: 400px;">
        <button class="btn btn-outline-secondary" type="button" id="button-addon1">Button</button>
        <input type="text" class="form-control" placeholder="" aria-label="Example text with button addon" aria-describedby="button-addon1">
    </div>
    <div class="input-group mb-3" style="max-width: 400px;">
        <input type="text" class="form-control" placeholder="Recipient's username" aria-label="Recipient's username" aria-describedby="button-addon2">
        <button class="btn btn-outline-secondary" type="button" id="button-addon2">Button</button>
    </div>
    <div class="input-group mb-4" style="max-width: 400px;">
        <button class="btn btn-outline-primary" type="button">Button</button>
        <input type="text" class="form-control" placeholder="" aria-label="Example text with two button addons">
        <button class="btn btn-outline-primary" type="button">Button</button>
    </div>

    <!-- DROPDOWN INPUT GROUP -->
    <h5>Dropdown Input Group</h5>
    <div class="input-group mb-3" style="max-width: 400px;">
        <button class="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">Dropdown</button>
        <ul class="dropdown-menu">
            <li><a class="dropdown-item" href="#">Action</a></li>
            <li><a class="dropdown-item" href="#">Another action</a></li>
            <li><a class="dropdown-item" href="#">Something else</a></li>
        </ul>
        <input type="text" class="form-control" aria-label="Text input with dropdown button">
    </div>

    <!-- INPUT GROUP SIZES -->
    <h5>Input Group Sizes</h5>
    <div class="input-group input-group-sm mb-2" style="max-width: 400px;">
        <span class="input-group-text">Small</span>
        <input type="text" class="form-control">
    </div>
    <div class="input-group mb-2" style="max-width: 400px;">
        <span class="input-group-text">Default</span>
        <input type="text" class="form-control">
    </div>
    <div class="input-group input-group-lg mb-4" style="max-width: 400px;">
        <span class="input-group-text">Large</span>
        <input type="text" class="form-control">
    </div>

    <!-- SEGMENTED BUTTONS -->
    <h5>Segmented Buttons</h5>
    <div class="input-group mb-4" style="max-width: 400px;">
        <button type="button" class="btn btn-outline-secondary">Action</button>
        <button type="button" class="btn btn-outline-secondary dropdown-toggle dropdown-toggle-split" data-bs-toggle="dropdown">
            <span class="visually-hidden">Toggle Dropdown</span>
        </button>
        <ul class="dropdown-menu">
            <li><a class="dropdown-item" href="#">Action</a></li>
            <li><a class="dropdown-item" href="#">Another action</a></li>
        </ul>
        <input type="text" class="form-control" aria-label="Text input with segmented dropdown button">
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.input-group`**: Wrapper that contains the input and addons - uses flexbox
- **`.input-group-text`**: Container for the prefix/suffix text or icon
- **`.form-control`**: The actual input field
- **`.input-group-sm`**: Makes entire input group smaller
- **`.input-group-lg`**: Makes entire input group larger
- **`.form-check-input`**: Checkbox within input group
- **`.mt-0`**: Removes top margin from checkbox
- **Button in input group**: Works like text addon but with button styling
- **Dropdown in input group**: Combines dropdown with input field

## Expected Visual Result

The page displays multiple input group examples:

1. **Basic Input Groups**: Username field with @ prefix, email field with @example.com suffix

2. **URL Input**: Website URL with prefix showing the base URL

3. **Currency Input**: Dollar amount with $ prefix and .00 suffix

4. **Textarea**: Large text area with label prefix

5. **Checkbox Input Group**: Checkbox on the left side of text input

6. **Button Input Groups**: 
   - Button before input
   - Button after input
   - Buttons on both sides

7. **Dropdown Input Group**: Dropdown button followed by input field

8. **Sizes**: Small, default, and large input groups showing different heights

9. **Segmented Buttons**: Split button with dropdown combined with input

All examples show the addon elements seamlessly connected to the input field.
