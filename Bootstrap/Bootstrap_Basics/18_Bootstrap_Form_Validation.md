# Bootstrap Form Validation

## Definition

Bootstrap provides visual feedback for form validation through CSS classes that indicate valid or invalid states. The `.is-valid` and `.is-invalid` classes add green (success) or red (error) styling to form controls. Combined with validation feedback messages, users can see which fields are correctly or incorrectly filled before submitting.

## Key Bullet Points

- **`.is-valid`**: Shows valid/green styling on input
- **`.is-invalid`**: Shows invalid/red styling on input
- **`.valid-feedback`**: Success message for valid inputs
- **`.invalid-feedback`**: Error message for invalid inputs
- **`.was-validated`**: Parent class indicating form has been validated
- **HTML5 Validation**: Works with required, minlength, pattern attributes
- **Tooltip Style**: Use `.valid-tooltip` for tooltip-style messages

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Form Validation</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Form Validation</h2>

    <!-- VALIDATION STATES -->
    <h5>Validation States</h5>
    <form class="mb-4" style="max-width: 400px;">
        <div class="mb-3">
            <label for="validationDefault01" class="form-label">Valid Input</label>
            <input type="text" class="form-control is-valid" id="validationDefault01" value="Correct value" required>
            <div class="valid-feedback">Looks good!</div>
        </div>
        <div class="mb-3">
            <label for="validationDefault02" class="form-label">Invalid Input</label>
            <input type="text" class="form-control is-invalid" id="validationDefault02" value="Wrong value">
            <div class="invalid-feedback">Please provide a valid input.</div>
        </div>
    </form>

    <hr>

    <!-- COMPLETE VALIDATED FORM -->
    <h5>Complete Validated Form</h5>
    <form class="row g-3 needs-validation" novalidate style="max-width: 500px;">
        <div class="col-md-6">
            <label for="validationCustom01" class="form-label">First name</label>
            <input type="text" class="form-control" id="validationCustom01" required>
            <div class="valid-feedback">Looks good!</div>
            <div class="invalid-feedback">Please enter your first name.</div>
        </div>
        <div class="col-md-6">
            <label for="validationCustom02" class="form-label">Last name</label>
            <input type="text" class="form-control" id="validationCustom02" required>
            <div class="valid-feedback">Looks good!</div>
            <div class="invalid-feedback">Please enter your last name.</div>
        </div>
        <div class="col-md-12">
            <label for="validationCustomEmail" class="form-label">Email</label>
            <input type="email" class="form-control" id="validationCustomEmail" required>
            <div class="valid-feedback">Looks good!</div>
            <div class="invalid-feedback">Please enter a valid email.</div>
        </div>
        <div class="col-md-6">
            <label for="validationCustom03" class="form-label">City</label>
            <input type="text" class="form-control" id="validationCustom03" required>
            <div class="invalid-feedback">Please provide a valid city.</div>
        </div>
        <div class="col-md-3">
            <label for="validationCustom04" class="form-label">State</label>
            <select class="form-select" id="validationCustom04" required>
                <option selected disabled value="">Choose...</option>
                <option>California</option>
                <option>New York</option>
                <option>Texas</option>
            </select>
            <div class="invalid-feedback">Please select a state.</div>
        </div>
        <div class="col-md-3">
            <label for="validationCustom05" class="form-label">Zip</label>
            <input type="text" class="form-control" id="validationCustom05" required>
            <div class="invalid-feedback">Please provide a valid zip.</div>
        </div>
        <div class="col-12">
            <div class="form-check">
                <input class="form-check-input" type="checkbox" value="" id="invalidCheck" required>
                <label class="form-check-label" for="invalidCheck">Agree to terms and conditions</label>
                <div class="invalid-feedback">You must agree before submitting.</div>
            </div>
        </div>
        <div class="col-12">
            <button class="btn btn-primary" type="submit">Submit Form</button>
        </div>
    </form>

    <hr>

    <!-- TOOLTIP STYLE VALIDATION -->
    <h5>Tooltip Style Validation</h5>
    <form class="row g-3 needs-validation" novalidate style="max-width: 500px;">
        <div class="col-md-6 position-relative">
            <label for="tooltipValidation01" class="form-label">First name</label>
            <input type="text" class="form-control" id="tooltipValidation01" required>
            <div class="valid-tooltip">Looks good!</div>
            <div class="invalid-tooltip">Please enter first name.</div>
        </div>
        <div class="col-md-6 position-relative">
            <label for="tooltipValidation02" class="form-label">Last name</label>
            <input type="text" class="form-control" id="tooltipValidation02" required>
            <div class="valid-tooltip">Looks good!</div>
            <div class="invalid-tooltip">Please enter last name.</div>
        </div>
        <div class="col-12">
            <button class="btn btn-primary" type="submit">Submit</button>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // JavaScript to enable form validation
        (function () {
            'use strict'
            var forms = document.querySelectorAll('.needs-validation')
            Array.prototype.slice.call(forms)
                .forEach(function (form) {
                    form.addEventListener('submit', function (event) {
                        if (!form.checkValidity()) {
                            event.preventDefault()
                            event.stopPropagation()
                        }
                        form.classList.add('was-validated')
                    }, false)
                })
        })()
    </script>
</body>
</html>
```

## Component Explanation

- **`.is-valid`**: Adds green border and checkmark icon to input
- **`.is-invalid`**: Adds red border and X icon to input
- **`.valid-feedback`**: Green success message, visible when input is valid
- **`.invalid-feedback`**: Red error message, visible when input is invalid
- **`.was-validated`**: Added to form after submission attempt - shows all feedback messages
- **`.valid-tooltip`**: Success message styled as tooltip
- **`.invalid-tooltip`**: Error message styled as tooltip
- **`.needs-validation`**: Custom class to mark form for JavaScript validation
- **`required`**: HTML5 attribute making field mandatory
- **`novalidate`**: Disables browser's default validation to use Bootstrap styling
- **`position-relative`**: Required for tooltip positioning

## Expected Visual Result

The page displays three form validation examples:

1. **Validation States**: Shows inputs with pre-applied valid (green) and invalid (red) states with corresponding feedback messages

2. **Complete Validated Form**: A full registration-style form with multiple fields. When submitted empty:
   - Valid fields show green borders and "Looks good!" message
   - Invalid fields show red borders and specific error messages
   - Checkbox requires agreement

3. **Tooltip Style**: Similar form but feedback messages appear as floating tooltips instead of inline text

When you click submit without filling required fields, the form shows red borders on invalid fields with error messages explaining what's wrong. Valid fields show green checkmarks.
