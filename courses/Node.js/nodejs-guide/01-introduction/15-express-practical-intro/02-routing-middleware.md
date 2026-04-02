# Route Creation, Handling, and Middleware Implementation

## What You'll Learn

- Advanced routing patterns
- Creating reusable middleware
- Route parameter validation
- Request/response pipeline management

## Advanced Routing

### Route Organization

```javascript
// src/routes/index.js — Route aggregator

import { Router } from 'express';
import userRoutes from './users.js';
import productRoutes from './products.js';
import orderRoutes from './orders.js';

const router = Router();

router.use('/users', userRoutes);
router.use('/products', productRoutes);
router.use('/orders', orderRoutes);

export default router;
```

### Route Parameter Validation

```javascript
// src/middleware/validate.js — Validation middleware

function validateId(req, res, next) {
    const id = parseInt(req.params.id, 10);
    if (isNaN(id) || id < 1) {
        return res.status(400).json({ error: 'Invalid ID parameter' });
    }
    req.params.id = id;
    next();
}

// Usage
router.get('/:id', validateId, controller.getById);
router.put('/:id', validateId, controller.update);
router.delete('/:id', validateId, controller.remove);
```

### Query Parameter Parsing

```javascript
// src/middleware/parseQuery.js — Query parameter handling

function parsePagination(req, res, next) {
    req.pagination = {
        page: Math.max(1, parseInt(req.query.page, 10) || 1),
        limit: Math.min(100, Math.max(1, parseInt(req.query.limit, 10) || 20)),
        offset: 0,
    };
    req.pagination.offset = (req.pagination.page - 1) * req.pagination.limit;
    next();
}

function parseSort(req, res, next) {
    const allowedFields = ['name', 'createdAt', 'updatedAt', 'price'];
    const field = req.query.sort || 'createdAt';
    const order = req.query.order === 'asc' ? 'asc' : 'desc';
    
    req.sort = {
        field: allowedFields.includes(field) ? field : 'createdAt',
        order,
    };
    next();
}

// Usage
router.get('/', parsePagination, parseSort, controller.list);
```

## Creating Reusable Middleware

### Authentication Middleware

```javascript
// src/middleware/auth.js — Authentication

import jwt from 'jsonwebtoken';

export function authenticate(req, res, next) {
    const authHeader = req.headers.authorization;
    
    if (!authHeader?.startsWith('Bearer ')) {
        return res.status(401).json({ error: 'No token provided' });
    }
    
    const token = authHeader.slice(7);
    
    try {
        const decoded = jwt.verify(token, process.env.JWT_SECRET);
        req.user = decoded;
        next();
    } catch (err) {
        if (err.name === 'TokenExpiredError') {
            return res.status(401).json({ error: 'Token expired' });
        }
        return res.status(401).json({ error: 'Invalid token' });
    }
}

export function authorize(...roles) {
    return (req, res, next) => {
        if (!req.user) {
            return res.status(401).json({ error: 'Not authenticated' });
        }
        if (!roles.includes(req.user.role)) {
            return res.status(403).json({ error: 'Insufficient permissions' });
        }
        next();
    };
}
```

### Request Validation Middleware

```javascript
// src/middleware/validate.js — Request body validation

export function validateBody(schema) {
    return (req, res, next) => {
        const errors = [];
        
        for (const [field, rules] of Object.entries(schema)) {
            const value = req.body[field];
            
            if (rules.required && (value === undefined || value === null)) {
                errors.push(`${field} is required`);
                continue;
            }
            
            if (value !== undefined) {
                if (rules.type === 'string' && typeof value !== 'string') {
                    errors.push(`${field} must be a string`);
                }
                if (rules.type === 'number' && typeof value !== 'number') {
                    errors.push(`${field} must be a number`);
                }
                if (rules.minLength && typeof value === 'string' && value.length < rules.minLength) {
                    errors.push(`${field} must be at least ${rules.minLength} characters`);
                }
                if (rules.max && typeof value === 'number' && value > rules.max) {
                    errors.push(`${field} must be at most ${rules.max}`);
                }
                if (rules.pattern && typeof value === 'string' && !rules.pattern.test(value)) {
                    errors.push(`${field} format is invalid`);
                }
            }
        }
        
        if (errors.length > 0) {
            return res.status(400).json({ error: 'Validation failed', details: errors });
        }
        
        next();
    };
}

// Usage
router.post('/users', validateBody({
    name: { required: true, type: 'string', minLength: 2 },
    email: { required: true, type: 'string', pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/ },
    age: { type: 'number', min: 0, max: 150 },
}), userController.create);
```

