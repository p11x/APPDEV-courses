# What is a Templating Engine?

## 📌 What You'll Learn
- What a templating engine is and why it's used
- How templating engines work with Express
- Popular templating engines for Express

## 🧠 Concept Explained (Plain English)

Imagine you're writing a letter to many friends. The letter mostly says the same thing, but you want to personalize each one with the friend's name and maybe a specific detail. Instead of writing each letter from scratch, you create a template with placeholders like "Dear [NAME]," and then fill in the placeholders for each friend.

A **templating engine** works similarly for web applications. It allows you to create HTML templates with placeholders for dynamic data. When a user requests a page, the engine fills in the placeholders with actual data (like user information, product details, etc.) and sends the resulting HTML to the client.

In Express, templating engines enable server-side rendering (SSR), where the server generates the HTML for each request. This is different from sending static HTML files or sending JSON to a client-side framework (like React or Vue) that renders the HTML in the browser.

Templating engines are particularly useful for:
- Applications where SEO is important (search engines can easily crawl server-rendered HTML)
- Applications that want to minimize client-side JavaScript
- Applications that prefer to keep rendering logic on the server

Popular templating engines for Express include EJS, Pug, and Handlebars. Each has its own syntax and features, but they all serve the same purpose: to make it easy to generate dynamic HTML.

## 💻 Code Example

```javascript
// ES Module - Setting up a Templating Engine (EJS example)

import express from 'express';

const app = express();

// ========================================
// SETTING UP A TEMPLATING ENGINE
// ========================================
// We'll use EJS as an example, but the concept is similar for other engines

// 1. Set the view engine to EJS
app.set('view engine', 'ejs');

// 2. Set the views directory (where your template files are stored)
//    By default, Express looks for a folder named 'views'
//    You can change it if you want, but we'll use the default here.
// app.set('views', './views'); // This is optional if you're using the default

// 3. Create a route that renders a template
app.get('/', (req, res) => {
    // The render method takes two arguments:
    // 1. The name of the template file (without the extension)
    // 2. An object containing the data to pass to the template
    res.render('index', { 
        title: 'Home Page', 
        message: 'Welcome to my website!' 
    });
});

// ========================================
// WHAT HAPPENS WHEN YOU CALL res.render()
// ========================================
// 1. Express looks for the file 'views/index.ejs'
// 2. The templating engine (EJS) reads the file
// 3. The engine replaces placeholders in the template with the data you provided
// 4. The engine returns the resulting HTML string
// 5. Express sends that HTML as the response

// Example template (views/index.ejs):
/*
<!DOCTYPE html>
<html>
<head>
    <title><%= title %></title>
</head>
<body>
    <h1><%= message %></h1>
</body>
</html>
*/

// When the data { title: 'Home Page', message: 'Welcome to my website!' } is passed,
// the engine produces:
// <!DOCTYPE html>
// <html>
// <head>
//     <title>Home Page</title>
// </head>
// <body>
//     <h1>Welcome to my website!</h1>
// </body>
// </html>

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 4 | `const app = express();` | Create an Express application instance |
| 8 | `app.set('view engine', 'ejs');` | Set the view engine to EJS |
| 12 | `app.set('views', './views');` | (Optional) Set the views directory |
| 16 | `app.get('/', (req, res) => {` | Define a route for the root URL |
| 19 | `res.render('index', { ... });` | Render the 'index' template with the provided data |
| 24-32 | `// Example template (views/index.ejs):` | Shows what the template file might look like |
| 33-42 | `// When the data ... is passed,` | Shows the resulting HTML after rendering |

## ⚠️ Common Mistakes

**1. Forgetting to set the view engine**
If you don't call `app.set('view engine', 'ejs')` (or the engine you're using), Express won't know how to render your templates.

**2. Not creating the views directory**
Express looks for a folder named 'views' by default. If it doesn't exist, you'll get an error when trying to render a template.

**3. Using the wrong file extension**
Make sure your template files have the correct extension for your view engine (e.g., .ejs for EJS, .pug for Pug, .hbs for Handlebars).

**4. Forgetting to pass data to the template**
If your template expects certain data and you don't provide it, those placeholders will be empty or cause errors.

**5. Using the wrong template name in res.render()**
The first argument to `res.render()` is the name of the template file without the extension. Make sure it matches the actual file name.

## ✅ Quick Recap

- A templating engine allows you to create HTML templates with placeholders for dynamic data
- In Express, you set the view engine with `app.set('view engine', 'engine-name')`
- You render a template with `res.render('template-name', data)`
- The engine replaces placeholders in the template with the data you provide
- Popular templating engines for Express include EJS, Pug, and Handlebars

## 🔗 What's Next

Let's learn how to set up the views folder for your templates.
