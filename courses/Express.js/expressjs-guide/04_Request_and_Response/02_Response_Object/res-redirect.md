# res.redirect() - Redirecting Requests

## 📌 What You'll Learn
- How to redirect users to different URLs
- Absolute vs relative redirects
- Redirect status codes

## 🧠 Concept Explained (Plain English)

The `res.redirect()` method tells the browser or client to go to a different URL. Think of it like a receptionist redirecting you to a different office - they send you somewhere else to get what you need.

Redirects are commonly used for:
- After form submission, sending users to a success page
- Handling old URLs that have moved (SEO)
- Authentication flows (redirect to login, then back)

## 💻 Code Example

```javascript
// ES Module - Using res.redirect()

import express from 'express';

const app = express();

// ========================================
// Basic redirects
// ========================================

// Redirect to another page (302 Found - default)
app.get('/old-page', (req, res) => {
    // req = request, res = response
    res.redirect('/new-page');
});

// Redirect to external URL
app.get('/home', (req, res) => {
    res.redirect('https://example.com');
});

// Redirect with 301 (Permanent - for SEO)
app.get('/old-url', (req, res) => {
    res.redirect(301, '/new-url');
});

// Redirect with 302 (Temporary)
app.get('/temporary', (req, res) => {
    res.redirect(302, '/maintenance');
});

// ========================================
// Common redirect patterns
// ========================================

// After successful form submission
app.post('/contact', (req, res) => {
    // Process form...
    // Redirect to thank you page
    res.redirect('/contact/thank-you');
});

// Redirect back to previous page
app.post('/login', (req, res) => {
    const isValid = true; // Check credentials
    
    if (isValid) {
        // Redirect to previous page or default to /dashboard
        const returnUrl = req.query.returnUrl || '/dashboard';
        res.redirect(returnUrl);
    } else {
        res.redirect('/login?error=invalid');
    }
});

// Logout - redirect to home
app.get('/logout', (req, res) => {
    // Clear session...
    res.redirect('/');
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 15 | `res.redirect('/new-page');` | Redirect to internal path |
| 19 | `res.redirect('https://example.com');` | Redirect to external URL |
| 23 | `res.redirect(301, '/new-url');` | Permanent redirect (SEO) |
| 35 | `res.redirect('/contact/thank-you');` | Post-form redirect |

## ⚠️ Common Mistakes

**1. Not returning after redirect**
Always return after redirect to prevent further code execution.

**2. Using redirect for API endpoints**
APIs should return JSON, not redirect. Use redirects only for browser pages.

**3. Infinite redirect loops**
Make sure your redirect conditions can't create loops.

## ✅ Quick Recap

- `res.redirect(url)` redirects to URL
- Default is 302 (temporary)
- Use 301 for permanent moves (SEO)
- Common after form submissions
- Use for pages, not APIs

## 🔗 What's Next

Learn about [res.cookie()](./res-cookie.md) for setting cookies