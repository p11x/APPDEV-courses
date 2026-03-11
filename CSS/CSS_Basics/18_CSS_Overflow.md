# CSS Overflow

## Definition

CSS overflow controls what happens when content is too large for its container. When you set a specific width or height on an element and the content inside is too big, it will overflow (stick out). The overflow property lets you choose whether to hide the overflow, show scrollbars, or let the browser decide automatically.

## Key Points

- overflow: hidden clips content that doesn't fit (hides it)
- overflow: scroll always shows scrollbars (even if not needed)
- overflow: auto shows scrollbars only when needed
- overflow-x controls horizontal overflow
- overflow-y controls vertical overflow
- Default overflow is visible (content shows outside the box)
- Use overflow to create scrollable areas

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Overflow</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
            background-color: #f5f5f5;
        }
        
        h1 {
            text-align: center;
            color: #333;
        }
        
        h2 {
            color: #555;
            margin-top: 40px;
        }
        
        .box {
            width: 200px;
            height: 100px;
            background-color: white;
            border: 2px solid #3498db;
            margin: 10px;
            display: inline-block;
            vertical-align: top;
            padding: 10px;
        }
        
        /* Overflow values */
        .overflow-visible {
            overflow: visible;
        }
        
        .overflow-hidden {
            overflow: hidden;
        }
        
        .overflow-scroll {
            overflow: scroll;
        }
        
        .overflow-auto {
            overflow: auto;
        }
        
        /* Separate overflow-x and overflow-y */
        .overflow-xy {
            overflow-x: hidden;
            overflow-y: scroll;
        }
        
        .long-content {
            width: 300px;
        }
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
        
        .label {
            display: block;
            margin-bottom: 10px;
            font-weight: bold;
            color: #333;
        }
    </style>
</head>
<body>
    <h1>CSS Overflow</h1>
    
    <h2>1. Overflow: Visible (Default)</h2>
    <div class="demo-section">
        <span class="label">overflow: visible - Content shows outside the box</span>
        <div class="box overflow-visible long-content">
            This is some very long text that will overflow outside the container. The content is visible even though it extends beyond the box boundaries.
        </div>
    </div>
    
    <h2>2. Overflow: Hidden</h2>
    <div class="demo-section">
        <span class="label">overflow: hidden - Excess content is clipped/hidden</span>
        <div class="box overflow-hidden long-content">
            This is some very long text that will be hidden when it overflows. The content that doesn't fit is simply cut off and not visible.
        </div>
    </div>
    
    <h2>3. Overflow: Scroll</h2>
    <div class="demo-section">
        <span class="label">overflow: scroll - Always shows scrollbars</span>
        <div class="box overflow-scroll long-content">
            This is some very long text that can be scrolled. Scrollbars appear even if the content doesn't overflow.
        </div>
    </div>
    
    <h2>4. Overflow: Auto</h2>
    <div class="demo-section">
        <span class="label">overflow: auto - Browser decides when to show scrollbars</span>
        <div class="box overflow-auto long-content">
            This is some very long text that can be scrolled. Scrollbars only appear when needed.
        </div>
    </div>
    
    <h2>5. Separate X and Y Overflow</h2>
    <div class="demo-section">
        <span class="label">overflow-x: hidden, overflow-y: scroll</span>
        <div class="box overflow-xy long-content">
            This is some very long text that can be scrolled vertically but horizontally hidden.
        </div>
    </div>
    
    <h2>Practical Example: Scrollable Message List</h2>
    <div class="demo-section">
        <div class="box" style="width: 250px; height: 200px; overflow-y: auto;">
            <h3>Messages</h3>
            <p>Message 1: Hello!</p>
            <p>Message 2: How are you?</p>
            <p>Message 3: CSS is fun!</p>
            <p>Message 4: Learning web dev</p>
            <p>Message 5: Keep practicing</p>
            <p>Message 6: You're doing great!</p>
            <p>Message 7: Almost there!</p>
            <p>Message 8: Final message</p>
        </div>
    </div>
</body>
</html>
```

## Explanation

### Overflow: Visible (Default)

- **overflow: visible;** - Content that doesn't fit is shown outside the box
- This is the default behavior
- Useful when you want content to spill over

### Overflow: Hidden

- **overflow: hidden;** - Excess content is clipped and not shown
- Useful for creating fixed-size boxes
- Content that doesn't fit is completely hidden

### Overflow: Scroll

- **overflow: scroll;** - Always shows scrollbars (horizontal and vertical)
- Even if content fits, scrollbars are visible
- Useful when you always want scrolling capability

### Overflow: Auto

- **overflow: auto;** - Browser decides whether to show scrollbars
- Shows scrollbars only when content actually overflows
- Best choice for most situations

### Separate X and Y

- **overflow-x: hidden;** - Hide horizontal overflow
- **overflow-y: scroll;** - Always show vertical scrollbar
- Useful for creating message lists, chat windows, etc.

## Visual Result

- Visible: Text extends outside the box boundaries
- Hidden: Text is cut off at the box edge
- Scroll: Scrollbars are always visible
- Auto: Scrollbars appear only when needed
- Separate X/Y shows horizontal hidden, vertical scroll
- The practical example shows a scrollable message list

The overflow property is essential for creating contained, scrollable areas in your layouts.