### Rate Limiting Middleware

```javascript
// src/middleware/rateLimit.js — Custom rate limiter

const requests = new Map();

export function rateLimit({ windowMs = 60000, max = 100, keyGenerator } = {}) {
    return (req, res, next) => {
        const key = keyGenerator ? keyGenerator(req) : req.ip;
        const now = Date.now();
        
        if (!requests.has(key)) {
            requests.set(key, []);
        }
        
        const timestamps = requests.get(key);
        
        // Remove expired timestamps
        while (timestamps.length > 0 && timestamps[0] < now - windowMs) {
            timestamps.shift();
        }
        
        if (timestamps.length >= max) {
            const retryAfter = Math.ceil((timestamps[0] + windowMs - now) / 1000);
            res.set('Retry-After', retryAfter);
            return res.status(429).json({
                error: 'Too many requests',
                retryAfter,
            });
        }
        
        timestamps.push(now);
        
        res.set('X-RateLimit-Limit', max);
        res.set('X-RateLimit-Remaining', max - timestamps.length);
        
        next();
    };
}

// Usage
app.use('/api/', rateLimit({ windowMs: 15 * 60 * 1000, max: 100 }));
app.use('/api/auth/', rateLimit({ windowMs: 15 * 60 * 1000, max: 5 }));
```

### Async Error Handler

```javascript
// src/middleware/asyncHandler.js — Wraps async route handlers

export function asyncHandler(fn) {
    return (req, res, next) => {
        Promise.resolve(fn(req, res, next)).catch(next);
    };
}

// Usage — no more try/catch in every route
router.get('/:id', asyncHandler(async (req, res) => {
    const user = await User.findById(req.params.id);
    if (!user) {
        throw new AppError('User not found', 404);
    }
    res.json(user);
}));
```

### Custom Error Class

```javascript
// src/errors/AppError.js — Custom application error

export class AppError extends Error {
    constructor(message, statusCode = 500, code = 'INTERNAL_ERROR') {
        super(message);
        this.statusCode = statusCode;
        this.code = code;
        this.isOperational = true;
    }
}

// Global error handler
app.use((err, req, res, next) => {
    if (err.isOperational) {
        res.status(err.statusCode).json({
            error: { message: err.message, code: err.code }
        });
    } else {
        console.error('Unexpected error:', err);
        res.status(500).json({
            error: { message: 'Internal server error', code: 'INTERNAL_ERROR' }
        });
    }
});
```

## Middleware Execution Order

```javascript
// Middleware order matters!

// 1. Security headers (first)
app.use(helmet());

// 2. CORS
app.use(cors());

// 3. Body parsing
app.use(express.json());

// 4. Request logging
app.use(requestLogger);

// 5. Rate limiting
app.use(rateLimiter);

// 6. Authentication (before protected routes)
app.use('/api/protected', authenticate);

// 7. Routes
app.use('/api/users', userRoutes);
app.use('/api/products', productRoutes);

// 8. 404 handler
app.use(notFoundHandler);

// 9. Error handler (MUST be last, 4 parameters)
app.use(errorHandler);
```

## Best Practices Checklist

- [ ] Use Router for modular route organization
- [ ] Create reusable middleware functions
- [ ] Validate all input parameters
- [ ] Use asyncHandler to avoid try/catch duplication
- [ ] Order middleware correctly (security → parsing → auth → routes → errors)
- [ ] Implement rate limiting on auth endpoints
- [ ] Use custom error classes for consistent error responses

## Cross-References

- See [Basic Server](./01-basic-server.md) for Express setup
- See [Static Files and API](./03-static-files-api.md) for serving files
- See [Security Best Practices](../21-security-modern/01-security-headers-deps.md) for hardening

## Next Steps

Continue to [Static Files and API](./03-static-files-api.md) for file serving and API patterns.
