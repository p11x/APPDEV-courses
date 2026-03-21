# Authorization in Express.js

## What is Authorization?

**Authorization** determines what a user can do after they've been authenticated. While authentication answers "who are you?", authorization answers "what can you do?"

Examples:
- Regular users can view their own profile
- Admins can delete any user's data
- Premium users can access premium features

## Role-Based Access Control (RBAC)

**RBAC** assigns users roles (like "user", "admin", "moderator") and controls access based on those roles.

### Create Roles Middleware

```javascript
// middleware/authorization.js
// Authorization middleware for role-based access control

// Middleware to check if user has specific role(s)
export const authorize = (...roles) => {
    return (req, res, next) => {
        // req.user should be set by authentication middleware
        // req.user.role contains the user's role from the database
        
        if (!req.user) {
            return res.status(401).json({
                error: 'Not authenticated'
            });
        }
        
        // Check if user's role is in the allowed roles
        // Example: authorize('admin', 'moderator') allows both
        if (!roles.includes(req.user.role)) {
            return res.status(403).json({
                error: 'Access denied. Insufficient permissions.'
            });
        }
        
        // User has permission, continue
        next();
    };
};

// Middleware to check if user owns the resource
// Useful for "edit your own data" scenarios
export const authorizeOwner = (paramName = 'id') => {
    return (req, res, next) => {
        // Get resource ID from request params
        const resourceId = req.params[paramName];
        
        // User must either:
        // 1. Be an admin (can do anything)
        // 2. Own the resource (resource.userId matches user.id)
        if (req.user.role !== 'admin' && req.user.id !== parseInt(resourceId)) {
            return res.status(403).json({
                error: 'Access denied. You can only modify your own data.'
            });
        }
        
        next();
    };
};
```

### Using Authorization in Routes

```javascript
// server.js
import express from 'express';
import { authenticate } from './middleware/auth.js';
import { authorize } from './middleware/authorization.js';

const app = express();
app.use(express.json());

// ============================================
// Route Permissions Table
// ============================================
// | URL              | Method | Role Required | Description          |
// |------------------|--------|---------------|----------------------|
// | /admin/users     | GET    | admin        | View all users      |
// | /admin/users     | POST   | admin        | Create user         |
// | /posts           | GET    | any          | View posts          |
// | /posts           | POST   | user         | Create post         |
// | /posts/:id       | PUT    | owner        | Edit own post       |
// | /posts/:id       | DELETE | owner/admin  | Delete post         |
// ============================================

// Public route - anyone can access
app.get('/posts', (req, res) => {
    res.json({ message: 'Public posts' });
});

// Protected route - requires authentication
app.post('/posts', authenticate, (req, res) => {
    // Create post logic
    res.json({ message: 'Post created' });
});

// Admin only route
app.get('/admin/users', 
    authenticate,                    // First: check if logged in
    authorize('admin'),               // Second: check if admin
    (req, res) => {
        // Admin-only logic
        res.json({ message: 'Admin area' });
    }
);

// Moderator and admin routes
app.delete('/posts/:id',
    authenticate,
    authorize('admin', 'moderator'),
    (req, res) => res.json({ message: 'Post deleted' })
);

// Owner-only edit (check post belongs to user)
app.put('/posts/:id', 
    authenticate,
    (req, res, next) => {
        // Custom logic to check ownership
        // In real app: check if post.userId === req.user.id
        next();
    },
    (req, res) => res.json({ message: 'Post updated' })
);

// Error handler
app.use((err, req, res, next) => {
    res.status(500).json({ error: 'Server error' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Complete Example with User Roles

```javascript
// controllers/postController.js
import { authenticate } from '../middleware/auth.js';
import { authorize } from '../middleware/authorization.js';

// Mock database
let posts = [
    { id: 1, userId: 1, title: 'Post 1', content: 'Content 1' },
    { id: 2, userId: 2, title: 'Post 2', content: 'Content 2' }
];

// Get all posts - public
export const getPosts = (req, res) => {
    res.json({ data: posts });
};

// Get single post - public
export const getPost = (req, res) => {
    const post = posts.find(p => p.id === parseInt(req.params.id));
    
    if (!post) {
        return res.status(404).json({ error: 'Post not found' });
    }
    
    res.json({ data: post });
};

// Create post - authenticated users only
export const createPost = (req, res) => {
    // req.userId set by authenticate middleware
    const newPost = {
        id: posts.length + 1,
        userId: req.userId,
        title: req.body.title,
        content: req.body.content
    };
    
    posts.push(newPost);
    
    res.status(201).json({ data: newPost });
};

// Update post - only owner or admin
export const updatePost = (req, res) => {
    const postId = parseInt(req.params.id);
    const postIndex = posts.findIndex(p => p.id === postId);
    
    if (postIndex === -1) {
        return res.status(404).json({ error: 'Post not found' });
    }
    
    // Check ownership or admin
    // req.user set by authenticate middleware with user data from DB
    const isOwner = posts[postIndex].userId === req.user.id;
    const isAdmin = req.user.role === 'admin';
    
    if (!isOwner && !isAdmin) {
        return res.status(403).json({ 
            error: 'Not authorized to edit this post' 
        });
    }
    
    // Update post
    posts[postIndex] = {
        ...posts[postIndex],
        ...req.body
    };
    
    res.json({ data: posts[postIndex] });
};

// Delete post - admin only
export const deletePost = (req, res) => {
    const postId = parseInt(req.params.id);
    const postIndex = posts.findIndex(p => p.id === postId);
    
    if (postIndex === -1) {
        return res.status(404).json({ error: 'Post not found' });
    }
    
    // Remove post
    posts.splice(postIndex, 1);
    
    res.json({ message: 'Post deleted' });
};

// Apply middleware in routes
// app.get('/posts', getPosts);
// app.post('/posts', authenticate, createPost);
// app.put('/posts/:id', authenticate, updatePost);
// app.delete('/posts/:id', authenticate, authorize('admin'), deletePost);
```

## Permission Matrix

| Action | Regular User | Moderator | Admin |
|--------|--------------|-----------|-------|
| View own profile | ✅ | ✅ | ✅ |
| Edit own profile | ✅ | ✅ | ✅ |
| View other profiles | ❌ | ✅ | ✅ |
| Delete own posts | ✅ | ✅ | ✅ |
| Delete others' posts | ❌ | ✅ | ✅ |
| View all users | ❌ | ✅ | ✅ |
| Create users | ❌ | ❌ | ✅ |
| Delete users | ❌ | ❌ | ✅ |

## Best Practices

| Practice | Why |
|----------|-----|
| Check permissions at every level | Defense in depth |
| Use middleware for reusability | Clean, maintainable code |
| Log access attempts | Security auditing |
| Don't rely solely on UI hiding | Server-side checks always |
| Use least privilege | Give minimum necessary access |

## What's Next?

- **[Security Best Practices](./03_best_practices.md)** — Comprehensive security checklist
- **[Testing](../09_Testing/01_unit_testing.md)** — Testing your API
