# Error Propagation and Centralized Handling

## What You'll Learn

- Error propagation patterns
- Custom error classes
- Centralized error handling middleware
- Async error handling

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

class UnauthorizedError extends AppError {
    constructor(message = 'Unauthorized') {
        super(message, 401, 'UNAUTHORIZED');
    }
}
```

## Centralized Error Handler

```javascript
// Express error handling middleware (4 parameters)
app.use((err, req, res, next) => {
    // Log error
    console.error(`[${req.method}] ${req.path}:`, err.message);

    // Operational errors — safe to expose
    if (err.isOperational) {
        return res.status(err.statusCode).json({
            error: {
                message: err.message,
                code: err.code,
                ...(err.details && { details: err.details }),
            },
        });
    }

    // Programming errors — hide details
    console.error('Unexpected error:', err.stack);
    res.status(500).json({
        error: {
            message: 'Internal server error',
            code: 'INTERNAL_ERROR',
        },
    });
});
```

## Async Error Wrapper

```javascript
const asyncHandler = (fn) => (req, res, next) => {
    Promise.resolve(fn(req, res, next)).catch(next);
};

// Usage
app.get('/users/:id', asyncHandler(async (req, res) => {
    const user = await userService.findById(req.params.id);
    if (!user) throw new NotFoundError('User');
    res.json(user);
}));
```

## Best Practices Checklist

- [ ] Create custom error classes for different error types
- [ ] Use centralized error handler middleware
- [ ] Wrap async route handlers with asyncHandler
- [ ] Never expose stack traces in production
- [ ] Log all errors for debugging

## Cross-References

- See [Recovery Strategies](./02-recovery-strategies.md) for retry/circuit breaker
- See [Graceful Degradation](./03-graceful-degradation.md) for fallback patterns
- See [Design Patterns](../10-design-patterns/01-creational-patterns.md) for architecture

## Next Steps

Continue to [Recovery Strategies](./02-recovery-strategies.md) for retry patterns.
