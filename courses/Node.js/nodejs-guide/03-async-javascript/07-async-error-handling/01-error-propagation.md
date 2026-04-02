# Async Error Propagation Mechanisms

## What You'll Learn

- Error propagation through async chains
- Custom async error classes
- Centralized error handling
- Error boundary patterns

## Error Propagation in Async Code

```javascript
// Errors propagate through await
async function level3() {
    throw new Error('Something went wrong');
}

async function level2() {
    await level3(); // Error propagates up
}

async function level1() {
    await level2(); // Error propagates up
}

// Error caught at top level
try {
    await level1();
} catch (err) {
    console.error('Caught:', err.message); // 'Something went wrong'
    console.error('Stack:', err.stack);    // Full stack trace
}
```

## Custom Async Error Classes

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

class TimeoutError extends AppError {
    constructor(operation, ms) {
        super(`${operation} timed out after ${ms}ms`, 408, 'TIMEOUT');
    }
}

// Usage in async code
async function getUser(id) {
    const user = await db.query('SELECT * FROM users WHERE id = $1', [id]);
    if (!user) throw new NotFoundError('User');
    return user;
}

async function updateUser(id, data) {
    if (!data.name) {
        throw new ValidationError([{ field: 'name', message: 'Name required' }]);
    }
    return db.query('UPDATE users SET name = $1 WHERE id = $2', [data.name, id]);
}
```

## Centralized Error Handler

```javascript
// Express error handler
app.use((err, req, res, next) => {
    if (err.isOperational) {
        return res.status(err.statusCode).json({
            error: { message: err.message, code: err.code }
        });
    }

    console.error('Unexpected error:', err);
    res.status(500).json({
        error: { message: 'Internal server error', code: 'INTERNAL_ERROR' }
    });
});

// Async handler wrapper
const asyncHandler = (fn) => (req, res, next) => {
    Promise.resolve(fn(req, res, next)).catch(next);
};

// Usage
app.get('/users/:id', asyncHandler(async (req, res) => {
    const user = await getUser(req.params.id);
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

- See [Error Boundaries](./02-error-boundaries.md) for boundary patterns
- See [Graceful Degradation](./03-graceful-degradation.md) for fallback patterns
- See [Promise Debugging](../03-promises/04-promise-debugging.md) for debugging

## Next Steps

Continue to [Error Boundaries](./02-error-boundaries.md) for boundary patterns.
