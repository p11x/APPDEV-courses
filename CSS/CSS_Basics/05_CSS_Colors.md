# CSS Colors

## Definition

CSS colors allow you to add color to text, backgrounds, borders, and other elements. There are several ways to specify colors in CSS: using color names (like "red" or "blue"), hexadecimal codes (like "#FF0000"), RGB values (like "rgb(255, 0, 0)"), and RGBA which adds transparency. Understanding color will help you create visually appealing webpages.

## Key Points

- Color names are easy to use but limited (like "red", "blue", "green")
- Hexadecimal codes use # followed by 6 characters (0-9 and A-F)
- RGB uses three numbers from 0-255 for red, green, and blue
- RGBA adds a fourth number (0-1) for transparency
- Hex shorthand uses 3 characters for simpler colors
- Higher RGB values mean more of that color (255 is maximum)
- RGBA alpha of 0 is fully transparent, 1 is fully opaque

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Colors</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
            background-color: #f5f5f5;
        }
        
        /* Color name example */
        .color-name {
            color: crimson;
        }
        
        /* Hexadecimal color example */
        .hex-color {
            color: #2E86AB;
        }
        
        /* Hex shorthand example - easier to write */
        .hex-shorthand {
            color: #F00; /* Same as #FF0000 (red) */
        }
        
        /* RGB color example */
        .rgb-color {
            color: rgb(46, 134, 171);
        }
        
        /* RGBA color with transparency example */
        .rgba-color {
            color: rgba(46, 134, 171, 0.5);
        }
        
        /* Background colors */
        .bg-red {
            background-color: #ff6b6b;
            color: white;
            padding: 15px;
            margin: 10px 0;
        }
        
        .bg-green {
            background-color: #51cf66;
            color: white;
            padding: 15px;
            margin: 10px 0;
        }
        
        .bg-blue {
            background-color: #339af0;
            color: white;
            padding: 15px;
            margin: 10px 0;
        }
        
        /* Semi-transparent background */
        .bg-transparent {
            background-color: rgba(255, 0, 0, 0.3);
            padding: 15px;
            margin: 10px 0;
        }
        
        h1 {
            color: #333;
        }
    </style>
</head>
<body>
    <h1>CSS Colors</h1>
    
    <h2>Text Colors</h2>
    <p class="color-name">This text uses a color name: "crimson"</p>
    <p class="hex-color">This text uses hex code: #2E86AB</p>
    <p class="hex-shorthand">This text uses hex shorthand: #F00</p>
    <p class="rgb-color">This text uses RGB: rgb(46, 134, 171)</p>
    <p class="rgba-color">This text uses RGBA with 50% transparency</p>
    
    <h2>Background Colors</h2>
    <div class="bg-red">Red background using #ff6b6b</div>
    <div class="bg-green">Green background using #51cf66</div>
    <div class="bg-blue">Blue background using #339af0</div>
    <div class="bg-transparent">Semi-transparent red background (0.3 alpha)</div>
</body>
</html>
```

## Explanation

### Color Names

- **color: crimson;** - Uses a predefined color name
- Easy to remember and use for common colors
- Limited palette - only about 140 named colors

### Hexadecimal Colors

- **color: #2E86AB;** - Uses hex (hexadecimal) code
- # followed by 6 characters: first two are red, middle two are green, last two are blue
- Each pair ranges from 00 (no color) to FF (maximum color)
- #000000 is black, #FFFFFF is white
- Very precise control over colors

### Hex Shorthand

- **color: #F00;** - Shortened version of #FF0000
- Only works when each pair has the same digit (like #AABBCC)
- Reduces to 3 characters

### RGB Colors

- **color: rgb(46, 134, 171);** - Uses RGB function
- Three numbers: (red, green, blue)
- Each from 0 (none) to 255 (maximum)
- rgb(255, 0, 0) is pure red
- rgb(0, 255, 0) is pure green
- rgb(0, 0, 255) is pure blue

### RGBA Colors

- **color: rgba(46, 134, 171, 0.5);** - Adds alpha channel for transparency
- Fourth number is alpha: 0 (invisible) to 1 (fully visible)
- 0.5 means 50% transparent
- Useful for overlays and layered effects

## Visual Result

- The page has a light gray background for contrast
- The heading appears in dark gray (#333)
- Each text color example shows a different shade: crimson, blue, red, and semi-transparent blue
- The background color boxes show bright, modern colors (red, green, blue)
- The semi-transparent box shows how it blends with the page background
- Text inside colored boxes is white for good readability

This demonstrates that you can use colors for both text and backgrounds, and transparency adds another dimension of visual control.