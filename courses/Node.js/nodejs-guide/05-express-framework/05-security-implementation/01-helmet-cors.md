# Express.js Security Implementation

## What You'll Learn

- Helmet.js configuration
- CORS policies
- Authentication middleware
- Input validation and sanitization

## Helmet.js Configuration

```bash
npm install helmet
```

```javascript
import helmet from 'helmet';

app.use(helmet({
    contentSecurityPolicy: {
        directives: {
            defaultSrc: ["'self'"],
            scriptSrc: ["'self'"],
            styleSrc: ["'self'", "'unsafe-inline'"],
            imgSrc: ["'self'", 'data:', 'https:'],
        },
    },
    hsts: { maxAge: 31536000, includeSubDomains: true },
    referrerPolicy: { policy: 'strict-origin-when-cross-origin' },
}));
```

## CORS Configuration

```bash
npm install cors
```

```javascript
import cors from 'cors';

// Allow specific origins
app.use(cors({
    origin: ['https://example.com', 'https://app.example.com'],
    methods: ['GET', 'POST', 'PUT', 'DELETE'],
    allowedHeaders: ['Content-Type', 'Authorization'],
    credentials: true,
    maxAge: 86400,
}));
```

## Input Validation

```bash
npm install express-validator
```

```javascript
import { body, validationResult } from 'express-validator';

const validateUser = [
    body('name').trim().notEmpty().withMessage('Name required'),
    body('email').isEmail().normalizeEmail(),
    body('password').isLength({ min: 8 }).withMessage('Min 8 chars'),
    body('age').optional().isInt({ min: 0, max: 150 }),

    (req, res, next) => {
        const errors = validationResult(req);
        if (!errors.isEmpty()) {
            return res.status(400).json({ errors: errors.array() });
        }
        next();
    }
];

app.post('/api/users', validateUser, createUser);
```

## Rate Limiting

```bash
npm install express-rate-limit
```

```javascript
import rateLimit from 'express-rate-limit';

const limiter = rateLimit({
    windowMs: 15 * 60 * 1000,
    max: 100,
    standardHeaders: true,
    message: { error: 'Too many requests' },
});

app.use('/api/', limiter);

// Stricter for auth endpoints
const authLimiter = rateLimit({
    windowMs: 15 * 60 * 1000,
    max: 5,
});
app.use('/api/auth/', authLimiter);
```

## Best Practices Checklist

- [ ] Use Helmet.js for security headers
- [ ] Configure CORS for specific origins
- [ ] Validate all user input
- [ ] Implement rate limiting
- [ ] Use HTTPS in production

## Cross-References

- See [Middleware Guide](../03-middleware-guide/01-custom-middleware.md) for middleware patterns
- See [Error Handling](../08-error-handling/01-centralized-errors.md) for error handling
- See [Enterprise](../15-enterprise-implementation/01-auth-patterns.md) for auth

## Next Steps

Continue to [Performance Optimization](../06-performance-optimization/01-caching-strategies.md) for optimization.
