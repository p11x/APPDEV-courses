# Bootstrap Alerts

## Definition

Bootstrap alerts are styled message boxes used to communicate important information to users. They come in contextual color variants (success, info, warning, danger) that convey the type of message being displayed. Alerts can also be dismissible, meaning users can close them by clicking an X button.

## Key Bullet Points

- **`.alert`**: Base class for alert styling
- **Color Variants**: alert-primary, alert-success, alert-info, alert-warning, alert-danger, alert-secondary, alert-light, alert-dark
- **`.alert-heading`**: Styles the heading inside an alert
- **Dismissible Alerts**: Add `.alert-dismissible` and a close button for dismissible alerts
- **Fade/Show**: Use `.fade .show` classes for smooth dismiss animation
- **Link Color**: Use `.alert-link` for links inside alerts that match alert color

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Alerts</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Alert Examples</h2>

    <!-- BASIC ALERTS -->
    <h5>Basic Alert Colors</h5>
    <div class="alert alert-primary" role="alert">
        This is a primary alert - basic information message!
    </div>
    <div class="alert alert-secondary" role="alert">
        This is a secondary alert - less prominent message!
    </div>
    <div class="alert alert-success" role="alert">
        This is a success alert - something went well!
    </div>
    <div class="alert alert-danger" role="alert">
        This is a danger alert - important warning or error!
    </div>
    <div class="alert alert-warning" role="alert">
        This is a warning alert - be careful!
    </div>
    <div class="alert alert-info" role="alert">
        This is an info alert - for your information!
    </div>

    <hr>

    <!-- ALERT WITH LINK -->
    <h5>Alert with Link</h5>
    <div class="alert alert-success" role="alert">
        Operation completed successfully! 
        <a href="#" class="alert-link">Click here to see details</a>
    </div>
    <div class="alert alert-danger" role="alert">
        An error occurred. 
        <a href="#" class="alert-link">Learn more about this issue</a>
    </div>

    <hr>

    <!-- ALERT WITH HEADING -->
    <h5>Alert with Heading</h5>
    <div class="alert alert-info" role="alert">
        <h4 class="alert-heading">Well done!</h4>
        <p>This is an alert with a heading. You can include any content here.</p>
        <hr>
        <p class="mb-0">This is additional text with a separator line above.</p>
    </div>

    <hr>

    <!-- DISMISSIBLE ALERT -->
    <h5>Dismissible Alert (with close button)</h5>
    <div class="alert alert-warning alert-dismissible fade show" role="alert">
        <strong>Warning!</strong> You can dismiss this alert by clicking the X button.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>

    <!-- AUTO-DISMISS ALERT (JavaScript needed) -->
    <h5>Multiple Dismissible Alerts</h5>
    <div class="alert alert-success alert-dismissible fade show" role="alert">
        <strong>Success!</strong> Your changes have been saved.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
    <div class="alert alert-info alert-dismissible fade show" role="alert">
        <strong>Info:</strong> New updates are available.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
    <div class="alert alert-danger alert-dismissible fade show" role="alert">
        <strong>Error!</strong> Something went wrong.
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.alert`**: Base class that provides padding, border-radius, and background color
- **`.alert-primary`**: Blue alert - for general information
- **`.alert-success`**: Green alert - for successful operations
- **`.alert-warning`**: Yellow alert - for warnings
- **`.alert-danger`**: Red alert - for errors and critical messages
- **`.alert-info`**: Cyan alert - for informational messages
- **`.alert-secondary`**: Gray alert - for less important messages
- **`.alert-heading`**: Styles headings inside alerts with proper color inheritance
- **`.alert-dismissible`**: Enables the dismiss functionality
- **`.fade`**: Adds fade transition when closing
- **`.show`**: Makes alert visible
- **`.btn-close`**: Close button (X) that triggers the dismiss action
- **`data-bs-dismiss="alert"`**: Bootstrap attribute that tells JavaScript to close the alert
- **`<hr>`**: Horizontal rule - used to separate content within alerts

## Expected Visual Result

The page displays multiple alert examples:

1. **Basic Alerts**: A vertical stack of colored alert boxes - blue (primary), gray (secondary), green (success), red (danger), yellow (warning), and cyan (info)

2. **Alert with Link**: Alerts where links styled in matching alert color appear within the text

3. **Alert with Heading**: A larger alert containing a bold heading ("Well done!") with body text and a horizontal divider

4. **Dismissible Alerts**: Alerts with an X button in the top-right corner that, when clicked, closes/hides the alert with a fade animation

Each alert type has a distinct color that helps users understand the nature of the message at a glance.
