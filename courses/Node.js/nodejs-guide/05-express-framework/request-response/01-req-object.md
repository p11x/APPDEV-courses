# Express Request Object

## What You'll Learn

- Accessing request body, params, query
- Request headers
- Request properties

## Request Object Properties

### req.body

Parsed request body (requires express.json()):

```javascript
app.use(express.json());

app.post('/users', (req, res) => {
  const { name, email } = req.body;
  res.json({ name, email });
});
```

### req.params

URL parameters:

```javascript
app.get('/users/:id', (req, res) => {
  const userId = req.params.id;
  res.json({ id: userId });
});
```

### req.query

Query string parameters:

```javascript
app.get('/search', (req, res) => {
  const q = req.query.q;
  const page = req.query.page;
  res.json({ q, page });
});
```

### req.headers

Request headers:

```javascript
app.get('/', (req, res) => {
  const contentType = req.headers['content-type'];
  const auth = req.headers.authorization;
  res.json({ contentType, auth });
});
```

## Code Example

```javascript
// req-demo.js - Request object demonstration

import express from 'express';
const app = express();
app.use(express.json());

// Request body
app.post('/api/data', (req, res) => {
  res.json({
    body: req.body,
    params: req.params,
    query: req.query
  });
});

// Route parameters
app.get('/users/:userId/posts/:postId', (req, res) => {
  res.json({
    userId: req.params.userId,
    postId: req.params.postId
  });
});

// Query string
app.get('/search', (req, res) => {
  res.json(req.query);
});

// Headers
app.get('/headers', (req, res) => {
  res.json({
    userAgent: req.headers['user-agent'],
    accept: req.headers.accept
  });
});

app.listen(3000);
```

## Try It Yourself

### Exercise 1: Access All Properties
Create routes that demonstrate all request properties.

### Exercise 2: Parse Body Data
Create a route that receives JSON and echoes it back.

## Next Steps

Continue to [Response Object](./02-res-object.md).
