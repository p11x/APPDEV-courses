# Setting Up the Views Folder

## 📌 What You'll Learn
- How to configure the views directory in Express
- Where to place your template files
- How to customize the views folder location
- Best practices for organizing your views

## 🧠 Concept Explained (Plain English)

When you use a templating engine with Express, you need to tell Express where to find your template files. By default, Express looks for a folder named `views` in your application's root directory. This is where you'll store all your template files (like .ejs, .pug, or .hbs files).

Think of the views folder as a special drawer in your filing cabinet where you keep all your letter templates. Just as you wouldn't mix your letter templates with your financial reports, you keep your template files separate from your JavaScript code, static assets (like CSS and images), and other application files.

You can customize where Express looks for views if you want to use a different folder name or location. This might be useful if you have a specific project structure or if you're working with conventions from another framework.

## 💻 Code Example

```javascript
// ES Module - Setting Up the Views Folder

import express from 'express';
import path from 'path';

const app = express();

// ========================================
// SETTING UP THE VIEWS FOLDER (DEFAULT)
// ========================================
// By default, Express looks for a folder named 'views'
// in the root of your application
app.set('view engine', 'ejs');

// No need to explicitly set views folder if using default
// app.set('views', './views'); // This is the default

// ========================================
// CUSTOMIZING THE VIEWS FOLDER LOCATION
// ========================================
// If you want to use a different folder name or location:

// Option 1: Relative path from the current file
// app.set('views', './templates'); // Looks for 'templates' folder in same directory

// Option 2: Absolute path using path module (recommended for production)
// const viewsPath = path.join(process.cwd(), 'templates');
// app.set('views', viewsPath);

// Option 3: Relative to the directory where this file is located
// const viewsPath = path.join(__dirname, 'views');
// app.set('views', viewsPath);

// ========================================
// CREATING A SAMPLE TEMPLATE
// ========================================
// For demonstration, let's assume we have a views folder with this file:
// views/user-profile.ejs
/*
<!DOCTYPE html>
<html>
<head>
    <title>User Profile</title>
</head>
<body>
    <h1>Profile for <%= username %></h1>
    <p>Email: <%= email %></p>
    <p>Age: <%= age %></p>
</body>
</html>
*/

// Route that renders the template
app.get('/user/:username', (req, res) => {
    // In a real app, you'd fetch this data from a database
    const userData = {
        username: req.params.username,
        email: `${req.params.username}@example.com`,
        age: 25
    };
    
    // Express will look for views/user-profile.ejs
    res.render('user-profile', userData);
});

// ========================================
// ORGANIZING VIEWS IN SUBFOLDERS
// ========================================
// You can also organize your views in subfolders:
// views/
//   ├── layouts/
//   │   └── main.ejs
//   ├── users/
//   │   ├── profile.ejs
//   │   └── list.ejs
//   └── products/
//       ├── detail.ejs
//       └── list.ejs

// To render a view in a subfolder:
app.get('/users', (req, res) => {
    // This will look for views/users/list.ejs
    res.render('users/list', { users: [] });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 2 | `import path from 'path';` | Import Node.js path module for working with file paths |
| 5 | `const app = express();` | Create an Express application instance |
| 9 | `app.set('view engine', 'ejs');` | Set the view engine to EJS |
| 13 | `// app.set('views', './views');` | Shows the default views folder setting (commented out) |
| 19 | `app.set('views', './templates');` | Example: Setting views folder to 'templates' (relative) |
| 23 | `const viewsPath = path.join(process.cwd(), 'templates');` | Example: Creating absolute path using path module |
| 27 | `const viewsPath = path.join(__dirname, 'views');` | Example: Path relative to current file |
| 33 | `app.get('/user/:username', (req, res) => {` | Define a route with a parameter |
| 40 | `res.render('user-profile', userData);` | Render the user-profile template with data |
| 52 | `// views/` | Comment showing views folder structure |
| 60 | `app.get('/users', (req, res) => {` | Define route for users list |
| 63 | `res.render('users/list', { users: [] });` | Render template in subfolder |

## ⚠️ Common Mistakes

**1. Forgetting to create the views folder**
If you set a views folder but don't actually create it, you'll get an error when trying to render templates.

**2. Using incorrect path syntax**
Be careful with relative vs. absolute paths. On Windows, paths use backslashes (\), while Unix-like systems use forward slashes (/). Using `path.join()` helps avoid these issues.

**3. Forgetting to restart the server after changing views folder**
If you change the views folder location, you need to restart your Node.js server for the change to take effect.

**4. Using the wrong template path in res.render()**
When using subfolders, make sure to include the correct path in the first argument to `res.render()` (e.g., 'users/list' for views/users/list.ejs).

**5. Mixing up view engine file extensions**
Make sure your template files have the correct extension for your view engine (e.g., .ejs for EJS, .pug for Pug).

## ✅ Quick Recap

- By default, Express looks for a folder named 'views' in your application root
- You can customize the views folder location using `app.set('views', 'path/to/folder')`
- Use the `path` module to create platform-independent file paths
- You can organize views in subfolders and reference them in `res.render()` using the folder structure
- Always create the views folder before trying to render templates from it

## 🔗 What's Next

Now that we know how to set up the views folder, let's learn how to use the `res.render()` method to actually render our templates with data.
