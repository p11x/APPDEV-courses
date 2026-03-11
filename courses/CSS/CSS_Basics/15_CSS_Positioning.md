# CSS Positioning

## Definition

CSS positioning controls where elements appear on the webpage. The position property determines how an element is positioned in the document. There are five main positioning modes: static (default normal flow), relative (offset from normal position), absolute (positioned relative to positioned ancestor), fixed (locked to viewport), and sticky (hybrid that starts static then becomes fixed).

## Key Points

- position: static is the default - elements flow normally
- position: relative moves element from its normal position using top, right, bottom, left
- position: absolute positions relative to the nearest positioned ancestor
- position: fixed positions relative to the browser window (stays when scrolling)
- position: sticky starts as static, becomes fixed when reaching threshold
- top, right, bottom, left offset properties only work with relative, absolute, fixed
- A "positioned" element has any position value except static

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Positioning</title>
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
            height: 200px;
            background-color: white;
            border: 2px dashed #999;
            margin: 20px 0;
        }
        
        .box {
            width: 100px;
            height: 100px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
        }
        
        /* Static - default positioning */
        .static-box {
            position: static;
            background-color: #95a5a6;
        }
        
        /* Relative - offset from normal position */
        .relative-box {
            position: relative;
            top: 20px;
            left: 30px;
            background-color: #3498db;
        }
        
        /* Absolute - relative to container */
        .absolute-box {
            position: absolute;
            top: 10px;
            right: 10px;
            background-color: #e74c3c;
        }
        
        /* Fixed - stays in viewport */
        .fixed-box {
            position: fixed;
            bottom: 20px;
            right: 20px;
            background-color: #2ecc71;
            width: 80px;
            height: 80px;
            border-radius: 50%;
            z-index: 100;
        }
        
        /* Sticky - sticks when scrolling */
        .sticky-box {
            position: sticky;
            top: 10px;
            background-color: #9b59b6;
            display: inline-block;
            width: auto;
            padding: 10px 20px;
            margin: 10px 0;
        }
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
        
        /* For scrollable demo */
        .scroll-container {
            height: 300px;
            overflow-y: auto;
            background-color: #f9f9f9;
        }
    </style>
</head>
<body>
    <h1>CSS Positioning</h1>
    
    <h2>1. Position: Static (Default)</h2>
    <div class="demo-section">
        <p>Static positioning is the default. Elements appear in normal document flow.</p>
        <div class="container">
            <div class="box static-box">Static</div>
            <div class="box static-box">Static</div>
        </div>
    </div>
    
    <h2>2. Position: Relative</h2>
    <div class="demo-section">
        <p>Relative moves the element FROM its normal position using top, left, etc.</p>
        <div class="container">
            <div class="box static-box" style="margin: 0;">Normal</div>
            <div class="box relative-box">Relative<br>top: 20px<br>left: 30px</div>
        </div>
    </div>
    
    <h2>3. Position: Absolute</h2>
    <div class="demo-section">
        <p>Absolute positions relative to the nearest positioned ancestor.</p>
        <div class="container">
            <div class="box absolute-box">Absolute<br>top: 10px<br>right: 10px</div>
        </div>
    </div>
    
    <h2>4. Position: Fixed</h2>
    <div class="demo-section">
        <p>Fixed stays in the same place even when you scroll. Try scrolling!</p>
        <p>Look at the green circle in the bottom right corner.</p>
        <div style="height: 200px;">
            <p>Scroll down to see the fixed element stay in place...</p>
        </div>
    </div>
    
    <h2>5. Position: Sticky</h2>
    <div class="demo-section">
        <p>Sticky starts normal, then sticks to position when you scroll past it.</p>
        <div class="scroll-container">
            <div class="box sticky-box">I Stick!</div>
            <p>Scroll down...</p>
            <p>Keep scrolling...</p>
            <p>More content...</p>
            <p>More content...</p>
            <p>More content...</p>
            <p>More content...</p>
            <p>More content...</p>
        </div>
    </div>
    
    <!-- Fixed element will appear in corner -->
    <div class="box fixed-box">Fixed</div>
</body>
</html>
```

## Explanation

### Position: Static

- Default value for all elements
- Elements appear in normal document flow
- top, right, bottom, left properties have no effect

### Position: Relative

- Element stays in normal flow but can be offset
- Use top, right, bottom, left to move from original position
- The original space the element occupied stays reserved
- Good for small adjustments or as parent for absolute children

### Position: Absolute

- Element is removed from normal flow
- Positions relative to nearest positioned ancestor (parent with position: relative/absolute/fixed)
- If no positioned ancestor, positions relative to viewport
- Can overlap with other elements

### Position: Fixed

- Element is removed from normal flow
- Always positions relative to viewport
- Stays in same position when scrolling
- Common for: navigation bars, back-to-top buttons, chat widgets
- Good for always-visible UI elements

### Position: Sticky

- Acts like static until you scroll past it
- Then "sticks" in place like fixed
- Must have a threshold (top, right, bottom, or left value)
- Great for table headers and navigation within sections

## Visual Result

- Static boxes appear in normal flow
- Relative box is shifted from its original position
- Absolute box is positioned in the corner of its container
- Fixed box stays in the bottom-right corner while scrolling
- Sticky box sticks to the top when scrolled past

Positioning is essential for creating complex layouts and interactive UI elements.