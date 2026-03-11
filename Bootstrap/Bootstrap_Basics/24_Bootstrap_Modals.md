# Bootstrap Modals

## Definition

Bootstrap modals are dialog boxes that appear on top of the current page, dimming the background (overlay). They are used to present important information, forms, or alerts that require immediate attention. Modals interrupt user interaction with the main page until the modal is closed, making them effective for focused tasks like login forms or confirmations.

## Key Bullet Points

- **`.modal`**: Container for modal
- **`.modal-dialog`**: Dialog wrapper that positions and sizes the modal
- **`.modal-content`**: Contains header, body, footer
- **`.modal-header`**: Top section with title and close button
- **`.modal-body`**: Main content area
- **`.modal-footer`**: Bottom section for action buttons
- **`.modal-lg` / `.modal-sm`**: Size variations
- **`.modal-backdrop`**: Dark overlay behind modal (automatic)
- **Trigger**: Button with data-bs-toggle="modal" and data-bs-target

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Modals</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Modal Examples</h2>

    <!-- BUTTON TO TRIGGER MODAL -->
    <h5>Basic Modal</h5>
    <button type="button" class="btn btn-primary mb-2" data-bs-toggle="modal" data-bs-target="#exampleModal">
        Open Modal
    </button>

    <!-- MODAL STRUCTURE -->
    <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Modal Title</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>This is the modal body content. You can put any content here including forms, text, images, etc.</p>
                    <p>Modals are great for:</p>
                    <ul>
                        <li>Login/Register forms</li>
                        <li>Confirmations</li>
                        <li>Alerts and notices</li>
                        <li>Additional information</li>
                    </ul>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>
            </div>
        </div>
    </div>

    <!-- SCROLLABLE MODAL -->
    <h5>Scrollable Modal</h5>
    <button type="button" class="btn btn-primary mb-2" data-bs-toggle="modal" data-bs-target="#scrollableModal">
        Open Scrollable Modal
    </button>

    <div class="modal fade" id="scrollableModal" tabindex="-1" aria-labelledby="scrollableModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="scrollableModalLabel">Scrollable Modal</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>This modal has scrollable content. When there is more content than fits in the visible area, only the body scrolls.</p>
                    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
                    <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                    <p>Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.</p>
                    <p>Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                    <p>More content here to demonstrate scrolling...</p>
                    <p>Even more content...</p>
                    <p>Keep scrolling...</p>
                    <p>Almost at the end...</p>
                    <p>Final content!</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Confirm</button>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL SIZES -->
    <h5>Modal Sizes</h5>
    <div class="d-flex gap-2 mb-4">
        <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#smallModal">Small</button>
        <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#defaultModal">Default</button>
        <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#largeModal">Large</button>
        <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#fullModal">Full Screen</button>
    </div>

    <!-- Small Modal -->
    <div class="modal fade" id="smallModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Small Modal</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">This is a small modal.</div>
            </div>
        </div>
    </div>

    <!-- Large Modal -->
    <div class="modal fade" id="largeModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Large Modal</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">This is a large modal for more content.</div>
            </div>
        </div>
    </div>

    <!-- Full Screen Modal -->
    <div class="modal fade" id="fullModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-fullscreen">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Full Screen Modal</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">This modal takes up the full screen.</div>
            </div>
        </div>
    </div>

    <!-- CENTERED MODAL -->
    <h5>Centered Modal</h5>
    <button type="button" class="btn btn-success mb-4" data-bs-toggle="modal" data-bs-target="#centeredModal">
        Open Centered Modal
    </button>

    <div class="modal fade" id="centeredModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Centered Modal</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">This modal is centered vertically on the page.</div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.modal`**: Outer wrapper for modal
- **`.modal-dialog`**: Positions the modal in the center of viewport
- **`.modal-content`**: Contains header, body, footer with background and borders
- **`.modal-header`**: Header area with title and close button
- **`.modal-title`**: Styled title in header
- **`.modal-body`**: Main content area
- **`.modal-footer`**: Footer with action buttons
- **`.modal-dialog-scrollable`**: Makes modal body scrollable when content is long
- **`.modal-dialog-centered`**: Vertically centers modal
- **`.modal-sm`**: Small width modal
- **`.modal-lg`**: Large width modal
- **`.modal-fullscreen`**: Full screen modal on all devices
- **`.btn-close`**: Close button (X) in top right
- **`data-bs-toggle="modal"`**: Bootstrap attribute to trigger modal
- **`data-bs-target`**: Selector targeting the modal to open
- **`data-bs-dismiss="modal"`**: Attribute to close modal

## Expected Visual Result

The page displays buttons to open various modals:

1. **Basic Modal**: Opens a centered dialog with title, body text, and footer buttons. Background dims with overlay.

2. **Scrollable Modal**: Opens modal with long content - only the body scrolls while header/footer stay fixed.

3. **Size Examples**: Buttons for small (narrower), large (wider), and full-screen modals.

4. **Centered Modal**: Opens vertically centered on screen.

Clicking any button opens the corresponding modal. Clicking outside the modal or the X button closes it.
