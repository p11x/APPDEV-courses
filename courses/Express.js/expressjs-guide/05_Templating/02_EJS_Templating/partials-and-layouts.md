# Partials and Layouts in EJS

## 📌 What You'll Learn
- What partials are and why they're useful
- How to create and include partials in EJS
- How to create a basic layout system
- Best practices for organizing reusable template components

## 🧠 Concept Explained (Plain English)

Imagine you're building a house. Instead of drawing the entire house from scratch for each room, you have standard components that appear in every room: walls, doors, windows, electrical outlets. You don't redesign these every time - you reuse the same patterns.

In web development, **partials** are like those reusable components. They're template files that contain code (like a header, footer, or navigation menu) that you want to include in multiple pages. Instead of copying and pasting the same HTML everywhere, you create it once as a partial and include it wherever needed.

**Layouts** take this concept further. A layout is a wrapper template that defines the basic structure of your page (like the HTML head, body tag, and common sections). Individual page templates then "fill in" their specific content within that layout.

Using partials and layouts makes your templates:
- Easier to maintain (change once, update everywhere)
- More consistent (same header/footer on every page)
- Cleaner and more organized
- Faster to develop (reuse existing components)

## 💻 Code Example

```javascript
// ES Module - Using Partials and Layouts in EJS

import express from 'express';

const app = express();

// Set up EJS as our view engine
app.set('view engine', 'ejs');

// ========================================
// ROUTES THAT USE PARTIALS AND LAYOUTS
// ========================================

// Home page route
app.get('/', (req, res) => {
    res.render('pages/home', {
        pageTitle: 'Home Page',
        heroTitle: 'Welcome to Our Site',
        heroSubtitle: 'Building amazing web experiences'
    });
});

// About page route
app.get('/about', (req, res) => {
    res.render('pages/about', {
        pageTitle: 'About Us',
        content: 'We are a company dedicated to building great web applications.'
    });
});

// Products page route
app.get('/products', (req, res) => {
    res.render('pages/products', {
        pageTitle: 'Our Products',
        products: [
            { id: 1, name: 'Widget A', price: 19.99 },
            { id: 2, name: 'Widget B', price: 29.99 },
            { id: 3, name: 'Widget C', price: 39.99 }
        ]
    });
});

/*
// ========================================
// FOLDER STRUCTURE FOR THIS EXAMPLE
// ========================================
// views/
// ├── layout.ejs              (Main layout wrapper)
// ├── partials/
// │   ├── header.ejs          (Navigation and page header)
// │   ├── footer.ejs          (Footer content)
// │   └── navbar.ejs          (Navigation menu)
// └── pages/
//     ├── home.ejs            (Home page content)
//     ├── about.ejs           (About page content)
//     └── products.ejs        (Products page content)
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 📄 Template Files

### views/layout.ejs (Main Layout)
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title><%= pageTitle %></title>
    <style>
        body { font-family: Arial, sans-serif; margin: 0; padding: 0; }
        header { background: #333; color: white; padding: 20px; }
        nav a { color: white; margin-right: 15px; text-decoration: none; }
        nav a:hover { text-decoration: underline; }
        main { padding: 20px; min-height: 400px; }
        footer { background: #f4f4f4; padding: 20px; text-align: center; }
        .container { max-width: 1200px; margin: 0 auto; }
    </style>
</head>
<body>
    <!-- Include the header partial -->
    <% include ../partials/header %>
    
    <main>
        <div class="container">
            <!-- The specific page content goes here -->
            <%- body %>
        </div>
    </main>
    
    <!-- Include the footer partial -->
    <% include ../partials/footer %>
</body>
</html>
```

### views/partials/header.ejs
```html
<header>
    <div class="container">
        <h1>My Website</h1>
        <% include navbar %>
    </div>
</header>
```

### views/partials/navbar.ejs
```html
<nav>
    <a href="/">Home</a>
    <a href="/about">About</a>
    <a href="/products">Products</a>
    <a href="/contact">Contact</a>
</nav>
```

### views/partials/footer.ejs
```html
<footer>
    <div class="container">
        <p>&copy; <%= new Date().getFullYear() %> My Website. All rights reserved.</p>
        <p>
            <a href="/privacy">Privacy Policy</a> | 
            <a href="/terms">Terms of Service</a>
        </p>
    </div>
</footer>
```

### views/pages/home.ejs
```html
<!-- Home page content -->
<section class="hero">
    <h2><%= heroTitle %></h2>
    <p><%= heroSubtitle %></p>
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

### views/pages/about.ejs
```html
<!-- About page content -->
<h2>About Us</h2>
<p><%= content %></p>

<h3>Our Mission</h3>
<p>To provide the best web development resources and education to developers around the world.</p>

<h3>Our Team</h3>
<p>We are a group of passionate developers, designers, and educators working together to create amazing learning experiences.</p>
```

### views/pages/products.ejs
```html
<!-- Products page content -->
<h2><%= pageTitle %></h2>

<% if (products && products.length > 0) { %>
    <div class="products-grid" style="display: grid; grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); gap: 20px; margin-top: 20px;">
        <% products.forEach(product => { %>
            <div style="border: 1px solid #ddd; padding: 15px; border-radius: 5px;">
                <h3><%= product.name %></h3>
                <p style="font-size: 1.2em; font-weight: bold;">$<%= product.price.toFixed(2) %></p>
                <button style="background: #28a745; color: white; border: none; padding: 10px 15px; cursor: pointer; border-radius: 3px;">
                    Add to Cart
                </button>
            </div>
        <% }); %>
    </div>
<% } else { %>
    <p>No products available at this time.</p>
<% } %>
```

## 🔍 Key Partial Concepts

| Concept | Description |
|---------|-------------|
| `<% include path/to/partial %>` | Includes another template file at this location |
| Partial paths | Paths are relative to the template doing the including |
| Access to variables | Partials have access to all variables passed to the parent template |
| Reusability | Create once, use anywhere - reduces code duplication |
| Organization | Keep partials in a dedicated folder (e.g., `partials/`) |

## ⚠️ Common Mistakes

**1. Using incorrect paths in include statements**
Paths in EJS includes are relative to the template doing the including. If you're in `pages/home.ejs` and want to include `partials/header.ejs`, use `<% include ../partials/header %>`.

**2. Not making partials flexible enough**
Don't hardcode too much in partials. Pass variables to make them flexible (like passing pageTitle to the header).

**3. Forgetting that partials share the same scope**
Variables passed to `res.render()` are available in all included partials, which is usually helpful but can cause confusion if you're not expecting it.

**4. Creating too many small partials**
While partials are great, creating one for every single element can make your code harder to follow. Group related elements together.

**5. Not handling missing data in partials**
If a partial expects a variable that's not passed, it might cause errors or unexpected output. Always check for optional data.

## ✅ Quick Recap

- Partials are reusable template components stored in separate files
- Include partials using `<% include path/to/partial %>`
- Organize partials in a dedicated folder (like `views/partials/`)
- Layouts wrap page content and provide a consistent page structure
- Variables passed to `res.render()` are available in all partials
- Use relative paths when including partials (../ to go up directories)
- Partials make maintenance easier by centralizing common elements

## 🔗 What's Next

Let's look at another popular templating engine - Pug (formerly known as Jade) - which has a very different syntax but offers similar functionality with some unique features.
