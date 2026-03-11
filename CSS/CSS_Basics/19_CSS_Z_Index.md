# CSS Z-Index

## Definition

CSS z-index controls the stacking order of elements - which elements appear in front of or behind others. When elements overlap, z-index determines which one is visible on top. Think of it like layers in a stack of papers: higher z-index means the element is closer to you (on top), lower z-index means it's further down in the stack.

## Key Points

- z-index only works on positioned elements (relative, absolute, fixed, or sticky)
- Higher z-index values appear in front of lower values
- z-index can be negative to push elements behind
- Default z-index is auto (or 0 for positioned elements)
- Elements with the same z-index stack in the order they appear in HTML
- A stacking context is created by positioned elements with z-index
- Z-index only affects visual layering, not document order

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Z-Index</title>
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
        
        .container {
            position: relative;
            width: 300px;
            height: 200px;
            margin: 20px auto;
        }
        
        .box {
            position: absolute;
            width: 150px;
            height: 150px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
            border-radius: 10px;
        }
        
        /* Z-index demonstration */
        .box1 {
            top: 0;
            left: 0;
            background-color: #e74c3c;
            z-index: 1;
        }
        
        .box2 {
            top: 40px;
            left: 40px;
            background-color: #2ecc71;
            z-index: 2;
        }
        
        .box3 {
            top: 80px;
            left: 80px;
            background-color: #3498db;
            z-index: 3;
        }
        
        /* Negative z-index */
        .behind {
            z-index: -1;
        }
        
        /* Without z-index (HTML order) */
        .container-no-zindex {
            position: relative;
            width: 300px;
            height: 200px;
            margin: 20px auto;
            background-color: white;
            border: 2px dashed #999;
        }
        
        .box-no-zindex {
            position: absolute;
            width: 100px;
            height: 100px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
        }
        
        .first {
            top: 20px;
            left: 20px;
            background-color: #e74c3c;
        }
        
        .second {
            top: 60px;
            left: 60px;
            background-color: #2ecc71;
        }
        
        .third {
            top: 100px;
            left: 100px;
            background-color: #3498db;
        }
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
            text-align: center;
        }
    </style>
</head>
<body>
    <h1>CSS Z-Index</h1>
    
    <h2>1. With Z-Index (Stacking Order)</h2>
    <div class="demo-section">
        <p>Box 1: z-index: 1 | Box 2: z-index: 2 | Box 3: z-index: 3</p>
        <div class="container">
            <div class="box box1">z: 1</div>
            <div class="box box2">z: 2</div>
            <div class="box box3">z: 3</div>
        </div>
    </div>
    
    <h2>2. Without Z-Index (HTML Order)</h2>
    <div class="demo-section">
        <p>Elements stack in the order they appear in HTML</p>
        <div class="container-no-zindex">
            <div class="box-no-zindex first">1st</div>
            <div class="box-no-zindex second">2nd</div>
            <div class="box-no-zindex third">3rd</div>
        </div>
    </div>
    
    <h2>3. Negative Z-Index</h2>
    <div class="demo-section">
        <p>Negative z-index pushes element behind the background</p>
        <div class="container" style="background-color: #f0f0f0;">
            <div class="box" style="top: 20px; left: 20px; background-color: #333; z-index: 1;">Normal</div>
            <div class="box" style="top: 60px; left: 60px; background-color: #e74c3c; z-index: -1;">Behind!</div>
        </div>
    </div>
    
    <h2>4. Practical: Modal Overlay</h2>
    <div class="demo-section">
        <div style="position: relative; height: 200px; background-color: white; border: 2px solid #ddd;">
            <p>Page content here...</p>
            <div style="position: absolute; top: 50px; left: 50px; background-color: #333; color: white; padding: 20px; width: 200px;">
                Behind overlay
            </div>
            <!-- Modal overlay -->
            <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; background-color: rgba(0,0,0,0.5); z-index: 100; display: flex; align-items: center; justify-content: center;">
                <div style="background-color: white; padding: 30px; border-radius: 10px; z-index: 101;">
                    <h3>Modal Dialog</h3>
                    <p>This appears on top!</p>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
```

## Explanation

### How Z-Index Works

- Z-index only works on positioned elements (position: relative, absolute, fixed, or sticky)
- Higher z-index = element appears in front
- Lower z-index = element appears behind
- Without z-index, elements stack in HTML order (later elements appear on top)

### Z-Index Values

- **z-index: 1;** - Low value, appears behind
- **z-index: 100;** - High value, appears in front
- **z-index: -1;** - Negative value, appears behind the element's background
- **z-index: auto;** - Same as 0, creates new stacking context

### Stacking Context

- A new stacking context is created when an element has a z-index
- Elements within a stacking context stack relative to their parent
- This can sometimes cause unexpected behavior

### Common Use Cases

- **Modal dialogs**: High z-index to appear on top of everything
- **Dropdowns**: Higher z-index than regular content
- **Sticky headers**: z-index to stay above page content
- **Tooltips**: Above other content

## Visual Result

- Z-index example shows clear stacking order based on values
- Without z-index, later HTML elements appear on top
- Negative z-index example shows element behind parent background
- Modal overlay demonstrates practical use: semi-transparent overlay at z-index 100, modal box at z-index 101

Z-index is essential for creating layered interfaces like modals, dropdowns, and sticky elements.