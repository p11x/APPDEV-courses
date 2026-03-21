# Express Response Object

## What You'll Learn

- Sending different response types
- Setting status codes
- Redirecting

## Response Methods

### res.send()

Send HTML or text:

```javascript
app.get('/', (req, res) => {
  res.send('<h1>Hello!</h1>');
});
```

### res.json()

Send JSON:

```javascript
app.get('/api', (req, res) => {
  res.json({ message: 'Hello', count: 5 });
});
```

### res.status()

Set status code:

```javascript
app.get('/error', (req, res) => {
  res.status(404).json({ error: 'Not found' });
});
```

### res.redirect()

Redirect:

```javascript
app.get('/old', (req, res) => {
  res.redirect('/new');
});
```

## Code Example

```javascript
// res-demo.js - Response demonstration

import express from 'express';
const app = express();

// JSON response
app.get('/api/users', (req, res) => {
  res.json([
    { id: 1, name: 'Alice' },
    { id: 2, name: 'Bob' }
  ]);
});

// HTML response
app.get('/', (req, res) => {
  res.send('<h1>Welcome!</h1><p>This is HTML</p>');
});

// Status codes
app.get('/not-found', (req, res) => {
  res.status(404).json({ error: 'User not found' });
});

app.get('/created', (req, res) => {
  res.status(201).json({ message: 'Created!' });
});

// Redirect
app.get('/old-page', (req, res) => {
  res.redirect('/new-page');
});

app.get('/new-page', (req, res) => {
  res.send('New page content');
});

app.listen(3000);
```

## Try It Yourself

### Exercise 1: Different Responses
Create routes that return JSON, HTML, and redirect.

### Exercise 2: Status Codes
Create routes with different status codes.

## Next Steps

Continue to [Error Handling in Express](./03-error-handling.md).
