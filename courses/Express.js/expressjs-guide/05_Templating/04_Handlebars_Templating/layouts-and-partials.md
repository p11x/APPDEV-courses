# Layouts and Partials in Handlebars

## 📌 What You'll Learn
- How to use layouts in Handlebars
- How to create and use partials
- How to register custom helpers
- How to organize templates for maintainability

## 🧠 Concept Explained (Plain English)

Handlebars has its own way of handling layouts and reusable components that's different from EJS and Pug. Understanding these concepts will help you build maintainable template systems.

**Partials** in Handlebars are like reusable template snippets that you can include anywhere. Think of them like stampers - you create a design once (the partial), and then you can "stamp" it anywhere you need it in your templates. Common uses include:
- Headers and footers
- Navigation menus
- Cards or list items that repeat
- Form elements

**Layouts** in Handlebars work a bit differently than in other engines. With the express-handlebars package (the most common way to use Handlebars with Express), layouts wrap your page content. The layout defines the overall page structure, and individual pages provide their specific content.

**Helpers** are functions that you can call from within templates to transform data or perform logic. Handlebars comes with built-in helpers like `#if`, `#each`, and `#with`, but you can also create custom helpers for your specific needs.

## 💻 Code Example

```javascript
// ES Module - Layouts, Partials, and Helpers in Handlebars

import express from 'express';
import { engine } from 'express-handlebars';
import path from 'path';

const app = express();

// ========================================
// SETTING UP HANDLEBARS WITH LAYOUTS AND PARTIALS
// ========================================
app.engine('hbs', engine({
    extname: '.hbs',
    defaultLayout: 'main',           // Default layout file
    layoutsDir: 'views/layouts/',    // Where layout files live
    partialsDir: 'views/partials/', // Where partial files live
    
    // Register custom helpers
    helpers: {
        // Helper to uppercase text
        uppercase: (str) => str.toUpperCase(),
        
        // Helper to lowercase text  
        lowercase: (str) => str.toLowerCase(),
        
        // Helper to format currency
        formatCurrency: (amount) => `$${amount.toFixed(2)}`,
        
        // Helper with conditional logic
        formatPoints: (points) => {
            if (points >= 100) return 'Gold Member';
            if (points >= 50) return 'Silver Member';
            return 'Bronze Member';
        },
        
        // Block helper for active link
        activeLink: (url, currentUrl, options) => {
            return url === currentUrl ? options.fn(this) : options.inverse(this);
        },
        
        // Helper for repeating content
        repeat: (str, count) => str.repeat(count),
        
        // JSON helper for debugging
        json: (obj) => JSON.stringify(obj, null, 2)
    }
}));

app.set('view engine', 'hbs');
app.set('views', './views');

// ========================================
// ROUTES
// ========================================

// Home page
app.get('/', (req, res) => {
    res.render('pages/home', {
        pageTitle: 'Home - My Website',
        currentUrl: '/'
    });
});

// About page
app.get('/about', (req, res) => {
    res.render('pages/about', {
        pageTitle: 'About Us - My Website',
        content: 'We are a company dedicated to building great web applications.',
        currentUrl: '/about'
    });
});

// Products page
app.get('/products', (req, res) => {
    res.render('pages/products', {
        pageTitle: 'Products - My Website',
        products: [
            { id: 1, name: 'Widget A', price: 19.99, category: 'Electronics' },
            { id: 2, name: 'Widget B', price: 29.99, category: 'Electronics' },
            { id: 3, name: 'Gadget X', price: 39.99, category: 'Gadgets' }
        ],
        currentUrl: '/products'
    });
});

/*
// ========================================
// FOLDER STRUCTURE
// ========================================
// views/
// ├── layouts/
// │   └── main.hbs              (Main layout wrapper)
// ├── partials/
// │   ├── header.hbs            (Navigation header)
// │   ├── footer.hbs           (Footer content)
// │   ├── navbar.hbs            (Navigation menu)
// │   └── productCard.hbs       (Reusable product card)
// └── pages/
//     ├── home.hbs
//     ├── about.hbs
//     └── products.hbs
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 📄 Template Files

### views/layouts/main.hbs (Main Layout)
```handlebars
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{{pageTitle}}</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 0; }
        header { background: #333; color: white; padding: 20px; }
        nav a { color: white; margin-right: 15px; text-decoration: none; }
        nav a.active { text-decoration: underline; font-weight: bold; }
        main { padding: 20px; min-height: 400px; }
        footer { background: #f4f4f4; padding: 20px; text-align: center; }
        .container { max-width: 1200px; margin: 0 auto; }
        .products-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); gap: 20px; }
    </style>
