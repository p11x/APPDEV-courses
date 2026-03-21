# Express.js Project Structure

## Organizing Your Application

As your Express application grows, keeping your code organized becomes essential. This guide shows you how to structure your project for maintainability and scalability.

## Why Structure Matters

A well-organized project:

- Makes it easier to find and fix bugs
- Helps team members understand the codebase
- Scales better as features are added
- Simplifies testing

## Recommended Folder Structure

Here's a professional Express.js project structure:

```
my-express-app/
├── node_modules/           # Dependencies (auto-created by npm)
├── src/
│   ├── config/             # Configuration files
│   │   └── database.js    # Database connection settings
│   ├── controllers/       # Request handlers (business logic)
│   │   └── userController.js
│   ├── middleware/        # Custom middleware functions
│   │   ├── auth.js
│   │   └── logger.js
│   ├── models/            # Data models (database schemas)
│   │   └── User.js
│   ├── routes/            # Route definitions
│   │   ├── userRoutes.js
│   │   └── index.js
│   ├── utils/             # Helper functions
│   │   └── helpers.js
│   ├── views/             # Template files (if using server-side rendering)
│   │   └── index.ejs
│   └── app.js             # Main Express app configuration
├── tests/                 # Test files
│   └── user.test.js
├── package.json           # Project dependencies
├── server.js              # Entry point
└── .env                   # Environment variables (secret!)
```

## Creating the Structure

### Step 1: Create Directories

```bash
mkdir -p src/{config,controllers,middleware,models,routes,utils,views}
mkdir tests
```

### Step 2: Create the Entry Point (server.js)

The entry point starts your application:

```javascript
// server.js
// Entry point - this is where the application starts

import app from './src/app.js';
// 'app' is imported from our configured Express application

// Define the port
// process.env.PORT allows hosting platforms to specify their own port
const PORT = process.env.PORT || 3000;

// Start the server
// app.listen() binds the app to a network port
app.listen(PORT, () => {
    console.log(`Server running in ${process.env.NODE_ENV || 'development'} mode`);
    console.log(`Visit: http://localhost:${PORT}`);
});
```

### Step 3: Create the App Configuration (src/app.js)

This is where we configure Express and connect all parts:

```javascript
// src/app.js
// Main application configuration

import express from 'express';
import userRoutes from './routes/userRoutes.js';

// Create Express application
const app = express();

// Built-in middleware to parse JSON bodies
// This allows Express to read JSON data from requests
// Example: req.body will contain parsed JSON
app.use(express.json());

// Another built-in middleware for URL-encoded form data
app.use(express.urlencoded({ extended: true }));

// Mount routes
// This connects /api/users routes to our app
app.use('/api/users', userRoutes);

// Root route
app.get('/', (req, res) => {
    res.json({ 
        message: 'Welcome to the API',
        version: '1.0.0'
    });
});

// Export the app for testing and use in server.js
export default app;
```

### Step 4: Create Route Files (src/routes/userRoutes.js)

Routes define how the app responds to different URLs:

```javascript
// src/routes/userRoutes.js
// Defines routes for user-related endpoints

import express from 'express';
// Router() creates a mini Express app for modular routes
const router = express.Router();
import { 
    getUsers, 
    getUserById, 
    createUser, 
    updateUser, 
    deleteUser 
} from '../controllers/userController.js';

// Route table:
// | Method | URL          | Controller Function | Description           |
// |--------|--------------|---------------------|-----------------------|
// | GET    | /users       | getUsers           | Get all users        |
// | GET    | /users/:id   | getUserById        | Get single user      |
// | POST   | /users       | createUser         | Create new user      |
// | PUT    | /users/:id   | updateUser         | Update user          |
// | DELETE | /users/:id   | deleteUser         | Delete user          |

// GET /users - Retrieve all users
router.get('/', getUsers);

// GET /users/:id - Retrieve user by ID
router.get('/:id', getUserById);

// POST /users - Create a new user
router.post('/', createUser);

// PUT /users/:id - Update a user
router.put('/:id', updateUser);

// DELETE /users/:id - Delete a user
router.delete('/:id', deleteUser);

export default router;
```

### Step 5: Create Controller Files (src/controllers/userController.js)

Controllers contain the actual logic for handling requests:

```javascript
// src/controllers/userController.js
// Request handlers - these contain the actual business logic

// Mock data (in real apps, this would come from a database)
const users = [
    { id: 1, name: 'Alice', email: 'alice@example.com' },
    { id: 2, name: 'Bob', email: 'bob@example.com' }
];

// GET /users - Get all users
export const getUsers = (req, res) => {
    // req = request object (information from client)
    // res = response object (what we send back)
    res.json(users);
};

// GET /users/:id - Get user by ID
export const getUserById = (req, res) => {
    const userId = parseInt(req.params.id);
    const user = users.find(u => u.id === userId);
    
    if (!user) {
        return res.status(404).json({ message: 'User not found' });
    }
    
    res.json(user);
};

// POST /users - Create new user
export const createUser = (req, res) => {
    // req.body contains the JSON data sent by the client
    const newUser = {
        id: users.length + 1,
        ...req.body
    };
    users.push(newUser);
    res.status(201).json(newUser);
};

// PUT /users/:id - Update user
export const updateUser = (req, res) => {
    const userId = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === userId);
    
    if (userIndex === -1) {
        return res.status(404).json({ message: 'User not found' });
    }
    
    // Update user with new data
    users[userIndex] = { ...users[userIndex], ...req.body };
    res.json(users[userIndex]);
};

// DELETE /users/:id - Delete user
export const deleteUser = (req, res) => {
    const userId = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === userId);
    
    if (userIndex === -1) {
        return res.status(404).json({ message: 'User not found' });
    }
    
    // Remove user from array
    users.splice(userIndex, 1);
    res.status(204).send();
};
```

## Environment Variables

Never hardcode sensitive information! Use environment variables:

### Creating a .env File

Create a `.env` file in your project root:

```
PORT=3000
DB_HOST=localhost
DB_USER=myuser
DB_PASSWORD=secretpassword
NODE_ENV=development
```

### Accessing Environment Variables

```javascript
// In any file
const PORT = process.env.PORT || 3000;
const DB_HOST = process.env.DB_HOST;

// process.env accesses environment variables
// These are set outside your code for security and flexibility
```

> **Why use process.env?** 
> - Keeps secrets out of your code
> - Different values for development vs production
> - Hosting platforms (like Heroku) provide their own values

## MVC Pattern

The structure we created follows **MVC** (Model-View-Controller):

| Layer | Responsibility | Our Files |
|-------|----------------|-----------|
| **Model** | Data and business rules | `src/models/` |
| **View** | Presentation (templates) | `src/views/` |
| **Controller** | Request handling logic | `src/controllers/` |

## What's Next?

Now that your project is organized, dive into:

- **[Basic Routing](../02_Routing/01_basic_routing.md)** — Learn route matching
- **[Middleware](../03_Middleware/01_introduction.md)** — Add functionality to requests
- **[Request & Response](../04_Request_Response/01_request_object.md)** — Work with data

---

> 💡 **Tip**: Start with this structure from day one. It'll save you refactoring time later!
