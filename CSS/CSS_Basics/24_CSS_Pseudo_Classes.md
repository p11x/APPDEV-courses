# CSS Pseudo Classes

## Definition

CSS pseudo classes are special keywords that let you style elements based on their state or position without adding extra HTML. They start with a colon (:) and target specific states like when a user hovers over an element, clicks it, or when an element is the first child. Pseudo classes are essential for interactive and dynamic styling.

## Key Points

- :hover styles when mouse is over an element
- :active styles when element is being clicked
- :focus styles when element has keyboard focus
- :first-child targets the first child element
- :last-child targets the last child element
- :nth-child(n) targets the nth child
- :visited styles links that have been clicked
- Pseudo classes don't change HTML but affect how elements look

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Pseudo Classes</title>
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
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
        
        /* Hover - when mouse is over element */
        .hover-box {
            width: 200px;
            padding: 30px;
            background-color: #3498db;
            color: white;
            text-align: center;
            transition: background-color 0.3s;
        }
        
        .hover-box:hover {
            background-color: #2980b9;
        }
        
        /* Active - when element is being clicked */
        .active-box {
            width: 200px;
            padding: 30px;
            background-color: #e74c3c;
            color: white;
            text-align: center;
            cursor: pointer;
        }
        
        .active-box:active {
            background-color: #c0392b;
            transform: scale(0.95);
        }
        
        /* Focus - when element has keyboard focus */
        .focus-input {
            padding: 10px;
            border: 2px solid #ddd;
            width: 200px;
            outline: none;
        }
        
        .focus-input:focus {
            border-color: #3498db;
            box-shadow: 0 0 5px rgba(52, 152, 219, 0.5);
        }
        
        /* Link states */
        .demo-link {
            color: #3498db;
            text-decoration: none;
            margin-right: 15px;
        }
        
        .demo-link:link {
            color: #3498db;
        }
        
        .demo-link:visited {
            color: #9b59b6;
        }
        
        .demo-link:hover {
            color: #e74c3c;
            text-decoration: underline;
        }
        
        .demo-link:active {
            color: #e67e22;
        }
        
        /* First and Last Child */
        .child-list {
            list-style: none;
            padding: 0;
        }
        
        .child-list li {
            padding: 10px;
            border-bottom: 1px solid #eee;
        }
        
        .child-list li:first-child {
            background-color: #e8f8f5;
            font-weight: bold;
        }
        
        .child-list li:last-child {
            background-color: #fef9e7;
            font-weight: bold;
        }
        
        /* nth-child */
        .nth-list {
            list-style: none;
            padding: 0;
            display: flex;
            gap: 10px;
        }
        
        .nth-list li {
            padding: 15px;
            background-color: #3498db;
            color: white;
        }
        
        .nth-list li:nth-child(2) {
            background-color: #e74c3c;
        }
        
        .nth-list li:nth-child(odd) {
            background-color: #2ecc71;
        }
        
        .nth-list li:nth-child(3n) {
            background-color: #9b59b6;
        }
    </style>
</head>
<body>
    <h1>CSS Pseudo Classes</h1>
    
    <h2>1. :hover</h2>
    <div class="demo-section">
        <p>Hover over the blue box:</p>
        <div class="hover-box">Hover Me!</div>
    </div>
    
    <h2>2. :active</h2>
    <div class="demo-section">
        <p>Click and hold the red box:</p>
        <div class="active-box">Click Me!</div>
    </div>
    
    <h2>3. :focus</h2>
    <div class="demo-section">
        <p>Click inside the input field:</p>
        <input type="text" class="focus-input" placeholder="Click me!">
    </div>
    
    <h2>4. Link States (:link, :visited, :hover, :active)</h2>
    <div class="demo-section">
        <a href="#" class="demo-link">Link</a>
        <a href="#" class="demo-link" style="color: #9b59b6;">Visited</a>
        <p><em>Try clicking and hovering these links</em></p>
    </div>
    
    <h2>5. :first-child and :last-child</h2>
    <div class="demo-section">
        <ul class="child-list">
            <li>First item (first-child)</li>
            <li>Second item</li>
            <li>Third item</li>
            <li>Fourth item</li>
            <li>Last item (last-child)</li>
        </ul>
    </div>
    
    <h2>6. :nth-child</h2>
    <div class="demo-section">
        <p>2nd item is red, odd items are green, every 3rd is purple:</p>
        <ul class="nth-list">
            <li>1</li>
            <li>2</li>
            <li>3</li>
            <li>4</li>
            <li>5</li>
            <li>6</li>
        </ul>
    </div>
</body>
</html>
```

## Explanation

### :hover

- Applies when cursor is over the element
- Most common for buttons and links
- Used for providing visual feedback

### :active

- Applies while the element is being activated (clicked)
- Creates a "button press" effect
- Often combined with :hover

### :focus

- Applies when element receives keyboard focus
- Important for accessibility
- Used for input fields and buttons

### Link Pseudo Classes

- **:link** - Unvisited links
- **:visited** - Visited links
- Must come in this order: :link, :visited, :hover, :active

### Child Pseudo Classes

- **:first-child** - First child of parent
- **:last-child** - Last child of parent
- **:nth-child(n)** - nth child
  - Can use numbers: :nth-child(2)
  - Can use keywords: :nth-child(odd), :nth-child(even)
  - Can use formulas: :nth-child(3n)

## Visual Result

- Hover: Box changes color when mouse is over it
- Active: Box shrinks slightly when clicked
- Focus: Input field gets blue border and glow when focused
- Link states: Colors change based on link history and interaction
- First/Last child: First and last items have different backgrounds
- nth-child: Different items are styled based on their position

Pseudo classes are incredibly useful for creating dynamic, interactive interfaces.