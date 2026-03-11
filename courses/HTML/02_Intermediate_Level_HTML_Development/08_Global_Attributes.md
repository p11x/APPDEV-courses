# Global Attributes

## Topic Title
HTML Global Attributes

## Concept Explanation

### What Are Global Attributes?

Global attributes are attributes that can be used on any HTML element. They provide additional information about elements and are supported across all browsers.

### Common Global Attributes

| Attribute | Purpose |
|-----------|---------|
| `id` | Unique identifier |
| `class` | Element classification |
| `style` | Inline CSS styling |
| `title` | Tooltip information |
| `data-*` | Custom data storage |
| `lang` | Language specification |
| `dir` | Text direction |
| `tabindex` | Keyboard navigation |
| `hidden` | Hide element |
| `contenteditable` | Editable content |
| `draggable` | Drag and drop |

## Why This Concept Is Important

Global attributes matter because:

1. **Styling** - class and style for CSS
2. **Scripting** - id and data-* for JavaScript
3. **Accessibility** - lang, aria-*
4. **User interaction** - draggable, contenteditable
5. **Navigation** - tabindex for keyboard

## Code Examples

### Example 1: ID and Class

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>ID and Class</title>
    <style>
        #header {
            background-color: #333;
            color: white;
            padding: 20px;
        }
        .highlight {
            background-color: yellow;
        }
        .btn {
            padding: 10px 20px;
            border: none;
            cursor: pointer;
        }
        .btn-primary {
            background-color: blue;
            color: white;
        }
    </style>
</head>
<body>
    <header id="header">
        <h1>Website Title</h1>
    </header>
    
    <main>
        <p class="highlight">This paragraph is highlighted.</p>
        
        <button class="btn btn-primary">Primary Button</button>
        <button class="btn">Secondary Button</button>
    </main>
</body>
</html>
```

### Example 2: Data Attributes

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Data Attributes</title>
</head>
<body>
    <h1>Products</h1>
    
    <div class="product" 
         data-id="123" 
         data-name="Wireless Headphones"
         data-price="199.99"
         data-category="electronics">
        <h2>Wireless Headphones</h2>
        <p>Price: $199.99</p>
    </div>
    
    <div class="product" 
         data-id="456" 
         data-name="Smart Watch"
         data-price="299.99"
         data-category="electronics">
        <h2>Smart Watch</h2>
        <p>Price: $299.99</p>
    </div>
    
    <script>
        // Accessing data attributes
        const products = document.querySelectorAll('.product');
        
        products.forEach(product => {
            console.log('ID:', product.dataset.id);
            console.log('Name:', product.dataset.name);
            console.log('Price:', product.dataset.price);
            console.log('Category:', product.dataset.category);
        });
    </script>
</body>
</html>
```

### Example 3: Title and Tooltips

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Title Attribute</title>
</head>
<body>
    <!-- Basic tooltip -->
    <p title="This is a tooltip">Hover over this text</p>
    
    <!-- Links with titles -->
    <a href="page.html" title="Go to our about page">About Us</a>
    
    <!-- Images with titles -->
    <img src="photo.jpg" alt="Mountain" title="Beautiful mountain view">
    
    <!-- Abbreviations -->
    <p>The <abbr title="HyperText Markup Language">HTML</abbr> is used for web pages.</p>
</body>
</html>
```

### Example 4: Hidden and Contenteditable

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Hidden and Contenteditable</title>
    <style>
        .edit-area {
            padding: 10px;
            border: 1px solid #ccc;
            min-height: 50px;
        }
        .edit-area:focus {
            outline: 2px solid blue;
        }
    </style>
</head>
<body>
    <!-- Hidden element -->
    <p hidden>This paragraph is hidden</p>
    
    <!-- Show when needed -->
    <div id="error-message" hidden>
        An error occurred. Please try again.
    </div>
    <button onclick="document.getElementById('error-message').hidden = false">
        Show Error
    </button>
    
    <!-- Contenteditable -->
    <h2>Editable Content</h2>
    <div class="edit-area" contenteditable="true">
        Click here to edit this text!
    </div>
    
    <!-- Save button -->
    <button onclick="saveContent()">Save</button>
    
    <script>
        function saveContent() {
            const content = document.querySelector('.edit-area').innerHTML;
            console.log('Saved content:', content);
            alert('Content saved!');
        }
    </script>
</body>
</html>
```

