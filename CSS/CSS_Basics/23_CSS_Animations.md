# CSS Animations

## Definition

CSS animations allow you to create more complex, multi-step visual effects than transitions. While transitions go from one state to another, animations can have multiple steps, loop indefinitely, and run automatically without user interaction. Animations are defined using @keyframes which specify what happens at each point in the animation.

## Key Points

- @keyframes defines the animation sequence
- animation-name links the animation to an element
- animation-duration specifies how long one cycle takes
- animation-iteration-count controls how many times it runs (infinite for loops)
- animation-timing-function controls speed curve
- animation-delay starts the animation after a delay
- Animations run automatically, unlike transitions which need triggers

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Animations</title>
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
            text-align: center;
        }
        
        /* Basic animation example */
        @keyframes fadeIn {
            from {
                opacity: 0;
            }
            to {
                opacity: 1;
            }
        }
        
        .fade-in {
            width: 100px;
            height: 100px;
            background-color: #3498db;
            animation: fadeIn 2s;
        }
        
        /* Move animation */
        @keyframes moveRight {
            0% {
                transform: translateX(0);
            }
            50% {
                transform: translateX(100px);
            }
            100% {
                transform: translateX(0);
            }
        }
        
        .move-box {
            width: 50px;
            height: 50px;
            background-color: #e74c3c;
            animation: moveRight 2s ease infinite;
        }
        
        /* Color change animation */
        @keyframes colorChange {
            0% {
                background-color: #e74c3c;
            }
            25% {
                background-color: #f39c12;
            }
            50% {
                background-color: #2ecc71;
            }
            75% {
                background-color: #3498db;
            }
            100% {
                background-color: #e74c3c;
            }
        }
        
        .color-box {
            width: 100px;
            height: 100px;
            animation: colorChange 4s infinite;
        }
        
        /* Rotate animation */
        @keyframes spin {
            from {
                transform: rotate(0deg);
            }
            to {
                transform: rotate(360deg);
            }
        }
        
        .spin-box {
            width: 80px;
            height: 80px;
            background-color: #9b59b6;
            margin: 20px auto;
            animation: spin 2s linear infinite;
        }
        
        /* Pulse animation */
        @keyframes pulse {
            0%, 100% {
                transform: scale(1);
            }
            50% {
                transform: scale(1.1);
            }
        }
        
        .pulse-box {
            width: 100px;
            height: 100px;
            background-color: #1abc9c;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
            animation: pulse 1s ease-in-out infinite;
        }
        
        /* Bounce animation */
        @keyframes bounce {
            0%, 100% {
                transform: translateY(0);
            }
            50% {
                transform: translateY(-50px);
            }
        }
        
        .bounce-box {
            width: 50px;
            height: 50px;
            background-color: #e67e22;
            animation: bounce 1s ease infinite;
        }
        
        /* Combined animation */
        @keyframes complexAnim {
            0% {
                transform: translateX(0) rotate(0deg);
                background-color: #3498db;
            }
            50% {
                transform: translateX(100px) rotate(180deg);
                background-color: #e74c3c;
            }
            100% {
                transform: translateX(0) rotate(360deg);
                background-color: #3498db;
            }
        }
        
        .complex-box {
            width: 50px;
            height: 50px;
            background-color: #3498db;
            animation: complexAnim 3s ease-in-out infinite;
        }
    </style>
</head>
<body>
    <h1>CSS Animations</h1>
    
    <h2>1. Fade In Animation</h2>
    <div class="demo-box">
        <div class="fade-in" style="margin: 0 auto;"></div>
    </div>
    
    <h2>2. Move Animation</h2>
    <div class="demo-box">
        <div class="move-box" style="margin: 0 auto;"></div>
    </div>
    
    <h2>3. Color Change Animation</h2>
    <div class="demo-box">
        <div class="color-box" style="margin: 0 auto;"></div>
    </div>
    
    <h2>4. Spin Animation</h2>
    <div class="demo-box">
        <div class="spin-box"></div>
    </div>
    
    <h2>5. Pulse Animation</h2>
    <div class="demo-box">
        <div class="pulse-box">Pulse!</div>
    </div>
    
    <h2>6. Bounce Animation</h2>
    <div class="demo-box">
        <div class="bounce-box" style="margin: 0 auto;"></div>
    </div>
    
    <h2>7. Complex Animation</h2>
    <div class="demo-box">
        <div class="complex-box" style="margin: 0 auto;"></div>
    </div>
</body>
</html>
```

## Explanation

### Defining @keyframes

```css
@keyframes animationName {
    from {
        /* starting styles */
    }
    to {
        /* ending styles */
    }
}

/* Or using percentages */
@keyframes animationName {
    0% { /* styles at start */ }
    50% { /* styles at middle */ }
    100% { /* styles at end */ }
}
```

### Applying Animation

```css
.element {
    animation-name: animationName;    /* which animation */
    animation-duration: 2s;           /* how long */
    animation-timing-function: ease;  /* speed curve */
    animation-iteration-count: infinite; /* repeat forever */
}
```

### Animation Properties

- **animation-name**: Name of the @keyframes
- **animation-duration**: Time in seconds (s) or milliseconds (ms)
- **animation-timing-function**: ease, linear, ease-in, ease-out, ease-in-out
- **animation-iteration-count**: Number or infinite
- **animation-delay**: Time before starting
- **animation-direction**: normal, reverse, alternate

### Shorthand

```css
animation: name duration timing-function delay iteration-count;
```

## Visual Result

- Fade in: Box gradually becomes visible over 2 seconds
- Move: Box moves right and back continuously
- Color: Box cycles through multiple colors
- Spin: Box rotates continuously like a loading spinner
- Pulse: Box grows and shrinks rhythmically
- Bounce: Box moves up and down like bouncing
- Complex: Combined movement, rotation, and color change

Animations add life and interactivity to websites.