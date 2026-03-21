# Understanding res.render()

## 📌 What You'll Learn
- What the res.render() method does
- How to pass data to templates
- How res.render() works with different templating engines
- Common patterns for using res.render()

## 🧠 Concept Explained (Plain English)

Think of `res.render()` as a special delivery service for your web application. When a user requests a page, instead of sending back a pre-written HTML file or raw data, you use `res.render()` to:

1. Take a template file (like a letter template with blanks to fill in)
2. Fill in those blanks with specific data (like the user's name, their recent orders, etc.)
3. Generate a complete HTML page on the spot
4. Send that freshly generated HTML to the user's browser

The `res.render()` method is what makes server-side rendering possible in Express. It connects your route handlers (which gather data) with your templating engine (which formats that data into HTML).

When you call `res.render()`, Express:
- Looks for the template file in your views folder
- Passes your data object to the templating engine
- Lets the engine replace placeholders in the template with your data
- Takes the resulting HTML string and sends it as the HTTP response

This is different from `res.send()` or `res.json()`, which send raw text or JSON directly. With `res.render()`, you're always sending HTML (generated from your template).

## 💻 Code Example

```javascript
// ES Module - Using res.render() with Data

import express from 'express';

const app = express();

// Set up EJS as our view engine
app.set('view engine', 'ejs');

// ========================================
// BASIC res.render() USAGE
// ========================================
app.get('/', (req, res) => {
    // res.render() takes two main arguments:
    // 1. The name of the view (template) file (without extension)
    // 2. An object containing data to pass to the template
    res.render('index', { 
        title: 'Home Page', 
        welcomeMessage: 'Welcome to our website!' 
    });
});

// ========================================
// PASSING COMPLEX DATA
// ========================================
app.get('/users', (req, res) => {
    // In a real app, this data would come from a database
    const users = [
        { id: 1, name: 'Alice Smith', email: 'alice@example.com' },
        { id: 2, name: 'Bob Jones', email: 'bob@example.com' },
        { id: 3, name: 'Carol Lee', email: 'carol@example.com' }
    ];
    
    // We can pass arrays, objects, or any JavaScript values
    res.render('users/index', { 
        users: users,          // Passing an array of user objects
        count: users.length,   // Passing a computed value
        showDetails: true      // Passing a boolean flag
    });
});

// ========================================
// USING res.render() WITH LOCALS
// ========================================
// Sometimes you want to make data available to ALL templates
// You can use app.locals or res.locals for this

// app.locals are available in ALL templates for the lifetime of the application
app.locals.siteName = 'My Awesome Site';
app.locals.currentYear = new Date().getFullYear();

// res.locals are available only for the current request
app.use((req, res, next) => {
    res.locals.user = req.user; // If you have user authentication
    res.locals.isAuthenticated = !!req.user;
    next();
});

// Now in any template, you can use:
// <footer>&copy; <%= siteName %> <%= currentYear %></footer>
// <% if (isAuthenticated) { %>
//   <a href="/logout">Logout</a>
// <% } %>

// ========================================
// WHAT THE TEMPLATE FILES MIGHT LOOK LIKE
// ========================================
// views/index.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title><%= title %></title>
</head>
<body>
    <h1><%= welcomeMessage %></h1>
</body>
</html>
*/

// views/users/index.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title>Users List</title>
</head>
<body>
    <h1>Users (<%= count %>)</h1>
    <% if (showDetails) { %>
        <ul>
            <% users.forEach(user => { %>
                <li>
                    <strong><%= user.name %></strong> 
                    (<%= user.email %>)
                </li>
            <% }); %>
        </ul>
    <% } else { %>
        <p><%= count %> users total</p>
    <% } %>
</body>
</html>
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 4 | `const app = express();` | Create an Express application instance |
| 7 | `app.set('view engine', 'ejs');` | Set the view engine to EJS |
| 11 | `app.get('/', (req, res) => {` | Define a route for the root URL |
| 14 | `res.render('index', { ... });` | Render the 'index' template with title and welcomeMessage |
| 20 | `app.get('/users', (req, res) => {` | Define a route for /users |
| 27 | `const users = [ ... ];` | Create an array of user objects (simulating database data) |
| 36 | `res.render('users/index', { ... });` | Render the users/index template with users array and other data |
| 43 | `app.locals.siteName = 'My Awesome Site';` | Make siteName available to all templates |
| 44 | `app.locals.currentYear = new Date().getFullYear();` | Make currentYear available to all templates |
| 47 | `app.use((req, res, next) => {` | Middleware to set res.locals for each request |
| 48 | `res.locals.user = req.user;` | Make user data available to current request's templates |
| 49 | `res.locals.isAuthenticated = !!req.user;` | Make authentication status available |
| 52 | `next();` | Pass control to the next middleware |
| 55-68 | `// views/index.ejs` | Example of what the index template might look like |
| 70-88 | `// views/users/index.ejs` | Example of what the users/index template might look like |

## ⚠️ Common Mistakes

**1. Forgetting to set the view engine**
If you don't call `app.set('view engine', 'ejs')` (or your chosen engine), `res.render()` will throw an error because Express doesn't know how to process the template.

**2. Passing undefined data to templates**
If you pass a variable that's undefined to your template, and the template tries to use it, you might get errors or unexpected output. Always check that your data exists before passing it.

**3. Using the wrong template name**
The first argument to `res.render()` is the name of the template file without the extension. Make sure it matches the actual file name in your views folder.

**4. Trying to send multiple responses**
Calling `res.render()` more than once in a route handler will cause an error because you can only send one response per request. Make sure you have only one `res.render()` (or `res.send()`, etc.) per code path.

**5. Forgetting that res.render() ends the request-response cycle**
After calling `res.render()`, you should not try to send another response or perform additional operations that assume the request is still active (unless you're in middleware and calling `next()`).

## ✅ Quick Recap

- `res.render()` renders a template file with data and sends the resulting HTML as the response
- It takes two arguments: the template name (without extension) and an object with data
- Data passed to `res.render()` becomes available as variables in your template
- You can use `app.locals` for data available to all templates and `res.locals` for data available to the current request
- `res.render()` ends the request-response cycle, so don't try to send another response after calling it

## 🔗 What's Next

Let's look at how to pass data to templates in more detail, including different data types and how to use them in your templates.
