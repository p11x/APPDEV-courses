# CSS Width and Height

## Definition

CSS width and height properties control the dimensions of elements on your webpage. You can set specific sizes in pixels, percentages, or other units. Understanding how to properly size elements is essential for creating layouts that look good on all screen sizes. You can also use max-width and min-width to create flexible, responsive designs.

## Key Points

- width sets the horizontal size of an element
- height sets the vertical size of an element
- Values can be in pixels (px), percentages (%), or other units
- max-width limits how wide an element can grow
- min-width sets the minimum width an element must be
- Same applies for height with max-height and min-height
- By default, height is auto (content determines height)
- Use max-width for responsive containers that adapt to screen size

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Width and Height</title>
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
        
        /* Base box styling */
        .box {
            background-color: #3498db;
            color: white;
            padding: 20px;
            margin: 10px 0;
        }
        
        /* Fixed width */
        .fixed-width {
            width: 300px;
        }
        
        /* Fixed height */
        .fixed-height {
            height: 100px;
        }
        
        /* Percentage width */
        .percent-width {
            width: 50%;
        }
        
        /* Max width - limits growth */
        .max-width {
            max-width: 500px;
            background-color: #2ecc71;
        }
        
        /* Min width - prevents shrinking */
        .min-width {
            min-width: 300px;
            background-color: #e74c3c;
        }
        
        /* Width and height together */
        .fixed-size {
            width: 200px;
            height: 150px;
            background-color: #9b59b6;
        }
        
        /* Responsive example */
        .container {
            max-width: 800px;
            margin: 0 auto;
            background-color: white;
            padding: 20px;
        }
        
        .responsive-box {
            width: 100%; /* Takes full container width */
            height: 200px;
            background-color: #3498db;
            margin-bottom: 20px;
        }
        
        .half-box {
            width: 50%; /* Takes half container width */
            height: 100px;
            background-color: #e67e22;
            display: inline-block;
            margin-right: -4px; /* Fix inline spacing */
        }
        
        .example-box {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 2px solid #ddd;
        }
    </style>
</head>
<body>
    <h1>CSS Width and Height</h1>
    
    <h2>1. Fixed Width</h2>
    <div class="box fixed-width">width: 300px - Always stays 300px wide</div>
    
    <h2>2. Fixed Height</h2>
    <div class="box fixed-height">height: 100px - Fixed height</div>
    
    <h2>3. Percentage Width</h2>
    <div class="example-box">
        <div class="box percent-width">width: 50% - Takes 50% of parent width</div>
    </div>
    
    <h2>4. Max Width</h2>
    <div class="box max-width">max-width: 500px - Won't grow beyond 500px but can shrink</div>
    
    <h2>5. Min Width</h2>
    <div class="box min-width">min-width: 300px - Won't shrink below 300px</div>
    
    <h2>6. Fixed Width and Height</h2>
    <div class="box fixed-size">width: 200px, height: 150px - Both fixed</div>
    
    <h2>7. Responsive Example</h2>
    <div class="container">
        <p>Resize the browser window to see how these boxes respond:</p>
        <div class="responsive-box">Full width box (100%)</div>
        <div class="half-box">50% width</div>
        <div class="half-box">50% width</div>
    </div>
</body>
</html>
```

## Explanation

### Basic Width and Height

- **width: 300px;** - Sets exact width to 300 pixels
- **height: 100px;** - Sets exact height to 100 pixels
- Pixel values give you precise control but don't adapt to screen size

### Percentage Width

- **width: 50%;** - Element takes 50% of its parent's width
- Parent must have a defined width for percentages to work
- Useful for responsive layouts that adapt to screen size

### Max Width

- **max-width: 500px;** - Element can be smaller but won't exceed 500px
- Great for text containers - prevents lines from getting too long to read
- Essential for responsive design on large screens

### Min Width

- **min-width: 300px;** - Element won't shrink below 300px
- Prevents content from becoming too squished
- Useful for navigation items and buttons

### Combined Usage

- Often use both max-width and width together
- Example: `width: 100%; max-width: 800px;`
- Means: take full width, but stop at 800px

### Important Notes

- Height is usually auto by default (grows with content)
- Setting height can cause content overflow if too small
- For responsive designs, prefer max-width over fixed width

## Visual Result

- Fixed width box is exactly 300px wide regardless of screen
- Fixed height box is exactly 100px tall
- Percentage box adjusts based on its parent container
- Max-width box shrinks on small screens, stays 500px on large
- Min-width box doesn't shrink below 300px
- Fixed size box has both dimensions set
- The responsive container shows boxes adapting to available space

Width and height properties are fundamental for creating layouts that work across different screen sizes.