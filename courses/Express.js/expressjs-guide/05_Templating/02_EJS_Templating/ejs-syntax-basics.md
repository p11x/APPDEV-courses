# EJS Syntax Basics

## 📌 What You'll Learn
- The basic syntax of EJS (Embedded JavaScript templates)
- How to output data in EJS templates
- How to use control structures (conditionals and loops) in EJS
- How to include partial templates in EJS

## 🧠 Concept Explained (Plain English)

EJS stands for Embedded JavaScript. It's a simple templating language that lets you generate HTML markup with plain JavaScript. Think of it as HTML with superpowers - you can write regular HTML, but you also get to embed JavaScript code directly in your templates to make them dynamic.

With EJS, you can:
- Output variables directly into your HTML
- Run JavaScript logic (like if statements and loops) to control what gets displayed
- Include reusable pieces of templates (like headers and footers)
- Escape HTML to prevent security issues

The beauty of EJS is that if you know HTML and JavaScript, you already know most of what you need to use EJS effectively. The syntax is designed to be intuitive and familiar.

## 💻 Code Example

```javascript
// ES Module - EJS Syntax Examples

import express from 'express';

const app = express();

// Set up EJS as our view engine
app.set('view engine', 'ejs');

// ========================================
// ROUTE FOR DEMONSTRATING EJS SYNTAX
// ========================================
app.get('/demo', (req, res) => {
    // Sample data to pass to our template
    const demoData = {
        title: 'EJS Demo Page',
        user: {
            name: 'Alice Smith',
            isPremium: true,
            points: 150
        },
        items: ['Apple', 'Banana', 'Cherry'],
        showList: true,
        htmlContent: '<strong>This is bold text</strong>',
        currentYear: new Date().getFullYear()
    };
    
    res.render('demo', demoData);
});

// ========================================
// WHAT THE EJS TEMPLATE (views/demo.ejs) LOOKS LIKE
// ========================================
/*
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%= title %></title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .premium { background-color: #fff8dc; padding: 10px; border-radius: 5px; }
        .item { padding: 5px; border-bottom: 1px solid #eee; }
    </style>
</head>
<body>
    <h1><%= title %></h1>
    
    <!-- ========================================
         OUTPUTTING DATA (ESCAPED BY DEFAULT)
         ======================================== -->
    <h2>User Information</h2>
    <p>Name: <%= user.name %></p>
    <p>Points: <%= user.points %></p>
    
    <!-- ========================================
         CONDITIONALS (IF STATEMENTS)
         ======================================== -->
    <% if (user.isPremium) { %>
        <div class="premium">
            <p>Welcome back, premium member!</p>
            <p>You have <%= user.points %> loyalty points.</p>
        </div>
    <% } else { %>
        <p>Upgrade to premium for special benefits!</p>
    <% } %>
    
    <!-- ========================================
         LOOPS (FOR EACH)
         ======================================== -->
    <% if (showList) { %>
        <h2>Shopping List</h2>
        <% items.forEach(function(item) { %>
            <div class="item">
                <strong><%= item %></strong>
            </div>
        <% }); %>
    <% } %>
    
    <!-- ========================================
         ALTERNATIVE LOOP SYNTAX
         ======================================== -->
    <h2>Items (Alternative Syntax)</h2>
    <ul>
        <% for (let i = 0; i < items.length; i++) { %>
            <li><%= items[i] %></li>
        <% } %>
    </ul>
    
    <!-- ========================================
         OUTPUTTING RAW HTML (USE WITH CAUTION!)
         ======================================== -->
    <h2>HTML Content</h2>
    <p>Escaped (safe): <%= htmlContent %></p>
    <p>Unescaped (raw): <%- htmlContent %></p>
    
    <!-- ========================================
         COMMENTS (NOT SENT TO CLIENT)
         ======================================== -->
    <% /* This is a comment that won't appear in the HTML */ %>
    
    <!-- ========================================
         INCLUDING PARTIALS
         ======================================== -->
    <hr>
    <footer>
        <% include ./partials/footer %>
    </footer>
</body>
</html>
*/

// ========================================
// EXAMPLE PARTIAL (views/partials/footer.ejs)
// ========================================
/*
<p>&copy; <%= currentYear %> My Website. All rights reserved.</p>
<p>Page generated at: <%= new Date().toLocaleTimeString() %></p>
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown (Template)

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `<!DOCTYPE html>` | Standard HTML5 doctype |
| 7 | `<title><%= title %></title>` | Outputs the title variable (escaped by default) |
| 14 | `<p>Name: <%= user.name %></p>` | Outputs a nested property (user.name) |
| 18 | `<% if (user.isPremium) { %>` | Starts a JavaScript if statement |
| 23 | `} else { %>` | Else clause in the if statement |
| 28 | `<% } %>` | Closes the if statement block |
| 32 | `<% if (showList) { %>` | Another conditional check |
| 35 | `<% items.forEach(function(item) { %>` | Loops through each item in the array |
| 38 | `<% }); %>` | Closes the forEach loop |
| 45 | `<% for (let i = 0; i < items.length; i++) { %>` | Traditional for loop syntax |
| 48 | `<% } %>` | Closes the for loop |
| 53 | `<p>Escaped (safe): <%= htmlContent %></p>` | Outputs HTML-escaped content (shows the tags) |
| 55 | `<p>Unescaped (raw): <%- htmlContent %></p>` | Outputs raw HTML (renders the tags) |
| 59 | `<% /* This is a comment */ %>` | EJS comment (not sent to client) |
| 63 | `<% include ./partials/footer %>` | Includes another EJS file (partial) |

## ⚠️ Common Mistakes

**1. Confusing <%= vs <%- vs <%**
- `<%= %>` outputs escaped HTML (safe for user input)
- `<%- %>` outputs raw HTML (use only with trusted content)
- `<% %>` executes JavaScript but doesn't output anything

**2. Forgetting to close JavaScript blocks**
Every `<% if`, `<% for`, or `<% while` must have a matching `<% } %>` or you'll get a syntax error.

**3. Using JavaScript syntax incorrectly inside EJS tags**
Inside `<% %>` tags, you write regular JavaScript. Don't try to use template literals or JSX syntax here.

**4. Not escaping user-generated content**
Always use `<%= %>` (not `<%- %>`) for any data that comes from users to prevent XSS attacks.

**5. Incorrect paths in includes**
The path in `<% include ./partials/footer %>` is relative to the current template file. Use `./` for current directory or `../` to go up.

## ✅ Quick Recap

- `<%= %>` outputs escaped HTML (safe for dynamic data)
- `<%- %>` outputs raw HTML (use only with trusted content)
- `<% %>` executes JavaScript (for conditionals, loops, etc.)
- Use JavaScript control structures (if/else, for/while loops) inside `<% %>` tags
- Comments use `<% /* comment */ %>` syntax and don't appear in the output
- Include partials with `<% include ./path/to/partial %>`
- Always escape user-generated content to prevent security vulnerabilities

## 🔗 What's Next

Now that we've covered EJS syntax basics, let's look at how to use Pug (formerly Jade) as an alternative templating engine with Express.
