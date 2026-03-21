# Using Compression Middleware

## 📌 What You'll Learn
- What compression is and why it's important for performance
- How to install and use the compression middleware in Express
- How to configure compression options

## 🧠 Concept Explained (Plain English)

**Compression** reduces the size of the data sent from your server to the client, making your application faster and more efficient. This is especially important for text-based responses like HTML, JSON, and CSS.

Think of it like zipping a file before sending it over email. The recipient gets a smaller file, which transfers faster, and then unzips it to get the original content. The `compression` middleware does this automatically for your HTTP responses.

When a client sends a request indicating it can accept compressed data (via the `Accept-Encoding: gzip` header), the compression middleware will compress the response before sending it. The client then decompresses it automatically.

This middleware is essential for production applications to reduce bandwidth usage and improve load times.

## 💻 Code Example

```javascript
// ES Module - Using Compression Middleware

import express from 'express';
import compression from 'compression';

const app = express();

// ========================================
// IMPORTANT: Add Compression MIDDLEWARE
// ========================================
// This should be added early in your middleware stack, after any middleware that might modify the response body
// but before your routes.
// Note: The order of middleware matters.
app.use(compression());

// We still need to parse JSON for our routes
app.use(express.json());

// Example route that returns a large JSON object
app.get('/api/users', (req, res) => {
    // This will be automatically compressed if the client accepts gzip
    const users = Array.from({ length: 1000 }, (_, i) => ({
        id: i + 1,
        name: `User ${i + 1}`,
        email: `user${i + 1}@example.com`
    }));
    res.json({ users });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `import express from 'express';` | Import the Express framework |
| 2 | `import compression from 'compression';` | Import the compression middleware |
| 4 | `const app = express();` | Create an Express application instance |
| 7 | `app.use(compression());` | Add compression middleware |
| 10 | `app.use(express.json());` | Add JSON parsing middleware |
| 13-22 | `app.get('/api/users', ...)` | Example route that returns a large JSON object |
| 25 | `app.listen(PORT, ...)` | Start the server |

## Configuring Compression Options

You can pass an options object to the `compression()` function to customize its behavior:

```javascript
const compressionOptions = {
    threshold: 1024, // Only compress if the response is larger than 1KB (in bytes)
    filter: (req, res) => {
        // Decide whether to compress based on the request/response
        // For example, don't compress images
        if (req.headers['x-no-compression']) {
            return false; // Don't compress if this header is set
        }
        // Use the default filter function
        return compression.filter(req, res);
    },
    level: 6 // Compression level (0-9, where 9 is best compression but slowest)
};

app.use(compression(compressionOptions));
```

### Common Compression Options

| Option | Type | Description |
|--------|------|-------------|
| **threshold** | Number | Only compress responses larger than this many bytes (default: 1024) |
| **filter** | Function | Function to decide whether to compress based on request/response |
| **level** | Number | Compression level (0-9) |
| **memLevel** | Number | Memory level (1-9) |
| **strategy** | String | Strategy for compressing data |
| **windowBits** | Number | Window size for compression |

## How Compression Works

1. Client sends request with `Accept-Encoding: gzip, deflate` (or similar)
2. Server processes the request and generates a response
3. Compression middleware checks if the response should be compressed
4. If yes, it compresses the response body and sets the `Content-Encoding` header
5. Client receives the compressed response and automatically decompresses it

## ⚠️ Common Mistakes

**1. Placing compression middleware too late**
If you place compression after middleware that modifies the response body (like some template engines), it might not work correctly. Generally, place it after middleware that sets headers but before middleware that modifies the body.

**2. Compressing already compressed data**
Don't compress responses that are already compressed (like image files). The filter function can help avoid this.

**3. Using too high a compression level**
Higher levels (like 9) provide better compression but use more CPU. For most applications, level 6 is a good balance.

**4. Forgetting to test compression**
Use browser DevTools or curl to check if responses are being compressed:
```bash
curl -H "Accept-Encoding: gzip" -I http://localhost:3000/api/users
```
Look for `Content-Encoding: gzip` in the response headers.

## ✅ Quick Recap

- Compression reduces response size, improving performance
- Use `app.use(compression())` to add the middleware
- Place it early in your middleware stack (after middleware that sets headers, before middleware that modifies body)
- Configure options to control when and how compression is applied
- Essential for production applications to reduce bandwidth

## 🔗 What's Next

Let's learn about using cookie-parser middleware to handle cookies.
