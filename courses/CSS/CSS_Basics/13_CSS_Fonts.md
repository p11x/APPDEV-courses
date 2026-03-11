# CSS Fonts

## Definition

CSS font properties control the typography of your text - the typeface, size, style, and weight. Choosing appropriate fonts and styling them correctly is crucial for readability and establishing the visual personality of your website. Good typography makes content easy to read and helps create a professional appearance.

## Key Points

- font-family specifies which font to use (like Arial, Times New Roman)
- font-size controls how big the text appears
- font-style toggles italic on and off
- font-weight sets how bold or light the text appears
- Always provide fallback fonts in case the first one isn't available
- Font sizes should be readable (16px is a good minimum for body text)
- Use relative units (em, rem) for more flexible responsive designs

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Fonts</title>
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
        
        .font-box {
            background-color: white;
            padding: 20px;
            margin: 10px 0;
            border: 1px solid #ddd;
        }
        
        /* Font Family */
        .font-arial { font-family: Arial, sans-serif; }
        .font-times { font-family: "Times New Roman", serif; }
        .font-courier { font-family: "Courier New", monospace; }
        .font-georgia { font-family: Georgia, "Times New Roman", serif; }
        .font-verdana { font-family: Verdana, sans-serif; }
        
        /* Font Size */
        .size-small { font-size: 12px; }
        .size-medium { font-size: 16px; }
        .size-large { font-size: 24px; }
        .size-xlarge { font-size: 32px; }
        
        /* Font Style */
        .style-normal { font-style: normal; }
        .style-italic { font-style: italic; }
        
        /* Font Weight */
        .weight-light { font-weight: 300; }
        .weight-normal { font-weight: 400; }
        .weight-bold { font-weight: 700; }
        .weight-black { font-weight: 900; }
        
        /* Combined font properties */
        .font-combined {
            font-family: Georgia, "Times New Roman", serif;
            font-size: 20px;
            font-style: italic;
            font-weight: bold;
        }
        
        /* Font shorthand example */
        .font-shorthand {
            font: italic bold 20px Georgia, serif;
        }
    </style>
</head>
<body>
    <h1>CSS Fonts</h1>
    
    <h2>1. Font Family</h2>
    <div class="font-box font-arial">This uses Arial font (sans-serif)</div>
    <div class="font-box font-times">This uses Times New Roman (serif)</div>
    <div class="font-box font-courier">This uses Courier New (monospace)</div>
    <div class="font-box font-georgia">This uses Georgia (elegant serif)</div>
    <div class="font-box font-verdana">This uses Verdana (readable sans-serif)</div>
    
    <h2>2. Font Size</h2>
    <div class="font-box size-small">Small text (12px)</div>
    <div class="font-box size-medium">Medium text (16px) - Good for reading</div>
    <div class="font-box size-large">Large text (24px)</div>
    <div class="font-box size-xlarge">Extra large text (32px)</div>
    
    <h2>3. Font Style</h2>
    <div class="font-box style-normal">Normal style text</div>
    <div class="font-box style-italic">Italic style text - good for emphasis</div>
    
    <h2>4. Font Weight</h2>
    <div class="font-box weight-light">Light weight (300)</div>
    <div class="font-box weight-normal">Normal weight (400)</div>
    <div class="font-box weight-bold">Bold weight (700)</div>
    <div class="font-box weight-black">Black weight (900)</div>
    
    <h2>5. Combined Font Properties</h2>
    <div class="font-box font-combined">
        Combined: Georgia, italic, bold, 20px
    </div>
    
    <h2>6. Font Shorthand</h2>
    <div class="font-box font-shorthand">
        Using font shorthand: font: italic bold 20px Georgia, serif;
    </div>
</body>
</html>
```

## Explanation

### Font Family

- **font-family: Arial, sans-serif;** - Sets the typeface
- The second value (sans-serif) is a fallback if Arial isn't available
- Common font categories: sans-serif, serif, monospace
- Web-safe fonts work on most computers: Arial, Times New Roman, Georgia, Verdana

### Font Size

- **font-size: 16px;** - Sets text size in pixels
- Common sizes: 12px (small), 16px (body), 24px (headings)
- Can also use em, rem, or percentages

### Font Style

- **font-style: italic;** - Makes text italic
- **font-style: normal;** - Removes italic (default)
- Used for emphasizing text or indicating titles

### Font Weight

- **font-weight: bold;** or **font-weight: 700;** - Makes text bold
- Common values: 300 (light), 400 (normal), 700 (bold), 900 (black)
- Can use keywords: lighter, normal, bold, bolder

### Font Shorthand

- Can combine all font properties in one line:
- `font: italic bold 20px Georgia, serif;`
- Order: style weight size family
- Required values: size and family

### Best Practices

- Use readable font sizes (16px minimum for body text)
- Limit to 2-3 different fonts per website
- Always include fallback fonts
- Use font weights consistently for hierarchy

## Visual Result

- Font family examples show different typefaces
- Font size examples show the range from small to extra large
- Font style examples show normal vs italic
- Font weight examples show different thicknesses
- Combined example shows multiple properties working together
- Shorthand example shows the same result with less code

Good typography is essential for creating professional, readable websites.