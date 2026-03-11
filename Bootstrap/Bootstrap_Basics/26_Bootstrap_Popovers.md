# Bootstrap Popovers

## Definition

Bootstrap popovers are similar to tooltips but contain more content and are triggered by clicking instead of hovering. They are small overlay boxes that can show titles, content, and a close button. Popovers are ideal for showing detailed information, forms, or interactive content that users can interact with.

## Key Bullet Points

- **`.popover`**: Base class applied automatically
- **Trigger**: Click instead of hover (unlike tooltips)
- **Data Attributes**: data-bs-toggle="popover" with title and data-bs-content
- **JavaScript Required**: Must initialize with JavaScript
- **Placement**: Same as tooltips (top, bottom, left, right)
- **Dismiss on Next Click**: Use data-bs-trigger="focus" to dismiss when clicking elsewhere
- **Close Button**: Popovers can include close buttons

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Popovers</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Popover Examples</h2>

    <h5>Basic Popovers</h5>
    <div class="d-flex gap-2 mb-4 flex-wrap">
        <button type="button" class="btn btn-primary" data-bs-toggle="popover" title="Popover Title" data-bs-content="This is the popover content with more details!">
            Click for Popover
        </button>
        <button type="button" class="btn btn-success" data-bs-toggle="popover" data-bs-placement="bottom" title="Bottom Popover" data-bs-content="This popover appears at the bottom!">
            Bottom
        </button>
        <button type="button" class="btn btn-danger" data-bs-toggle="popover" data-bs-placement="left" title="Left Popover" data-bs-content="This popover appears on the left!">
            Left
        </button>
        <button type="button" class="btn btn-warning" data-bs-toggle="popover" data-bs-placement="right" title="Right Popover" data-bs-content="This popover appears on the right!">
            Right
        </button>
    </div>

    <hr>

    <h5>Dismiss on Next Click (Focus)</h5>
    <button type="button" class="btn btn-info mb-4" data-bs-toggle="popover" data-bs-trigger="focus" title="Dismissable Popover" data-bs-content="Click anywhere else to close this popover!">
        Click me, then click outside
    </button>

    <hr>

    <h5>Popovers with Form</h5>
    <button type="button" class="btn btn-primary mb-4" data-bs-toggle="popover" data-bs-title="Login" data-bs-content='
        <form>
            <div class="mb-2">
                <input type="text" class="form-control form-control-sm" placeholder="Username">
            </div>
            <div class="mb-2">
                <input type="password" class="form-control form-control-sm" placeholder="Password">
            </div>
            <button type="submit" class="btn btn-primary btn-sm w-100">Login</button>
        </form>
    ' data-bs-html="true">
        Login Popover
    </button>

    <hr>

    <h5>Disabled Popover</h5>
    <button type="button" class="btn btn-secondary" disabled data-bs-toggle="popover" title="Disabled Popover" data-bs-content="This popover will not show">
        Disabled Button
    </button>

    <hr>

    <h5>Toggle Popover with JavaScript</h5>
    <button type="button" class="btn btn-outline-primary mb-4" id="togglePopover">
        Toggle via JavaScript
    </button>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // Initialize all popovers
        var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
        var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
            return new bootstrap.Popover(popoverTriggerEl)
        })

        // Toggle popover with JavaScript
        document.getElementById('togglePopover').addEventListener('click', function() {
            var popover = bootstrap.Popover.getOrCreateInstance(this)
            popover.toggle()
        })
    </script>
</body>
</html>
