# Layouts and Inheritance in Pug

## 📌 What You'll Learn
- How Pug's block and extends system works
- How to create a base layout template
- How to extend layouts in child templates
- How to use block appending and prepending

## 🧠 Concept Explained (Plain English)

In Pug, you don't have separate "layouts" and "partials" like you might in other templating engines. Instead, Pug uses a powerful **inheritance system** with two main concepts:

1. **Layout (Base Template)**: A master template that defines the overall page structure - the HTML skeleton, header, footer, and any common elements. It defines special "blocks" that child templates can fill in.

2. **Extends**: A child template that "extends" a layout, providing content for the blocks defined in the layout.

Think of it like a family inheritance: the layout is like a parent that passes down certain traits (the page structure), while the child template inherits those traits and adds its own unique characteristics (the specific page content).

The key difference from partials is:
- **Partials**: Insert content at a specific point (like copy-pasting)
- **Layouts**: Define the overall structure that pages fill into

Pug also lets you:
- **Block** content that can be replaced or added to
- **Append** to blocks (add more content at the end)
- **Prepend** to blocks (add more content at the beginning)

## 💻 Code Example

```javascript
// ES Module - Using Layouts and Inheritance in Pug

import express from 'express';

const app = express();

// Set up Pug as our view engine
app.set('view engine', 'pug');
app.set('views', './views');

// ========================================
// ROUTES THAT USE LAYOUTS
// ========================================

// Home page route - uses base layout
app.get('/', (req, res) => {
    res.render('pages/home', {
        pageTitle: 'Home - My Website'
    });
});

// About page route - uses base layout
app.get('/about', (req, res) => {
    res.render('pages/about', {
        pageTitle: 'About Us - My Website',
        content: 'We are a company dedicated to building great web applications.'
    });
});

// Products page route - uses alternate layout
app.get('/products', (req, res) => {
    res.render('pages/products', {
        pageTitle: 'Our Products - My Website',
        products: [
            { id: 1, name: 'Widget A', price: 19.99 },
            { id: 2, name: 'Widget B', price: 29.99 },
            { id: 3, name: 'Widget C', price: 39.99 }
        ]
    });
});

// Blog page route - uses blog-specific layout
app.get('/blog', (req, res) => {
    res.render('pages/blog', {
        pageTitle: 'Blog - My Website',
        posts: [
            { title: 'Getting Started', summary: 'Learn the basics...' },
            { title: 'Advanced Topics', summary: 'Go deeper...' }
        ]
    });
});

/*
// ========================================
// FOLDER STRUCTURE FOR THIS EXAMPLE
// ========================================
// views/
// ├── layouts/
// │   ├── base.pug           (Main layout with header/footer)
// │   └── blog-layout.pug    (Blog-specific layout)
// ├── partials/
// │   ├── header.pug
// │   └── footer.pug
// └── pages/
//     ├── home.pug
//     ├── about.pug
//     ├── products.pug
//     └── blog.pug
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 📄 Template Files

### views/layouts/base.pug (Main Layout)
```pug
//- views/layouts/base.pug

