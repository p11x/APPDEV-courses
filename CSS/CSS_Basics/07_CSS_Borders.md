# CSS Borders

## Definition

CSS borders are lines that surround elements, creating visual separation and definition. Borders can be styled in many ways: different thicknesses, various line styles (solid, dashed, dotted), and any color. You can also create rounded corners using border-radius. Borders help elements stand out and create visual hierarchy on your webpage.

## Key Points

- border-width sets how thick the border is (in pixels)
- border-style defines the line type: solid, dashed, dotted, double, none
- border-color sets the border color
- border-radius creates rounded corners (can be different for each corner)
- Borders appear outside the padding but inside the margin
- Shorthand border property combines width, style, and color
- border-left, border-right, border-top, border-bottom target specific sides

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Borders</title>
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
        
        .box {
            padding: 30px;
            margin: 20px;
            background-color: white;
        }
        
        /* Basic border examples */
        .solid-border {
            border: 3px solid #333;
        }
        
        .dashed-border {
            border: 3px dashed #e74c3c;
        }
        
        .dotted-border {
            border: 3px dotted #3498db;
        }
        
        .double-border {
            border: 5px double #27ae60;
        }
        
        /* Different borders on each side */
        .multi-border {
            border-top: 3px solid #9b59b6;
            border-right: 3px dashed #e67e22;
            border-bottom: 3px dotted #1abc9c;
            border-left: 3px double #e74c3c;
        }
        
        /* Rounded corners */
        .rounded-small {
            border: 3px solid #2c3e50;
            border-radius: 5px;
        }
        
        .rounded-medium {
            border: 3px solid #2c3e50;
            border-radius: 15px;
        }
        
        .rounded-full {
            border: 3px solid #2c3e50;
            border-radius: 50%;
            width: 150px;
            height: 150px;
            text-align: center;
            line-height: 150px;
        }
        
        /* Circle example */
        .circle {
            border: 4px solid #3498db;
            border-radius: 50%;
            width: 120px;
            height: 120px;
            background-color: #3498db;
            color: white;
            text-align: center;
            line-height: 120px;
            margin: 20px auto;
        }
        
        h2 {
            color: #555;
            margin-top: 30px;
        }
    </style>
</head>
<body>
    <h1>CSS Borders</h1>
    
    <h2>1. Solid Border</h2>
    <div class="box solid-border">This has a solid border - most common style.</div>
    
    <h2>2. Dashed Border</h2>
    <div class="box dashed-border">This has a dashed border - good for decorative borders.</div>
    
    <h2>3. Dotted Border</h2>
    <div class="box dotted-border">This has a dotted border - often used for emphasis.</div>
    
    <h2>4. Double Border</h2>
    <div class="box double-border">This has a double border - creates two parallel lines.</div>
    
    <h2>5. Different Borders on Each Side</h2>
    <div class="box multi-border">Each side has a different border style!</div>
    
    <h2>6. Rounded Corners</h2>
    <div class="box rounded-small">Slightly rounded (5px)</div>
    <div class="box rounded-medium">More rounded (15px)</div>
    
    <h2>7. Circle</h2>
    <div class="circle">Circle</div>
</body>
</html>
```

## Explanation

### Basic Border Properties

- **border: 3px solid #333;** - The shorthand property
  - 3px is the width (thickness)
  - solid is the style (line type)
  - #333 is the color
- Can also write separately: border-width, border-style, border-color

### Border Styles

- **solid** - A single solid line
- **dashed** - Broken line (rectangular dashes)
- **dotted** - Round or square dots
- **double** - Two parallel solid lines
- **none** - No border (removes default borders)

### Individual Sides

- **border-top** - Only top border
- **border-right** - Only right border
- **border-bottom** - Only bottom border
- **border-left** - Only left border
- Each can have different width, style, and color

### Border Radius

- **border-radius: 5px;** - Rounds all corners equally
- **border-radius: 50%;** - Makes a perfect circle (on square elements)
- Can specify each corner: border-radius: 10px 20px 30px 40px;
  - Order: top-left, top-right, bottom-right, bottom-left
- Rounded corners make elements look friendlier and more modern

## Visual Result

- Each box is clearly defined by its border
- Solid borders appear as solid black lines
- Dashed borders show broken rectangular lines in red
- Dotted borders show round dots in blue
- Double borders show two parallel green lines
- Multi-border shows different styles on each side
- Rounded corners vary from slightly to fully rounded
- The circle demonstrates how border-radius creates circular shapes
- Borders effectively separate and highlight content areas

Borders are essential for creating visual structure and can dramatically change the appearance of any element.