### Example 5: Tabindex and Direction

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Tabindex and Direction</title>
</head>
<body>
    <!-- Tab navigation order -->
    <h2>Form with Custom Tab Order</h2>
    <form>
        <label>First Name:</label>
        <input type="text" tabindex="3"><br>
        
        <label>Email:</label>
        <input type="email" tabindex="1"><br>
        
        <label>Phone:</label>
        <input type="tel" tabindex="2"><br>
        
        <!-- Skip element in tab order -->
        <input type="text" tabindex="-1" placeholder="Skipped">
    </form>
    
    <!-- Text direction -->
    <h2>Right-to-Left Text (Arabic)</h2>
    <p dir="rtl" lang="ar">مرحبا بك في موقعنا</p>
    
    <h2>Left-to-Right Text (English)</h2>
    <p dir="ltr">Welcome to our website</p>
</body>
</html>
```

### Example 6: Angular Data Binding

```html
<!-- Angular uses similar concepts -->
<div [id]="'dynamic-' + item.id" 
     [class.active]="item.isActive"
     [style.backgroundColor]="item.color"
     [attr.data-id]="item.id"
     [attr.aria-label]="item.name">
    {{ item.name }}
</div>

<!-- Event binding equivalent to tabindex -->
<button [attr.tabindex]="isEnabled ? 0 : -1">Button</button>
```

## Best Practices

### ID Best Practices

1. **Unique IDs** - Each ID should be unique on the page
2. **Descriptive names** - Use meaningful IDs: header, nav, main-content
3. **No spaces** - Use hyphens or camelCase: main-content or mainContent
4. **Start with letter** - IDs shouldn't start with numbers

### Class Best Practices

1. **Reusable classes** - Same class for similar elements
2. **Multiple classes** - Elements can have multiple classes
3. **Semantic classes** - Name by purpose, not appearance
4. **BEM naming** - Consider Block Element Modifier pattern

### Data Attributes Best Practices

1. **Descriptive names** - data-product-name not data-pn
2. **Hyphenated** - Use kebab-case: data-product-price
3. **JSON data** - Can store complex data as JSON string

## Real-World Examples

### Example 1: Navigation with Active State

```html
<nav>
    <a href="/home" class="nav-link">Home</a>
    <a href="/about" class="nav-link active">About</a>
    <a href="/contact" class="nav-link">Contact</a>
</nav>
```

### Example 2: Modal with Data

```html
<div class="modal" 
     id="product-modal"
     data-product-id="123"
     data-product-name="Headphones"
     data-modal-title="Product Details">
    <h2 id="modal-title"></h2>
    <p id="modal-description"></p>
</div>
```

### Example 3: Interactive List

```html
<ul class="todo-list">
    <li class="todo-item" 
        data-priority="high"
        data-due-date="2024-01-20"
        draggable="true">
        Complete assignment
    </li>
</ul>
```

## Common Mistakes

### Mistake 1: Duplicate IDs

```html
<!-- Wrong -->
<div id="header"></div>
<div id="header"></div>

<!-- Correct -->
<div id="header"></div>
<div id="sidebar"></div>
```

### Mistake 2: Using Style Instead of Classes

```html
<!-- Wrong - inline styles -->
<div style="background: red; padding: 10px;"></div>

<!-- Correct - use CSS classes -->
<div class="alert"></div>
```

### Mistake 3: Wrong Tabindex Usage

```html
<!-- Wrong - positive tabindex on all -->
<input tabindex="1">
<input tabindex="2">
<input tabindex="3">

<!-- Correct - use 0 or -1 -->
<input tabindex="0">
<input tabindex="0">
<input tabindex="-1">
```

## Exercises

### Exercise 1: Add ID and Class
Add id and class attributes to elements.

### Exercise 2: Use Data Attributes
Store product information in data attributes.

### Exercise 3: Create Tooltips
Add title attributes for tooltips.

### Exercise 4: Custom Tab Order
Set custom tab order using tabindex.

## Mini Practice Tasks

### Task 1: ID
Add an ID to an element.

### Task 2: Class
Add a class to an element.

### Task 3: Title
Add a title tooltip.

### Task 4: Data
Add a data attribute.

### Task 5: Hidden
Hide an element using hidden attribute.
