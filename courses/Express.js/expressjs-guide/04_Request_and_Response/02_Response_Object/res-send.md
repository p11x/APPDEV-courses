# res.send() - Sending Responses

## 📌 What You'll Learn
- How to send different types of responses
- The difference between res.send() and res.json()
- Setting content types automatically

## 🧠 Concept Explained (Plain English)

The `res.send()` method is one of the most common ways to send a response back to the client. Think of it like a versatile letter sender - it can figure out what kind of content you're sending (text, HTML, JSON) and sets the appropriate headers automatically.

When you call `res.send()`, Express:
1. Automatically sets the Content-Type header based on what you pass
2. Can send strings, Buffers, or objects
3. Closes the response (you can't send more data after this)

## 💻 Code Example

```javascript
// ES Module - Using res.send()

import express from 'express';

const app = express();

// ========================================
// res.send() with different data types
// ========================================

// Send string (Content-Type: text/html)
app.get('/text', (req, res) => {
    // req = request object, contains info from client
    // res = response object, what we send back
    res.send('Hello, World!');
});

// Send HTML
app.get('/html', (req, res) => {
    res.send('<h1>Hello, World!</h1><p>This is HTML</p>');
});

// Send object (automatically sets Content-Type: application/json)
app.get('/json', (req, res) => {
    res.send({ 
        message: 'Hello, World!',
        timestamp: new Date() 
    });
});

// Send array
app.get('/array', (req, res) => {
    res.send(['apple', 'banana', 'cherry']);
});

// Send Buffer (binary data)
app.get('/buffer', (req, res) => {
    const buffer = Buffer.from('Hello, World!');
    res.send(buffer);
});

// ========================================
// IMPORTANT: res.send() closes the response
// ========================================

app.get('/error-example', (req, res) => {
    res.send('First response');
    // This won't work - response is already sent!
    // res.send('Second response'); // Error!
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 10 | `app.get('/text', (req, res) => {` | Route for GET /text |
| 13 | `res.send('Hello, World!');` | Send string response |
| 26 | `res.send({ message: ... });` | Send JSON object (auto-converted) |
| 46 | `app.listen(PORT, ...)` | Start server listening on port |

## ⚠️ Common Mistakes

**1. Calling res.send() twice**
Once you call res.send(), the response is complete. Calling it again will cause an error.

**2. Not returning after res.send()**
Always return after sending to prevent further code execution.

**3. Using res.send() for JSON APIs**
Use res.json() instead for APIs - it's more explicit and handles edge cases better.

## ✅ Quick Recap

- `res.send()` automatically sets Content-Type based on data
- Works with strings, objects, arrays, and Buffers
- Closes the response after sending
- Use for mixed content responses

## 🔗 What's Next

Learn about [res.json()](./res-json.md) for JSON API responses