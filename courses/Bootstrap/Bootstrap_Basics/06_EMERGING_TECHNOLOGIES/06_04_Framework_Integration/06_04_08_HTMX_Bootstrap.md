---
title: "HTMX Bootstrap Integration"
topic: "Framework Integration"
difficulty: 2
duration: "30 minutes"
prerequisites: ["HTML fundamentals", "Server-side rendering (any backend)", "Bootstrap basics"]
tags: ["htmx", "bootstrap", "server-driven-ui", "partial-updates", "hypermedia"]
---

## Overview

HTMX enables server-driven UI updates via HTML partials, making it a natural fit with Bootstrap 5's component system. Instead of client-side JavaScript frameworks managing state, HTMX sends AJAX requests triggered by HTML attributes (`hx-get`, `hx-post`, `hx-trigger`) and replaces DOM elements with server-returned HTML fragments. Bootstrap provides the styling, layout, and interactive patterns; HTMX provides the dynamic behavior without custom JavaScript.

This architecture is ideal for server-rendered applications (Django, Rails, Laravel, Express, Go) where the server already generates Bootstrap HTML. HTMX adds interactivity — infinite scroll, lazy loading, inline editing, modal loading, form validation — through declarative HTML attributes that swap server-rendered partials into the page.

## Basic Implementation

### HTML Setup

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>HTMX + Bootstrap</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"
        rel="stylesheet">
  <script src="https://unpkg.com/htmx.org@2.0.0"></script>
</head>
<body>
  <nav class="navbar navbar-expand-lg bg-body-tertiary">
    <div class="container">
      <a class="navbar-brand" href="/">HTMX App</a>
    </div>
  </nav>

  <div class="container py-5">
    <div id="content">
      <h1>Welcome</h1>
      <button class="btn btn-primary"
              hx-get="/api/greeting"
              hx-target="#content"
              hx-swap="innerHTML">
        Load Greeting
      </button>
    </div>
  </div>
</body>
</html>
```

### Server-Side Partial (Express.js Example)

```js
// server.js
const express = require('express');
const app = express();

app.use(express.static('public'));
app.use(express.urlencoded({ extended: true }));

// Return HTML partial
app.get('/api/greeting', (req, res) => {
  res.send(`
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Hello from the server!</h5>
        <p class="card-text">This content was loaded via HTMX.</p>
        <button class="btn btn-outline-secondary"
                hx-get="/api/greeting"
                hx-target="#content"
                hx-swap="innerHTML">
          Refresh
        </button>
      </div>
    </div>
  `);
});

app.listen(3000);
```

## Advanced Variations

### Inline Form Validation

```html
<!-- Form with live validation -->
<form hx-post="/api/users" hx-target="#result" hx-swap="outerHTML">
  <div class="mb-3">
    <label class="form-label">Email</label>
    <input type="email" name="email" class="form-control"
           hx-post="/api/validate/email"
           hx-trigger="change, keyup delay:300ms"
           hx-target="#email-feedback"
           hx-swap="innerHTML">
    <div id="email-feedback"></div>
  </div>
  <div class="mb-3">
    <label class="form-label">Password</label>
    <input type="password" name="password" class="form-control"
           hx-post="/api/validate/password"
           hx-trigger="change, keyup delay:300ms"
           hx-target="#password-feedback"
           hx-swap="innerHTML">
    <div id="password-feedback"></div>
  </div>
  <button type="submit" class="btn btn-primary"
          hx-indicator="#spinner">
    <span id="spinner" class="spinner-border spinner-border-sm htmx-indicator"
          role="status"></span>
    Create Account
  </button>
</form>
```

```js
// Server-side validation endpoint
app.post('/api/validate/email', (req, res) => {
  const { email } = req.body;
  const isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

  if (!email) {
    return res.send('');
  }

  res.send(isValid
    ? '<div class="valid-feedback d-block">Looks good!</div>'
    : '<div class="invalid-feedback d-block">Please enter a valid email.</div>'
  );
});
```

### Lazy Loading with Bootstrap Spinner

```html
<!-- Load user list when visible -->
<div class="card"
     hx-get="/api/users"
     hx-trigger="revealed"
     hx-swap="innerHTML">
  <div class="card-body text-center">
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
  </div>
