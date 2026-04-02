# Advanced Route Parameters and Validation

## What You'll Learn

- Route parameter extraction and validation
- Dynamic route loading
- Route middleware and guards
- Route versioning strategies

## Route Parameters

```javascript
// Basic parameters
app.get('/users/:id', (req, res) => {
    const { id } = req.params;
    res.json({ userId: id });
});

// Multiple parameters
app.get('/users/:userId/posts/:postId', (req, res) => {
    const { userId, postId } = req.params;
    res.json({ userId, postId });
});

// Optional parameters
app.get('/users/:id/:format?', (req, res) => {
    const { id, format = 'json' } = req.params;
    res.json({ id, format });
});

// Regex constraints
app.get('/users/:id(\\d+)', (req, res) => {
    // Only matches numeric IDs
    res.json({ id: req.params.id });
});
```

## Parameter Validation Middleware

```javascript
import { param, validationResult } from 'express-validator';

// Validation middleware
const validateUserId = [
    param('id')
        .isInt({ min: 1 })
        .withMessage('ID must be a positive integer')
        .toInt(),

    (req, res, next) => {
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return res.status(400).json({ errors: errors.array() });
        }
        next();
    }
];

app.get('/users/:id', validateUserId, getUser);
```

## Route Guards

```javascript
// Authentication guard
function requireAuth(req, res, next) {
    const token = req.headers.authorization?.split(' ')[1];
    if (!token) return res.status(401).json({ error: 'Unauthorized' });

    try {
        req.user = verifyToken(token);
        next();
    } catch {
        res.status(401).json({ error: 'Invalid token' });
    }
}

// Role-based guard
function requireRole(...roles) {
    return (req, res, next) => {
        if (!roles.includes(req.user.role)) {
            return res.status(403).json({ error: 'Forbidden' });
        }
        next();
    };
}

app.get('/admin/users', requireAuth, requireRole('admin'), listUsers);
```

## Route Versioning

```javascript
// URL versioning
app.use('/api/v1/users', v1UserRoutes);
app.use('/api/v2/users', v2UserRoutes);

// Header versioning
app.get('/api/users', (req, res, next) => {
    const version = req.headers['api-version'] || '1';
    if (version === '2') return v2Handler(req, res, next);
    return v1Handler(req, res, next);
});
```

## Best Practices Checklist

- [ ] Validate all route parameters
- [ ] Use parameter middleware for common validations
- [ ] Implement route guards for protected routes
- [ ] Version APIs for backward compatibility
- [ ] Document all route parameters

## Cross-References

- See [Router Internals](../01-express-architecture/02-router-internals.md) for router implementation
- See [Middleware Guide](../03-middleware-guide/01-custom-middleware.md) for middleware
- See [Security](../05-security-implementation/01-helmet-cors.md) for security

## Next Steps

Continue to [Route Composition](./02-route-composition.md) for modular routing.
