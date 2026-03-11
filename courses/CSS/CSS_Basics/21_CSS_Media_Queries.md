# CSS Media Queries

## Definition

CSS media queries are a powerful tool that allows you to apply different styles based on the characteristics of the user's device or screen. The most common use is detecting screen width to create responsive layouts that adapt to different devices. Media queries let you say "if the screen is this wide, use these styles" - essential for mobile-friendly websites.

## Key Points

- @media starts a media query
- (max-width: 768px) means "when screen is 768px or less"
- (min-width: 768px) means "when screen is 768px or more"
- You can combine conditions with "and"
- Common breakpoints: 480px (mobile), 768px (tablet), 1024px (desktop)
- Mobile-first: write base styles, then add min-width queries for larger screens
- Desktop-first: write base styles, then add max-width queries for smaller screens

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Media Queries</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        * {
            box-sizing: border-box;
        }
        
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
        }
        
        /* Base styles (apply to all screen sizes) */
        .box {
            padding: 30px;
            text-align: center;
            color: white;
            font-weight: bold;
            margin-bottom: 20px;
        }
        
        .header {
            background-color: #3498db;
        }
        
        .sidebar {
            background-color: #e74c3c;
            padding: 20px;
            margin-bottom: 20px;
        }
        
        .content {
            background-color: #2ecc71;
            padding: 20px;
            margin-bottom: 20px;
        }
        
        .footer {
            background-color: #9b59b6;
        }
        
        /* Desktop layout: side by side */
        .main-layout {
            display: flex;
            gap: 20px;
            margin-bottom: 20px;
        }
        
        .sidebar-section {
            flex: 1;
        }
        
        .content-section {
            flex: 3;
        }
        
        /* BASE STYLES - for mobile first */
        .header, .footer {
            padding: 20px;
        }
        
        /* Media Query for tablets (768px and up) */
        @media (min-width: 768px) {
            .header h1 {
                font-size: 24px;
            }
        }
        
        /* Media Query for desktops (1024px and up) */
        @media (min-width: 1024px) {
            .header h1 {
                font-size: 36px;
            }
        }
        
        /* Max-width example - applies when screen is smaller than 768px */
        @media (max-width: 768px) {
            .main-layout {
                flex-direction: column;
            }
            
            .header, .footer {
                background-color: #2c3e50;
            }
        }
        
        /* Multiple conditions */
        @media (min-width: 768px) and (max-width: 1024px) {
            .content {
                background-color: #f39c12;
            }
        }
    </style>
</head>
<body>
    <header class="box header">
        <h1>Media Queries Demo</h1>
    </header>
    
    <div class="main-layout">
        <aside class="sidebar sidebar-section">
            <h2>Sidebar</h2>
            <p>Resize the browser window to see how this layout adapts!</p>
        </aside>
        
        <main class="content content-section">
            <h2>Main Content</h2>
            <p>This is the main content area. Try resizing your browser window to see how the layout changes.</p>
            <p><strong>Desktop (>1024px):</strong> Orange content, large header</p>
            <p><strong>Tablet (768-1024px):</strong> Orange content</p>
            <p><strong>Mobile (<768px):</strong> Stacked layout, dark header</p>
        </main>
    </div>
    
    <footer class="box footer">
        <p>Footer - Always at the bottom</p>
    </footer>
</body>
</html>
```

## Explanation

### Basic Syntax

```css
@media (condition) {
    /* styles to apply when condition is true */
}
```

### Common Conditions

- **(max-width: 768px)** - Screen is 768px or less
- **(min-width: 768px)** - Screen is 768px or more
- **(min-width: 768px) and (max-width: 1024px)** - Between two sizes

### Common Breakpoints

```css
/* Small devices (phones) */
@media (max-width: 480px) { }

/* Medium devices (tablets) */
@media (max-width: 768px) { }

/* Large devices (desktops) */
@media (max-width: 1024px) { }

/* Extra large devices */
@media (min-width: 1200px) { }
```

### Mobile-First vs Desktop-First

**Mobile-First** (recommended):
```css
/* Base styles for mobile */
.container { width: 100%; }

@media (min-width: 768px) {
    .container { width: 750px; }
}

@media (min-width: 1024px) {
    .container { width: 960px; }
}
```

**Desktop-First**:
```css
/* Base styles for desktop */
.container { width: 960px; }

@media (max-width: 1024px) {
    .container { width: 750px; }
}
```

## Visual Result

- On mobile (<768px): Layout stacks vertically, dark header, side-by-side doesn't apply
- On tablet (768-1024px): Side-by-side layout, orange content background
- On desktop (>1024px): Side-by-side, green content, larger header text
- Resize the browser to see all the changes in action

Media queries are the key to making responsive, mobile-friendly websites.