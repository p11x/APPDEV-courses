# Error Handling in Express

## What You'll Learn

- Creating error-handling middleware
- Throwing errors from routes
- 404 handling

## Error-Handling Middleware

Error handlers have 4 parameters:

```javascript
// Error handler
app.use((err, req, res, next) => {
  console.error(err.stack);
  res.status(500).json({ error: 'Something went wrong!' });
});
```

## Throwing Errors

```javascript
app.get('/error', (req, res, next) => {
  const error = new Error('Oops!');
  next(error);  // Pass to error handler
});
```

## 404 Handler

```javascript
// 404 - must be at the end!
app.use((req, res) => {
  res.status(404).send('Not found');
});
```

## Code Example

```javascript
// error-handling.js - Express error handling

import express from 'express';
const app = express();
app.use(express.json());

// Regular routes
app.get('/success', (req, res) => {
  res.json({ message: 'Success!' });
});

app.get('/error', (req, res, next) => {
  const error = new Error('Something went wrong');
  error.status = 500;
  next(error);
});

// Error-handling middleware
app.use((err, req, res, next) => {
  const status = err.status || 500;
  res.status(status).json({
    error: err.message
  });
});

// 404 handler (must be last)
app.use((req, res) => {
  res.status(404).json({ error: 'Route not found' });
});

app.listen(3000);
```

## Try It Yourself

### Exercise 1: Error Handler
Add global error handling to your Express app.

### Exercise 2: Custom Errors
Create custom error classes for different error types.
