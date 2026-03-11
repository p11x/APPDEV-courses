# CSS Transitions

## Definition

CSS transitions allow you to change property values smoothly over a specified duration instead of instantly. When a property changes (like on hover), transitions make the change gradual and visually pleasing. Transitions are perfect for buttons, links, and interactive elements that change when you interact with them.

## Key Points

- transition makes property changes smooth instead of instant
- transition-duration specifies how long the transition takes (in seconds or ms)
- transition-property specifies which properties to animate
- transition-timing-function controls the speed curve (ease, linear, etc.)
- transition is a shorthand for all transition properties
- Common use: hover effects on buttons and links
- Not all CSS properties can be transitioned

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Transitions</title>
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
            padding: 20px;
            margin: 20px 0;
            background-color: white;
            border: 1px solid #ddd;
        }
        
        /* Button with transition */
        .btn {
            display: inline-block;
            padding: 15px 30px;
            background-color: #3498db;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            
            /* The transition property */
            transition: background-color 0.3s ease;
        }
        
        .btn:hover {
            background-color: #2980b9;
        }
        
        /* Multiple properties */
        .btn-multiple {
            display: inline-block;
            padding: 15px 30px;
            background-color: #2ecc71;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            margin: 10px;
            
            /* Transition multiple properties */
            transition: background-color 0.3s, transform 0.3s, border-radius 0.3s;
        }
        
        .btn-multiple:hover {
            background-color: #27ae60;
            transform: scale(1.05);
            border-radius: 20px;
        }
        
        /* Color transition */
        .color-box {
            width: 100px;
            height: 100px;
            background-color: #e74c3c;
            
            /* Transition: property duration timing-function */
            transition: background-color 0.5s ease-in-out;
        }
        
        .color-box:hover {
            background-color: #3498db;
        }
        
        /* Width transition */
        .width-box {
            height: 50px;
            background-color: #9b59b6;
            width: 100px;
            
            transition: width 0.5s ease;
        }
        
        .width-box:hover {
            width: 300px;
        }
        
        /* Opacity transition */
        .opacity-box {
            width: 100px;
            height: 100px;
            background-color: #f39c12;
            
            transition: opacity 0.5s;
        }
        
        .opacity-box:hover {
            opacity: 0.3;
        }
        
        /* Transform transition */
        .transform-box {
            width: 100px;
            height: 100px;
            background-color: #1abc9c;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
            
            transition: transform 0.3s ease;
        }
        
        .transform-box:hover {
            transform: rotate(180deg) scale(1.2);
        }
        
        /* Transition all */
        .all-transition {
            padding: 20px;
            background-color: #34495e;
            color: white;
            border-radius: 5px;
            
            transition: all 0.3s ease;
        }
        
        .all-transition:hover {
            background-color: #2c3e50;
            transform: translateY(-5px);
            box-shadow: 0 5px 15px rgba(0,0,0,0.3);
        }
    </style>
</head>
<body>
    <h1>CSS Transitions</h1>
    
    <h2>1. Basic Button Transition</h2>
    <div class="demo-box">
        <p>Hover over the button to see the smooth color change:</p>
        <a href="#" class="btn">Hover Me!</a>
    </div>
    
    <h2>2. Multiple Properties</h2>
    <div class="demo-box">
        <p>Hover to see background, size, and shape change:</p>
        <a href="#" class="btn-multiple">Hover Me!</a>
    </div>
    
    <h2>3. Color Transition</h2>
    <div class="demo-box">
        <div class="color-box"></div>
    </div>
    
    <h2>4. Width Transition</h2>
    <div class="demo-box">
        <div class="width-box"></div>
    </div>
    
    <h2>5. Opacity Transition</h2>
    <div class="demo-box">
        <div class="opacity-box"></div>
    </div>
    
    <h2>6. Transform Transition</h2>
    <div class="demo-box">
        <div class="transform-box">Rotate!</div>
    </div>
    
    <h2>7. Transition All</h2>
    <div class="demo-box">
        <div class="all-transition">
            Hover to see all properties transition
        </div>
    </div>
</body>
</html>
```

## Explanation

### Basic Transition Syntax

```css
transition: property duration timing-function;
```

### Components

- **transition-property**: Which property to animate (background-color, width, etc.)
- **transition-duration**: How long (0.3s, 500ms)
- **transition-timing-function**: How the animation speeds up/down:
  - `ease` - Starts slow, speeds up, slows down (default)
  - `linear` - Same speed throughout
  - `ease-in` - Starts slow, speeds up
  - `ease-out` - Starts fast, slows down
  - `ease-in-out` - Starts slow, speeds up, slows down

### Shorthand

```css
/* Long way */
transition-property: background-color;
transition-duration: 0.3s;
transition-timing-function: ease;

/* Shorthand */
transition: background-color 0.3s ease;
```

### Multiple Properties

```css
/* Multiple transitions */
transition: background-color 0.3s, transform 0.3s, border-radius 0.3s;

/* Or transition all */
transition: all 0.3s ease;
```

### Properties That Can Transition

- Colors (background-color, color, border-color)
- Sizes (width, height, font-size)
- Positions (transform, left, top)
- Opacity
- Border radius
- Box shadow

## Visual Result

- Button shows smooth color change on hover
- Multiple property example shows color, size, and shape changing together
- Color box transitions from red to blue smoothly
- Width box grows from 100px to 300px
- Opacity box fades to 30%
- Transform box rotates and scales
- The "all" example shows a complex hover effect with shadow

Transitions make websites feel polished and interactive.