# req.headers

## 📌 What You'll Learn
- What req.headers is and how it's populated
- How to access HTTP headers in your Express application
- Commonly used headers and their purposes

## 🧠 Concept Explained (Plain English)

HTTP headers are key-value pairs sent in the request and response that provide additional information about the HTTP communication. They are not part of the request body or URL, but rather metadata about the request.

For example, headers can tell the server what type of data the client accepts (Accept), what language the client prefers (Accept-Language), or what browser the client is using (User-Agent).

In Express, all incoming request headers are available in `req.headers` as an object. Header names are converted to lowercase, so you can access them consistently regardless of how the client sent them.

Think of it like the envelope of a letter. The letter itself is the request body, but the envelope has information like the sender's address, the recipient's address, and any special handling instructions. HTTP headers are like that envelope — they provide context and instructions for how to handle the request.

## 💻 Code Example

```javascript
// ES Module - Accessing Request Headers with req.headers

import express from 'express';

const app = express();

// We'll use express.json() for parsing JSON bodies in other examples, but for this one we focus on headers
// app.use(express.json());

// ========================================
// VIEW ALL HEADERS
// ========================================

app.get('/headers', (req, res) => {
    // req.headers is an object containing all request headers
    // Header names are in lowercase
    res.json({ 
        message: 'Here are all the request headers',
        headers: req.headers
    });
});

// ========================================
// ACCESSING SPECIFIC HEADERS
// ========================================

// Example: Get the User-Agent header
app.get('/user-agent', (req, res) => {
    const userAgent = req.headers['user-agent'];
    res.json({ 
        message: 'Your User-Agent',
        userAgent: userAgent || 'Not provided'
    });
});

// Example: Get the Accept header (what content types the client accepts)
app.get('/accept', (req, res) => {
    const accept = req.headers['accept'];
    res.json({ 
        message: 'Accept header',
        accept: accept || 'Not provided'
    });
});

// Example: Get the Authorization header (often used for tokens)
app.get('/auth', (req, res) => {
    const authorization = req.headers['authorization'];
    if (!authorization) {
        return res.status(401).json({ error: 'Authorization header required' });
    }
    res.json({ 
        message: 'Authorization header present',
        // In a real app, you would validate the token here
        // For security, don't echo the token back in production
        hasToken: true
    });
});

// Example: Get the Content-Type header (important for POST/PUT requests)
app.post('/data', (req, res) => {
    const contentType = req.headers['content-type'];
    res.json({ 
        message: 'Content-Type header',
        contentType: contentType || 'Not provided'
    });
});

// ========================================
// CHECKING FOR HEADER PRESENCE
// ========================================

// Example: Check if the client accepts JSON
app.get('/check-accept', (req, res) => {
    const accept = req.headers['accept'] || '';
    if (accept.includes('application/json')) {
        res.json({ message: 'Client accepts JSON' });
    } else {
        res.status(406).json({ error: 'Client does not accept JSON' });
    }
});

// Example: Get the Referer header (where the request came from)
app.get('/referer', (req, res) => {
    const referer = req.headers['referer'];
    res.json({ 
        message: 'Referer header',
        referer: referer || 'Direct access or not provided'
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 8 | `app.get('/headers', (req, res) => {` | Defines a route to view all headers |
| 10 | `req.headers` | Accesses the headers object |
| 14 | `app.get('/user-agent', (req, res) => {` | Defines a route for User-Agent |
| 16 | `const userAgent = req.headers['user-agent'];` | Accesses the user-agent header |
| 20 | `app.get('/accept', (req, res) => {` | Defines a route for Accept header |
| 22 | `const accept = req.headers['accept'];` | Accesses the accept header |
| 26 | `app.get('/auth', (req, res) => {` | Defines a route for Authorization header |
| 28 | `const authorization = req.headers['authorization'];` | Accesses the authorization header |
| 31 | `if (!authorization) {` | Checks if the header is missing |
| 32 | `return res.status(401).json({ error: 'Authorization header required' });` | Returns error if missing |
| 38 | `app.post('/data', (req, res) => {` | Defines a POST route for Content-Type |
| 40 | `const contentType = req.headers['content-type'];` | Accesses the content-type header |
| 44 | `app.get('/check-accept', (req, res) => {` | Defines a route to check Accept header |
| 46 | `const accept = req.headers['accept'] || '';` | Gets accept header, defaults to empty string |
| 48 | `if (accept.includes('application/json')) {` | Checks if it includes JSON |
| 52 | `app.get('/referer', (req, res) => {` | Defines a route for Referer header |
| 54 | `const referer = req.headers['referer'];` | Accesses the referer header |
| 56 | `res.json({ ... });` | Sends the referer in the response |

## Common Request Headers

| Header | Description | Example Value |
|--------|-------------|---------------|
| **host** | The domain name of the server (for virtual hosting) | "example.com:3000" |
| **user-agent** | Information about the client's browser | "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36" |
| **accept** | What content types the client can understand | "application/json, text/plain, */*" |
| **accept-language** | What languages the client prefers | "en-US,en;q=0.9" |
| **accept-encoding** | What encodings the client can handle | "gzip, deflate, br" |
| **referer** | The URL of the page that made the request | "https://example.com/page" |
| **authorization** | Credentials for authentication (often a token) | "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." |
| **content-type** | The MIME type of the request body (for POST/PUT) | "application/json" |
| **content-length** | The length of the request body in bytes | "342" |
| **connection** | Whether the client wants to keep the connection open | "keep-alive" |
| **cookie** | Cookies sent by the client | "session_id=abc123; user_pref=dark" |

## ⚠️ Common Mistakes

**1. Header names are case-insensitive in HTTP, but Express converts them to lowercase**
Always access headers using lowercase names in `req.headers` (e.g., `req.headers['user-agent']`, not `req.headers['User-Agent']`).

**2. Forgetting that headers might be missing**
Not all headers are present in every request. Always check if a header exists before using it.

**3. Confusing request headers with response headers**
`req.headers` contains incoming request headers. To set response headers, you use `res.setHeader()` or `res.header()`.

**4. Not handling multiple values**
Some headers can have multiple values (like Cookie). Express joins them with commas, so you may need to split them.

**5. Security risks with user-provided headers**
Never trust header values for security decisions without proper validation (e.g., don't use Referer for authorization).

## ✅ Quick Recap

- `req.headers` contains all incoming request headers as an object
- Header names are in lowercase in `req.headers`
- Access headers using `req.headers['header-name']`
- Common headers include User-Agent, Accept, Authorization, Content-Type
- Always check for header presence and validate values when needed

## 🔗 What's Next

Let's learn about accessing cookies with `req.cookies` (requires cookie-parser middleware).