</div>
```

```js
app.get('/api/users', (req, res) => {
  const users = getUsersFromDB();
  const rows = users.map(u => `
    <tr>
      <td>${u.name}</td>
      <td>${u.email}</td>
      <td>
        <button class="btn btn-sm btn-outline-primary"
                hx-get="/api/users/${u.id}/edit"
                hx-target="closest tr"
                hx-swap="outerHTML">
          Edit
        </button>
      </td>
    </tr>
  `).join('');

  res.send(`
    <div class="card-header">Users</div>
    <div class="card-body p-0">
      <table class="table table-hover mb-0">
        <thead><tr><th>Name</th><th>Email</th><th>Actions</th></tr></thead>
        <tbody>${rows}</tbody>
      </table>
    </div>
  `);
});
```

### Modal Loading

```html
<!-- Button that loads modal content from server -->
<button class="btn btn-danger"
        hx-get="/api/confirm-delete/42"
        hx-target="#modal-container"
        hx-swap="innerHTML">
  Delete Item
</button>

<div id="modal-container"></div>
```

```js
app.get('/api/confirm-delete/:id', (req, res) => {
  res.send(`
    <div class="modal fade show d-block" tabindex="-1" style="background:rgba(0,0,0,.5)">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Confirm Deletion</h5>
            <button type="button" class="btn-close"
                    hx-get="/api/empty"
                    hx-target="#modal-container"
                    hx-swap="innerHTML"></button>
          </div>
          <div class="modal-body">
            <p>Are you sure you want to delete this item?</p>
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary"
                    hx-get="/api/empty"
                    hx-target="#modal-container"
                    hx-swap="innerHTML">Cancel</button>
            <button class="btn btn-danger"
                    hx-delete="/api/items/${req.params.id}"
                    hx-target="#modal-container"
                    hx-swap="innerHTML">Delete</button>
          </div>
        </div>
      </div>
    </div>
  `);
});

app.get('/api/empty', (req, res) => res.send(''));
```

### Infinite Scroll

```html
<div id="products">
  <!-- Initial product list rendered server-side -->
</div>

<div hx-get="/api/products?page=2"
     hx-trigger="revealed"
     hx-swap="afterend"
     hx-target="#products">
  <div class="text-center py-3">
    <div class="spinner-border text-primary"></div>
  </div>
</div>
```

## Best Practices

1. **Use `hx-target`** to specify exactly which element receives the server response, avoiding unintended DOM replacements.
2. **Use `hx-swap`** strategically: `innerHTML` for content, `outerHTML` for replacing elements, `beforeend` for appending.
3. **Use `hx-indicator`** with Bootstrap spinners to show loading state during requests.
4. **Use `hx-trigger="revealed"`** for lazy loading and infinite scroll patterns.
5. **Return HTML partials, not JSON** — HTMX works with the hypermedia paradigm where servers return markup.
6. **Include Bootstrap CSS and HTMX script** in the base layout so all partials inherit styling.
7. **Use `hx-boost="true"`** on links and forms for SPA-like navigation without full page reloads.
8. **Use `hx-vals`** to send additional data with requests without hidden form fields.
9. **Set `hx-preserve`** on elements that should survive HTMX swaps (e.g., music players, active editors).
10. **Use `hx-on`** for local event handling instead of global JavaScript when needed.

## Common Pitfalls

1. **Returning full HTML pages** instead of fragments — HTMX expects partial HTML for DOM replacement.
2. **Forgetting `hx-target`** causes the response to replace the triggering element instead of the intended container.
3. **Not including Bootstrap CSS in partials** — if the base page hasn't loaded Bootstrap CSS, partial content appears unstyled.
4. **Using `hx-swap="innerHTML"` on `<tr>` or `<li>`** — these require `outerHTML` for correct replacement.
5. **Missing CSRF tokens** in `hx-headers` for POST/PUT/DELETE requests in frameworks that require them.

## Accessibility Considerations

HTMX preserves server-rendered ARIA attributes in HTML partials. Use `aria-live="polite"` on containers that receive dynamic updates to announce changes to screen readers. Include `role="status"` on loading indicators. HTMX's `hx-disabled-elt` attribute disables form elements during submission, preventing duplicate requests while maintaining accessible form behavior. Server-returned Bootstrap components carry their full ARIA markup (modals with `aria-modal`, alerts with `role="alert"`).

## Responsive Behavior

HTMX does not affect Bootstrap's responsive CSS — all responsive classes work identically. Server-returned HTML partials include responsive Bootstrap grid classes that render correctly at any viewport width. Use `hx-swap="outerHTML"` to replace responsive grid items. When implementing infinite scroll, ensure new items follow the same responsive grid structure. HTMX-driven navigation (`hx-boost`) preserves the full responsive layout by replacing only the `<body>` content.