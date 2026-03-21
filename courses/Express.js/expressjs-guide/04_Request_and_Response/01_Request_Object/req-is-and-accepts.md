# req.is() and req.accepts()

## 📌 What You'll Learn
- How to check the request's Content-Type with req.is()
- How to check what the client accepts with req.accepts()
- How to use these methods for content negotiation

## 🧠 Concept Explained (Plain English)

When building APIs, it's often useful to know what type of data the client is sending in the request body (Content-Type) and what type of data the client wants to receive (Accept header).

Express provides two helpful methods on the request object: `req.is()` and `req.accepts()`.

- `req.is(type)` checks if the request's Content-Type matches the given MIME type.
- `req.accepts(types)` checks if the client accepts any of the given MIME types, returning the best match or false.

Think of it like a waiter at a restaurant:
- `req.is()` is like checking what the customer ordered (what they're sending).
- `req.accepts()` is like asking what the customer is willing to eat (what they want to receive).

These methods help you implement content negotiation, where your API can respond in different formats based on what the client can handle.

## 💻 Code Example

```javascript
// ES Module - Using req.is() and req.accepts()

import express from 'express';

const app = express();

// We need to parse JSON bodies to test req.is()
// app.use(express.json());

// ========================================
// USING req.is() TO CHECK CONTENT-TYPE
// ========================================

app.post('/data', (req, res) => {
    // Check if the request body is JSON
    if (req.is('application/json')) {
        // We can safely access req.body
        res.json({ 
            message: 'Received JSON data',
            data: req.body
        });
    } else if (req.is('text/plain')) {
        // Handle plain text
        res.json({ 
            message: 'Received plain text',
            text: req.body
        });
    } else {
        // Unsupported content type
        res.status(415).json({ error: 'Unsupported media type' });
    }
});

// ========================================
// USING req.accepts() FOR CONTENT NEGOTIATION
// ========================================

app.get('/api/users', (req, res) => {
    // Check what the client accepts
    // If they accept JSON, send JSON
    // If they accept HTML, send HTML (simplified example)
    // If they accept neither, return 406 Not Acceptable
    
    if (req.accepts('application/json')) {
        res.json({ users: ['Alice', 'Bob'] });
    } else if (req.accepts('text/html')) {
        res.send('<h1>Users</h1><ul><li>Alice</li><li>Bob</li></ul>');
    } else {
        res.status(406).json({ error: 'Not acceptable' });
    }
});

// ========================================
// USING req.accepts() WITH AN ARRAY
// ========================================

app.get('/api/data', (req, res) => {
    // Let the client choose the format
    // req.accepts() returns the best match or false
    const format = req.accepts(['application/json', 'text/html', 'text/plain']);
    
    if (!format) {
        return res.status(406).json({ error: 'Not acceptable' });
    }
    
    const data = { message: 'Hello World', timestamp: new Date() };
    
    switch (format) {
        case 'application/json':
            res.json(data);
            break;
        case 'text/html':
            res.send(`<h1>${data.message}</h1><p>${data.timestamp}</p>`);
            break;
        case 'text/plain':
            res.send(`${data.message}\n${data.timestamp}`);
            break;
    }
});

// ========================================
// USING req.accepts() WITH MIME TYPE WILDCARDS
// ========================================

app.get('/api/feed', (req, res) => {
    // The client might accept any XML or any JSON
    if (req.accepts('application/json')) {
        res.json({ feed: ['item1', 'item2'] });
    } else if (req.accepts('text/xml')) {
        res.type('text/xml').send('<feed><item>item1</item><item>item2</item></feed>');
    } else if (req.accepts('image/png')) {
        // In a real app, you would send actual image data
        res.type('image/png').send('PNG data would go here');
    } else {
        res.status(406).json({ error: 'Not acceptable' });
    }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.post('/data', (req, res) => {` | Defines a POST route for /data |
| 10 | `if (req.is('application/json')) {` | Checks if Content-Type is application/json |
| 14 | `} else if (req.is('text/plain')) {` | Checks if Content-Type is text/plain |
| 18 | `} else {` | Handles unsupported content types |
| 19 | `res.status(415).json({ error: 'Unsupported media type' });` | Returns 415 status |
| 24 | `app.get('/api/users', (req, res) => {` | Defines a GET route for /api/users |
| 27 | `if (req.accepts('application/json')) {` | Checks if client accepts JSON |
| 31 | `} else if (req.accepts('text/html')) {` | Checks if client accepts HTML |
| 35 | `} else {` | Handles when client accepts neither |
| 36 | `res.status(406).json({ error: 'Not acceptable' });` | Returns 406 status |
| 42 | `app.get('/api/data', (req, res) => {` | Defines a GET route for /api/data |
| 45 | `const format = req.accepts(['application/json', 'text/html', 'text/plain']);` | Gets the best match from the array |
| 48 | `if (!format) {` | Checks if no acceptable format was found |
| 49 | `return res.status(406).json({ error: 'Not acceptable' });` | Returns 406 if none accepted |
| 52 | `switch (format) {` | Switch on the accepted format |
| 55 | `case 'application/json':` | Handles JSON response |
| 58 | `case 'text/html':` | Handles HTML response |
| 61 | `case 'text/plain':` | Handles plain text response |
| 64 | `app.get('/api/feed', (req, res) => {` | Defines a GET route for /api/feed |
| 66 | `if (req.accepts('application/json')) {` | Checks if client accepts JSON |
| 69 | `} else if (req.accepts('text/xml')) {` | Checks if client accepts XML |
| 72 | `} else if (req.accepts('image/png')) {` | Checks if client accepts PNG |
| 75 | `} else {` | Handles when none of the above are accepted |
| 76 | `res.status(406).json({ error: 'Not acceptable' });` | Returns 406 status |

## ⚠️ Common Mistakes

**1. Forgetting to parse the request body**
If you're using `req.is()` to check for JSON or URL-encoded data, you need the appropriate body parsing middleware (express.json() or express.urlencoded()) to populate `req.body`.

**2. Not handling the case when req.accepts() returns false**
If the client doesn't accept any of the types you offer, you should return a 406 Not Acceptable status.

**3. Confusing req.is() with checking req.headers['content-type'] directly**
While you could check the header directly, `req.is()` is more robust because it handles things like charset parameters (e.g., "application/json; charset=utf-8").

**4. Not using the wildcard formats correctly**
You can use wildcards like `req.accepts('application/*')` to check for any subtype of a media type.

**5. Forgetting that req.accepts() returns the best match**
When given an array, `req.accepts()` returns the single best match (the first one in the array that the client accepts, considering quality values if present in the Accept header).

## ✅ Quick Recap

- `req.is(type)` checks if the request's Content-Type matches the given type
- `req.accepts(type)` or `req.accepts([type1, type2, ...])` checks what the client accepts
- Use `req.is()` to determine how to parse the request body
- Use `req.accepts()` for content negotiation (deciding what format to send back)
- Always handle the case when the client doesn't accept any of your offered formats (return 406)

## 🔗 What's Next

That completes the Request Object section. Next, we'll learn about the Response Object.
