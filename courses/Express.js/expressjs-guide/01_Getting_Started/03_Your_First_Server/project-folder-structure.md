# Project Folder Structure

## 📌 What You'll Learn
- How to organize your Express project
- Common folder structures and their purposes
- Best practices for scalable applications

## 🧠 Concept Explained (Plain English)

As your Express application grows, organizing files becomes crucial. A well-structured project makes it easy to find what you're looking for, collaborate with others, and maintain your code over time.

Think of it like organizing a filing cabinet. If everything is in one drawer, you can't find anything. But if you have labeled folders — "invoices," "receipts," "contracts" — everything is easy to locate.

Express doesn't force a particular structure, which is both a blessing and a curse. The blessing is flexibility; the curse is that beginners might not know the best way to organize. This guide shows you proven patterns used in real-world Express applications.

## 💻 Recommended Project Structure

```
my-express-app/
├── node_modules/           # Dependencies (auto-created by npm)
├── src/
│   ├── config/             # Configuration files
│   │   └── database.js    # Database connection settings
│   ├── controllers/        # Route handlers (business logic)
│   │   └── userController.js
│   ├── middleware/        # Custom middleware functions
│   │   ├── auth.js
│   │   └── logger.js
│   ├── models/            # Database schemas/models
│   │   └── User.js
│   ├── routes/            # Route definitions
│   │   ├── userRoutes.js
│   │   └── index.js
│   ├── services/          # Business logic (optional layer)
│   │   └── userService.js
│   ├── utils/             # Helper functions
│   │   └── helpers.js
│   ├── views/             # Template files (if using server-side rendering)
│   │   └── index.ejs
│   ├── app.js             # Main Express app configuration
│   └── server.js          # Entry point
├── tests/                 # Test files
│   └── user.test.js
├── public/                # Static files (CSS, images)
│   ├── css/
│   │   └── style.css
│   └── images/
├── .env                   # Environment variables (secret!)
├── .gitignore             # Git ignore rules
├── package.json           # Project dependencies
├── package-lock.json      # Locked dependency versions
└── README.md             # Project documentation
```

## Understanding Each Folder

| Folder | Purpose | Contains |
|--------|---------|----------|
| `src/config` | Configuration | Database connections, API keys |
| `src/controllers` | Business logic | How to handle requests |
| `src/middleware` | Request processing | Authentication, logging |
| `src/models` | Data schemas | Database models |
| `src/routes` | URL definitions | Which URLs exist |
| `src/services` | Reusable logic | Shared business logic |
| `src/utils` | Helpers | Utility functions |
| `src/views` | Templates | HTML templates |
| `public` | Static files | CSS, images, client JS |
| `tests` | Tests | Test files |

## Entry Point Files

### server.js (Entry Point)

```javascript
// server.js
// This is where your application starts
// It imports the app and starts the server

import app from './src/app.js';

const PORT = process.env.PORT || 3000;

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

### src/app.js (Main Configuration)

```javascript
// src/app.js
// Main Express app configuration
// All middleware and routes are set up here

import express from 'express';
import userRoutes from './routes/userRoutes.js';

const app = express();

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Routes
app.use('/api/users', userRoutes);

// Root route
app.get('/', (req, res) => {
    res.json({ message: 'Welcome to the API' });
});

export default app;
```

## Alternative Structure for Small Apps

For very simple applications, a flatter structure works:

```
simple-app/
├── src/
│   ├── routes.js          # All routes in one file
│   ├── app.js            # App setup
│   └── server.js         # Entry point
├── package.json
└── .env
```

## ⚠️ Common Mistakes

**1. Putting everything in one file**
As your app grows, one file becomes unmanageable. Start with a structured approach early.

**2. Not separating concerns**
Keep routes, controllers, and models in separate files. Mixing them makes debugging harder.

**3. Forgetting to ignore node_modules and .env**
Add these to your .gitignore so you don't commit secrets or huge folders.

## ✅ Quick Recap

- Organize by function (routes, controllers, models, etc.)
- Use a `src/` folder for your source code
- Separate configuration from application code
- Keep static files in `public/`

## 🔗 What's Next

Let's learn about ports and hosts.
