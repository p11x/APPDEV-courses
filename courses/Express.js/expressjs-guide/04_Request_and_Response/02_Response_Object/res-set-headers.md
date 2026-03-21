# res.set() - Setting Response Headers

## 📌 What You'll Learn
- How to set custom HTTP headers
- Common header use cases
- Setting multiple headers

## 🧠 Concept Explained (Plain English)

HTTP headers are metadata about the request or response. Think of them like the address on an envelope - they provide context about what's inside. The `res.set()` method lets you set custom headers on your response.

Common uses include:
- Setting caching policies
- Setting content types for special formats
- Custom authentication headers

## 💻 Code Example

```javascript
// ES Module - Setting Headers with res.set()

import express from 'express';

const app = express();

// Set single header
app.get('/json', (req, res) => {
    // req = request, res = response
    res.set('Content-Type', 'application/json');
    res.json({ message: 'Hello' });
});

// Set multiple headers
app.get('/custom', (req, res) => {
    res.set({
        'Content-Type': 'application/json',
        'X-Custom-Header': 'MyValue',
        'X-API-Version': '1.0'
    });
    res.json({ data: 'value' });
});

// Set caching headers
app.get('/static-data', (req, res) => {
    // Cache for 1 hour (3600 seconds)
    res.set('Cache-Control', 'public, max-age=3600');
    res.json({ data: 'This data is cached' });
});

// Disable caching
app.get('/dynamic-data', (req, res) => {
    res.set('Cache-Control', 'no-store');
    res.json({ timestamp: Date.now() });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 13 | `res.set('Content-Type', 'application/json');` | Set single header |
| 19 | `res.set({ 'X-Custom-Header': ... });` | Set multiple headers |
| 28 | `res.set('Cache-Control', 'public, max-age=3600');` | Set caching header |

## ✅ Quick Recap

- Use res.set() to set response headers
- Can set single or multiple headers
- Common for caching and custom metadata

## 🔗 What's Next

Learn about [res.cookie()](./res-cookie.md)