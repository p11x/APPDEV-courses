# CSS Responsive Design

## Definition

Responsive design is an approach to web design that makes web pages work well on all devices and screen sizes. Instead of creating separate websites for different devices, responsive design uses CSS to adapt the layout based on the screen size. This ensures a good user experience whether someone visits on a phone, tablet, or desktop computer.

## Key Points

- Responsive design adapts to different screen sizes
- Mobile-first means designing for small screens first, then expanding
- Use relative units (%, em, rem) instead of fixed pixels where possible
- Flexible images resize with their container
- Media queries change styles at different breakpoints
- The viewport meta tag is essential for mobile devices
- Test on multiple devices or use browser developer tools

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Responsive Design</title>
    <!-- Important: This meta tag makes the page work properly on mobile -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        * {
            box-sizing: border-box;
        }
        
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }
        
        h1 {
            text-align: center;
            color: #333;
        }
        
        /* Basic container */
        .container {
            max-width: 1200px;
            margin: 0 auto;
            background-color: white;
            padding: 20px;
        }
        
        /* Header */
        header {
            background-color: #3498db;
            color: white;
            padding: 20px;
            text-align: center;
        }
        
        /* Navigation */
        nav {
            background-color: #2c3e50;
            padding: 10px;
        }
        
        nav a {
            color: white;
            text-decoration: none;
            padding: 10px 15px;
            display: inline-block;
        }
        
        /* Main content */
        .content {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            padding: 20px 0;
        }
        
        /* Sidebar */
        .sidebar {
            flex: 1;
            min-width: 200px;
            background-color: #e74c3c;
            color: white;
            padding: 20px;
        }
        
        /* Main area */
        .main {
            flex: 3;
            min-width: 300px;
            background-color: #2ecc71;
            color: white;
            padding: 20px;
        }
        
        /* Cards */
        .cards {
            display: flex;
            flex-wrap: wrap;
            gap: 20px;
            padding: 20px 0;
        }
        
        .card {
            flex: 1;
            min-width: 250px;
            background-color: #9b59b6;
            color: white;
            padding: 20px;
            border-radius: 5px;
        }
        
        /* Footer */
        footer {
            background-color: #34495e;
            color: white;
            padding: 20px;
            text-align: center;
        }
        
        /* Simple media query example */
        @media (max-width: 768px) {
            .sidebar, .main {
                flex: 100%;
            }
        }
    </style>
</head>
<body>
    <div class="container">
        <header>
            <h1>Responsive Website</h1>
        </header>
        
        <nav>
            <a href="#">Home</a>
            <a href="#">About</a>
            <a href="#">Services</a>
            <a href="#">Contact</a>
        </nav>
        
        <div class="content">
            <aside class="sidebar">
                <h3>Sidebar</h3>
                <p>On desktop, this is on the left side.</p>
                <p>On mobile, this moves above the main content.</p>
            </aside>
            
            <main class="main">
                <h2>Main Content</h2>
                <p>This is the main content area of the webpage. It takes up more space than the sidebar on desktop.</p>
                <p>The layout adapts based on screen size using CSS flexbox and media queries.</p>
            </main>
        </div>
        
        <div class="cards">
            <div class="card">
                <h3>Card 1</h3>
                <p>This card adapts to screen width.</p>
            </div>
            <div class="card">
                <h3>Card 2</h3>
                <p>Cards stack on mobile.</p>
            </div>
            <div class="card">
                <h3>Card 3</h3>
                <p>And sit side by side on desktop.</p>
            </div>
        </div>
        
        <footer>
            <p>&copy; 2024 Responsive Design Demo</p>
        </footer>
    </div>
</body>
</html>
```

## Explanation

### Viewport Meta Tag

```html
<meta name="viewport" content="width=device-width, initial-scale=1.0">
```
- This line is essential for responsive design
- It tells mobile browsers to use the actual device width
- Without it, mobile phones zoom out to show the whole page

### Flexible Layouts

- Use `max-width` instead of fixed width for containers
- Use flexbox or grid for flexible layouts
- Use percentages or flex units instead of fixed pixels

### Flexible Images

```css
img {
    max-width: 100%;
    height: auto;
}
```
- max-width: 100% prevents image from exceeding container
- height: auto maintains aspect ratio

### Responsive Units

- Use `%` for widths (relative to parent)
- Use `rem` for fonts (relative to root)
- Use `vw` and `vh` for viewport-based sizing

### Media Queries

```css
@media (max-width: 768px) {
    /* Styles for screens smaller than 768px */
}
```
- Change layout at specific breakpoints
- We'll learn more about this in the next lesson

## Visual Result

- On desktop: Sidebar on left, main content on right, cards side by side
- On mobile: All elements stack vertically, full width
- Header and navigation adapt to available space
- Cards resize or stack based on screen width
- The page is usable on both large and small screens

Responsive design ensures your websites work well for everyone, regardless of their device.