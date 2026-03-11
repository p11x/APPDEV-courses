# CSS Backgrounds

## Definition

CSS background properties control what appears behind your content. You can set solid background colors, add background images, control how images repeat (tile), and position them exactly where you want. Backgrounds are essential for creating visually interesting webpages and can dramatically affect the overall look and feel of a design.

## Key Points

- background-color sets a solid color behind an element
- background-image adds an image using a URL path
- background-repeat controls if/how an image tiles horizontally and vertically
- background-position sets the starting position of the image
- background-size controls the image dimensions
- Multiple background properties can be combined in shorthand
- Backgrounds only show behind the element's content and padding (not margin)

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Backgrounds</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f0f0f0;
        }
        
        /* Simple background color */
        .bg-color {
            background-color: lightblue;
            padding: 40px;
            margin: 20px 0;
            text-align: center;
        }
        
        /* Background image example */
        .bg-image {
            /* Using a gradient as placeholder image */
            background-image: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 60px;
            margin: 20px 0;
            text-align: center;
            color: white;
        }
        
        /* Background repeat examples */
        .repeat-x {
            background-image: linear-gradient(90deg, #ff6b6b 10px, transparent 10px);
            background-size: 20px 20px;
            background-repeat: repeat-x;
            padding: 40px;
            margin: 20px 0;
            color: white;
        }
        
        .no-repeat {
            background-image: linear-gradient(135deg, #51cf66 0%, #20c997 100%);
            background-repeat: no-repeat;
            background-position: center;
            padding: 80px;
            margin: 20px 0;
            text-align: center;
            color: white;
        }
        
        /* Background size */
        .cover-size {
            background-image: linear-gradient(135deg, #fcc419 0%, #ff922b 100%);
            background-size: cover;
            background-position: center;
            padding: 100px;
            margin: 20px 0;
            text-align: center;
            color: white;
        }
        
        /* Multiple backgrounds combined */
        .combined-bg {
            background-color: #e9ecef;
            background-image: 
                linear-gradient(45deg, #ccc 25%, transparent 25%),
                linear-gradient(-45deg, #ccc 25%, transparent 25%),
                linear-gradient(45deg, transparent 75%, #ccc 75%),
                linear-gradient(-45deg, transparent 75%, #ccc 75%);
            background-size: 20px 20px;
            background-position: 0 0, 0 10px, 10px -10px, -10px 0px;
            padding: 60px;
            margin: 20px 0;
            text-align: center;
        }
        
        h1 {
            text-align: center;
            color: #333;
        }
        
        h2 {
            color: #555;
            margin-top: 30px;
        }
    </style>
</head>
<body>
    <h1>CSS Backgrounds</h1>
    
    <h2>1. Background Color</h2>
    <div class="bg-color">
        This box has a solid light blue background color.
    </div>
    
    <h2>2. Background Image (Gradient)</h2>
    <div class="bg-image">
        This box uses a gradient as a background image. Gradients are commonly used as background images in modern web design.
    </div>
    
    <h2>3. Repeat X (Horizontal)</h2>
    <div class="repeat-x">
        This box has a background that repeats only horizontally.
    </div>
    
    <h2>4. No Repeat with Center Position</h2>
    <div class="no-repeat">
        This box has a background image that doesn't repeat and is centered.
    </div>
    
    <h2>5. Cover Size</h2>
    <div class="cover-size">
        The background image covers the entire element using "cover".
    </div>
    
    <h2>6. Combined Multiple Backgrounds</h2>
    <div class="combined-bg">
        This box uses multiple background layers to create a checkerboard pattern.
    </div>
</body>
</html>
```

## Explanation

### background-color

- **background-color: lightblue;** - Sets a solid color behind the element
- Works the same as the color property but for backgrounds

### background-image

- **background-image: linear-gradient(...);** - Creates a color gradient treated as an image
- For real images, you'd use: `background-image: url('image.jpg');`
- Gradients are popular because they're lightweight and look modern

### background-repeat

- **background-repeat: repeat-x;** - Tiles image only horizontally
- **background-repeat: no-repeat;** - Shows image only once
- **background-repeat: repeat;** - Default, tiles in both directions
- **background-repeat: repeat-y;** - Tiles image only vertically

### background-position

- **background-position: center;** - Centers the image
- Can use keywords: left, right, top, bottom, center
- Can use pixel values: `background-position: 20px 10px;`

### background-size

- **background-size: cover;** - Scales image to cover entire element (may crop)
- **background-size: contain;** - Scales image to fit entirely (may leave space)
- **background-size: 100px 50px;** - Sets specific width and height

### Multiple Backgrounds

- Can layer multiple backgrounds by separating with commas
- First listed appears on top
- Creates complex patterns from simple images

## Visual Result

- Each example box has distinct visual styling
- The color box is solid light blue
- The gradient box shows a modern purple-blue diagonal gradient
- The repeat-x box shows horizontal striped lines
- The no-repeat box has a centered green gradient
- The cover box appears filled with the gradient
- The combined box shows a subtle checkerboard pattern
- All boxes have proper padding making the text readable
- The overall page demonstrates multiple background techniques

This shows how versatile CSS backgrounds are for creating visual effects without needing image editing software.