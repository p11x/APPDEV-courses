# Bootstrap Forms

## Definition

Bootstrap forms provide styled form controls (inputs, selects, checkboxes, radios) that look consistent across browsers. The `.form-control` class adds proper styling, focus states, and features to HTML form elements. Bootstrap forms support various input types and layouts including horizontal forms, inline forms, and grid-based forms.

## Key Bullet Points

- **`.form-control`**: Base class for text inputs, textareas, and selects
- **Input Types**: text, email, password, number, date, file, color
- **`.form-label`**: Styled label for form controls
- **`.form-text`**: Help text below inputs
- **Checkboxes/Radios**: `.form-check` wrapper, `.form-check-input`, `.form-check-label
- **Select**: `.form-select` for dropdown selects
- **Grid Layout**: Use row and col for horizontal form layouts

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Forms</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Form Examples</h2>

    <!-- BASIC FORM -->
    <h5>Basic Form</h5>
    <form class="mb-4" style="max-width: 400px;">
        <div class="mb-3">
            <label for="exampleInputEmail1" class="form-label">Email address</label>
            <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
            <div class="form-text">We'll never share your email with anyone else.</div>
        </div>
        <div class="mb-3">
            <label for="exampleInputPassword1" class="form-label">Password</label>
            <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password">
        </div>
        <div class="mb-3 form-check">
            <input type="checkbox" class="form-check-input" id="exampleCheck1">
            <label class="form-check-label" for="exampleCheck1">Check me out</label>
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>
    </form>

    <hr>

    <!-- SELECT INPUT -->
    <h5>Select Dropdown</h5>
    <form class="mb-4" style="max-width: 400px;">
        <label for="exampleSelect" class="form-label">Select Option</label>
        <select class="form-select mb-3" id="exampleSelect">
            <option selected>Open this select menu</option>
            <option value="1">Option 1</option>
            <option value="2">Option 2</option>
            <option value="3">Option 3</option>
        </select>
        
        <label for="multipleSelect" class="form-label">Multiple Select</label>
        <select class="form-select" id="multipleSelect" multiple>
            <option value="1">Option 1</option>
            <option value="2">Option 2</option>
            <option value="3">Option 3</option>
            <option value="4">Option 4</option>
        </select>
    </form>

    <hr>

    <!-- TEXTAREA -->
    <h5>Textarea</h5>
    <form class="mb-4" style="max-width: 400px;">
        <label for="exampleTextarea" class="form-label">Message</label>
        <textarea class="form-control" id="exampleTextarea" rows="4" placeholder="Enter your message"></textarea>
    </form>

    <hr>

    <!-- CHECKBOXES AND RADIOS -->
    <h5>Checkboxes and Radios</h5>
    <form class="mb-4" style="max-width: 400px;">
        <h6>Checkboxes</h6>
        <div class="form-check">
            <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault">
            <label class="form-check-label" for="flexCheckDefault">Default checkbox</label>
        </div>
        <div class="form-check">
            <input class="form-check-input" type="checkbox" value="" id="flexCheckChecked" checked>
            <label class="form-check-label" for="flexCheckChecked">Checked checkbox</label>
        </div>
        <div class="form-check">
            <input class="form-check-input" type="checkbox" value="" id="flexCheckDisabled" disabled>
            <label class="form-check-label" for="flexCheckDisabled">Disabled checkbox</label>
        </div>

        <h6 class="mt-3">Radios</h6>
        <div class="form-check">
            <input class="form-check-input" type="radio" name="flexRadioDefault" id="flexRadioDefault1">
            <label class="form-check-label" for="flexRadioDefault1">Default radio</label>
        </div>
        <div class="form-check">
            <input class="form-check-input" type="radio" name="flexRadioDefault" id="flexRadioDefault2" checked>
            <label class="form-check-label" for="flexRadioDefault2">Default checked radio</label>
        </div>

        <h6 class="mt-3">Inline</h6>
        <div class="form-check form-check-inline">
            <input class="form-check-input" type="checkbox" id="inlineCheckbox1" value="option1">
            <label class="form-check-label" for="inlineCheckbox1">1</label>
        </div>
        <div class="form-check form-check-inline">
            <input class="form-check-input" type="checkbox" id="inlineCheckbox2" value="option2">
            <label class="form-check-label" for="inlineCheckbox2">2</label>
        </div>
    </form>

    <hr>

    <!-- GRID-BASED FORM -->
    <h5>Grid-Based Form</h5>
    <form class="mb-4">
        <div class="row">
            <div class="col">
                <label class="form-label">First Name</label>
                <input type="text" class="form-control" placeholder="First name">
            </div>
            <div class="col">
                <label class="form-label">Last Name</label>
                <input type="text" class="form-control" placeholder="Last name">
            </div>
        </div>
    </form>

    <hr>

    <!-- FLOATING LABELS -->
    <h5>Floating Labels</h5>
    <form class="mb-4" style="max-width: 400px;">
        <div class="form-floating mb-3">
            <input type="email" class="form-control" id="floatingInput" placeholder="name@example.com">
            <label for="floatingInput">Email address</label>
        </div>
        <div class="form-floating mb-3">
            <input type="password" class="form-control" id="floatingPassword" placeholder="Password">
            <label for="floatingPassword">Password</label>
        </div>
        <div class="form-floating">
            <textarea class="form-control" placeholder="Leave a comment" id="floatingTextarea" style="height: 100px"></textarea>
            <label for="floatingTextarea">Comments</label>
        </div>
    </form>

    <!-- FILE INPUT -->
    <h5>File Input</h5>
    <form class="mb-4" style="max-width: 400px;">
        <div class="mb-3">
            <label for="formFile" class="form-label">Default file input</label>
            <input class="form-control" type="file" id="formFile">
        </div>
        <div class="mb-3">
            <label for="formFileMultiple" class="form-label">Multiple files</label>
            <input class="form-control" type="file" id="formFileMultiple" multiple>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.form-control`**: Styled input field with padding, border, focus state
- **`.form-label`**: Label styled to align with form controls
- **`.form-text`**: Small help text displayed below input
- **`.form-check`**: Wrapper for checkboxes/radios
- **`.form-check-input`**: Styled checkbox/radio button
- **`.form-check-label`**: Label for checkbox/radio
- **`.form-select`**: Styled dropdown select
- **`.form-floating`**: Creates floating label effect where label moves above input when focused
- **`.form-control-lg` / `.form-control-sm`**: Size variations for inputs
- **`disabled`**: Disables the input, makes it non-editable
- **`placeholder`**: Placeholder text shown when empty
- **`multiple`**: Allows multiple selections in select

## Expected Visual Result

The page displays multiple form examples:

1. **Basic Form**: Email input with help text, password input, checkbox, and submit button

2. **Select Dropdown**: Single select and multiple select dropdown examples

3. **Textarea**: Multi-line text input with 4 rows

4. **Checkboxes and Radios**: Single checkboxes, checked checkboxes, disabled checkboxes, radio buttons, inline checkboxes

5. **Grid Form**: Two-column layout for first name and last name

6. **Floating Labels**: Modern form where labels animate/float above input when focused

7. **File Input**: File upload inputs for single and multiple files

All form controls have proper styling, focus states (blue outline when clicked), and consistent appearance.