doctype html
html
    head
        meta(charset='UTF-8')
        meta(name='viewport', content='width=device-width, initial-scale=1.0')
        title= pageTitle
        
        style
            body { font-family: Arial, sans-serif; margin: 0; padding: 0; }
            header { background: #333; color: white; padding: 20px; }
            nav a { color: white; margin-right: 15px; text-decoration: none; }
            main { padding: 20px; min-height: 400px; }
            footer { background: #f4f4f4; padding: 20px; text-align: center; }
            .container { max-width: 1200px; margin: 0 auto; }
    
    body
        //- Include header partial
        include ../partials/header
        
        main
            .container
                //- This block will be filled by child templates
                block content
        
        //- Include footer partial
        include ../partials/footer
```

### views/partials/header.pug
```pug
//- views/partials/header.pug
header
    .container
        h1 My Website
        nav
            a(href='/') Home
            a(href='/about') About
            a(href='/products') Products
            a(href='/blog') Blog
            a(href='/contact') Contact
```

### views/partials/footer.pug
```pug
//- views/partials/footer.pug
footer
    .container
        p &copy; #{new Date().getFullYear()} My Website. All rights reserved.
        nav
            a(href='/privacy') Privacy Policy
            |  | 
            a(href='/terms') Terms of Service
```

### views/pages/home.pug (Extends Base Layout)
```pug
//- views/pages/home.pug

//- Extend the base layout
extends ../layouts/base.pug

//- Replace the 'content' block
block content
    section.hero
        h2 Welcome to Our Site
        p Building amazing web experiences
        a(href='/products', style='display: inline-block; padding: 10px 20px; background: #007bff; color: white; text-decoration: none; border-radius: 5px;') Browse Products
    
    section(style='margin-top: 40px;')
        h3 Why Choose Us?
        ul
            li Quality products
            li Fast shipping
            li Great customer service
```

### views/pages/about.pug (Extends Base Layout)
```pug
//- views/pages/about.pug

extends ../layouts/base.pug

block content
    h2 About Us
    p= content
    
    h3 Our Mission
    p To provide the best web development resources and education to developers around the world.
    
    h3 Our Team
    p We are a group of passionate developers, designers, and educators working together to create amazing learning experiences.
```

### views/layouts/blog-layout.pug (Blog-Specific Layout)
```pug
//- views/layouts/blog-layout.pug

doctype html
html
    head
        meta(charset='UTF-8')
        meta(name='viewport', content='width=device-width, initial-scale=1.0')
        title= pageTitle
        
        style
            body { font-family: Arial, sans-serif; margin: 0; padding: 0; }
            header { background: #2c3e50; color: white; padding: 20px; }
            main { max-width: 800px; margin: 0 auto; padding: 40px 20px; }
            .post { border-bottom: 1px solid #eee; padding-bottom: 20px; margin-bottom: 20px; }
            .post h2 { margin-top: 0; }
            footer { background: #f4f4f4; padding: 20px; text-align: center; }
    
    body
        header
            h1 My Blog
        
        main
            //- Blog-specific content block
            block blog-content
        
        footer
            p &copy; #{new Date().getFullYear()} My Blog
```

### views/pages/blog.pug (Extends Blog Layout)
```pug
//- views/pages/blog.pug

extends ../layouts/blog-layout.pug

//- Define content for the blog-content block
block blog-content
    h2 Latest Posts
    
    each post in posts
        .post
            h2= post.title
            p= post.summary
            a(href='#') Read more →
    
    //- Can also append or prepend to blocks in the layout
    //- block append scripts
    //-     script(src='/js/blog.js')
```

## 🔍 Key Inheritance Concepts

| Concept | Description |
|---------|-------------|
| `extends /path/to/layout.pug` | Tells Pug to inherit from another template |
| `block blockName` | Defines a named section that child templates can fill |
| `block content` | The most common block name for main page content |
| `block append` | Adds content to the END of an existing block |
| `block prepend` | Adds content to the BEGINNING of an existing block |
| `include /path/to/partial.pug` | Includes a reusable partial template |

## ⚠️ Common Mistakes

**1. Wrong path in extends**
The path in `extends` is relative to the template file doing the extending. If your template is in `pages/home.pug` and layout is in `layouts/base.pug`, use `extends ../layouts/base.pug`.

**2. Not defining blocks in the layout**
A layout must define blocks using `block blockName` syntax. Without blocks, child templates have no way to insert content.

**3. Trying to use blocks without extends**
Blocks only work when combined with `extends`. If you just include a template, blocks won't be replaced.

**4. Confusing block with include**
- `include` copies content into the current location
- `extends` creates an inheritance relationship where blocks are replaced

**5. Not matching block names exactly**
Block names must match exactly between the layout and the child template. A block named `content` in the layout must be referred to as `block content` in the child.

## ✅ Quick Recap

- Use `extends` to inherit from a layout template
- Define `block` sections in the layout where child content should go
- Child templates use `block blockName` to provide content for those sections
- Use `block append` or `block prepend` to add to (not replace) existing blocks
- Include partials with `include` for reusable components
- Paths in extends and include are relative to the current template file

## 🔗 What's Next

Now let's look at Handlebars.js, another popular templating engine that uses a different approach - using mustache syntax with curly braces for templates.
