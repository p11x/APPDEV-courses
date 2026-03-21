# Passing Data to Templates

## 📌 What You'll Learn
- Different ways to pass data to templates
- How to use various data types in templates (strings, numbers, arrays, objects)
- How to work with loops and conditionals in templates
- Best practices for data preparation before passing to templates

## 🧠 Concept Explained (Plain English)

When you're building a web page with a templating engine, you often need to show different information based on the situation. For example:
- A user's profile page shows their name, email, and preferences
- A product listing shows different products with their prices and descriptions
- A blog post shows the title, content, author, and comments

The process of getting this data ready and sending it to your template is what we call "passing data to templates." Think of it like preparing ingredients before cooking: you gather all the vegetables, spices, and proteins you need, chop them up, measure them out, and then they're ready to go into the recipe.

In Express with a templating engine:
1. You gather data from various sources (database queries, API calls, form data, etc.)
2. You format and prepare that data into a structure that makes sense for your template
3. You pass that prepared data to `res.render()` as the second argument
4. Your template engine receives that data and uses it to fill in placeholders

Different templating engines have slightly different syntaxes for accessing this data, but the concept is the same: you provide data, and the template uses it to generate the final HTML.

## 💻 Code Example

```javascript
// ES Module - Passing Various Data Types to Templates

import express from 'express';

const app = express();

// Set up EJS as our view engine
app.set('view engine', 'ejs');

// ========================================
// PASSING STRING DATA
// ========================================
app.get('/welcome/:name', (req, res) => {
    // We can pass strings directly
    res.render('welcome', { 
        userName: req.params.name,
        greeting: 'Hello' 
    });
});

// ========================================
// PASSING NUMBER DATA
// ========================================
app.get('/product/:id', (req, res) => {
    // Numbers work just like strings
    const productId = parseInt(req.params.id);
    const price = 29.99;
    const discount = 15; // percentage
    
    res.render('product', { 
        productId: productId,
        price: price,
        discount: discount,
        finalPrice: price * (1 - discount/100) // We can do calculations too
    });
});

// ========================================
// PASSING BOOLEAN DATA
// ========================================
app.get('/profile', (req, res) => {
    // Booleans are great for conditionals in templates
    const isLoggedIn = true; // In real app, this would come from session/auth
    const hasPremium = false;
    
    res.render('profile', { 
        isLoggedIn: isLoggedIn,
        hasPremium: hasPremium,
        userName: 'John Doe'
    });
});

// ========================================
// PASSING ARRAY DATA
// ========================================
app.get('/blog', (req, res) => {
    // Arrays are perfect for lists of items
    const posts = [
        { 
            id: 1, 
            title: 'Getting Started with Express', 
            excerpt: 'Learn the basics of Express.js...',
            author: 'Jane Smith',
            date: new Date('2023-01-15')
        },
        { 
            id: 2, 
            title: 'Middleware in Express', 
            excerpt: 'Understanding how middleware works...',
            author: 'Bob Wilson',
            date: new Date('2023-02-20')
        },
        { 
            id: 3, 
            title: 'Routing Basics', 
            excerpt: 'How to define routes in Express...',
            author: 'Alice Johnson',
            date: new Date('2023-03-10')
        }
    ];
    
    res.render('blog/index', { 
        posts: posts,
        totalPosts: posts.length
    });
});

// ========================================
// PASSING OBJECT DATA
// ========================================
app.get('/user/:id', (req, res) => {
    // Objects are great for complex data structures
    // In a real app, this would come from a database
    const user = {
        id: req.params.id,
        name: 'Alex Taylor',
        email: 'alex@example.com',
        profile: {
            bio: 'Software developer and open source enthusiast',
            location: 'San Francisco, CA',
            website: 'https://alexdev.com'
        },
        preferences: {
            theme: 'dark',
            notifications: true,
            newsletter: false
        },
        metadata: {
            createdAt: new Date('2022-05-10'),
            lastLogin: new Date('2023-03-19'),
            loginCount: 42
        }
    };
    
    res.render('user/profile', { 
        user: user,
        pageTitle: `Profile - ${user.name}`
    });
});

// ========================================
// PASSING NULL AND UNDEFINED
// ========================================
app.get('/search', (req, res) => {
    // Sometimes data might be missing, and that's okay
    const searchTerm = req.query.q || null; // Might be null if not provided
    const results = req.query.q ? ['result1', 'result2'] : []; // Empty array if no search
    
    res.render('search', { 
        searchTerm: searchTerm, // Could be null
        results: results,       // Empty array
        hasResults: results.length > 0 // Boolean based on results
    });
});

// ========================================
// WHAT THE TEMPLATE FILES MIGHT LOOK LIKE
// ========================================
// views/welcome.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title>Welcome</title>
</head>
<body>
    <h1><%= greeting %>, <%= userName %>!</h1>
    <p>Nice to meet you!</p>
</body>
</html>
*/

// views/product.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title>Product <%= productId %></title>
</head>
<body>
    <h1>Product Details</h1>
    <p>Price: $<%= price.toFixed(2) %></p>
    <p>Discount: <%= discount %>%</p>
    <p><strong>Final Price: $<%= finalPrice.toFixed(2) %></strong></p>
</body>
</html>
*/

// views/profile.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title>User Profile</title>
</head>
<body>
    <h1>Profile: <%= userName %></h1>
    <% if (isLoggedIn) { %>
        <p>Welcome back!</p>
        <% if (hasPremium) { %>
            <p>You have premium access!</p>
        <% } %>
    <% } else { %>
        <p>Please <a href="/login">log in</a> to view your profile.</p>
    <% } %>
</body>
</html>
*/

// views/blog/index.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title>Blog Posts</title>
</head>
<body>
    <h1>Blog Posts (<%= totalPosts %>)</h1>
    <% posts.forEach(post => { %>
        <div class="post">
            <h2><%= post.title %></h2>
            <p><em>By <%= post.author %> on <%= post.date.toDateString() %></em></p>
            <p><%= post.excerpt %></p>
            <a href="/posts/<%= post.id %>">Read more</a>
        </div>
    <% }); %>
</body>
</html>
*/

// views/user/profile.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title><%= pageTitle %></title>
</head>
<body>
    <h1><%= user.name %></h1>
    <p>Email: <%= user.email %></p>
    
    <h2>Profile Info</h2>
    <p><%= user.profile.bio %></p>
    <p>Location: <%= user.profile.location %></p>
    <p>Website: <a href="<%= user.profile.website %>"><%= user.profile.website %></a></p>
    
    <h2>Preferences</h2>
    <ul>
        <li>Theme: <%= user.preferences.theme %></li>
        <li>Notifications: <%= user.preferences.notifications ? 'Yes' : 'No' %></li>
        <li>Newsletter: <%= user.preferences.newsletter ? 'Yes' : 'No' %></li>
    </ul>
    
    <h2>Account Info</h2>
    <p>Member since: <%= user.metadata.createdAt.toDateString() %></p>
    <p>Last login: <%= user.metadata.lastLogin.toDateString() %></p>
    <p>Login count: <%= user.metadata.loginCount %></p>
</body>
</html>
*/

// views/search.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title>Search Results</title>
</head>
<body>
    <h1>Search Results</h1>
    <% if (searchTerm) { %>
        <p>You searched for: "<%= searchTerm %>"</p>
    <% } else { %>
        <p>Please enter a search term.</p>
    <% } %>
    
    <% if (hasResults) { %>
        <ul>
            <% results.forEach(result => { %>
                <li><%= result %></li>
            <% }); %>
        </ul>
    <% } else if (searchTerm) { %>
        <p>No results found for "<%= searchTerm %>".</p>
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
| 12 | `app.get('/welcome/:name', (req, res) => {` | Define route with parameter |
| 16 | `res.render('welcome', { ... });` | Pass string data to template |
| 22 | `app.get('/product/:id', (req, res) => {` | Define route for product |
| 30 | `const price = 29.99;` | Define number variables |
| 37 | `res.render('product', { ... });` | Pass number data (including calculated value) |
| 42 | `app.get('/profile', (req, res) => {` | Define route for profile |
| 46 | `const isLoggedIn = true;` | Define boolean variables |
| 52 | `res.render('profile', { ... });` | Pass boolean data for conditionals |
| 57 | `app.get('/blog', (req, res) => {` | Define route for blog |
| 65 | `const posts = [ ... ];` | Create array of post objects |
| 76 | `res.render('blog/index', { ... });` | Pass array data for looping |
| 81 | `app.get('/user/:id', (req, res) => {` | Define route for user profile |
| 90 | `const user = { ... };` | Create complex object with nested properties |
| 104 | `res.render('user/profile', { ... });` | Pass object data for deep property access |
| 109 | `app.get('/search', (req, res) => {` | Define route for search |
| 112 | `const searchTerm = req.query.q || null;` | Handle potentially missing data |
| 115 | `res.render('search', { ... });` | Pass mixed data types including null and empty array |
| 118-160 | `// Template examples` | Show how different data types are used in templates |

## ⚠️ Common Mistakes

**1. Passing raw database results without formatting**
Sometimes database results contain extra fields or data types that aren't template-friendly. Always prepare your data specifically for what the template needs.

**2. Forgetting to handle null or undefined values**
If your data might be missing, make sure to check for null/undefined in your template or provide default values when passing data.

**3. Passing too much data to templates**
Only pass the data that your template actually needs. Passing unnecessary data can impact performance and make templates harder to understand.

**4. Mutating passed data in templates**
In most templating engines, you shouldn't try to modify the data objects passed from within the template itself. Treat the data as read-only.

**5. Forgetting to convert data types**
Route parameters are always strings. If you need numbers for calculations or comparisons, remember to convert them with `parseInt()` or `parseFloat()`.

## ✅ Quick Recap

- You can pass any JavaScript data type to templates: strings, numbers, booleans, arrays, objects, null, and undefined
- Prepare your data specifically for what your template needs before passing it
- Use objects and arrays to pass complex data structures
- In templates, access object properties with dot notation (user.name) or bracket notation (user['name'])
- Use array methods like forEach() or map() in templates to loop through data
- Remember that route parameters are strings and may need conversion to numbers
- Handle potentially missing data by providing default values or checking in templates

## 🔗 What's Next

We've covered the basics of templating engines in Express. In the next section, we'll look at specific templating engines like EJS, Pug, and Handlebars in more detail, including their unique features and syntax differences.
