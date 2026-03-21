# res.type() - Setting Content-Type

## 📌 What You'll Learn
- How to set Content-Type header
- Common MIME types
- Using res.type() vs res.set()

## 🧠 Concept Explained (Plain English)

The Content-Type header tells the browser what kind of data you're sending. Think of it like a label on a package - it tells the recipient what's inside so they know how to handle it.

The `res.type()` method is a shortcut for setting this header.

## 💻 Code Example

```javascript
// ES Module - Setting Content-Type

import express from 'express';

const app = express();

// Set content type
app.get('/json', (req, res) => {
    // req = request, res = response
    res.type('application/json');
    res.send('{"message":"hello"}');
});

// Using shorthand
app.get('/html', (req, res) => {
    res.type('html');
    res.send('<h1>Hello</h1>');
});

// Common types
app.get('/xml', (req, res) => {
    res.type('xml');
    res.send('<root><item>value</item></root>');
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 13 | `res.type('application/json');` | Set Content-Type to JSON |

## ✅ Quick Recap

- Use res.type() to set Content-Type
- Shorthand for res.set('Content-Type', ...)
- Common types: json, html, xml, text

## 🔗 What's Next

Continue to [Request Object](../01_Request_Object/req-body.md)