</head>
<body>
    {{> header }}
    
    <main>
        <div class="container">
            {{{body}}}
        </div>
    </main>
    
    {{> footer }}
</body>
</html>
```

### views/partials/header.hbs
```handlebars
<header>
    <div class="container">
        <h1>My Website</h1>
        {{> navbar }}
    </div>
</header>
```

### views/partials/navbar.hbs
```handlebars
<nav>
    {{!-- Use our custom activeLink helper to highlight current page --}}
    <a href="/" class="{{#if (activeLink '/' currentUrl)}}active{{/if}}">Home</a>
    <a href="/about" class="{{#if (activeLink '/about' currentUrl)}}active{{/if}}">About</a>
    <a href="/products" class="{{#if (activeLink '/products' currentUrl)}}active{{/if}}">Products</a>
    <a href="/contact">Contact</a>
</nav>
```

### views/partials/footer.hbs
```handlebars
<footer>
    <div class="container">
        <p>&copy; {{currentYear}} My Website. All rights reserved.</p>
        <nav>
            <a href="/privacy">Privacy Policy</a> | 
            <a href="/terms">Terms of Service</a>
        </nav>
    </div>
</footer>
```

### views/partials/productCard.hbs (Reusable Partial)
```handlebars
<div class="product-card" style="border: 1px solid #ddd; padding: 15px; border-radius: 5px;">
    <h3>{{name}}</h3>
    <p class="category" style="color: #666;">{{category}}</p>
    <p class="price" style="font-size: 1.2em; font-weight: bold;">{{formatCurrency price}}</p>
    <button style="background: #28a745; color: white; border: none; padding: 10px 15px; cursor: pointer; border-radius: 3px;">
        Add to Cart
    </button>
</div>
```

### views/pages/home.hbs
```handlebars
{{!-- Home page content --}}

<section class="hero" style="text-align: center; padding: 40px 0;">
    <h2>Welcome to Our Site</h2>
    <p>Building amazing web experiences</p>
    <a href="/products" style="display: inline-block; padding: 10px 20px; background: #007bff; color: white; text-decoration: none; border-radius: 5px;">
        Browse Products
    </a>
</section>

<section style="margin-top: 40px;">
    <h3>Why Choose Us?</h3>
    <ul>
        <li>Quality products</li>
        <li>Fast shipping</li>
        <li>Great customer service</li>
    </ul>
</section>
```

### views/pages/about.hbs
```handlebars
{{!-- About page content --}}

<h2>About Us</h2>
<p>{{content}}</p>

<h3>Our Mission</h3>
<p>To provide the best web development resources and education to developers around the world.</p>

<h3>Our Team</h3>
<p>We are a group of passionate developers, designers, and educators working together to create amazing learning experiences.</p>
```

### views/pages/products.hbs
```handlebars
{{!-- Products page content --}}

<h2>Our Products</h2>

{{#if products}}
    <div class="products-grid">
        {{#each products}}
            {{> productCard }}
        {{/each}}
    </div>
{{else}}
    <p>No products available at this time.</p>
{{/if}}
```

## 🔍 Key Concepts

| Concept | Syntax | Description |
|---------|--------|-------------|
| Include partial | `{{> partialName }}` | Includes a partial template |
| Layout wrapper | `{{{body}}}` | Place where page content goes |
| Custom helper | `{{helperName arg}}` | Calls a registered helper function |
| Conditional | `{{#if condition}}...{{/if}}` | Basic conditional block |
| Loop | `{{#each array}}...{{/each}}` | Iterates over array |

## ⚠️ Common Mistakes

**1. Not registering partials directory**
Make sure you set `partialsDir` in the Handlebars engine configuration so Express knows where to find partials.

**2. Forgetting the greater-than sign in partials**
The syntax is `{{> partialName }}`, not `{{partialName}}`. The `>` is essential.

**3. Trying to use helpers without registering them**
Any helper you want to use in templates must be registered in the `helpers` object when configuring the engine.

**4. Using {{{body}}} instead of {{body}}**
The layout placeholder for page content uses triple braces to avoid double-escaping HTML in the page content.

**5. Not passing required data to partials**
Partials have access to the data passed to the parent template, but you may need to pass additional data for more complex partials.

## ✅ Quick Recap

- Use `{{> partialName }}` syntax to include partials
- Register custom helpers in the engine configuration
- Use `{{{body}}}` in layouts to indicate where page content goes
- Partials can accept additional data when included: `{{> partialName data}}`
- The express-handlebars package provides built-in support for layouts and partials
- Keep layouts in `layoutsDir` and partials in `partialsDir`
- Helpers can be simple functions or block helpers with options

## 🔗 What's Next

The templating section is now complete. Let's move on to Error Handling in Express, where we'll learn how to handle errors gracefully in your applications.
