# CSS Shadow Effects

## Definition

CSS shadow effects add depth and dimension to elements through box-shadow for element shadows and text-shadow for text shadows. Shadows make elements pop out, create depth perception, and add visual interest. They're commonly used on cards, buttons, modals, and floating elements to create a modern, layered appearance.

## Key Points

- box-shadow adds shadows to element boxes
- text-shadow adds shadows to text
- Shadow syntax: offset-x offset-y blur-radius color
- Multiple shadows can be layered with commas
- box-shadow creates realistic 3D effects
- text-shadow improves text readability on complex backgrounds
- Inset shadows appear inside the element

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Shadow Effects</title>
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
            padding: 30px;
            margin: 20px 0;
            border: 1px solid #ddd;
            text-align: center;
        }
        
        /* Basic box shadow */
        .shadow-basic {
            width: 150px;
            height: 100px;
            background-color: #3498db;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 20px auto;
            
            /* box-shadow: horizontal vertical blur color */
            box-shadow: 5px 5px 10px rgba(0,0,0,0.3);
        }
        
        /* Shadow with blur */
        .shadow-blur {
            width: 150px;
            height: 100px;
            background-color: #e74c3c;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 20px auto;
            
            box-shadow: 0 5px 20px rgba(0,0,0,0.3);
        }
        
        /* Shadow with spread */
        .shadow-spread {
            width: 150px;
            height: 100px;
            background-color: #2ecc71;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 20px auto;
            
            /* box-shadow: h v blur spread color */
            box-shadow: 0 0 15px 5px rgba(0,0,0,0.3);
        }
        
        /* Multiple shadows */
        .shadow-multiple {
            width: 150px;
            height: 100px;
            background-color: #9b59b6;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 20px auto;
            
            box-shadow: 5px 5px 0px #8e44ad, 
                        10px 10px 0px #9b59b6;
        }
        
        /* Inset shadow */
        .shadow-inset {
            width: 150px;
            height: 100px;
            background-color: #34495e;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 20px auto;
            
            box-shadow: inset 5px 5px 10px rgba(0,0,0,0.5);
        }
        
        /* Card shadow */
        .card {
            width: 250px;
            padding: 20px;
            background-color: white;
            border-radius: 10px;
            margin: 20px auto;
            
            box-shadow: 0 4px 6px rgba(0,0,0,0.1), 
                        0 1px 3px rgba(0,0,0,0.08);
        }
        
        /* Elevated shadow */
        .elevated {
            width: 150px;
            height: 100px;
            background-color: #3498db;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 20px auto;
            
            box-shadow: 0 10px 20px rgba(0,0,0,0.19), 
                        0 6px 6px rgba(0,0,0,0.23);
        }
        
        /* Text shadow */
        .text-shadow {
            font-size: 40px;
            font-weight: bold;
            color: #333;
            
            /* text-shadow: h v blur color */
            text-shadow: 2px 2px 4px rgba(0,0,0,0.3);
        }
        
        /* Multiple text shadows */
        .text-shadow-multiple {
            font-size: 50px;
            font-weight: bold;
            color: #3498db;
            
            text-shadow: 3px 3px 0px #2980b9,
                         6px 6px 0px rgba(0,0,0,0.2);
        }
        
        /* Glow effect */
        .glow {
            width: 100px;
            height: 100px;
            background-color: #e74c3c;
            border-radius: 50%;
            margin: 20px auto;
            
            box-shadow: 0 0 20px #e74c3c,
                        0 0 40px #e74c3c;
        }
    </style>
</head>
<body>
    <h1>CSS Shadow Effects</h1>
    
    <h2>1. Basic Box Shadow</h2>
    <div class="demo-section">
        <div class="shadow-basic">Basic</div>
    </div>
    
    <h2>2. Shadow with Blur</h2>
    <div class="demo-section">
        <div class="shadow-blur">Blurred</div>
    </div>
    
    <h2>3. Shadow with Spread</h2>
    <div class="demo-section">
        <div class="shadow-spread">Spread</div>
    </div>
    
    <h2>4. Multiple Shadows</h2>
    <div class="demo-section">
        <div class="shadow-multiple">Layered</div>
    </div>
    
    <h2>5. Inset Shadow</h2>
    <div class="demo-section">
        <div class="shadow-inset">Inset</div>
    </div>
    
    <h2>6. Card Style Shadow</h2>
    <div class="demo-section">
        <div class="card">
            <h3>Card Title</h3>
            <p>This card has a subtle, modern shadow effect.</p>
        </div>
    </div>
    
    <h2>7. Elevated Shadow</h2>
    <div class="demo-section">
        <div class="elevated">Elevated</div>
    </div>
    
    <h2>8. Text Shadow</h2>
    <div class="demo-section">
        <div class="text-shadow">Text with Shadow</div>
    </div>
    
    <h2>9. Multiple Text Shadows</h2>
    <div class="demo-section">
        <div class="text-shadow-multiple">3D Text</div>
    </div>
    
    <h2>10. Glow Effect</h2>
    <div class="demo-section">
        <div class="glow"></div>
    </div>
</body>
</html>
```

## Explanation

### Box Shadow Syntax

```css
box-shadow: h-offset v-offset blur spread color;
```

- **h-offset**: Horizontal offset (positive = right)
- **v-offset**: Vertical offset (positive = down)
- **blur**: How blurry the shadow is (0 = sharp)
- **spread**: How far the shadow extends
- **color**: Shadow color (use rgba for transparency)

### Basic Box Shadow

```css
box-shadow: 5px 5px 10px rgba(0,0,0,0.3);
```
- 5px right, 5px down, 10px blur, 30% black

### Inset Shadow

```css
box-shadow: inset 5px 5px 10px rgba(0,0,0,0.5);
```
- Creates shadow inside the element
- Makes element look pressed in

### Text Shadow Syntax

```css
text-shadow: h-offset v-offset blur color;
```

### Multiple Shadows

```css
box-shadow: 5px 5px 0px #color1, 10px 10px 0px #color2;
```
- Layer multiple shadows for more depth

## Visual Result

- Basic: Simple offset shadow
- Blurred: Soft, diffused shadow
- Spread: Shadow extends outward
- Multiple: Layered shadow effect
- Inset: Shadow appears inside the box
- Card: Modern card with subtle shadow
- Elevated: Strong 3D lift effect
- Text Shadow: Text with depth
- Multiple Text: 3D text effect
- Glow: Glowing circle effect

Shadows add depth and polish to any design.