# Using ORMs with Express

## 📌 What You'll Learn
- What an ORM is and why you'd use one
- How to use Sequelize (a popular Node.js ORM)
- Defining models and relationships
- Performing CRUD operations with an ORM

## 🧠 Concept Explained (Plain English)

An ORM (Object-Relational Mapping) is like a translator between your code and the database. Instead of speaking SQL (the database's language), you can speak JavaScript (your programming language), and the ORM translates it to SQL for you.

Think of it like traveling in a foreign country:
- **Without ORM**: You need to speak the local language (SQL) directly
- **With ORM**: You have a translator (ORM) who speaks both languages

Benefits of using an ORM:
- You write JavaScript instead of SQL
- Code is more readable and maintainable
- You can switch databases (MySQL to PostgreSQL) with minimal code changes
- Built-in protections against SQL injection
- Easy relationships between data models

Popular Node.js ORMs:
- **Sequelize**: Supports PostgreSQL, MySQL, SQLite, MariaDB
- **TypeORM**: Works with TypeScript and JavaScript
- **Prisma**: Modern ORM with great developer experience
- **Mongoose**: MongoDB ODM (Object Document Mapper)

## 💻 Code Example

```javascript
// ES Module - Using Sequelize ORM with Express

import express from 'express';

const app = express();

app.use(express.json());

/*
// ========================================
// SEQUELIZE SETUP EXAMPLE
// ========================================

import { Sequelize, DataTypes, Model } from 'sequelize';

// Using environment variables for database credentials
// This keeps sensitive info out of your code
const sequelize = new Sequelize(
    process.env.DB_NAME || 'myapp',
    process.env.DB_USER || 'postgres',
    process.env.DB_PASSWORD || 'password',
    {
        host: process.env.DB_HOST || 'localhost',
        dialect: 'postgres', // or 'mysql', 'sqlite', etc.
        logging: false // Set to console.log to see SQL queries
    }
);

// ========================================
// DEFINING MODELS
// ========================================

// Define User model
class User extends Model {}
User.init({
    username: {
        type: DataTypes.STRING,
        allowNull: false,
        unique: true
    },
    email: {
        type: DataTypes.STRING,
        allowNull: false,
        validate: {
            isEmail: true
        }
    },
    password: {
        type: DataTypes.STRING,
        allowNull: false
    },
    role: {
        type: DataTypes.ENUM('user', 'admin'),
        defaultValue: 'user'
    }
}, {
    sequelize,
    modelName: 'User',
    tableName: 'users'
});

// Define Post model
class Post extends Model {}
Post.init({
    title: {
        type: DataTypes.STRING,
        allowNull: false
    },
    content: {
        type: DataTypes.TEXT,
        allowNull: false
    },
    published: {
        type: DataTypes.BOOLEAN,
        defaultValue: false
    }
}, {
    sequelize,
    modelName: 'Post',
    tableName: 'posts'
});

// ========================================
// DEFINING RELATIONSHIPS
// ========================================

// One-to-Many: A User can have many Posts
User.hasMany(Post, { foreignKey: 'authorId' });
Post.belongsTo(User, { as: 'author', foreignKey: 'authorId' });

// ========================================
// CONNECT AND SYNC
// ========================================

// Connect to database and create tables
async function initializeDatabase() {
    try {
        await sequelize.authenticate();
        console.log('Database connection established');
        
        await sequelize.sync({ alter: true }); // Creates/updates tables
        console.log('Database synchronized');
    } catch (error) {
        console.error('Unable to connect to database:', error);
    }
}

initializeDatabase();
*/

// ========================================
// EXAMPLE ROUTES WITH "ORM-LIKE" OPERATIONS
// ========================================

// Mock data (simulating database)
const users = [
    { id: 1, username: 'alice', email: 'alice@example.com', role: 'admin' },
    { id: 2, username: 'bob', email: 'bob@example.com', role: 'user' }
];

const posts = [
    { id: 1, title: 'Hello World', content: 'My first post!', authorId: 1, published: true },
    { id: 2, title: 'Express Tips', content: 'Express is awesome!', authorId: 1, published: true }
];

// ========================================
// CRUD OPERATIONS
// ========================================

// CREATE - Add new record
app.post('/users', async (req, res) => {
    const { username, email, role } = req.body;
    
    // Simulate ORM create operation
    // In real Sequelize: const user = await User.create({...});
    const newUser = {
        id: users.length + 1,
        username,
        email,
        role: role || 'user'
    };
    
    users.push(newUser);
    res.status(201).json(newUser);
});

// READ - Get all records
app.get('/users', async (req, res) => {
    // In real Sequelize: const users = await User.findAll();
    // You can also add where, order, limit, etc.
    // const users = await User.findAll({ where: { role: 'admin' } });
    res.json(users);
});

// READ - Get single record
app.get('/users/:id', async (req, res) => {
    const user = users.find(u => u.id === parseInt(req.params.id));
    
    if (!user) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    res.json(user);
});

// UPDATE - Modify record
app.put('/users/:id', async (req, res) => {
    const id = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === id);
    
    if (userIndex === -1) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    const { username, email, role } = req.body;
    
    // In Sequelize: await User.update({...}, { where: { id } });
    users[userIndex] = {
        ...users[userIndex],
        username: username || users[userIndex].username,
        email: email || users[userIndex].email,
        role: role || users[userIndex].role
    };
    
    res.json(users[userIndex]);
});

// DELETE - Remove record
app.delete('/users/:id', async (req, res) => {
    const id = parseInt(req.params.id);
    const userIndex = users.findIndex(u => u.id === id);
    
    if (userIndex === -1) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    // In Sequelize: await User.destroy({ where: { id } });
    users.splice(userIndex, 1);
    res.status(204).send();
});

// ========================================
// QUERYING WITH RELATIONSHIPS
// ========================================

// Get user with posts
app.get('/users/:id/posts', async (req, res) => {
    const userId = parseInt(req.params.id);
    const userPosts = posts.filter(p => p.authorId === userId);
    
    res.json(userPosts);
});

// Get posts with author
app.get('/posts', async (req, res) => {
    // In Sequelize with eager loading:
    // const posts = await Post.findAll({ include: User });
    
    // Simulated
    const postsWithAuthors = posts.map(post => {
        const author = users.find(u => u.id === post.authorId);
        return { ...post, author };
    });
    
    res.json(postsWithAuthors);
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});

/*
// ========================================
// COMMON SEQUELIZE QUERIES
// ========================================

// Find all
const users = await User.findAll();

// Find one
const user = await User.findByPk(1);

// Find with conditions
const admins = await User.findAll({ where: { role: 'admin' } });

// Find with ordering
const sorted = await User.findAll({ order: [['createdAt', 'DESC']] });

// Find with pagination
const paginated = await User.findAll({ limit: 10, offset: 0 });

// Count
const count = await User.count({ where: { role: 'admin' } });

// Increment/Decrement
await user.increment('loginCount');
await user.decrement('balance', { by: 50 });
*/
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 22 | `new Sequelize(...)` | Create Sequelize instance with config |
| 40 | `class User extends Model` | Define User model class |
| 55 | `User.init({...}, {...})` | Initialize model with attributes and options |
| 82 | `User.hasMany(Post, {...})` | Define one-to-many relationship |
| 93 | `sequelize.sync({ alter: true })` | Create/update database tables |
| 156 | `app.post('/users', async (req, res) => {...})` | Create route (async/await) |
| 198 | `app.get('/users/:id/posts', ...)` | Get related records |

## ⚠️ Common Mistakes

**1. Not using environment variables for credentials**
Never hardcode database passwords in your code!

**2. Mixing sync and async code**
ORM operations return Promises. Always use async/await or .then().

**3. Not handling errors**
Wrap ORM operations in try/catch to handle database errors properly.

**4. Creating N+1 queries**
When fetching related data, use eager loading (`include`) to avoid multiple queries.

**5. Not using transactions**
When making multiple database changes that must all succeed, use transactions.

## ✅ Quick Recap

- ORMs translate between programming languages and SQL
- Popular Node.js ORMs: Sequelize, TypeORM, Prisma
- Define models to represent database tables
- Use relationships to connect models (hasMany, belongsTo)
- Use `include` for eager loading related data
- Always use async/await with ORM operations
- Keep database credentials in environment variables

## 🔗 What's Next

Now let's look at the Advanced Topics section, which covers deployment, testing, and performance optimization.
