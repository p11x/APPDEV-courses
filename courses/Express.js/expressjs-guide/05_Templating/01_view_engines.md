# View Engines in Express.js

## What are View Engines?

**View engines** (also called template engines) let you create dynamic HTML pages. Instead of sending static HTML, you can inject data into templates — great for server-side rendering.

Think of templates as "HTML with superpowers" — you can use variables, loops, and conditions inside your HTML.

## Popular View Engines

| Engine | Description | Install |
|--------|-------------|---------|
| **EJS** | Simple, JavaScript-like syntax | `npm i ejs` |
| **Pug** | Clean, indentation-based syntax | `npm i pug` |
| **Handlebars** | Mustache-based, logic-less | `npm i hbs` |

## Setting Up EJS

**EJS** (Embedded JavaScript) is the easiest to learn if you know HTML and JavaScript.

### Step 1: Install EJS

```bash
npm install ejs
```

### Step 2: Configure Express

```javascript
// server.js
import express from 'express';

const app = express();
// 'app' is our Express application instance

// ============================================
// View Engine Configuration
// ============================================
// | Setting           | Value        | Description              |
// |-------------------|--------------|--------------------------|
// | view engine      | 'ejs'        | Template engine to use  |
// | views            | './views'    | Folder for templates    |
// ============================================

// Set the view engine to EJS
app.set('view engine', 'ejs');

// Set the views folder (where templates are stored)
// Default is ./views, so this is optional
app.set('views', './views');

// Serve static files (CSS, images)
app.use(express.static('public'));

// Routes
app.get('/', (req, res) => {
    // res.render() renders a template
    // First parameter: template name (without extension)
    // Second parameter: data to pass to template
    res.render('index', { 
        title: 'My Express App',
        message: 'Welcome to Express with EJS!'
    });
});

app.get('/users', (req, res) => {
    const users = [
        { name: 'Alice', email: 'alice@example.com' },
        { name: 'Bob', email: 'bob@example.com' },
        { name: 'Charlie', email: 'charlie@example.com' }
    ];
    
    res.render('users', { users });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### Step 3: Create Templates

Create a `views` folder and add templates:

**views/index.ejs** (Home page):
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title><%= title %></title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        h1 { color: #333; }
    </style>
</head>
<body>
    <h1><%= message %></h1>
    <p>This page is rendered with EJS!</p>
    
    <nav>
        <a href="/">Home</a> | <a href="/users">Users</a>
    </nav>
</body>
</html>
```

**views/users.ejs** (Users page with loop):
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Users</title>
</head>
<body>
    <h1>User List</h1>
    
    <ul>
        <% users.forEach(user => { %>
            <li>
                <strong><%= user.name %></strong>
                - <%= user.email %>
            </li>
        <% }); %>
    </ul>
    
    <p>Total users: <%= users.length %></p>
</body>
</html>
```

## EJS Syntax

### Key Tags

| Tag | Description | Example |
|-----|-------------|---------|
| `<%= %>` | Output escaped HTML | `<%= user.name %>` |
| `<%- %>` | Output raw HTML | `<%- user.html %>` |
| `<% %>` | Execute JavaScript | `<% if (user) { %>` |
| `<%# %>` | Comment (not output) | `<%# This is a comment %>` |

### Conditionals

```html
<% if (user) { %>
    <p>Welcome, <%= user.name %>!</p>
<% } else { %>
    <p>Please log in.</p>
<% } %>
```

### Loops

```html
<% items.forEach(item => { %>
    <div class="item">
        <%= item.name %> - $<%= item.price %>
    </div>
<% }); %>
```

## Setting Up Pug

**Pug** uses indentation instead of HTML tags.

### Install Pug

```bash
npm install pug
```

### Configure

```javascript
// server.js
import express from 'express';

const app = express();

app.set('view engine', 'pug');
app.set('views', './views');

// Routes
app.get('/', (req, res) => {
    res.render('index', { 
        title: 'My App',
        message: 'Hello from Pug!'
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### Create Pug Template

**views/index.pug**:
```pug
doctype html
html
  head
    title= title
  body
    h1= message
    p This is rendered with Pug
```

## Setting Up Handlebars

**Handlebars** keeps templates simple with "mustache" syntax.

### Install

```bash
npm install hbs
```

### Configure

```javascript
// server.js
import express from 'express';

const app = express();

app.set('view engine', 'hbs');
app.set('views', './views');

// Routes
app.get('/', (req, res) => {
    res.render('index', {
        title: 'My App',
        name: 'World'
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### Create Handlebars Template

**views/index.hbs**:
```html
<!DOCTYPE html>
<html>
<head>
    <title>{{title}}</title>
</head>
<body>
    <h1>Hello, {{name}}!</h1>
</body>
</html>
```

## Complete Example with Layouts

### Using EJS with a Layout

**views/layout.ejs** (Master template):
```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title><%= title %></title>
</head>
<body>
    <header>
        <nav>
            <a href="/">Home</a>
            <a href="/about">About</a>
        </nav>
    </header>
    
    <main>
        <!-- This is where page content is inserted -->
        <%- body %>
    </main>
    
    <footer>
        <p>&copy; 2024 My App</p>
    </footer>
</body>
</html>
```

**views/home.ejs** (Page template):
```html
<h1>Welcome!</h1>
<p>This is the home page.</p>
```

## Dynamic Data Examples

### Passing Data to Templates

```javascript
// server.js
app.get('/products', (req, res) => {
    const products = [
        { id: 1, name: 'Laptop', price: 999, inStock: true },
        { id: 2, name: 'Phone', price: 699, inStock: true },
        { id: 3, name: 'Tablet', price: 449, inStock: false }
    ];
    
    res.render('products', { 
        products,
        pageTitle: 'Our Products'
    });
});
```

**views/products.ejs**:
```html
<h1><%= pageTitle %></h1>

<table>
    <tr>
        <th>Name</th>
        <th>Price</th>
        <th>Status</th>
    </tr>
    <% products.forEach(product => { %>
        <tr>
            <td><%= product.name %></td>
            <td>$<%= product.price %></td>
            <td>
                <%= product.inStock ? 'In Stock' : 'Out of Stock' %>
            </td>
        </tr>
    <% }); %>
</table>
```

## Best Practices

| Practice | Why |
|----------|-----|
| Keep templates simple | Complex logic belongs in routes/controllers |
| Use partials for reusable code | Header, footer, navigation |
| Escape user input with `<%=` | Prevents XSS attacks |
| Separate data from presentation | Controllers handle data, views handle display |

## What's Next?

- **[Database Integration](../06_Database_Integration/01_mongodb.md)** — Connecting to databases
- **[REST APIs](./02_rest_apis.md)** — Building RESTful APIs
