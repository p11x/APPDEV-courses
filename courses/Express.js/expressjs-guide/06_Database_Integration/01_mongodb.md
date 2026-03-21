# MongoDB with Express.js

## What is MongoDB?

**MongoDB** is a popular NoSQL database that stores data in flexible, JSON-like documents. It's perfect for JavaScript applications because the data format matches how we work with objects in code.

Think of MongoDB as a big filing cabinet where each "file" (document) can have different fields — no rigid table structure required!

## Setting Up MongoDB with Mongoose

**Mongoose** is a library that makes working with MongoDB easier. It provides:
- Schema definition (structure your data)
- Data validation
- Connection management
- Query building

### Step 1: Install Dependencies

```bash
npm install mongoose dotenv
```

### Step 2: Connect to MongoDB

```javascript
// config/database.js
import mongoose from 'mongoose';

// MongoDB connection function
// This establishes a connection to your MongoDB database
export const connectDB = async () => {
    // process.env.MONGODB_URI stores the database URL
    // We use environment variables so the URL isn't hardcoded
    const MONGODB_URI = process.env.MONGODB_URI || 'mongodb://localhost:27017/myapp';
    
    try {
        // Connect to MongoDB
        await mongoose.connect(MONGODB_URI);
        
        console.log('✅ MongoDB Connected');
    } catch (error) {
        console.error('MongoDB connection error:', error.message);
        // Exit process if can't connect to database
        process.exit(1);
    }
};
```

### Step 3: Create a Model (Schema)

**Mongoose models** define what your data looks like:

```javascript
// models/User.js
import mongoose from 'mongoose';

// Define the user schema - what each user document looks like
const userSchema = new mongoose.Schema({
    // name is a required string
    name: {
        type: String,
        required: [true, 'Please provide a name'],
        trim: true
    },
    
    // email must be unique and required
    email: {
        type: String,
        required: [true, 'Please provide an email'],
        unique: true,
        lowercase: true,
        trim: true
    },
    
    // password - minimum 6 characters
    password: {
        type: String,
        required: [true, 'Please provide a password'],
        minlength: 6,
        select: false // Don't include in queries by default
    },
    
    // age is optional number
    age: {
        type: Number,
        min: 0,
        max: 150
    },
    
    // role with default value
    role: {
        type: String,
        enum: ['user', 'admin'],
        default: 'user'
    },
    
    // timestamps - automatically add createdAt and updatedAt
}, {
    timestamps: true
});

// Create the User model from the schema
// This gives us methods like User.find(), User.create(), etc.
const User = mongoose.model('User', userSchema);

export default User;
```

### Step 4: Use the Model in Routes

```javascript
// server.js
import express from 'express';
import { connectDB } from './config/database.js';
import User from './models/User.js';

const app = express();

// Parse JSON bodies
app.use(express.json());

// Connect to MongoDB before starting server
connectDB();

// ============================================
// User Routes Table
// ============================================
// | Method | URL           | Handler        | Description           |
// |--------|---------------|----------------|-----------------------|
// | GET    | /api/users    | getUsers      | Get all users        |
// | GET    | /api/users/:id| getUserById   | Get single user      |
// | POST   | /api/users    | createUser    | Create new user      |
// | PUT    | /api/users/:id| updateUser    | Update user          |
// | DELETE | /api/users/:id| deleteUser    | Delete user          |
// ============================================

// GET /api/users - Get all users
export const getUsers = async (req, res) => {
    try {
        // User.find() - returns all users in the collection
        // .select('-password') - excludes the password field
        const users = await User.find().select('-password');
        
        res.json({
            success: true,
            count: users.length,
            data: users
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            message: error.message
        });
    }
};

// GET /api/users/:id - Get single user
export const getUserById = async (req, res) => {
    try {
        const user = await User.findById(req.params.id).select('-password');
        
        if (!user) {
            return res.status(404).json({
                success: false,
                message: 'User not found'
            });
        }
        
        res.json({
            success: true,
            data: user
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            message: error.message
        });
    }
};

// POST /api/users - Create new user
export const createUser = async (req, res) => {
    try {
        // User.create() - creates a new document
        const user = await User.create(req.body);
        
        res.status(201).json({
            success: true,
            data: {
                id: user._id,
                name: user.name,
                email: user.email
            }
        });
    } catch (error) {
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// PUT /api/users/:id - Update user
export const updateUser = async (req, res) => {
    try {
        // User.findByIdAndUpdate() - finds and updates
        // { new: true } - return the updated document
        // { runValidators: true } - run schema validators
        const user = await User.findByIdAndUpdate(
            req.params.id,
            req.body,
            { new: true, runValidators: true }
        ).select('-password');
        
        if (!user) {
            return res.status(404).json({
                success: false,
                message: 'User not found'
            });
        }
        
        res.json({
            success: true,
            data: user
        });
    } catch (error) {
        res.status(400).json({
            success: false,
            message: error.message
        });
    }
};

// DELETE /api/users/:id - Delete user
export const deleteUser = async (req, res) => {
    try {
        const user = await User.findByIdAndDelete(req.params.id);
        
        if (!user) {
            return res.status(404).json({
                success: false,
                message: 'User not found'
            });
        }
        
        res.json({
            success: true,
            message: 'User deleted'
        });
    } catch (error) {
        res.status(500).json({
            success: false,
            message: error.message
        });
    }
};

// Register routes
app.get('/api/users', getUsers);
app.get('/api/users/:id', getUserById);
app.post('/api/users', createUser);
app.put('/api/users/:id', updateUser);
app.delete('/api/users/:id', deleteUser);

// Error handler
app.use((err, req, res, next) => {
    console.error(err.message);
    res.status(500).json({
        success: false,
        message: 'Server error'
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Common Mongoose Methods

| Method | Purpose |
|--------|---------|
| `Model.find()` | Get all matching documents |
| `Model.findById(id)` | Get document by ID |
| `Model.findOne()` | Get first matching document |
| `Model.create(data)` | Create new document |
| `Model.findByIdAndUpdate()` | Find and update |
| `Model.findByIdAndDelete()` | Find and delete |
| `Model.countDocuments()` | Count matching documents |

## Query Examples

```javascript
// Find all users
const users = await User.find();

// Find by condition
const admins = await User.find({ role: 'admin' });

// Find with sorting
const sorted = await User.find().sort({ name: 1 }); // 1 = ascending

// Find with pagination
const page = await User.find()
    .skip(0)      // Skip first 10
    .limit(10);   // Take 10

// Find with specific fields
const names = await User.find().select('name email');

// Find one
const user = await User.findOne({ email: 'alice@example.com' });
```

## Environment Variables

Create a `.env` file:

```
# .env
MONGODB_URI=mongodb://localhost:27017/myapp
PORT=3000
NODE_ENV=development
```

## What's Next?

- **[SQL Databases](./02_sql_databases.md)** — Using PostgreSQL/MySQL
- **[Authentication](../08_Security/01_authentication.md)** — User authentication
