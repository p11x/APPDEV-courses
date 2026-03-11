# CSS Flexbox

## Definition

Flexbox (Flexible Box Layout) is a CSS layout model designed for one-dimensional layouts - either rows OR columns. It makes it easy to align items, distribute space between them, and control their order without using floats or positioning. Flexbox is perfect for navigation menus, card layouts, and centering content.

## Key Points

- display: flex turns a container into a flex container
- Flex items (children) automatically align in a row by default
- flex-direction controls the direction (row or column)
- justify-content aligns items along the main axis (horizontal in row)
- align-items aligns items along the cross axis (vertical in row)
- flex-wrap controls whether items wrap to new lines
- Flexbox makes responsive layouts much easier to create

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Flexbox</title>
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
        
        /* Base flex container */
        .flex-container {
            display: flex;
            background-color: white;
            padding: 10px;
            margin: 10px 0;
            border: 2px solid #ddd;
        }
        
        .flex-item {
            background-color: #3498db;
            color: white;
            padding: 20px;
            margin: 5px;
            text-align: center;
            border-radius: 5px;
            width: 80px;
        }
        
        /* Different flex directions */
        .flex-row { flex-direction: row; }
        .flex-column { flex-direction: column; }
        .flex-row-reverse { flex-direction: row-reverse; }
        
        /* Justify Content (main axis) */
        .justify-start { justify-content: flex-start; }
        .justify-end { justify-content: flex-end; }
        .justify-center { justify-content: center; }
        .justify-between { justify-content: space-between; }
        .justify-around { justify-content: space-around; }
        
        /* Align Items (cross axis) */
        .align-stretch { align-items: stretch; }
        .align-center { align-items: center; }
        .align-start { align-items: flex-start; }
        .align-end { align-items: flex-end; }
        
        /* Flex Wrap */
        .flex-wrap { flex-wrap: wrap; }
        .flex-nowrap { flex-wrap: nowrap; }
        
        /* Gap */
        .flex-gap { gap: 15px; }
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
        
        /* For column demo */
        .column-demo .flex-item {
            width: auto;
        }
    </style>
</head>
<body>
    <h1>CSS Flexbox</h1>
    
    <h2>1. Basic Flex (Default: Row)</h2>
    <div class="demo-section">
        <p>Default flex-direction: row (horizontal)</p>
        <div class="flex-container flex-row">
            <div class="flex-item">1</div>
            <div class="flex-item">2</div>
            <div class="flex-item">3</div>
        </div>
    </div>
    
    <h2>2. Flex Direction: Column</h2>
    <div class="demo-section">
        <p>flex-direction: column (vertical)</p>
        <div class="flex-container flex-column column-demo">
            <div class="flex-item">Item 1</div>
            <div class="flex-item">Item 2</div>
            <div class="flex-item">Item 3</div>
        </div>
    </div>
    
    <h2>3. Justify Content (Horizontal Alignment)</h2>
    <div class="demo-section">
        <p>justify-content: center</p>
        <div class="flex-container justify-center">
            <div class="flex-item">1</div>
            <div class="flex-item">2</div>
            <div class="flex-item">3</div>
        </div>
        <p>justify-content: space-between</p>
        <div class="flex-container justify-between">
            <div class="flex-item">1</div>
            <div class="flex-item">2</div>
            <div class="flex-item">3</div>
        </div>
    </div>
    
    <h2>4. Align Items (Vertical Alignment)</h2>
    <div class="demo-section">
        <p>align-items: center (in a taller container)</p>
        <div class="flex-container" style="height: 150px;" align-center>
            <div class="flex-item">Centered</div>
            <div class="flex-item">Centered</div>
            <div class="flex-item">Centered</div>
        </div>
    </div>
    
    <h2>5. Flex Wrap</h2>
    <div class="demo-section">
        <p>flex-wrap: wrap (items wrap to new lines)</p>
        <div class="flex-container flex-wrap">
            <div class="flex-item" style="width: 150px;">1</div>
            <div class="flex-item" style="width: 150px;">2</div>
            <div class="flex-item" style="width: 150px;">3</div>
            <div class="flex-item" style="width: 150px;">4</div>
            <div class="flex-item" style="width: 150px;">5</div>
            <div class="flex-item" style="width: 150px;">6</div>
        </div>
    </div>
    
    <h2>6. Gap Property</h2>
    <div class="demo-section">
        <p>gap: 15px (space between items)</p>
        <div class="flex-container flex-gap">
            <div class="flex-item">1</div>
            <div class="flex-item">2</div>
            <div class="flex-item">3</div>
        </div>
    </div>
</body>
</html>
```

## Explanation

### Getting Started with Flexbox

- Add `display: flex;` to the parent container
- All children become flex items automatically
- Items align in a row by default

### Flex Direction

- **flex-direction: row;** - Items in a horizontal line (default)
- **flex-direction: column;** - Items in a vertical stack
- **flex-direction: row-reverse;** - Items in reverse horizontal order
- **flex-direction: column-reverse;** - Items in reverse vertical order

### Justify Content (Main Axis)

The main axis is horizontal (left to right) in row mode:
- **flex-start** - Align to left
- **flex-end** - Align to right
- **center** - Center everything
- **space-between** - Equal space between items
- **space-around** - Equal space around each item

### Align Items (Cross Axis)

The cross axis is vertical (top to bottom) in row mode:
- **stretch** - Fill the container (default)
- **flex-start** - Align to top
- **flex-end** - Align to bottom
- **center** - Center vertically

### Flex Wrap

- **flex-wrap: wrap;** - Items wrap to new lines if needed
- **flex-wrap: nowrap;** - All items in one line (may shrink)

### Gap

- **gap: 15px;** - Creates space between flex items
- Cleaner than using margins on items

## Visual Result

- Basic flex shows items in a horizontal row
- Column direction shows vertical stacking
- Justify content demonstrates different horizontal alignments
- Align items shows vertical centering within the container
- Flex wrap demonstrates items flowing to new lines
- Gap shows clean spacing between items

Flexbox is one of the most useful CSS layout tools for creating responsive designs.