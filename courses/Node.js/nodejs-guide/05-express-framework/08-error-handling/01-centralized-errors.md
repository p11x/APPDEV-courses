# Express.js Error Handling Patterns

## What You'll Learn

- Centralized error handling
- Custom error classes
- Error logging and monitoring
- Graceful error responses

## Custom Error Classes

```javascript
class AppError extends Error {
    constructor(message, statusCode = 500, code = 'INTERNAL_ERROR') {
        super(message);
        this.statusCode = statusCode;
        this.code = code;
        this.isOperational = true;
        Error.captureStackTrace(this, this.constructor);
    }
}

class NotFoundError extends AppError {
    constructor(resource = 'Resource') {
        super(`${resource} not found`, 404, 'NOT_FOUND');
    }
}

class ValidationError extends AppError {
    constructor(details) {
        super('Validation failed', 400, 'VALIDATION_ERROR');
        this.details = details;
    }
}
```

## Centralized Error Handler

```javascript
const asyncHandler = (fn) => (req, res, next) =>
    Promise.resolve(fn(req, res, next)).catch(next);

app.get('/users/:id', asyncHandler(async (req, res) => {
    const user = await db.getUser(req.params.id);
    if (!user) throw new NotFoundError('User');
    res.json(user);
}));

// Error handler middleware (must be last)
app.use((err, req, res, next) => {
    console.error(`[${req.method}] ${req.path}:`, err.message);

    if (err.isOperational) {
        return res.status(err.statusCode).json({
            error: { message: err.message, code: err.code }
        });
    }

    console.error('Unexpected:', err.stack);
    res.status(500).json({
        error: { message: 'Internal server error' }
    });
});
```

## Best Practices Checklist

- [ ] Create custom error classes for different types
- [ ] Use centralized error handler
- [ ] Wrap async handlers with asyncHandler
- [ ] Never expose stack traces in production
- [ ] Log all errors for debugging

## Cross-References

- See [Middleware](../03-middleware-guide/01-custom-middleware.md) for middleware patterns
- See [Security](../05-security-implementation/01-helmet-cors.md) for security
- See [Monitoring](../14-monitoring-observability/01-apm-setup.md) for logging

## Next Steps

Continue to [TypeScript Integration](../09-typescript-integration/01-typescript-setup.md) for TypeScript.
