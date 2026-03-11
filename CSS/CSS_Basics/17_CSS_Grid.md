# CSS Grid

## Definition

CSS Grid Layout is a two-dimensional layout system designed for complex layouts with rows AND columns at the same time. Unlike Flexbox which is one-dimensional (either row OR column), Grid lets you create complete page layouts with precise control over both dimensions. It's perfect for photo galleries, dashboards, and overall page structure.

## Key Points

- display: grid turns a container into a grid container
- grid-template-columns defines how many columns and their widths
- grid-template-rows defines row heights
- grid-gap (or gap) creates space between cells
- Grid creates a two-dimensional layout (rows and columns)
- fr unit represents a fraction of available space
- Can position items in specific grid cells

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Grid</title>
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
        
        /* Base grid container */
        .grid-container {
            display: grid;
            background-color: white;
            padding: 10px;
            margin: 10px 0;
            border: 2px solid #ddd;
        }
        
        .grid-item {
            background-color: #3498db;
            color: white;
            padding: 30px;
            text-align: center;
            border-radius: 5px;
            border: 2px solid #2980b9;
        }
        
        /* Column templates */
        .grid-3-cols {
            grid-template-columns: 1fr 1fr 1fr;
        }
        
        .grid-2-cols {
            grid-template-columns: 1fr 2fr;
        }
        
        .grid-fixed-cols {
            grid-template-columns: 100px 100px 100px;
        }
        
        /* Row template */
        .grid-rows {
            grid-template-rows: 100px 100px;
        }
        
        /* Gap */
        .grid-gap {
            gap: 20px;
            grid-template-columns: 1fr 1fr 1fr;
        }
        
        /* Grid areas */
        .grid-areas {
            grid-template-areas: 
                "header header header"
                "sidebar main main"
                "footer footer footer";
        }
        
        .header { grid-area: header; background-color: #e74c3c; }
        .sidebar { grid-area: sidebar; background-color: #2ecc71; }
        .main { grid-area: main; background-color: #3498db; }
        .footer { grid-area: footer; background-color: #9b59b6; }
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
    </style>
</head>
<body>
    <h1>CSS Grid</h1>
    
    <h2>1. Basic 3-Column Grid</h2>
    <div class="demo-section">
        <p>grid-template-columns: 1fr 1fr 1fr (three equal columns)</p>
        <div class="grid-container grid-3-cols">
            <div class="grid-item">1</div>
            <div class="grid-item">2</div>
            <div class="grid-item">3</div>
            <div class="grid-item">4</div>
            <div class="grid-item">5</div>
            <div class="grid-item">6</div>
        </div>
    </div>
    
    <h2>2. Unequal Columns</h2>
    <div class="demo-section">
        <p>grid-template-columns: 1fr 2fr (sidebar is smaller than main content)</p>
        <div class="grid-container grid-2-cols">
            <div class="grid-item">1 (1fr)</div>
            <div class="grid-item">2 (2fr)</div>
            <div class="grid-item">3 (1fr)</div>
            <div class="grid-item">4 (2fr)</div>
        </div>
    </div>
    
    <h2>3. Fixed Column Widths</h2>
    <div class="demo-section">
        <p>grid-template-columns: 100px 100px 100px (fixed widths)</p>
        <div class="grid-container grid-fixed-cols">
            <div class="grid-item">1</div>
            <div class="grid-item">2</div>
            <div class="grid-item">3</div>
        </div>
    </div>
    
    <h2>4. Grid with Gap</h2>
    <div class="demo-section">
        <p>gap: 20px (space between cells)</p>
        <div class="grid-container grid-gap">
            <div class="grid-item">1</div>
            <div class="grid-item">2</div>
            <div class="grid-item">3</div>
            <div class="grid-item">4</div>
            <div class="grid-item">5</div>
            <div class="grid-item">6</div>
        </div>
    </div>
    
    <h2>5. Grid Template Areas</h2>
    <div class="demo-section">
        <p>Using grid-template-areas for page layout</p>
        <div class="grid-container grid-areas">
            <div class="grid-item header">Header</div>
            <div class="grid-item sidebar">Sidebar</div>
            <div class="grid-item main">Main Content</div>
            <div class="grid-item footer">Footer</div>
        </div>
    </div>
</body>
</html>
```

## Explanation

### Getting Started with Grid

- Add `display: grid;` to the parent container
- Use grid-template-columns to define columns
- Use grid-template-rows to define rows
- All children become grid items automatically

### Grid Template Columns

- **grid-template-columns: 1fr 1fr 1fr;** - Three equal columns
- **fr** means "fraction" - 1fr takes 1 part of available space
- **grid-template-columns: 100px 100px;** - Fixed pixel widths

### Grid Template Rows

- **grid-template-rows: 100px 200px;** - First row 100px, second 200px
- If more items than rows, they auto-create new rows

### Gap

- **gap: 20px;** - Creates 20px space between all rows and columns
- Can also use row-gap and column-gap separately

### Grid Template Areas

- **grid-template-areas:** - Define named areas in a visual way
- Creates easy-to-understand page layouts
- Very useful for responsive design

### When to Use Grid vs Flexbox

- **Flexbox**: One-dimensional layouts (navigation, button groups)
- **Grid**: Two-dimensional layouts (page structure, galleries, dashboards)
- Both can be used together!

## Visual Result

- 3-column grid shows equal-width columns
- Unequal columns show 1fr vs 2fr proportions
- Fixed columns show precise widths
- Gap example shows spacing between cells
- Grid areas demonstrate a complete page layout

CSS Grid is powerful for creating complex, precise two-dimensional layouts.