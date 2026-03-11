# CSS Display Property

## Definition

The CSS display property determines how an element is displayed in the document flow. It controls whether an element behaves as a block (taking full width, stacking vertically), inline (sitting beside other elements, only taking as much width as needed), inline-block (combining features of both), or none (completely hidden). Understanding display is fundamental to CSS layout.

## Key Points

- display: block makes elements take full width and stack vertically
- display: inline makes elements flow horizontally, width determined by content
- display: inline-block combines block and inline behavior
- display: none completely hides an element (takes no space)
- display: flex enables flexible box layout
- display: grid enables grid layout
- Block elements always start on a new line; inline elements don't

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Display Property</title>
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
        
        .demo-box {
            background-color: #3498db;
            color: white;
            padding: 10px;
            margin: 5px;
            border: 2px solid #2980b9;
        }
        
        /* Display Block */
        .display-block span {
            display: block;
            background-color: #e74c3c;
            padding: 10px;
            margin: 5px;
        }
        
        /* Display Inline */
        .display-inline div {
            display: inline;
            background-color: #2ecc71;
            padding: 10px;
            margin: 5px;
        }
        
        /* Display Inline-Block */
        .display-inline-block div {
            display: inline-block;
            width: 100px;
            background-color: #9b59b6;
            padding: 15px;
            margin: 5px;
            text-align: center;
        }
        
        /* Display None */
        .display-none-example {
            display: none;
        }
        
        /* Nav menu example */
        .nav-menu {
            background-color: #2c3e50;
            padding: 0;
            margin: 0;
            list-style: none;
        }
        
        .nav-menu li {
            display: inline-block;
            padding: 15px 20px;
            color: white;
        }
        
        .nav-menu li:hover {
            background-color: #34495e;
        }
        
        .section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
    </style>
</head>
<body>
    <h1>CSS Display Property</h1>
    
    <h2>1. Display: Block (Default for div, p, h1, etc.)</h2>
    <div class="section display-block">
        <span>Block 1 - Takes full width</span>
        <span>Block 2 - Starts on new line</span>
        <span>Block 3 - Also on new line</span>
    </div>
    
    <h2>2. Display: Inline (Default for span, a, etc.)</h2>
    <div class="section display-inline">
        <div>Inline 1</div>
        <div>Inline 2</div>
        <div>Inline 3</div>
    </div>
    
    <h2>3. Display: Inline-Block</h2>
    <div class="section display-inline-block">
        <div>Box 1</div>
        <div>Box 2</div>
        <div>Box 3</div>
    </div>
    
    <h2>4. Display: None (Hidden)</h2>
    <div class="section">
        <p>This text is visible</p>
        <p class="display-none-example">This text is hidden with display: none</p>
        <p>This text is also visible</p>
    </div>
    
    <h2>5. Practical Example: Navigation Menu</h2>
    <ul class="nav-menu">
        <li>Home</li>
        <li>About</li>
        <li>Services</li>
        <li>Contact</li>
    </ul>
    
    <h2>Display Property Comparison</h2>
    <table border="1" cellpadding="10" style="border-collapse: collapse; width: 100%; max-width: 600px;">
        <tr style="background-color: #3498db; color: white;">
            <th>Display Value</th>
            <th>Full Width?</th>
            <th>New Line?</th>
            <th>Width/Height?</th>
        </tr>
        <tr>
            <td>block</td>
            <td>Yes</td>
            <td>Yes</td>
            <td>Yes</td>
        </tr>
        <tr>
            <td>inline</td>
            <td>No</td>
            <td>No</td>
            <td>No</td>
        </tr>
        <tr>
            <td>inline-block</td>
            <td>No</td>
            <td>No</td>
            <td>Yes</td>
        </tr>
        <tr>
            <td>none</td>
            <td>-</td>
            <td>-</td>
            <td>-</td>
        </tr>
    </table>
</body>
</html>
```

## Explanation

### Display: Block

- Elements take up the full available width
- Always start on a new line (stack vertically)
- Default for: `<div>`, `<p>`, `<h1>`-`<h6>`, `<li>`, `<section>`
- Can set width and height

### Display: Inline

- Elements only take up as much width as their content
- Flow horizontally (don't start new lines)
- Default for: `<span>`, `<a>`, `<strong>`, `<em>`
- Cannot set width and height (ignored)

### Display: Inline-Block

- Combines features of both: flow inline but accept width/height
- Great for navigation menus and button layouts
- Sits beside other elements but can have custom sizes

### Display: None

- Completely hides the element
- Element takes no space in the layout
- Different from visibility: hidden (which hides but keeps space)

### Common Use Cases

- **display: inline-block** - Navigation menus, buttons, toolbars
- **display: block** - Layout sections, containers
- **display: none** - Hide elements dynamically (with JavaScript)

## Visual Result

- Block elements stack vertically with full width
- Inline elements flow horizontally but ignore width/height
- Inline-block elements flow horizontally AND respect width/height
- The hidden element doesn't show or take any space
- Navigation menu demonstrates practical inline-block usage
- The comparison table shows the key differences clearly

The display property is fundamental to understanding CSS layout.