# SVG Graphics in HTML

## Topic Title
Using SVG Graphics in HTML

## Concept Explanation

### What is SVG?

SVG (Scalable Vector Graphics) is an XML-based format for creating vector graphics. Unlike raster images, SVG scales without losing quality.

### Why Use SVG?

1. **Scalable** - Looks sharp at any size
2. **Small file size** - Often smaller than images
3. **Animatable** - Can be animated with CSS/JS
4. **Editable** - Can be modified with code
5. **Accessible** - Can add titles and descriptions

### SVG Elements

- `<svg>` - Container element
- `<rect>` - Rectangle
- `<circle>` - Circle
- `<ellipse>` - Ellipse
- `<line>` - Line
- `<polyline>` - Multiple connected lines
- `<polygon>` - Multi-sided shape
- `<path>` - Complex paths
- `<text>` - Text
- `<g>` - Grouping

## Code Examples

### Example 1: Basic SVG Shapes

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>SVG Basic Shapes</title>
</head>
<body>
    <h1>SVG Basic Shapes</h1>
    
    <!-- Rectangle -->
    <svg width="200" height="100" style="border: 1px solid #ccc;">
        <rect x="10" y="10" width="150" height="60" fill="blue" />
    </svg>
    
    <!-- Circle -->
    <svg width="200" height="200" style="border: 1px solid #ccc;">
        <circle cx="100" cy="100" r="80" fill="red" />
    </svg>
    
    <!-- Line -->
    <svg width="200" height="100" style="border: 1px solid #ccc;">
        <line x1="10" y1="10" x2="180" y2="80" stroke="green" stroke-width="5" />
    </svg>
</body>
</html>
```

### Example 2: SVG with Text and Styling

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>SVG Text</title>
</head>
<body>
    <svg width="400" height="200">
        <!-- Background -->
        <rect width="100%" height="100%" fill="#f0f0f0"/>
        
        <!-- Text -->
        <text x="50%" y="50%" 
              font-family="Arial" 
              font-size="40" 
              fill="#333" 
              text-anchor="middle" 
              dominant-baseline="middle">
            Hello SVG!
        </text>
    </svg>
</body>
</html>
```

### Example 3: SVG Icon

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>SVG Icon</title>
</head>
<body>
    <!-- Home Icon -->
    <svg width="50" height="50" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/>
        <polyline points="9 22 9 12 15 12 15 22"/>
    </svg>
    
    <!-- User Icon -->
    <svg width="50" height="50" viewBox="0 0 24 24" fill="none" stroke="blue" stroke-width="2">
        <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
        <circle cx="12" cy="7" r="4"/>
    </svg>
    
    <!-- Search Icon -->
    <svg width="50" height="50" viewBox="0 0 24 24" fill="none" stroke="green" stroke-width="2">
        <circle cx="11" cy="11" r="8"/>
        <line x1="21" y1="21" x2="16.65" y2="16.65"/>
    </svg>
</body>
</html>
```

### Example 4: Inline SVG

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Inline SVG</title>
    <style>
        .logo {
            width: 100px;
            height: 100px;
        }
        .logo:hover {
            transform: scale(1.1);
        }
    </style>
</head>
<body>
    <svg class="logo" viewBox="0 0 100 100">
        <circle cx="50" cy="50" r="45" fill="#3498db"/>
        <text x="50" y="60" 
              text-anchor="middle" 
              fill="white" 
              font-size="40" 
              font-family="Arial">
            ABC
        </text>
    </svg>
</body>
</html>
```

## Best Practices

1. **Use inline SVG for icons** - Reduces HTTP requests
2. **Set viewBox** - Makes SVG responsive
3. **Add accessibility** - Include title and desc elements
4. **Optimize SVG** - Remove unnecessary data

## Common Mistakes

### Wrong: No ViewBox
```html
<!-- Wrong -->
<svg width="100" height="100">
<!-- Correct -->
<svg width="100" height="100" viewBox="0 0 100 100">
```

## Exercises

### Exercise 1: Create Icon
Create an SVG icon for a social media platform.

### Exercise 2: Create Banner
Create an SVG banner with text and shapes.
