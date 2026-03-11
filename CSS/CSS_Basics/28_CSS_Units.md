# CSS Units

## Definition

CSS units determine how sizes and measurements are calculated in your styles. There are two main types: absolute units (like pixels) that stay the same regardless of screen size, and relative units (like percentages, em, rem) that scale based on parent elements or viewport. Understanding units is essential for creating responsive designs.

## Key Points

- px is an absolute unit - fixed size regardless of screen
- % is relative to the parent element's size
- em is relative to the current element's font-size
- rem is relative to the root (html) font-size
- vw is viewport width (1vw = 1% of viewport width)
- vh is viewport height (1vh = 1% of viewport height)
- Use rem for consistent sizing, % for flexible layouts
- Pixels are easiest to understand but not best for responsiveness

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Units</title>
    <style>
        * {
            box-sizing: border-box;
        }
        
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
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
        
        /* Pixel example */
        .pixel-box {
            width: 200px;
            height: 100px;
            background-color: #3498db;
            color: white;
            padding: 10px;
        }
        
        /* Percentage example */
        .percentage-container {
            width: 300px;
            background-color: #e74c3c;
            padding: 10px;
        }
        
        .percentage-box {
            width: 50%;
            background-color: #3498db;
            color: white;
            padding: 10px;
        }
        
        /* EM example */
        .em-container {
            font-size: 20px;
        }
        
        .em-box {
            /* 2em = 2 x 20px = 40px */
            font-size: 2em;
            padding: 1em;
            background-color: #2ecc71;
            color: white;
        }
        
        /* REM example */
        .rem-box {
            /* rem is always relative to root (html) font-size */
            padding: 2rem;
            background-color: #9b59b6;
            color: white;
            margin: 1rem 0;
        }
        
        /* VW/VH example */
        .vw-box {
            width: 50vw;
            height: 20vh;
            background-color: #f39c12;
            color: white;
            padding: 10px;
        }
        
        /* Comparison display */
        .comparison-box {
            display: flex;
            gap: 10px;
            margin: 20px 0;
        }
        
        .comparison-box > div {
            padding: 20px;
            color: white;
        }
        
        /* Units in different contexts */
        .font-px { font-size: 16px; }
        .font-em { font-size: 1em; }
        .font-rem { font-size: 1rem; }
        .font-percent { font-size: 100%; }
        
        .px { background-color: #3498db; }
        .em { background-color: #e74c3c; }
        .rem { background-color: #2ecc71; }
        .percent { background-color: #9b59b6; }
    </style>
</head>
<body>
    <h1>CSS Units</h1>
    
    <h2>1. Pixels (px)</h2>
    <div class="demo-section">
        <p>Fixed size regardless of screen:</p>
        <div class="pixel-box">200px x 100px</div>
    </div>
    
    <h2>2. Percentage (%)</h2>
    <div class="demo-section">
        <p>50% of parent width:</p>
        <div class="percentage-container">
            <div class="percentage-box">I'm 50% of parent</div>
        </div>
    </div>
    
    <h2>3. EM</h2>
    <div class="demo-section">
        <p>2em = 2 x parent's font-size:</p>
        <div class="em-container">
            Parent font: 20px
            <div class="em-box">I'm 2em = 40px font-size</div>
        </div>
    </div>
    
    <h2>4. REM</h2>
    <div class="demo-section">
        <p>2rem = 2 x root (html) font-size:</p>
        <div class="rem-box">I'm 2rem padding</div>
    </div>
    
    <h2>5. VW and VH</h2>
    <div class="demo-section">
        <p>50vw = 50% viewport width, 20vh = 20% viewport height:</p>
        <div class="vw-box">Resize window to see me change</div>
    </div>
    
    <h2>6. Font Size Comparison</h2>
    <div class="demo-section">
        <div class="comparison-box">
            <div class="px font-px">16px</div>
            <div class="em font-em">1em</div>
            <div class="rem font-rem">1rem</div>
            <div class="percent font-percent">100%</div>
        </div>
    </div>
</body>
</html>
```

## Explanation

### Absolute Units

- **px (pixels)**: Fixed size, doesn't scale
- Best for: borders, precise layouts, things that shouldn't change

### Relative Units

- **% (percentage)**: Relative to parent element
- Best for: widths, flexible layouts

- **em**: Relative to parent's font-size
- 2em = 2 x parent's font-size
- Best for: padding, margins that scale with text

- **rem**: Relative to root (html) font-size
- 1rem = html font-size (usually 16px)
- Best for: consistent sizing throughout site

- **vw**: Viewport width
- 1vw = 1% of viewport width
- Best for: full-width hero sections

- **vh**: Viewport height
- 1vh = 1% of viewport height
- Best for: full-screen sections

### When to Use What

| Unit | Best Use |
|------|----------|
| px | Borders, shadows, exact sizes |
| % | Container widths |
| em | Padding, margins relative to text |
| rem | Font sizes, consistent spacing |
| vw/vh | Full-screen sections |

### Important Notes

- Default browser font-size is 16px
- Using rem for fonts creates consistent sizing
- Percentage widths create flexible layouts
- vw/vh units can cause unexpected results on very small/large screens

## Visual Result

- Pixel box stays fixed size
- Percentage box is half the width of its container
- EM box scales based on parent font-size
- REM box scales based on root font-size
- VW/VH boxes change when resizing the browser window
- All font-size units show the same visual result (16px equivalent)

Understanding units is key to creating responsive, maintainable CSS.