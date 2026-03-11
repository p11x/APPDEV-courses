# HTML Integration with CSS, JavaScript, and Angular

## Topic Title
Connecting HTML with CSS, JavaScript, and Frameworks

## Concept Explanation

### How HTML Works with Other Technologies

HTML is the foundation that CSS styles and JavaScript enhances. Modern frameworks like Angular extend HTML with additional capabilities.

### Integration Methods

1. **CSS Integration**
   - Inline styles
   - Internal stylesheet
   - External stylesheet
   - CSS-in-JS

2. **JavaScript Integration**
   - Inline JavaScript
   - External script file
   - Module scripts

3. **Angular Templates**
   - Component templates
   - Directives
   - Data binding

## Code Examples

### Example 1: CSS Integration

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>CSS Integration</title>
    
    <!-- External CSS (preferred) -->
    <link rel="stylesheet" href="styles.css">
    
    <!-- Internal CSS -->
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
        }
        .highlight {
            background-color: yellow;
        }
    </style>
</head>
<body>
    <!-- Inline style (avoid) -->
    <p style="color: blue;">Blue text</p>
    
    <!-- Using class -->
    <p class="highlight">Highlighted text</p>
</body>
</html>
```

### Example 2: JavaScript Integration

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>JavaScript Integration</title>
</head>
<body>
    <h1 id="title">Hello</h1>
    <button id="btn">Click Me</button>
    
    <!-- Internal JavaScript -->
    <script>
        // DOM manipulation
        document.getElementById('btn').addEventListener('click', function() {
            document.getElementById('title').textContent = 'Button Clicked!';
        });
    </script>
    
    <!-- External JavaScript -->
    <script src="script.js"></script>
    
    <!-- Module script -->
    <script type="module">
        import { functionName } from './module.js';
    </script>
</body>
</html>
```

### Example 3: Complete HTML with CSS and JS

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Complete Example</title>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <header>
        <nav>
            <ul id="menu">
                <li><a href="#" data-page="home">Home</a></li>
                <li><a href="#" data-page="about">About</a></li>
                <li><a href="#" data-page="contact">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <main id="content">
        <h1 id="page-title">Welcome</h1>
        <p id="page-content">Click a menu item to load content.</p>
    </main>
    
    <footer>
        <p>&copy; 2024</p>
    </footer>
    
    <script src="app.js"></script>
</body>
</html>
```

### Example 4: Angular Component Template

```html
<!-- app.component.html -->
<div class="container">
    <header>
        <h1>{{ title }}</h1>
        <nav>
            <a *ngFor="let item of menuItems" 
               [routerLink]="item.link">
                {{ item.label }}
            </a>
        </nav>
    </header>
    
    <main>
        <!-- Property binding -->
        <img [src]="imageUrl" [alt]="imageAlt">
        
        <!-- Event binding -->
        <button (click)="onSubmit()">Submit</button>
        
        <!-- Two-way binding -->
        <input [(ngModel)]="username" 
               type="text" 
               placeholder="Enter name">
        
        <!-- Conditional rendering -->
        <div *ngIf="isLoggedIn">
            Welcome, {{ username }}!
        </div>
        
        <!-- List rendering -->
        <ul>
            <li *ngFor="let item of items; let i = index">
                {{ i + 1 }}. {{ item.name }}
            </li>
        </ul>
    </main>
    
    <footer>
        <p>&copy; {{ currentYear }}</p>
    </footer>
</div>
```

### Example 5: Angular Component (TypeScript)

```typescript
// app.component.ts
import { Component } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'My Angular App';
    username = '';
    isLoggedIn = false;
    currentYear = new Date().getFullYear();
    
    menuItems = [
        { label: 'Home', link: '/home' },
        { label: 'About', link: '/about' },
        { label: 'Contact', link: '/contact' }
    ];
    
    items = [
        { name: 'Item 1' },
        { name: 'Item 2' },
        { name: 'Item 3' }
    ];
    
    onSubmit() {
        console.log('Form submitted!');
    }
}
```

## Best Practices

### HTML Best Practices

1. **Semantic HTML** - Use proper elements
2. **Clean structure** - Well-organized code
3. **Separation of concerns** - HTML, CSS, JS separate

### CSS Best Practices

1. **External stylesheets** - Reusable across pages
2. **Use classes** - Not IDs for styling
3. **Responsive design** - Mobile-friendly

### JavaScript Best Practices

1. **External files** - Separate from HTML
2. **Use defer/async** - Don't block rendering
3. **Modern ES6+** - Clean, modern syntax

### Angular Best Practices

1. **Component-based** - Break into components
2. **Smart/Dumb components** - Separation of concerns
3. **Use AsyncPipe** - Handle observables

## Common Mistakes

### Mistake 1: Inline Styles

```html
<!-- Wrong -->
<p style="color: red;">Text</p>

<!-- Correct -->
<p class="text-danger">Text</p>
```

### Mistake 2: Blocking Scripts

```html
<!-- Wrong - in head -->
<head>
    <script src="script.js"></script>
</head>

<!-- Correct - at end of body -->
<body>
    <script src="script.js"></script>
</body>
```

### Mistake 3: No Semantic Structure

```html
<!-- Wrong -->
<div class="header"></div>
<div class="main"></div>
<div class="footer"></div>

<!-- Correct -->
<header></header>
<main></main>
<footer></footer>
```

## Exercises

### Exercise 1: Separate CSS and JS
Create a page with external CSS and JS files.

### Exercise 2: Simple Angular Component
Create a basic Angular component with template.

### Exercise 3: Event Binding
Add click handlers to Angular template.

## Mini Practice Tasks

### Task 1: Link CSS
Add external stylesheet link.

### Task 2: Add Script
Add external JavaScript file.

### Task 3: Add Class
Add CSS class to element.
